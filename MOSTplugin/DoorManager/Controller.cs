using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MOSTplugin.SkittingWall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Schema;
using View = Autodesk.Revit.DB.View;

namespace MOSTplugin.DoorManager
{
    public abstract class ModelData
    {

        protected virtual BuiltInCategory Innercategory { get; } = BuiltInCategory.INVALID;
        public List<Model> TableData
        {
            get { return GetModelData(); }

        }
        protected virtual string Type_ParameterName { get; } = "Тип";
        protected virtual string Mark_ParameterName { get; } = "ADSK_Марка";


        protected List<Model> GetModelData()
        {
            List<Model> ModelList = new List<Model>();
            List<Element> DoorsCollector = (from door in (new FilteredElementCollector(Data.Doc).WhereElementIsNotElementType().OfCategory(Innercategory))
                                            let door_inst = door as FamilyInstance
                                            where door_inst?.SuperComponent == null
                                            let KSIparam = Data.Doc.GetElement(door.GetTypeId())?.LookupParameter("КСИ_Код класса#XNKC0001")
                                            where KSIparam != null
                                            where KSIparam.AsString().StartsWith("ACDB")
                                            select door).ToList();

            var groupByElementType = from Doors in DoorsCollector
                                     group Doors by Doors.LookupParameter(Type_ParameterName)?.AsValueString();
            foreach (var DoorGroup in groupByElementType)
            {
                Model model = new Model();
                model.TypeName = DoorGroup.First().LookupParameter(Type_ParameterName)?.AsValueString();
                model.TypeId = DoorGroup.First().GetTypeId();
                model.category = DoorGroup.First().Category;
                string mark = "";
                foreach (Element el in DoorGroup)
                {
                    model.ElementList.Add(el);
                    
                    
                }
                Element TypeEl = Data.Doc.GetElement(DoorGroup.First().GetTypeId());
                mark = TypeEl.LookupParameter(Mark_ParameterName)?.AsString();
                model.Mark = mark;
                RevitLegend.SetLegendId(model);
                ModelList.Add(model);

            }


            return ModelList;
        } // Метод сбора и группировки элементов из Revit для представления в dgv. Этот метод адаптирован для сбора окон и дверей. 
    } // Абстрактный метод, в котором описана логика сбора элементов для представления в таблице dgv
    public class ModelDataDoors : ModelData
    {
        protected override BuiltInCategory Innercategory { get; } = BuiltInCategory.OST_Doors;
    } //класс для сбора дверей.
    public class ModelDataWindows : ModelData
    {
        protected override BuiltInCategory Innercategory { get; } = BuiltInCategory.OST_Windows;

    } // класс для сбора окон

    public static class RevitLegend
    {

        public static void SetLegendId(Model model)
        {
            ElementId LegendId;
            if (LegendExists(model, out LegendId))
            {
                model.LegendId = LegendId;

            }

        }

