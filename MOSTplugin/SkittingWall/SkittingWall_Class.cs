using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace MOSTplugin.SkittingWall
{
    internal class SkittingWall_Class
    {
        static public void test() { 
        
        }
        static public List<Element> roomCollector() {
            List<Element> elements = new List<Element>();   
            Selection selection = Data.UIdoc.Selection;
            List<ElementId> elementIDs = selection.GetElementIds().ToList();
            foreach (ElementId elementID in elementIDs)
            {
                Element el = Data.Doc.GetElement(elementID);
                if (el.Category.Name == "Помещения")
                {
                    elements.Add(el);
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



    }
}
