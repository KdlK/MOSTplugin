using Autodesk.Revit.DB;
using MOSTplugin.SkittingWall;
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

namespace MOSTplugin.LintelBeam
{
    public partial class LintelBeam : Form
    {
        private RevitTask revitTask;
        List<Model> ModelList = new List<Model>();
        List<ViewSection> ViewSectionList = new List<ViewSection>();
        public static List<Element> Views = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).ToList();
        public LintelBeam()
        {
            
            revitTask = new RevitTask();
            InitializeComponent();
            ModelList = Logic.CreateModelList();
            refresh(ModelList);
            this.Show();
        }

        async private void btn_Check_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                ModelList = Logic.CreateModelList();
                Check();


                refresh(ModelList);

            });
        }
        private void Check()
        {
            Views = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).ToList();
            foreach (Model a  in ModelList) {
                a.CheckError();
            }
                


        }

        private void refresh(List<Model> ModelList) {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < ModelList.Count; i++) {
                dataGridView1.Rows.Add(ModelList[i].code, ModelList[i].mark, ModelList[i].count, ModelList[i].SectionStatus);
                if (ModelList[i].error == true)
                {

                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    
                }
            }
        }

        async private void btn_CreateSection_Click(object sender, EventArgs e)
        {
            Views = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).ToList();
            await revitTask.Run(app =>
            {
                
                Transaction t = new Transaction(Data.Doc,"go");
                t.Start();
                
                foreach (Model model in ModelList) {
                    
                    Logic.CreateSectionView(model);
                    
                }
                t.Commit();
            });
        }

        async private void btn_CreateImegeView_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                
                foreach (Model model in ModelList) {
                    Logic.CreateViewImage(model);
                    

                }
                

            });
        }

       
    }
}
