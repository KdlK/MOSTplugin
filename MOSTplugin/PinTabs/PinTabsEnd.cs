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
    public partial class PinTabEnd : Form
    {
        public PinTabEnd(List<BuiltInCategory> categories)
        {
            
            InitializeComponent();
            StartReview(categories);


        }



        private void StartReview(List<BuiltInCategory> categories) {
            Transaction t = new Transaction(Data.Doc, "Закрепить элементы");
            t.Start();
            string review = "";
            foreach (BuiltInCategory category in categories)
            {
                string CategoryReview = "";
                List<Element> elementList = new FilteredElementCollector(Data.Doc).OfCategory(category).WhereElementIsNotElementType().ToElements().ToList();
                int PinnedElementsCounter = 0;
                foreach (Element element in elementList)
                {
                    try
                    {
                        element.Pinned = true;
                        PinnedElementsCounter++;
                    }
                    catch
                    {

                    }

                }
                string CategoryName = Category.GetCategory(Data.Doc, category).Name;
                CategoryReview = String.Format("{0}\n---------------- \nКоличество в проекте: {1}\nЗакреплено: {2} \n\n", CategoryName.ToUpper(), elementList.Count(), PinnedElementsCounter);
                review += CategoryReview;
            }
            t.Commit();
            this.richTextBox1.Text = review;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
