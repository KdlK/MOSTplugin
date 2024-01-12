using Autodesk.Revit.DB;
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

namespace MOSTplugin.PinTabs

{ 
    public partial class PinTab : Form
    {
        public bool HasUnpinned = false;
        public List<BuiltInCategory> categoryList = null;
        public PinTab(List<BuiltInCategory> categories)
        {
            
            InitializeComponent();
            StartReview(categories);
            categoryList = categories;



        }



        private void StartReview(List<BuiltInCategory> categories) {
            Transaction t = new Transaction(Data.Doc, "Закрепить элементы");
            t.Start();
            string review = "";
            foreach (BuiltInCategory category in categories)
            {
                string CategoryReview = "";
                List<Element> elementList = new FilteredElementCollector(Data.Doc).OfCategory(category).WhereElementIsNotElementType().ToElements().ToList();
                int UnpinnedElementsCounter = 0;
                foreach (Element element in elementList)
                {
                    try
                    {
                        if(element.Pinned == false)
                            UnpinnedElementsCounter++;
                    }
                    catch
                    {

                    }

                }
                if (UnpinnedElementsCounter != 0) {
                    HasUnpinned = true;
                }
                string CategoryName = Category.GetCategory(Data.Doc, category).Name;
                CategoryReview = String.Format("{0}\n---------------- \nНезакрепленных: {2}\nВсего: {1} \n\n", CategoryName.ToUpper(), elementList.Count(), UnpinnedElementsCounter);
                review += CategoryReview;
            }
            t.Commit();
            this.richTextBox1.Text = review;  

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            PinTabEnd form = new PinTabEnd(categoryList);
            form.ShowDialog();  
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (HasUnpinned) {
                PinTabWarning form = new PinTabWarning();
                form.Show();
            }
        }
    }
}
