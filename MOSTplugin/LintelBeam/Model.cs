using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using MOSTplugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        
        public string Mark { 
            get { return CombinedMark(); 
            }
            set { SetMark(value); }            
        }
        private string _Code;
        public string code {
            get { return _Code; }
            set 
            { 
                _Code = value;
                SetCode();
            }


        }
        public bool error { get { return false; } }

        public ViewSection Section {
            get { return GetSection();}
            set { Section = value; }
            
        }
        public ImageView Image {
            get { return GetImage(); }
            set { }
        }
        

        private string CombinedMark() {
            List<String> Marks = new List<String>();
            foreach (Element el in LintelBeams) {
                string mark = el.LookupParameter(Params.MarkParameterName).AsString();
                if (mark != null && mark == "") mark = "Нет марки";

                if (Marks.Contains(mark) == false) Marks.Add(mark);  

            }
            return string.Join("/",Marks.ToArray());
        }
         public Bitmap Image_bitmap { 
            get
            {
                return GetSavedImage();
            }
            


            }
        public string ImageFilePath  {
            get { return @"D:\" + code + @".jpg" ; } 
             
        
        }
         
       
        
        public string SectionStatus
        {
            get { return GetSectionStatus(); }
        }

        private bool HasMark(string mark) {
            if (mark != null)
                return true;

            else
                return false;
        }



        private Bitmap GetSavedImage() {
            Bitmap bmp = null;


            string SelectedImgSize_combobox = Params.selectedImageSize;
            int bmp_heigh = Params.FormatSizes[SelectedImgSize_combobox].height;
            int bmp_width = Params.FormatSizes[SelectedImgSize_combobox].width;
            

            if (Image != null)
            {
                if (File.Exists(ImageFilePath))
                {
                    Bitmap bmp_image = new Bitmap(ImageFilePath);
                    bmp = new Bitmap(bmp_image, bmp_width,bmp_heigh);
                }
                else bmp = Resources.ImageToUpdate;


            }
            else {
                bmp = Resources.ImageNotExists;
            }

            return bmp;

        }
        private ImageView GetImage()
        {
            List<View> Views = (from view in new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views)
                                let v = view as View
                                where (v.ViewType.ToString() == "Rendering" && v.Name == code)
                                select v).ToList();
            if (Views.Count != 0)
                return Views.First() as ImageView;
            else return null;


        }

        private ViewSection GetSection() {
            List<View> Views = (from view in new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views)
                                   let v = view as View
                                   where (v.ViewType.ToString() == "Section" && v.Name == code)
                                   select v).ToList();
            if (Views.Count != 0)
                return Views.First() as ViewSection;
            else return null;

            
        }
        private string GetSectionStatus() {


            if (Section == null)
                return "Нет разреза";
            else {
                bool check = ViewContainsLintelBeam(Section);
                if (check)
                {
                    
                    return "ОК";
                }
                else {
                    return "Разрез не соответствует";
                    
                }
                



                
            }
            
           
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
        public void SetMark(string mark)
        {
            foreach (Element lintel in LintelBeams)
            {
                lintel.LookupParameter(Params.MarkParameterName).Set(mark);
            }

        }

    }
}
