using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MOSTplugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;
using MOSTplugin.SkittingWall;
using MOSTplugin.LintelBeam;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;
using Form = System.Windows.Forms.Form;

namespace MOSTplugin
{

    [Transaction(TransactionMode.Manual)]
    public static class Data { 
    
        private static ExternalCommandData _commandData = null;
        private static UIDocument _docUI = null;
        private static UIApplication _appUI = null;
        private static Application _app = null;
        private static Document _doc = null;
        private static string _message = "";
        public static ExternalCommandData CommandData {
            get { 
                return _commandData;
            }
            set {
                _commandData = value;
                _appUI  = _commandData.Application;
                _docUI = _appUI.ActiveUIDocument;
                _app = _appUI.Application;
                _doc = _docUI.Document;
                
            }
            

        }
        public static string Message { get { return _message; } set { _message = value; } }
        public static UIDocument UIdoc { get { return _docUI; } }
        public static UIApplication UIapp { get { return _appUI; } }
        public static Application App { get { return _app; } }
        public static Document Doc { get { return _doc; } }
        public static string message { get { return _message; } }

    }
    [Transaction(TransactionMode.Manual)]
    public class Commands : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Rename_views form = new Rename_views(doc,uidoc);
            

            
            return Result.Succeeded;
        }

    }
    [Transaction(TransactionMode.Manual)]
    public class Auto_numbering : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Auto_renambering_lists form = new Auto_renambering_lists(doc,uidoc);

            return Result.Succeeded;
        }
        
    }
    [Transaction(TransactionMode.Manual)]
    public class Peremichki_command : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Data.CommandData = commandData;
            Form Form = new LintelBeam.LintelBeam();
            //Peremichki form = new Peremichki();
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class SkittingWall_command : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            Data.CommandData = commandData;
            Data.Message = message;
            SkittingWall.SkittinWallForm form = new SkittingWall.SkittinWallForm();
            
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class check : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            Data.CommandData = commandData;
            Transaction t = new Transaction(Data.Doc, "Результат проверки");
            t.Start();
            List<Element> elems = new List<Element>();
            List<Element> linkedElems = new List<Element>();
            
            foreach (Document d in Data.App.Documents)
            {
                if (d.IsLinked)
                {
                    linkedElems = new FilteredElementCollector(d).WhereElementIsNotElementType().ToElements().ToList();

                }
            }
            elems = ElementCollector(Data.Doc);
            if (elems.Count() != 0)
            {
                foreach (Element El in elems)
                {
                    string otchet = "";
                    List<Element> linkedELs = (from linkedElem in linkedElems where linkedElem.Id.IntegerValue == El.Id.IntegerValue select linkedElem).ToList();
                    if (linkedELs.Count == 0)
                        otchet = "NEW";
                    else
                    {
                        Element linkedEl = linkedELs.First();

                        if (El.LookupParameter("Объем") != null)
                        {
                            if (!CheckElementVolume(El, linkedEl))
                            {
                                otchet = String.Join("_", otchet, "V");
                            }
                        }
                        if (!CheckLocation(El, linkedEl))
                        {

                            otchet = String.Join("_", otchet, "L");
                        }
                        if (!CheckBB(El, linkedEl))
                        {
                            otchet = String.Join("_", otchet, "B");
                        }

                    }
                    if (otchet != "" && otchet != "NEW")
                        otchet = otchet.Substring(1, otchet.Count() - 1);
                    if (El.LookupParameter("М_СравнениеВерсий") != null)
                    {
                        El.LookupParameter("М_СравнениеВерсий").Set(otchet);
                        
                    }


                }
            }
            else
                MessageBox.Show("Параметр М_СравнениеВерсий не загружен в проект или не выбраны категории");
            t.Commit();
            return Result.Succeeded;
        }
        public List<Element> ElementCollector(Document doc) {
            
            List<Element> ElementList = new List<Element>();
            Categories categories = Data.Doc.Settings.Categories;
            foreach (Category cat in categories) { 
                List<Element> elements = new List<Element>();
                elements = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategoryId(cat.Id).ToElements().ToList();
                if (elements.Count() != 0) {
                    if (elements.First().LookupParameter("М_СравнениеВерсий") != null)
                    {
                        ElementList.AddRange(elements);
                    }
                    else
                        continue;


                }
                else
                    continue;
            
            }

            return ElementList;

        
        }
        public List<Element> ElCollector(Document doc)
        {
            
            List<Category> CategoryList = new List<Category>();
            List<Element> ElementList = new List<Element>();
            BindingMap maps = doc.ParameterBindings;

            DefinitionBindingMapIterator iterator = maps.ForwardIterator();
            ///iterator.Reset();
            while (iterator.MoveNext()) { 
                InternalDefinition def = iterator.Key as InternalDefinition;
                if (def.Name.Equals("М_СравнениеВерсий")) {
                    CategorySet cat = (iterator.Current as ElementBinding).Categories;
                    CategorySetIterator cat_iter = cat.ForwardIterator();
                    cat_iter.Reset();
                    while (cat_iter.MoveNext()) {
                        CategoryList.Add(cat_iter.Current as Category);
                    
                    }

                    break;
                }
            
            }
            if (CategoryList.Count() != 0)
            {
                foreach (Category c in CategoryList)
                {
                    ElementList.AddRange(new FilteredElementCollector(doc).OfCategoryId(c.Id).WhereElementIsNotElementType().ToElements().ToList());
                    
                }

                return ElementList;
            }
            else {
                MessageBox.Show("параметр М_СравнениеВерсий не загружен в проект или не выбраны категории");
                return null;
            }
           
     
            


       
        }
        public bool CheckElementVolume(Element El1, Element El2) {
            string El1_volume = El1.LookupParameter("Объем").AsValueString();
            string El2_volume = El2.LookupParameter("Объем").AsValueString();
            
            float El1_volume_digit = float.Parse(El1_volume.Split(' ').First().Replace(',','.'),CultureInfo.InvariantCulture.NumberFormat);
            float El2_volume_digit = float.Parse(El2_volume.Split(' ').First().Replace(',','.'),CultureInfo.InvariantCulture.NumberFormat);
            if (Math.Abs(El1_volume_digit - El2_volume_digit) < 0.001)
                return true;
            else return false;
        }
        public bool CheckLocation(Element el1,Element  el2) {
            //if (Category.GetCategory(Data.Doc, el1.Id).Id.IntegerValue == -201330)
            if ((el1.Location as LocationPoint)?.Point != null)
            {
                XYZ el1_Point = (el1.Location as LocationPoint).Point;
                XYZ el2_Point = (el2.Location as LocationPoint).Point;
                if (Math.Abs(el1_Point.X - el2_Point.X) < 0.1 && Math.Abs(el1_Point.Y - el2_Point.Y) < 0.1 && (el1_Point.Z - el2_Point.Z) < 0.1)
                    return true;
                else
                    return false;

            }
            else if ((el1.Location as LocationCurve) != null)
            {
                XYZ el1_Point = ((el1.Location as LocationCurve).Curve as Line).Origin;
                XYZ el2_Point = ((el2.Location as LocationCurve).Curve as Line).Origin;
                if (Math.Abs(el1_Point.X - el2_Point.X)< 0.1 && Math.Abs(el1_Point.Y -el2_Point.Y)< 0.1 && (el1_Point.Z -el2_Point.Z)< 0.1)
                    return true;
                else
                    return false;


            }
            else {
                return true;
            
            }
            
        }
        public bool CheckBB(Element el1, Element el2) {
            BoundingBoxXYZ element1_BB = el1.get_BoundingBox(null);
            BoundingBoxXYZ element2_BB = el2.get_BoundingBox(null);
            if (element1_BB != null && element1_BB != null)
            {
                XYZ element1_BB_Min = element1_BB.Min;
                XYZ element1_BB_Max = element1_BB.Max;

                XYZ element2_BB_Min = element2_BB.Min;
                XYZ element2_BB_Max = element2_BB.Max;

                if (Math.Abs(element1_BB_Min.X - element2_BB_Min.X)< 0.1 && Math.Abs(element1_BB_Min.Y - element2_BB_Min.Y) < 0.1 && Math.Abs(element1_BB_Min.Z - element2_BB_Min.Z) < 0.1
                    && Math.Abs(element1_BB_Max.X - element2_BB_Max.X) < 0.1 && Math.Abs(element1_BB_Max.Y - element2_BB_Max.Y) < 0.1 && Math.Abs(element1_BB_Max.Z - element2_BB_Max.Z) < 0.1)

                {
                    return true;
                }
                else return false;
            }
            else return true;



            
        }
        //public void CreateSampleSharedParameters(Category cat)
        //{
        //    Category category = cat;
        //    CategorySet categorySet = Data.App.Create.NewCategorySet();
        //    categorySet.Insert(category);

        //    string originalFile = Data.App.SharedParametersFilename;
        //    string tempFile = @"I:\Most_OTIM\1_ПГС\1_3_Библиотека_Общая\1_3_1-ФОП_Общие_параметры\FP-MST-S2-ФОП_v1.txt";

        //    try
        //    {
        //        Data.App.SharedParametersFilename = tempFile;

        //        DefinitionFile sharedParameterFile = Data.App.OpenSharedParameterFile();

        //        foreach (DefinitionGroup dg in sharedParameterFile.Groups)
        //        {
        //            if (dg.Name == "009_Мст_BIM")
        //            {
        //                ExternalDefinition externalDefinition = dg.Definitions.get_Item("М_СравнениеВерсий") as ExternalDefinition;

                       
                        
                        
        //                InstanceBinding newIB = Data.App.Create.NewInstanceBinding(categorySet);
        //                Data.Doc.ParameterBindings.Insert(externalDefinition, newIB, BuiltInParameterGroup.PG_TEXT);
                        
                        
                        
        //            }
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
                

               
        //        FilteredElementCollector collector  = new FilteredElementCollector(Data.Doc)
        //                                             .WhereElementIsNotElementType()
        //                                             .OfClass(typeof(InternalDefinition));
        //        BindingMap map = Data.Doc.ParameterBindings;
        //        InternalDefinition internalDefinition = map
        //        foreach (InternalDefinition def in (from a in )) {
                    
                
                
        //        }
        //        foreach (Element e in collector)
        //        {
        //            SharedParameterElement param = e as SharedParameterElement;
        //            InternalDefinition def = param.GetDefinition();
                   

                   
        //        }
        //    }
        //}
    }

   
}