        public static bool LegendExists(Model model)
        {

            List<View> ViewList = (from element in (new FilteredElementCollector(Data.Doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Views))
                                   let view = element as View
                                   where view.Name == model.TypeName && view.ViewType == ViewType.Legend
                                   select view).ToList();
            if (ViewList.Count() != 0)
                return true;
            else return false;


        } // метод проверяет существует ли Легенда в проекте (проверка идет по названию легенды и названию типа элемента)
        public static bool LegendExists(Model model, out ElementId LegendId)
        {

            List<View> ViewList = (from element in (new FilteredElementCollector(Data.Doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Views))
                                   let view = element as View
                                   where view.Name == model.TypeName && view.ViewType == ViewType.Legend
                                   select view).ToList();
            if (ViewList.Count() != 0)
            {
                LegendId = ((Element)ViewList[0]).Id;
                return true;
            }

            else
            {
                LegendId = null;
                return false;
            }

        } // метод проверяет существует ли Легенда в проекте (проверка идет по названию легенды и названию типа элемента)
        public static void CreateLegend(Model model)
        {  // метод создает новую легенду в Revit 
            if (!LegendExists(model))
            {
                View BaseView = (from element in (new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType())
                                 let view = element as View
                                 where view.ViewType == ViewType.Legend && view.Name == "Базовая легенда"
                                 select view)?.First();
                ElementId newId = BaseView.Duplicate(ViewDuplicateOption.WithDetailing);
                Data.Doc.GetElement(newId).Name = model.TypeName;

                ReplaceComponent(newId, model);
                model.LegendId = newId;

            }
            else
            {
                ///...
            }

        }
        private static void ReplaceComponent(ElementId LegendId, Model model)
        {
            View view = Data.Doc.GetElement(LegendId) as View;
            if (view != null)
            {
                Element LegendComponent = new FilteredElementCollector(Data.Doc, LegendId).OfCategory(BuiltInCategory.OST_LegendComponents).First();
                Element element_TypeId = Data.Doc.GetElement(model.TypeId);
                ElementType elementtype = element_TypeId as ElementType;
                //string compType = model.category.Name.ToString() + " : " + elementtype.FamilyName + " : " + model.TypeName;
                var parameter = LegendComponent.get_Parameter(BuiltInParameter.LEGEND_COMPONENT);
                parameter.Set(model.TypeId);
                //LegendComponent.LookupParameter("Тип компонента").Set("Двери: 350 - 21 - 11_Дверь ГОСТ 475 - 2016 c 3D - текстом_r19_ - s(П + Л) : 350 - 21 - 10_Дверь_2000х900_правое");
                //LegendComponent.LookupParameter("Тип компонента")?.Set(model.TypeId);

            }



        }
        public static bool CorrectComponentOnView(ElementId LegendId, string TypeName)
        {
            Element LegendComponent = new FilteredElementCollector(Data.Doc, LegendId).OfCategory(BuiltInCategory.OST_LegendComponents).First();
            if (LegendComponent.GetType().Name.ToString() == TypeName)
                return true;
            else return false;
        }// метод для проверки совпадает ли название легенды с названием типоразмера компонента, расположенного на этом  легенды

    }


    public class ModelChecker
    {
        private List<Model> models = new List<Model>();


        private List<Model> models_DefferentMarks = new List<Model>();
        private List<Model> models_SimilarMarks = new List<Model>();

        public ModelChecker(List<Model> models)
        {
            this.models = models;
            CheckStart();

        }
        private void CheckStart()
        {
            foreach (Model model in models)
            {
                if (HasDifferentMarks(model))
                {
                    models_DefferentMarks.Add(model);
                }
                HasDublicatedMarks();
            }
        }
        private bool HasDifferentMarks(Model model)
        {
            if (model.Mark.Contains("/")) return true;
            else return false;
        }
        private void HasDublicatedMarks()
        {
            var groupByMark = from model in models
                              group model by model.Mark into newGroup
                              select newGroup;
            foreach (var newGroup in groupByMark)
            {
                if (newGroup.Count() != 1)
                {
                    foreach (Model m in newGroup)
                        models_SimilarMarks.Add(m);
                }
            }


        }

    }

    public static class AutoDimension
    {

