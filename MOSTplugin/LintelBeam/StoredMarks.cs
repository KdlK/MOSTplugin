using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MOSTplugin.LintelBeam
{
    static class StoredMarks
    {
        
      

        public static Dictionary<String, String> StoredCodeMarkPairs {
            get { return GetDictionaryFromSchedule(); }

        }

        public static List<String> CodeList {
            get
            {
                return new List<string>(StoredCodeMarkPairs.Keys);
            }
        }
        public static List<String> MarkList
        {
            get
            {
                return new List<string>(StoredCodeMarkPairs.Values);
            }
        }



        private static Dictionary<String, String> GetDictionaryFromSchedule() {
            Dictionary<string, string> _StoredCodeMarkPairs = new Dictionary<string, string>();
            var viewSchedule = new FilteredElementCollector(Data.Doc)
                                    .OfClass(typeof(ViewSchedule))
                                    .FirstOrDefault(e => e.Name == "Спецификация стиля обобщенной модели") as ViewSchedule;
                                    
            TableData table = viewSchedule.GetTableData();
            TableSectionData section = table.GetSectionData(SectionType.Body);
            int nRows = section.NumberOfRows;
            int nColumns = section.NumberOfColumns;
            
           
           
            if (nRows > 0)
            {
                //valueData.Add(viewSchedule.Name);

                for (int i = 0; i < nRows; i++)
                {
                    string value = viewSchedule.GetCellText(SectionType.Body, i, 0);
                    string key = viewSchedule.GetCellText(SectionType.Body, i, 1);
                    
                    
                    
                    if(!_StoredCodeMarkPairs.Keys.Contains(key))
                        _StoredCodeMarkPairs.Add(key, value);


                }
                


            }
            
            
            return _StoredCodeMarkPairs;


        }
        public static void InsertPair(string code,string mark) {
            var viewSchedule = new FilteredElementCollector(Data.Doc)
                                        .OfClass(typeof(ViewSchedule))
                                        .FirstOrDefault(e => e.Name == "Спецификация стиля обобщенной модели") as ViewSchedule;

            TableData table = viewSchedule.GetTableData();
            TableSectionData section = table.GetSectionData(SectionType.Body);
            //Transaction t = new Transaction(Data.Doc, "go");
            //t.Start();
            section.InsertRow(section.LastRowNumber);

            List<Element> elems = new FilteredElementCollector(Data.Doc, viewSchedule.Id).ToElements().ToList();
            Element newRow = (from rowElement in elems
                              where rowElement.LookupParameter("Ключевое имя")?.AsString().Contains("-") == false
                              select rowElement).ToList().First();

            newRow.LookupParameter("Ключевое имя").Set(mark);
            newRow.LookupParameter("Комментарии").Set(code);
            //t.Commit();



        }



    }
}
