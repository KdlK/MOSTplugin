using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;

namespace MOSTplugin.SkittingWall
{
    internal class Logic
    {
        public void RoomFinish(UIDocument uiDoc, Transaction tx, WallType wallType,float _offset, float _height, bool _joinwalls)
        {
            Document doc = uiDoc.Document;
            SkirtingBoardSetup skirtingBoardSetup = new SkirtingBoardSetup();
            skirtingBoardSetup.SelectedRooms = roomCollector();
            skirtingBoardSetup.Offset = _offset/304.8;
            
            skirtingBoardSetup.JoinWall = _joinwalls;
            skirtingBoardSetup.BoardHeight = _height/304.8;
            skirtingBoardSetup.SelectedWallType = wallType;



            CreateSkirtingBoard(doc, tx, skirtingBoardSetup);
            //Load the selection form



        }
        private  List<Room> roomCollector()
        {
            List<Room> elements = new List<Room>();
            Selection selection = Data.UIdoc.Selection;
            List<ElementId> elementIDs = selection.GetElementIds().ToList();
            foreach (ElementId elementID in elementIDs)
            {
                Element el = Data.Doc.GetElement(elementID);
                if (el.Category.Name == "Помещения")
                {
                    elements.Add(el as Room);
                }
            }
            return elements;
        }

        public void CreateSkirtingBoard(Document doc, Transaction tx, SkirtingBoardSetup skirtingBoardSetup)
        {
            tx.Start();

            WallType duplicatedWallType = DuplicateWallType(skirtingBoardSetup.SelectedWallType, doc);
            Dictionary<ElementId, ElementId> skirtingDictionary = CreateWalls(doc, skirtingBoardSetup.SelectedRooms, skirtingBoardSetup.BoardHeight, duplicatedWallType, skirtingBoardSetup.Offset);

            FailureHandlingOptions options = tx.GetFailureHandlingOptions();

            options.SetFailuresPreprocessor(new PlintePreprocessor());
            // Now, showing of any eventual mini-warnings will be postponed until the following transaction.
            tx.Commit(options);

            TransactionStatus transactionStatus = tx.GetStatus();

            if (transactionStatus != TransactionStatus.RolledBack)
            {
                tx.Start();


                List<ElementId> addedIds = new List<ElementId>(skirtingDictionary.Keys);
                foreach (ElementId addedSkirtingId in addedIds)
                {
                    if (doc.GetElement(addedSkirtingId) == null)
                    {
                        skirtingDictionary.Remove(addedSkirtingId);
                    }
                }

                Element.ChangeTypeId(doc, skirtingDictionary.Keys, skirtingBoardSetup.SelectedWallType.Id);

                //Join both wall
                if (skirtingBoardSetup.JoinWall)
                {
                    JoinGeometry(doc, skirtingDictionary);
                }

                doc.Delete(duplicatedWallType.Id);

                tx.Commit();
            }

        }

        private void JoinGeometry(Document doc, Dictionary<ElementId, ElementId> skirtingDictionary)
        {
            foreach (ElementId skirtingId in skirtingDictionary.Keys)
            {
                Wall skirtingWall = doc.GetElement(skirtingId) as Wall;

                if (skirtingWall != null)
                {
                    Parameter wallJustification = skirtingWall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
                    wallJustification.Set(3);
                    Wall baseWall = doc.GetElement(skirtingDictionary[skirtingId]) as Wall;

                    if (baseWall != null)
                    {
                        JoinGeometryUtils.JoinGeometry(doc, skirtingWall, baseWall);
                    }
                }
            }
        }

