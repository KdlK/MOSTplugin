using MOSTplugin.SkittingWall;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MOSTplugin.LintelBeam
{
    internal class ErrorReview
    {
        private List<Model> modelList;


        private string _NoSectionReview = "";
        private List<Model> _NoSectionList = null;
        private bool _NoSection_btn_enabled = false;
        private System.Drawing.Color _NoSectionColor = System.Drawing.Color.Green;

        public string NoSectionReview { get { return _NoSectionReview; } }
        public List<Model> NoSectionList { get { return _NoSectionList; } } 
        public bool NoSection_btn_enabled { get { return _NoSection_btn_enabled; } }
        public System.Drawing.Color NoSectionColor { get { return _NoSectionColor; } }




        private string _SectionInvalidReview = "";
        private List<Model> _SectionInvalidList = null;
        private bool _SectionInvalid_btn_enabled = false;
        private System.Drawing.Color _SectionInvalid_Color = System.Drawing.Color.Green;

        public string SectionInvalidReview { get { return _SectionInvalidReview; } }
        public List<Model> SectionInvalidList { get { return _SectionInvalidList; } }
        public bool SectionInvalid_btn_enabled { get { return _SectionInvalid_btn_enabled; } }
        public System.Drawing.Color SectionInvalidColor { get { return _SectionInvalid_Color; } }






        public ErrorReview(List<Model> ModelList) { 
            this.modelList = ModelList;
            StartCheckout();
        }


        private void StartCheckout() {
            SectionNotExists();
            SectionInvalid();


        }
        private void InvalidMark() { 
            
        
        }

        private void SectionNotExists() {
            
            List<Model> NoSection_List = new List<Model>(); 
            foreach (Model model in modelList)
            {
                if (model.SectionStatus == "Нет разреза")
                {
                    
                    NoSection_List.Add(model);  


                }
                
            }
            if (NoSection_List.Count() == 0)
            {
                _NoSectionReview = "Всем элементам назначан разрез. \n";
            }
            else {
                _NoSectionReview = NoSection_List.Count().ToString() + " элементов без разреза. \nРекомундуется создать. \n";
                _NoSectionColor = System.Drawing.Color.Red;
                _NoSection_btn_enabled = true;
                _NoSectionList = NoSection_List;
            }

        
        }

        private void SectionInvalid() {
            List<Model> InvalidSection_list = new List<Model>();
            foreach (Model model in modelList) {
                if (model.SectionStatus == "Разрез не соответствует")
                {
                    InvalidSection_list.Add(model);
                }

            }
            if (InvalidSection_list.Count() == 0 )
            {
                if (NoSectionList?.Count() == modelList?.Count())
                {
                    _SectionInvalidReview = "Сначала создайте разрезы, \n что бы проверить актуальность";
                    _SectionInvalid_Color = System.Drawing.Color.Red;
                }
                else
                    _SectionInvalidReview = "Все cуществующие разрезы актуальны\n";
            }
            else
            {
                _SectionInvalidReview = InvalidSection_list.Count().ToString() + " Разрезов требуют обновления \n";
                _SectionInvalid_Color = System.Drawing.Color.Red;
                _SectionInvalid_btn_enabled = true;
                _SectionInvalidList = InvalidSection_list;
            }



        }


    }
}
