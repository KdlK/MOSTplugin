using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSTplugin.DoorManager
{
    public class Model
    {
        public Category category = null;
        public string TypeName = null; //название типоразмера.
        public ElementId TypeId = null;
        public string LegendStatus { 
            get { if (LegendId == null) return "Не создано";
                else return "OK";
                        } }
        private string _mark = null;
        public string Mark { 
            get { return _mark; }
            set { _mark = value;
                
            }
        } public View LegendView { 
            get { return Data.Doc.GetElement(LegendId) as View; }
            
                
            
        }
        public ElementId LegendId = null;
        public View Image = null;
        
        public List<Element> ElementList = new List<Element>(); // список, в котором хранятся все экземпляры типоразмера 
        public int Count
        {
            get {
                return ElementList.Count();
            }
        }
        public void SetMark() {
            
                try
                {
                    Element el = ElementList.First();
                    Element element = Data.Doc.GetElement(el.GetTypeId());
                    if (element.LookupParameter("ADSK_Марка") != null )
                        element.LookupParameter("ADSK_Марка").Set(_mark);
                }
                catch { }
            
            
        }

    }
    
}