        private Dictionary<ElementId, ElementId> CreateWalls(Document doc, IEnumerable<Room> modelRooms, double height, WallType newWallType, double offset)
        {

            Dictionary<ElementId, ElementId> skirtingDictionary = new Dictionary<ElementId, ElementId>();

            //Loop on all rooms to get boundaries
            foreach (Room currentRoom in modelRooms)
            {
                ElementId roomLevelId = currentRoom.LevelId;

                SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                IList<IList<Autodesk.Revit.DB.BoundarySegment>> boundarySegmentArray = currentRoom.GetBoundarySegments(opt);
                if (null == boundarySegmentArray)  //the room may not be bound
                {
                    continue;
                }

                foreach (IList<Autodesk.Revit.DB.BoundarySegment> boundarySegArr in boundarySegmentArray)
                {
                    if (0 == boundarySegArr.Count)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Autodesk.Revit.DB.BoundarySegment boundarySegment in boundarySegArr)
                        {
                            //Check if the boundary is a room separation lines
                            Element boundaryElement = doc.GetElement(boundarySegment.ElementId);

                            if (boundaryElement == null) { continue; }

                            Categories categories = doc.Settings.Categories;
                            Category RoomSeparetionLineCat = categories.get_Item(BuiltInCategory.OST_RoomSeparationLines);

                            if (boundaryElement.Category.Id != RoomSeparetionLineCat.Id)
                            {
                                Wall currentWall = Wall.Create(doc, boundarySegment.GetCurve(), newWallType.Id, roomLevelId, height, offset, false, false);
                                Parameter wallJustification = currentWall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
                                wallJustification.Set(2);

                                skirtingDictionary.Add(currentWall.Id, boundarySegment.ElementId);
                            }
                        }
                    }
                }

            }

            return skirtingDictionary;
        }

        private WallType DuplicateWallType(WallType wallType, Document doc)
        {
            WallType newWallType;

            //Select the wall type in the document
            IEnumerable<WallType> _wallTypes = from elem in new FilteredElementCollector(doc).OfClass(typeof(WallType))
                                               let type = elem as WallType
                                               where type.Kind == WallKind.Basic
                                               select type;

            List<string> wallTypesNames = _wallTypes.Select(o => o.Name).ToList();

            if (!wallTypesNames.Contains("newWallTypeName"))
            {
                newWallType = wallType.Duplicate("newWallTypeName") as WallType;
            }
            else
            {
                newWallType = wallType.Duplicate("newWallTypeName2") as WallType;
            }

            CompoundStructure cs = newWallType.GetCompoundStructure();

            IList<CompoundStructureLayer> layers = cs.GetLayers();
            int layerIndex = 0;

            foreach (CompoundStructureLayer csl in layers)
            {
                double layerWidth = csl.Width * 2;
                if (cs.GetRegionsAssociatedToLayer(layerIndex).Count == 1)
                {
                    try
                    {
                        cs.SetLayerWidth(layerIndex, layerWidth);
                    }
                    catch
                    {
                        throw new ErrorMessageException(Tools.LangResMan.GetString("roomFinishes_verticallyCompoundError", Tools.Cult));
                    }
                }
                else
                {
                    throw new ErrorMessageException(Tools.LangResMan.GetString("roomFinishes_verticallyCompoundError", Tools.Cult));
                }

                layerIndex++;
            }

            newWallType.SetCompoundStructure(cs);

            return newWallType;
        }

        /// <summary>
        /// Implements the FailuresProcessing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor failuresAccessor = e.GetFailuresAccessor();
            //failuresAccessor
            String transactionName = failuresAccessor.GetTransactionName();

            IList<FailureMessageAccessor> failures = failuresAccessor.GetFailureMessages();

            if (failures.Count != 0)
            {
                foreach (FailureMessageAccessor f in failures)
                {
                    FailureDefinitionId id = f.GetFailureDefinitionId();

                    if (id == BuiltInFailures.JoinElementsFailures.CannotJoinElementsError)
                    {
                        // only default option being choosen,  not good enough!
                        //failuresAccessor.DeleteWarning(f);
                        failuresAccessor.ResolveFailure(f);
                        //failuresAccessor.
                        e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
                    }

                    return;
                }
            }

        }

    }




}


