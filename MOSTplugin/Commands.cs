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
                    otchet = "новый элемент";
                else {
                    Element linkedEl = linkedELs.First();
                    string El_OB = El.LookupParameter("Объем").AsValueString();
                    string linkedEl_OB = linkedEl.LookupParameter("Объем").AsValueString();
                    if (El_OB == linkedEl_OB && CheckLocation(El, linkedEl))
                    {
                        otchet = "элемент не изменялся";


                    }
                    else
                    {
                        otchet = "элемент изменен";
                        
                    }
                    El.LookupParameter("Комментарии").Set(otchet);

                }


            }
            t.Commit();
            return Result.Succeeded;
        }
        public List<Element> ElCollector(Document doc)
        {
            List<Element> karkas = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsNotElementType().ToElements().ToList();
            List<Element> columns = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsNotElementType().ToElements().ToList();
            List<Element> s = new List<Element>();
            s = s.Union(karkas).ToList();
            s = s.Union(columns).ToList();


            return s;
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
            else {
                XYZ el1_Point = ((el1.Location as LocationCurve).Curve as Line).Origin;
                XYZ el2_Point = ((el2.Location as LocationCurve).Curve as Line).Origin;
                if (el1_Point.X == el2_Point.X && el1_Point.Y == el2_Point.Y && el1_Point.Z == el2_Point.Z)
                    return true;
                else
                    return false;


            }
        }
    }

   
}
