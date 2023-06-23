using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
using Document = Autodesk.Revit.DB.Document;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Autodesk.Revit.DB.Visual;
using System.Xml.Linq;
using System.Windows.Media;
using Autodesk.Revit.UI.Selection;
using Transform = Autodesk.Revit.DB.Transform;
using System.Windows;

namespace MOSTplugin
{
    public partial class Peremichki : Form
    {
        private RevitTask revitTask;
        public List<DataGridData> data_list;
        public Peremichki()
        {

            revitTask = new RevitTask();
            InitializeComponent();
            this.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            refresh();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void btn_CreateSection_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                string str = "ПР-";
                Transaction t = new Transaction(Data.Doc, "go");
                t.Start("go");
                int counter = 1;
                foreach (DataGridData p in data_list)
                {

                    foreach (Peremichka per in p.AllDataItems)

                        per.Element.LookupParameter("ADSK_Марка").Set(String.Format("{0}-{1}", str, counter));
                    counter++;
                }
                t.Commit();
                refresh();
            });

        }
        async private void refresh()
        {
            await revitTask.Run(app =>
            {


                dataGridView1.Rows.Clear();

                data_list = Peremichka.DataList;
                foreach (DataGridData p in data_list)
                {
                    dataGridView1.Rows.Add(p.Code, p.count, p.Marks);
                }

            });

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            label1.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();


        }

        async private void button3_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(Data.Doc).OfClass(typeof(ViewFamilyType))
                                                              let type = elem as ViewFamilyType
                                                              where type.ViewFamily == ViewFamily.Section
                                                              select type;
                
                Element el = Peremichka.PCollector().First();
                LocationPoint lp = el.Location as LocationPoint;
                XYZ XYZ_location = lp.Point;
                double element_widthh = el.LookupParameter("ADSK_Размер_Ширина").AsDouble();
                XYZ VectorToAdd = new XYZ(0, XYZ_location.Y, 0);
                XYZ XYZ_location_2 = XYZ_location.Add(VectorToAdd);
                
                Curve curve = Line.CreateBound(XYZ_location,XYZ_location_2) as Curve;

                Transform curveTransform = curve.ComputeDerivatives(0.5, true);
                XYZ origin = curveTransform.Origin;
                XYZ viewDirection = curveTransform.BasisX.Normalize(); // tangent vector along the location curve 
                XYZ normal = viewDirection.CrossProduct(XYZ.BasisZ).Normalize();
                Transform transform = Transform.Identity;
                transform.Origin = origin;
                transform.BasisX = normal;
                transform.BasisY = XYZ.BasisZ;
                transform.BasisZ = normal.CrossProduct(XYZ.BasisZ);
                BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
                sectionBox.Transform = transform;
                sectionBox.Min = new XYZ(-10, 0, 0);
                sectionBox.Max = new XYZ(10, 12, 5);
                Transaction trans = new Transaction(Data.Doc, "Create section view");
                trans.Start();
                ViewSection viewSection = ViewSection.CreateSection(Data.Doc, viewFamilyTypes.First().Id, sectionBox);
                trans.Commit();












                /*Transaction trans = new Transaction(Data.Doc, "Create section view");
                trans.Start();
                Transform t = Transform.Identity;
                t.BasisY = XYZ.BasisZ;
                t.BasisX = XYZ.BasisY;
                t.BasisZ = XYZ.BasisY.CrossProduct(t.BasisZ);
               // t.Origin = XYZ_location;

                BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
                
                sectionBox.set_MinEnabled(1,true);
                sectionBox.set_MinEnabled(2, true);
                sectionBox.set_MinEnabled(3, true);
                sectionBox.set_MaxEnabled(1, true);
                sectionBox.set_MaxEnabled(2, true);
                sectionBox.set_MaxEnabled(3, true);
                sectionBox.Enabled = true;
                sectionBox.Transform = t;
                sectionBox.Min = new XYZ(XYZ_location.X-5, XYZ_location.Y -5, XYZ_location.Z);
                sectionBox.Max = new XYZ(XYZ_location.X + 5, XYZ_location.Y + 5, XYZ_location.Z + 5);
                
                
                ViewSection viewSection = ViewSection.CreateSection(Data.Doc, viewFamilyTypes.First().Id, sectionBox);
                trans.Commit();*/
               
                
               

            });

        }
    }

    public class Peremichka {
        private Element _Element = null; //Revit элемент перемычки
        private int? _ID = null; //ID перемычки

        static private string MarkParameterName = "ADSK_Марка";//Параметр, в котором хранится марка перемычки
        private string _Mark = null; // Марка перемычки

        static private string CodeParameterName = "Комментарии"; //Параметр, в котором хранится код перемычки
        private string _Code = null; // Код перемычки

        static private string PMotherParameterName = "Группа модели"; //Название параметра, который отличает Составную перемычку от вложенных перемычек
        public static string PMotherParameterValue = "Составная перемычка";


        static private string Sub1ParameterName = "М_Субкомпонент_Тип_1"; //Название параметра, в котором хранится марка субкомпонента1 Составной перемычки
        private string SubName1Value = null;                    //Марка субкомпонента1 Составной перемычки

        static private string Sub2ParameterName = "М_Субкомпонент_Тип_2"; //Название параметра, в котором хранится марка субкомпонента2 Составной перемычки
        private string SubName2Value = null;                    //Марка субкомпонента2 Составной перемычки

        static private string Sub3ParameterName = "М_Субкомпонент_Тип_3"; //Название параметра, в котором хранится марка субкомпонента3 Составной перемычки
        private string SubName3Value = null;                    //Марка субкомпонента3 Составной перемычки

        static private string Sub4ParameterName = "М_Субкомпонент_Тип_4"; //Название параметра, в котором хранится марка субкомпонента4 Составной перемычки
        private string SubName4Value = null;                    //Марка субкомпонента4 Составной перемычки

        static private string Sub5ParameterName = "М_Субкомпонент_Тип_5"; //Название параметра, в котором хранится марка субкомпонента5 Составной перемычки
        private string SubName5Value = null;                    //Марка субкомпонента5 Составной перемычки

        static private string Sub6ParameterName = "М_Субкомпонент_Тип_6"; //Название параметра, в котором хранится марка субкомпонента6 Составной перемычки
        private string SubName6Value = null;                    //Марка субкомпонента6 Составной перемычки

        List<string> SubNameValues;

        static private string WidthParameterName = "ADSK_Размер_Ширина";
        private string WidthParameterValue = null;
        static private string LenghtParameterName = "ADSK_Размер_Длина";
        private string LenghtParameterValue = null;

        static private string IndentParameterName = "М_Перемычка_Отступ_фасадный";
        private string IndentParameterValue = null;
        static private string OffsetParameterName = "М_Четверть_Разрез_Размер";
        private string OffsetParameterValue = null;
        private string PType = null; // [0 - Составная перемычка железобетонная, 1 - Составная перемычка металлическая]
        public string Code { 
            get { return _Code; 
            } 
            
            
        }
        public string Mark { get { return _Mark; } /*set { _Mark = Mark; }*/ }
        public string ID { get { return _ID.ToString(); } /*set { _Mark = Mark; }*/ }
        public Element Element { get { return _Element; } /*set { _Mark = Mark; }*/ }
        public static List<DataGridData> DataList { get { return DataAllItems(); } }
        public static List<Peremichka> AllItems{ get{return PList(PCollector());} }

        public Peremichka(Element element) {
            _Element = element;
            GetParameters();


            _Element.LookupParameter(CodeParameterName).Set(Code);
            





        }
        private void GetParameters() {
            ///_Code = _Element.LookupParameter(CodeParameterName)?.AsString();
            
            _ID = _Element.Id.IntegerValue;
            _Mark = _Element.LookupParameter(MarkParameterName)?.AsString();
            SubName1Value = _Element.LookupParameter(Sub1ParameterName)?.AsString();
            SubName2Value = _Element.LookupParameter(Sub2ParameterName)?.AsString();
            SubName3Value = _Element.LookupParameter(Sub3ParameterName)?.AsString();
            SubName4Value = _Element.LookupParameter(Sub4ParameterName)?.AsString();
            SubName5Value = _Element.LookupParameter(Sub5ParameterName)?.AsString();
            SubName6Value = _Element.LookupParameter(Sub6ParameterName)?.AsString();
            SubNameValues = new List<string>() { SubName1Value, SubName2Value, SubName3Value, SubName4Value, SubName5Value, SubName6Value };

            WidthParameterValue = _Element.LookupParameter(WidthParameterName)?.AsValueString();
            LenghtParameterValue = _Element.LookupParameter(LenghtParameterName)?.AsValueString();
            IndentParameterValue = _Element.LookupParameter(IndentParameterName)?.AsValueString();
            OffsetParameterValue = _Element.LookupParameter(OffsetParameterName)?.AsValueString();
            if ((Data.Doc.GetElement(_Element.GetTypeId()) as ElementType)?.LookupParameter(PMotherParameterName).AsString().Contains("металлическая") == true)
                PType = "1";
            else if ((Data.Doc.GetElement(_Element.GetTypeId()) as ElementType)?.LookupParameter(PMotherParameterName).AsString().Contains("железобетонная") == true)
                PType = "0";
            else PType = "???";



            _Code = GetCode();
        }
        public static List<Element> PCollector() {
            List<Element> GenericModelCollector = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_GenericModel).ToElements().ToList();

            List<Element> FilteredCollector = (from generic in GenericModelCollector where
                                               (Data.Doc.GetElement(generic.GetTypeId()) as ElementType)?
                                               .LookupParameter(PMotherParameterName)?
                                               .AsString().Contains(PMotherParameterValue) == true
                                               select generic).ToList();
            return FilteredCollector;
        }

        private static List<Peremichka> PList(List<Element> element_collector) {
            
            Transaction t = new Transaction(Data.Doc, "go");
            t.Start("go");
            List<Peremichka> list = new List<Peremichka>();
            foreach (Element el in element_collector)
                list.Add(new Peremichka(el));
            t.Commit();
            return list;
            

        }
        private string GetCode()
        {
            string sub_mark = SubcomponentsMark(SubNameValues);



            return string.Format("{0}__{1}x{2}__{3}{4}__{5}", PType, WidthParameterValue?.ToString(), LenghtParameterValue?.ToString(), sub_mark, IndentParameterValue, IndentParameterValue);



        }

        private string SubcomponentsMark(List<string> SubNameValues) {
            
            string mark = "";
            int count = 1;
            if (_Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() == 1)
            {
                mark = mark + String.Format("{0}_{1}__", count, SubNameValues[0]);

            }
            for (int i = 1; i < _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger(); i++) {
                if (SubNameValues[i] == SubNameValues[i - 1])
                {
                    count++;
                    if (i == _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() - 1)
                    {
                        mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i]);


                    }
                    


                }
                else {
                    mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i - 1]);

                    count = 1;
                    if (i == _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() - 1)
                    {
                        mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i]);


                    }

                }


            }
            return _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger().ToString() + "__" + mark;
        }
        private static List<DataGridData> DataAllItems()
        {


            List<DataGridData> data = new List<DataGridData>();
            Peremichka first = Peremichka.AllItems.First();
            data.Add(new DataGridData());
            data[0].count = 1;
            data[0].Marks = first.Mark;
            data[0].Code = first.Code;
            data[0].AllDataItems.Add(first);



            for (int i = 0; i < Peremichka.AllItems.Count(); i++)
            {
                int counter = 0;
                bool exists = false;
                for (int d = 0; d < data.Count(); d++)
                {
                    if (Peremichka.AllItems[i].Code == data[d].Code) {
                        
                        exists = true;
                        counter = d;
                        break;
                    }
                }
                if (exists)
                {
                    data[counter].count++;
                    if (!data[counter].Marks.Contains(Peremichka.AllItems[i].Mark))
                    {
                        data[counter].Marks = data[counter].Marks + "/" + Peremichka.AllItems[i].Mark;
                    }
                    data[counter].AllDataItems.Add(Peremichka.AllItems[i]);
                }
                else {
                    data.Add(new DataGridData());
                    data.Last().count = 1;
                    data.Last().Marks = Peremichka.AllItems[i].Mark;
                    data.Last().Code = Peremichka.AllItems[i].Code;
                    data.Last().AllDataItems.Add(Peremichka.AllItems[i]);
                }
            }
            return data;
        }
        public void SectionView() {
            
           
            LocationPoint p2 = Element.Location as LocationPoint;


        }
    }

    public class DataGridData{
        public string Code;
        public int count;
        public string Marks;
        public List<Peremichka> AllDataItems = new List<Peremichka>();

    }
    
}
