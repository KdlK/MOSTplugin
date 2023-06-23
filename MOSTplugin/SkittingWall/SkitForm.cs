using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a = System.Windows.Forms;

namespace MOSTplugin.SkittingWall
{
    public partial class SkitForm : a.Form
    {
        private RevitTask revitTask;
        public SkitForm()
        {
            revitTask = new RevitTask();
            InitializeComponent();
            this.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                List<Room> rooms = SkittingWall_Class.roomCollector();
                List<Element> walls = SkittingWall_Class.SkittingWallCollector();
                Transaction t = new Transaction(Data.Doc, "Go");
                t.Start();
                
                
                Element wall= new FilteredElementCollector(Data.Doc).OfClass(typeof(WallType)).First();



                IEnumerable<WallType> _wallTypes = from elem in new FilteredElementCollector(Data.Doc).OfClass(typeof(WallType))
                                                   let type = elem as WallType
                                                   where type.Kind == WallKind.Basic 
                                                   where type.Id.IntegerValue == 939395
                                                   select type;
                //WallType WallType = wall.WallType;
                WallType WallType = _wallTypes.First();
                SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
                WallType duplicatedWallType = SkittingWall_Class.DuplicateWallType(WallType, Data.Doc);
                List<ElementId> SkittinWallsList = new List<ElementId>();
                foreach (Room room in rooms)
                {
                    List<Curve> curves = SkittingWall_Class.GetRoomLines(room);
                    List<BoundarySegment> bs = room.GetBoundarySegments(opt)[0].ToList();

                    foreach (BoundarySegment b in bs)
                    {

                        //Curve curve = SkittingWall_Class.OffsetCurve(room, b.GetCurve());
                        Curve curve = b.GetCurve();
                        Wall baseWall = Data.Doc.GetElement(b.ElementId) as Wall;
                        
                        Wall skirtingWall = Wall.Create(Data.Doc, curve, duplicatedWallType.Id, room.LevelId, 2000/304.8, 15/ 304.8, false, false);
                        Parameter wallJustification = skirtingWall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
                        wallJustification.Set(2);
                        if(baseWall != null)
                        {
                            //Parameter wallJustification = skirtingWall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
                            wallJustification.Set(3);
                            JoinGeometryUtils.JoinGeometry(Data.Doc, skirtingWall, baseWall);
                        }
                        SkittinWallsList.Add(skirtingWall.Id);




                    }

                }
                Element.ChangeTypeId(Data.Doc, SkittinWallsList, WallType.Id);
                Data.Doc.Delete(duplicatedWallType.Id);
                t.Commit();
                //тест
            });
        }
    }
}
