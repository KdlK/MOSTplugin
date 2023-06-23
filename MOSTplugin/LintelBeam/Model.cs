using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View = Autodesk.Revit.DB.View;

namespace MOSTplugin.LintelBeam
{

    internal class Model
    {
        public List<Element> LintelBeams = new List<Element>();
        public int count { get { return LintelBeams.Count; } }
        public string mark { get { return LintelBeams.First().LookupParameter(Params.MarkParameterName)?.AsString(); } }
        public string code {
            get; set;


        }
        public bool error { get { return false; } }

        public ViewSection Section { 
            get;
            set;
            
        }
        
       
        
        public string SectionStatus = "";

        public void CheckError() {
            HasSection();
        }

        private bool HasMark(string mark) {
            if (mark != null)
                return true;

            else
                return false;
        }
        public void HasSection() {
            List<Element> Views = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).ToList();
            string SectionStatus = "Нет разреза";
            List<String> ViewNames = (from a in Views let name = a.Name select name).ToList();
            for (int i = 0; i < Views.Count(); i++) {
                string name = ViewNames[i];
                View view = Views[i] as View;
                if (name == code)
                {
                    bool check = ViewContainsLintelBeam(view);
                    if (check)
                    {

                        Section = Views[i] as ViewSection;
                        SectionStatus = "ОК";
                    }
                    else
                        SectionStatus = "Разрез не соответствует";

                    break;
                }
            }
            this.SectionStatus = SectionStatus;
        }
        private bool ViewContainsLintelBeam(View view) {
            bool check = false;
            List<Element> viewElements = new FilteredElementCollector(Data.Doc, view.Id).ToList();
            foreach (Element element in viewElements)
            {
                string parmatereName = element.LookupParameter(Params.CodeParameterName)?.AsString();
                if (parmatereName != null && parmatereName == code) {
                    check = true;
                    break;

                }


            }
            return check;
        }
        public void SetCode(){
            foreach (Element lintel in LintelBeams) {
                lintel.LookupParameter(Params.CodeParameterName).Set(code);
            }
        
        }
        
}
}
