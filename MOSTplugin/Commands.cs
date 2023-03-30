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

namespace MOSTplugin
{

    [Transaction(TransactionMode.Manual)]
    public static class Data { 
    
        private static ExternalCommandData _commandData = null;
        private static UIDocument _docUI = null;
        private static UIApplication _appUI = null;
        private static Application _app = null;
        private static Document _doc = null;
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
        public static UIDocument UIdoc { get { return _docUI; } }
        public static UIApplication UIapp { get { return _appUI; } }
        public static Application App { get { return _app; } }
        public static Document Doc { get { return _doc; } }

    }
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
            Peremichki form = new Peremichki();
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class SkittingWall_command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Data.CommandData = commandData;
            SkitForm form = new SkittingWall.SkitForm();
            
            return Result.Succeeded;
        }
    }
}
