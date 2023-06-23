using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MOSTplugin.SkittingWall
{
    internal class SkittingWall_Class
    {
        static public void test() { 
        
        }
        static public List<Room> roomCollector() {
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

        public static List<Element> SkittingWallCollector() {
            List<Element> skittingWalls = new List<Element> ();
            List<Element> walls = new FilteredElementCollector(Data.Doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .ToElements()
                .ToList();
            foreach (Element el in walls) {
                if (el.LookupParameter("Группа модели")?.AsString() == "Отделка") { 
                    skittingWalls.Add(el);
                }
            
            }
            return skittingWalls;
        }
        public static List<Curve> GetRoomLines(Room room) {
            List<Curve> boundaryLines = new List<Curve>();
            
            List<BoundarySegment> boundarySegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions())[0].ToList();
            foreach (BoundarySegment bs in boundarySegments) {
                boundaryLines.Add(bs.GetCurve());
            }
            return boundaryLines;
        
        
        }

        public static Curve OffsetCurve(Room room, Curve curve) {
            XYZ curveEnd = curve.GetEndPoint(1);
            double x1 = curveEnd.X;
            double y1 = curveEnd.Y;
            XYZ curveStart = curve.GetEndPoint(0);
            double x2 = curveStart.X;
            double y2 = curveStart.Y;
            LocationPoint location = room.Location as LocationPoint;
            XYZ point = location.Point;
            double x3 = point.X;
            double y3 = point.Y;
            double x = (x1 * x1 * x3 - 2 * x1 * x2 * x3 + x2 * x2 * x3 + x2 *
            (y1 - y2) * (y1 - y3) - x1 * (y1 - y2) * (y2 - y3)) / ((x1 - x2) *
                    (x1 - x2) + (y1 - y2) * (y1 - y2));
            double y = (x2 * x2 * y1 + x1 * x1 * y2 + x2 * x3 * (y2 - y1) - x1 *
            (x3 * (y2 - y1) + x2 * (y1 + y2)) + (y1 - y2) * (y1 - y2) * y3) / ((
                        x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            XYZ normalPoint = new XYZ(x, y, 0);
            XYZ vector = new XYZ();
            vector = (point - normalPoint);
            vector.Normalize();
            Line line = Line.CreateBound(point, normalPoint);
            
                


            Curve newCurve = curve.CreateOffset(15/304.8, vector);

            return newCurve;



        }
        public static WallType DuplicateWallType(WallType wallType, Document doc)
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
                    catch { }


                    layerIndex++;
                }
            }

            newWallType.SetCompoundStructure(cs);

            return newWallType;
        }




    }
}
