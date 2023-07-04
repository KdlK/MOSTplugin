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
            LintelBeam.LintelBeam Form = new LintelBeam.LintelBeam();
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
                    linkedElems = ElCollector(d);
                }
            }
            elems = ElCollector(Data.Doc);
            foreach (Element El in elems) {
                string otchet = "";
                List<Element> linkedELs = (from linkedElem in linkedElems where linkedElem.Id.IntegerValue == El.Id.IntegerValue select linkedElem).ToList();
                if (linkedELs.Count == 0)
                    otchet = "NEW";
                else {
                    Element linkedEl = linkedELs.First();
                    
                    if( El.LookupParameter("Объем") != null ) 
                    { 
                        if (!CheckElementVolume(El,linkedEl))
                        {
                            otchet = String.Join("_", otchet, "V");
                        }
                    }
                    if (!CheckLocation(El, linkedEl)) {

                        otchet = String.Join("_", otchet, "L");
                    }
                    if (!CheckBB(El, linkedEl)) {
                        otchet = String.Join("_",otchet, "B");
                    }
                    
                }
                if (otchet != "")
                    otchet = otchet.Substring(1, otchet.Count() - 1);
                if (El.LookupParameter("М_СравнениеВерсий") != null) {
                    El.LookupParameter("М_СравнениеВерсий").Set(otchet);
                    //CreateSampleSharedParameters(El.Category);
                }
                

            }
            t.Commit();
            return Result.Succeeded;
        }
        public List<Element> ElCollector(Document doc)
        {
            List<Element> karkas = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsNotElementType().ToElements().ToList();
            List<Element> columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsNotElementType().ToElements().ToList();
            List<Element> elems = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElements().ToList();
            List<Element> s = new List<Element>();
            s = columns;
            //s = s.Concat(columns).ToList();
            


            return elems;
        }
        public bool CheckElementVolume(Element El1, Element El2) {
            string El1_volume = El1.LookupParameter("Объем").AsValueString();
            string El2_volume = El2.LookupParameter("Объем").AsValueString();
            if (El1_volume == El2_volume)
                return true;
            else return false;
        }
        public bool CheckLocation(Element el1,Element  el2) {
            //if (Category.GetCategory(Data.Doc, el1.Id).Id.IntegerValue == -201330)
            if ((el1.Location as LocationPoint)?.Point != null)
            {
                XYZ el1_Point = (el1.Location as LocationPoint).Point;
                XYZ el2_Point = (el2.Location as LocationPoint).Point;
                if (el1_Point.X == el2_Point.X && el1_Point.Y == el2_Point.Y && el1_Point.Z == el2_Point.Z)
                    return true;
                else
                    return false;

            }
            else if ((el1.Location as LocationCurve) != null)
            {
                XYZ el1_Point = ((el1.Location as LocationCurve).Curve as Line).Origin;
                XYZ el2_Point = ((el2.Location as LocationCurve).Curve as Line).Origin;
                if (el1_Point.X == el2_Point.X && el1_Point.Y == el2_Point.Y && el1_Point.Z == el2_Point.Z)
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
            if (element1_BB != null || element1_BB != null)
            {
                XYZ element1_BB_Min = element1_BB.Min;
                XYZ element1_BB_Max = element1_BB.Max;

                XYZ element2_BB_Min = element2_BB.Min;
                XYZ element2_BB_Max = element2_BB.Max;

                if (element1_BB_Min.X == element2_BB_Min.X && element1_BB_Min.Y == element2_BB_Min.Y && element1_BB_Min.Z == element2_BB_Min.Z
                    && element1_BB_Max.X == element2_BB_Max.X && element1_BB_Max.Y == element2_BB_Max.Y && element1_BB_Max.Z == element2_BB_Max.Z)
                {
                    return true;
                }
                else return false;
            }
            else return true;



            
        }
        public void CreateSampleSharedParameters(Category cat)
        {
            Category category = cat;
            CategorySet categorySet = Data.App.Create.NewCategorySet();
            categorySet.Insert(category);

            string originalFile = Data.App.SharedParametersFilename;
            string tempFile = @"I:\Most_OTIM\1_ПГС\1_3_Библиотека_Общая\1_3_1-ФОП_Общие_параметры\FP-MST-S2-ФОП_v1.txt";

            try
            {
                Data.App.SharedParametersFilename = tempFile;

                DefinitionFile sharedParameterFile = Data.App.OpenSharedParameterFile();

                foreach (DefinitionGroup dg in sharedParameterFile.Groups)
                {
                    if (dg.Name == "009_Мст_BIM")
                    {
                        ExternalDefinition externalDefinition = dg.Definitions.get_Item("М_СравнениеВерсий") as ExternalDefinition;

                       
                        
                        
                        InstanceBinding newIB = Data.App.Create.NewInstanceBinding(categorySet);
                        Data.Doc.ParameterBindings.Insert(externalDefinition, newIB, BuiltInParameterGroup.PG_TEXT);
                        
                        
                        
                    }
                }
            }
            catch { }
            finally
            {
                

               
                FilteredElementCollector collector  = new FilteredElementCollector(Data.Doc)
                                                     .WhereElementIsNotElementType()
                                                     .OfClass(typeof(SharedParameterElement));
                foreach (Element e in collector)
                {
                    SharedParameterElement param = e as SharedParameterElement;
                    InternalDefinition def = param.GetDefinition();
                   

                   
                }
            }
        }
    }

   
}