        public static void CreateDimension(Model model)
        {
            //--------------------------------------------------------------
            //!!!!!!!!!!!!!!!Промежуточные вычисления - НЕ ТРОГАТЬ. НИ ССЫЛАТЬСЯ, НЕ МЕНЯТЬ, НЕ ИСПОЛЬЗОВАТЬ КАК ПЕРЕМЕННТЫЕ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Element DimensionObject = GetLegendElementFromView(model);
            List<PlanarFace> planarFaces = GetElementFaces(DimensionObject);

            List<PlanarFace> HorizontalFaces = GetHorizontalFaces(planarFaces);
            List<PlanarFace> VerticalFaces = GetVerticalFaces(planarFaces);
            var groupedVericalFaces = from face in GetVerticalFaces(planarFaces)
                                      group face by face.Origin.X
                                   into NewGroup
                                      orderby NewGroup.Key
                                      select NewGroup;
            var groupedHorizontalFaces = from face in GetHorizontalFaces(planarFaces)
                                      group face by face.Origin.Y
                                   into NewGroup
                                      orderby NewGroup.Key
                                      select NewGroup;

            //--------------------------------------------------------------
            //ПЕРМЕННЫЕ, КОТОРЫЕ МОЖНО ИСПОЛЬЗОВАТЬ.




            List<PlanarFace> sortedVerticalFaces = new List<PlanarFace>(); /// СПИСОК ВЕРТИКАЛЬНЫХ ФЕЙСОВ БЕЗ ДУБЛИРОВАНИЯ, И Т.П СКОЛЬКО В МОДЕЛИ ВИДНО ГРАНЕЙ
                                                                           /// 
            for (int i = 0; i < groupedVericalFaces.Count(); i++)
            {
                if (sortedVerticalFaces.Count != 0)
                {
                    XYZ lastOriginInSortedList = sortedVerticalFaces.Last().Origin;
                    XYZ CurrentOriginInSortedList = (from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.Y select face.Origin).ToList().First();
                    double diff = lastOriginInSortedList.X - CurrentOriginInSortedList.X;
                    if (diff > 0.000001 || -diff > 0.000001)
                    {

                        sortedVerticalFaces.Add((from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());


                    }
                }
                else
                    sortedVerticalFaces.Add((from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());
            }

            List<PlanarFace> sortedHorizontalFaces = new List<PlanarFace>();
            for (int i = 0; i < groupedVericalFaces.Count(); i++)
            {
                if (sortedHorizontalFaces.Count != 0)
                {
                    XYZ lastOriginInSortedList = sortedHorizontalFaces.Last().Origin;
                    XYZ CurrentOriginInSortedList = (from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.X select face.Origin).ToList().First();
                    double diff = lastOriginInSortedList.Y - CurrentOriginInSortedList.Y;
                    if (diff > 0.000001 || -diff > 0.000001)
                    {

                        sortedHorizontalFaces.Add((from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());


                    }
                }
                else
                    sortedHorizontalFaces.Add((from face in groupedVericalFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());
            }
            



            //-----------------------------------------------------------------------------------------------------
            if (Data.Doc.GetElement(model.ElementList.First().GetTypeId()).LookupParameter("М_Порог_Есть")?.AsInteger() == 1)
            {
                PutDimension("horizontal", new List<int>() { 0, 1,2}, model, "right", 240);
                PutDimension("horizontal", new List<int>() { 0, 4 }, model, "right", 500);

            }
            else {
                PutDimension("horizontal", new List<int>() { 0, 1 }, model, "right", 240);
                PutDimension("horizontal", new List<int>() { 0, 3 }, model, "right", 500);
            }
            //Размеры полотная. Если есть порог, то ставим размер на порог и полотно
            //-----------------------------------------------------------------------------------------------------
            
            //Ставим общий размер двери (от нижней грани до верхней)
            //-----------------------------------------------------------------------------------------------------
            

            if (sortedVerticalFaces.Count() % 2 == 0)
            {
                PutDimension("vertical", new List<int>() { 2, 3 }, model, "bottom", 240);
                PutDimension("vertical", new List<int>() { 0, 5 }, model, "bottom", 500);
            }
            else {
                PutDimension("vertical", new List<int>() { 2, 3, 4 }, model, "bottom", 240);
                PutDimension("vertical", new List<int>() { 0, 6 }, model, "bottom", 500);
            }


            



        }
        private static void PutDimension(string orientation,List<int> indexes, Model model, string side, double offset) {
            Element DimensionObject = GetLegendElementFromView(model);
            List<PlanarFace> planarFaces = GetElementFaces(DimensionObject);
            List<PlanarFace> sortedFaces = new List<PlanarFace>();
            XYZ vectorDirection = new XYZ();
            XYZ p1 = new XYZ();
            XYZ p2 = new XYZ();

            Line line = null;


            if (orientation == "horizontal") // если хотим проставить размеры горизонтальных референсов
            {
                var groupedFaces = from face in GetHorizontalFaces(planarFaces)
                                   group face by face.Origin.Y
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;
                // сборщик горизонтальных плоскостей


                for (int i = 0; i < groupedFaces.Count(); i++)
                {
                    if (sortedFaces.Count != 0)
                    {
                        XYZ lastOriginInSortedList = sortedFaces.Last().Origin;
                        XYZ CurrentOriginInSortedList = new XYZ();
                        if (side == "right") // берем левые крайние референсы, если на одном уровне находятся два и боле референсов
                        {
                            CurrentOriginInSortedList = (from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face.Origin).ToList().Last();

                        }
                        else if (side == "left") // берем правые крайние референсы, если на одном уровне находятся два и боле референсов
                        {
                            CurrentOriginInSortedList = (from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face.Origin).ToList().First();
                        }
                        XYZ v = lastOriginInSortedList - lastOriginInSortedList;
                        double dist = lastOriginInSortedList.DistanceTo(CurrentOriginInSortedList);
                        if (dist > 0.001)
                        {
                            if (side == "right")
                            {
                                sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().Last());
                            }
                            else if (side == "left")
                            {
                                sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());
                            }

                        }
                    }
                    else
                    {
                        if (side == "right")
                        {
                            sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().Last());
                        }
                        else if (side == "left")
                        {
                            sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());
                        }
                    }


                }
                
                if (side == "right") // если хотим проставить размеры справа
                {
                    PlanarFace Right_face = (from face in GetVerticalFaces(planarFaces)
                                             orderby face.Origin.X
                                             select face).Last();

                    
                    XYZ element_location = new XYZ();
                    GeometryElement geometry = DimensionObject.get_Geometry(new Options());
                    foreach (GeometryObject geomObj in geometry)
                    {
                        if (geomObj is GeometryInstance)
                        {
                            GeometryInstance instance = geomObj as GeometryInstance;
                            element_location = instance.Transform.Origin;
                            break;
                        }
                    }

                    p1 = new XYZ(element_location.X + Math.Abs(Right_face.Origin.X) + offset/304.8, element_location.Y, element_location.Z);
                    
                    vectorDirection = new XYZ(0, 1, 0);
                    p2 = p1.Add(vectorDirection);

                    line = Line.CreateBound(p1, p2);
                    
                }
                else if (side == "left") // если хотим проставить размеры слева
                {
                    PlanarFace left_face = (from face in GetVerticalFaces(planarFaces)
                                             orderby face.Origin.X
                                             select face).First();


                    XYZ element_location = new XYZ();
                    GeometryElement geometry = DimensionObject.get_Geometry(new Options());
                    foreach (GeometryObject geomObj in geometry)
                    {
                        if (geomObj is GeometryInstance)
                        {
                            GeometryInstance instance = geomObj as GeometryInstance;
                            element_location = instance.Transform.Origin;
                            break;
                        }
                    }

                    p1 = new XYZ(element_location.X - Math.Abs(left_face.Origin.X) - offset / 304.8, element_location.Y, element_location.Z);

                    vectorDirection = new XYZ(0, 1, 0);
                    p2 = p1.Add(vectorDirection);

                    line = Line.CreateBound(p1, p2);
                }
            }
            if (orientation == "vertical") // vertical 
            {
                var groupedFaces = from face in GetVerticalFaces(planarFaces)
                                   group face by face.Origin.X
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;

                for (int i = 0; i < groupedFaces.Count(); i++)
                {
                    if (sortedFaces.Count != 0)
                    {
                        XYZ lastOriginInSortedList = sortedFaces.Last().Origin;
                        XYZ CurrentOriginInSortedList = (from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face.Origin).ToList().First();
                        double diff = lastOriginInSortedList.X - CurrentOriginInSortedList.X;
                        if (diff > 0.000001 || -diff > 0.000001)
                        {
                            if (side == "bottom")
                                sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());
                            if (side == "top")
                                sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().Last());
                        }
                    }
                    else
                        sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());
                }
                //sortedFaces.Add((from face in groupedFaces.ElementAt(0) orderby face.Origin.Y select face).ToList().First());
                //sortedFaces.Add((from face in groupedFaces.Last() orderby face.Origin.Y select face).ToList().First());
                if (side == "top") // если хотим проставить размеры справа
                {
                    PlanarFace top_face = (from face in GetHorizontalFaces(planarFaces)
                                             orderby face.Origin.Y
                                             select face).Last();
                    XYZ element_location = new XYZ();
                    GeometryElement geometry = DimensionObject.get_Geometry(new Options());
                    foreach (GeometryObject geomObj in geometry)
                    {
                        if (geomObj is GeometryInstance)
                        {
                            GeometryInstance instance = geomObj as GeometryInstance;
                            element_location = instance.Transform.Origin;
                            break;
                        }
                    }
                    p1 = new XYZ(element_location.X, element_location.Y + Math.Abs(top_face.Origin.Y) + offset / 304.8, element_location.Z);
                    vectorDirection = new XYZ(1, 0, 0);
                    p2 = p1.Add(vectorDirection);
                    line = Line.CreateBound(p1, p2);
                }
                else if (side == "bottom") // если хотим проставить размеры справа
                {
                    PlanarFace bottom_face = (from face in GetHorizontalFaces(planarFaces)
                                             orderby face.Origin.Y
                                             select face).First();


                    XYZ element_location = new XYZ();
                    Options opt = new Options();
                    opt.IncludeNonVisibleObjects = false;
                    GeometryElement geometry = DimensionObject.get_Geometry(opt);
                    foreach (GeometryObject geomObj in geometry)
                    {
                        if (geomObj is GeometryInstance)
                        {
                            GeometryInstance instance = geomObj as GeometryInstance;
                            element_location = instance.Transform.Origin;
                            break;
                        }
                    }
                    p1 = new XYZ(element_location.X, element_location.Y + bottom_face.Origin.Y - offset / 304.8, element_location.Z);
                    vectorDirection = new XYZ(1, 0, 0);
                    p2 = p1.Add(vectorDirection);
                    line = Line.CreateBound(p1, p2);

                }
            }
            List<Reference> references = new List<Reference>();
            foreach (int index in indexes)
                try { references.Add(sortedFaces[index].Reference); }
                catch { }
            ReferenceArray refArray = new ReferenceArray();
            foreach (Reference reference in references)
                refArray.Append(reference);
                    
            Dimension dim = Data.Doc.Create.NewDimension(model.LegendView, line, refArray);


        }
        private static Element GetLegendElementFromView(Model model)
        {
            Element element = new FilteredElementCollector(Data.Doc, model.LegendId).OfCategory(BuiltInCategory.OST_LegendComponents)
                .WhereElementIsNotElementType().ToElements().ToList().First();
            return element;

        }
        private static List<PlanarFace> GetElementFaces(Element element)
        {
            Options opt = new Options();
            opt.ComputeReferences = true;
            GeometryElement geometry = element.get_Geometry(opt);
            List<Solid> solids = new List<Solid>();
            foreach (GeometryObject geomObj in geometry)
            {
                if (geomObj is GeometryInstance)
                {
                    GeometryInstance instance = geomObj as GeometryInstance;

                    // Access the SymbolGeometry property to get the actual geometry
                    GeometryElement instanceGeometry = instance.SymbolGeometry;

                    // Iterate through the instance geometry
                    foreach (GeometryObject instanceGeomObj in instanceGeometry)
                    {
                        if (instanceGeomObj is Solid)
                            solids.Add(instanceGeomObj as Solid);
                        // Process or use the geometry as needed
                        // Example: Access instanceGeomObj as a Solid, Mesh, etc.
                    }
                }
            }
            List<PlanarFace> faces = new List<PlanarFace>();
            foreach (Solid solid in solids)
            {
                if (solid.Faces.IsEmpty != true)
                    foreach (PlanarFace face in solid.Faces)
                        faces.Add(face);
            }
            return faces;
        }
        private static List<PlanarFace> GetVerticalFaces(List<PlanarFace> faces)
        {
            List<PlanarFace> verticalFaces = new List<PlanarFace>();
            foreach (PlanarFace face in faces)
            {
                if (face.FaceNormal.X == new XYZ(1, 0, 0).X || face.FaceNormal.X == new XYZ(-1, 0, 0).X)
                    verticalFaces.Add(face);

            }
            return verticalFaces;

        }
        private static List<PlanarFace> GetHorizontalFaces(List<PlanarFace> faces)
        {
            List<PlanarFace> horizontalFaces = new List<PlanarFace>();
            foreach (PlanarFace face in faces)
            {
                if (face.FaceNormal.Y == new XYZ(0, 1, 0).Y || face.FaceNormal.Y == new XYZ(0, -1, 0).Y)
                    horizontalFaces.Add(face);

            }
            return horizontalFaces;

        }
        private static void CreateOutDimension(int x, List<PlanarFace> faces, View view)
        { //Метод, который создает внешние размеры. x - Принимает значение 1 или 2
          // 1 - для создания размеров по вертикали (вдоль оси y)
          // 2 - для создания размеров по горизонтали (вдоль оси х)
          // faces - при x=1 неободимо подать список 

            List<PlanarFace> sortedFaces = new List<PlanarFace>();
            XYZ vectorDirection = new XYZ();
            XYZ p1 = new XYZ();
            if (x == 1) // horizontal
            {
                var groupedFaces = from face in faces
                                   group face by face.Origin.Y
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;

                sortedFaces.Add((from face in groupedFaces.ElementAt(0) orderby face.Origin.X select face).ToList().Last());
                sortedFaces.Add((from face in groupedFaces.Last() orderby face.Origin.X select face).ToList().Last());

                p1 = new XYZ(9.5, 13, 0);
                vectorDirection = new XYZ(0, 1, 0);
            }
            if (x == 2) // vertical 
            {
                var groupedFaces = from face in faces
                                   group face by face.Origin.X
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;

                sortedFaces.Add((from face in groupedFaces.ElementAt(0) orderby face.Origin.Y select face).ToList().First());
                sortedFaces.Add((from face in groupedFaces.Last() orderby face.Origin.Y select face).ToList().First());
                p1 = new XYZ(13, 4.8, 0);
                vectorDirection = new XYZ(1, 0, 0);
            }
            List<Reference> references = new List<Reference>();
            foreach (PlanarFace face in sortedFaces)
                references.Add(face.Reference);

            ReferenceArray refArray = new ReferenceArray();
            refArray.Append(references.First());
            refArray.Append(references.Last());
            //foreach (PlanarFace face in sortedFaces) {
            //    refArray.Append(face.Reference);
            //}


            XYZ p2 = p1.Add(vectorDirection);

            Line line = Line.CreateBound(p1, p2);
            //refArray.Append(references[0]);
            //refArray.Append(references.Last());
            Dimension dim = Data.Doc.Create.NewDimension(view, line, refArray);



        }
        private static void CreateInnerDimension(int x, List<PlanarFace> faces, View view)
        {
            List<PlanarFace> sortedFaces = new List<PlanarFace>();
            XYZ vectorDirection = new XYZ();
            XYZ p1 = new XYZ();
            if (x == 1) // horizontal
            {
                var groupedFaces = from face in faces
                                   group face by face.Origin.Y
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;


                for (int i = 1; i < groupedFaces.Count() - 1; i++)
                {
                    if (sortedFaces.Count != 0)
                    {
                        XYZ lastOriginInSortedList = sortedFaces.Last().Origin;
                        XYZ CurrentOriginInSortedList = (from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face.Origin).ToList().First();
                        XYZ v = lastOriginInSortedList - lastOriginInSortedList;
                        double dist = lastOriginInSortedList.DistanceTo(CurrentOriginInSortedList);
                        if (dist > 0.001)
                        {
                            sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());
                        }
                    }
                    else
                        sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.X select face).ToList().First());
                }
                //sortedFaces.Add((from face in groupedFaces.ElementAt(0) orderby face.Origin.X select face).ToList().First());
                //sortedFaces.Add((from face in groupedFaces.Last() orderby face.Origin.X select face).ToList().First());

                p1 = new XYZ(4.5, 2, 0);
                vectorDirection = new XYZ(0, 5, 0);
            }
            if (x == 2) // vertical 
            {
                var groupedFaces = from face in faces
                                   group face by face.Origin.X
                                   into NewGroup
                                   orderby NewGroup.Key
                                   select NewGroup;

                for (int i = 1; i < groupedFaces.Count() - 1; i++)
                {
                    if (sortedFaces.Count != 0)
                    {
                        XYZ lastOriginInSortedList = sortedFaces.Last().Origin;
                        XYZ CurrentOriginInSortedList = (from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face.Origin).ToList().First();
                        double diff = lastOriginInSortedList.X - CurrentOriginInSortedList.X;
                        if (diff > 0.000001 || -diff > 0.000001)
                        {
                            sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());
                        }
                    }
                    else
                        sortedFaces.Add((from face in groupedFaces.ElementAt(i) orderby face.Origin.Y select face).ToList().First());
                }
                //sortedFaces.Add((from face in groupedFaces.ElementAt(0) orderby face.Origin.Y select face).ToList().First());
                //sortedFaces.Add((from face in groupedFaces.Last() orderby face.Origin.Y select face).ToList().First());
                p1 = new XYZ(2, 13, 0);
                vectorDirection = new XYZ(5, 0, 0);
            }
            List<Reference> references = new List<Reference>();
            foreach (PlanarFace face in sortedFaces)
                references.Add(face.Reference);

            ReferenceArray refArray = new ReferenceArray();
            foreach (Reference reference in references)
                refArray.Append(reference);

            //foreach (PlanarFace face in sortedFaces) {
            //    refArray.Append(face.Reference);
            //}


            XYZ p2 = p1.Add(vectorDirection);

            Line line = Line.CreateBound(p1, p2);
            //refArray.Append(references[0]);
            //refArray.Append(references.Last());
            Dimension dim = Data.Doc.Create.NewDimension(view, line, refArray);




        }
    }


    class RevitImage
    {
        static public void CreateViewImage(Model model)
        {
            Transaction t = new Transaction(Data.Doc, "go");
            if (model?.LegendView != null)
            {
                Data.UIdoc.ActiveView = model.LegendView;
                t.Start();
                List<Element> ViewList = (from view in new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views)
                                          let v = view as View
                                          where v.ViewType.ToString() == "Rendering"
                                          select view).ToList();

                foreach (Element elem in ViewList)
                {
                    if (elem.Name == model.LegendView.Name)
                    {
                        Data.Doc.Delete(elem.Id);
                        break;
                    }

                }

                ImageExportOptions options = new ImageExportOptions()
                {
                    ViewName = model.LegendView.Name,


                    HLRandWFViewsFileType = ImageFileType.JPEGLossless,
                    ImageResolution = ImageResolution.DPI_600,
                    ZoomType = ZoomFitType.Zoom,

                    Zoom = 100




                };

                ElementId ImageId = Data.Doc.SaveToProjectAsImage(options);

                ElementId RasterImageId = (from ImageType in new FilteredElementCollector(Data.Doc).OfClass(typeof(ImageType))
                                           let name = ImageType.Name
                                           where name == model.LegendView.Name
                                           select ImageType.Id).First();


                model.Image = Data.Doc.GetElement(ImageId) as ImageView;
                try
                {
                    Element el = model.ElementList.First();
                    Element element = Data.Doc.GetElement(el.GetTypeId());
                    element.LookupParameter("М_Изображение")?.Set(RasterImageId);
                }
                catch { }
                t.Commit();
                UIView ViewToClose = (from UIView in Data.UIdoc.GetOpenUIViews()
                                      where UIView.ViewId == Data.Doc.ActiveView.Id
                                      select UIView).First();
                ViewToClose.Close();

            }


        }

    }
}


