using Autodesk.Revit.DB;
using MOSTplugin.LintelBeam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace MOSTplugin.WindowManager
{
    public partial class ManagerForm : Form
    {
        private RevitTask revitTask;
        List<Model> ModelList = new List<Model>();

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        ModelData model;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public ManagerForm(ModelData model)
        {
            revitTask = new RevitTask();
            InitializeComponent();
            refresh();
            this.Show();
            this.model = model;

        }

        private void refresh() {
            ModelDataWindows Data_Windows = new ModelDataWindows(); // создаем экземпляр класса, который собирает все нужные двери.
            ModelList = Data_Windows.TableData; //из этого класса достаем то, что должно отображаться в форме, в таблице DGV
            
            
            dgv_Table.Rows.Clear();
            foreach (Model model in ModelList) {
                dgv_Table.Rows.Add(model.TypeName, model.Mark, model.Count,model.LegendStatus);
            
            }
            //отображаем


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {

                Transaction t = new Transaction(Data.Doc,"Создание Легенд");
                t.Start();
                foreach (Model model in ModelList) {
                    try
                    {
                        RevitLegend.CreateLegend(model);
                        
                    }
                    catch { }

                    
                }
                t.Commit();

                Transaction tr = new Transaction(Data.Doc, "Размеры");
                tr.Start();
                foreach (Model model in ModelList)
                {
                    try
                    {
                        if (model.LegendId != null)
                        {
                            AutoDimension.CreateDimension(model);


                        }
                    }
                    catch { }

                }
                tr.Commit();





            });
            refresh();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                Transaction t = new Transaction(Data.Doc, "Обновление марки");
                t.Start();
                


                int counter = 0;
                foreach (DataGridViewRow row in dgv_Table.Rows)
                {
                    try
                    {
                        if (StoredMarks.CodeList.Contains(row.Cells[0].Value.ToString()) == true)
                        {
                            row.Cells[1].Value = StoredMarks.StoredCodeMarkPairs[row.Cells[0].Value.ToString()];
                        }
                        else
                        {

                            while (true)
                            {
                                counter++;
                                string generatedMark = "Д-" + counter.ToString();
                                if (!StoredMarks.MarkList.Contains(generatedMark))
                                {

                                    row.Cells[1].Value = generatedMark;
                                    StoredMarks.InsertPair(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());

                                    break;
                                }

                            }

                        }
                    }
                    catch { }


                }
                


                for (int i = 0; i < ModelList.Count(); i++)
                {
                    ModelList[i].Mark = dgv_Table[1, i].Value.ToString();
                    ModelList[i].SetMark();

                }
                t.Commit();
                
                //}
                //catch {
                //    Warning warn = new Warning();
                //    warn.ShowDialog();
                //}



            });
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app => {

                Transaction t = new Transaction(Data.Doc, "Размеры");
                t.Start();
                foreach (Model model in ModelList) {
                    try
                    {
                        if (model.LegendId != null)
                        {
                            AutoDimension.CreateDimension(model);


                        }
                    }
                    catch { }
                    
                }
                t.Commit();
            
            
            
            });
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app => {

                
                
                foreach (Model model in ModelList)
                {
                    try
                    {
                        if (model.LegendId != null)
                        {
                            RevitImage.CreateViewImage(model);


                        }
                    }
                    catch { }

                }
                
                //try
                //{
               




            });
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnl_TopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private async void btn_PutOnSheet_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app => {
                Transaction trans = new Transaction(Data.Doc, "Размещение на листе");
                trans.Start();
                try {
                    ElementId sheetId = Data.Doc.ActiveView.Id;
                    ViewSheet sheet = Data.Doc.GetElement(sheetId) as ViewSheet;
                    if (sheet == null)
                    {
                        Notifications.WarningForm form = new Notifications.WarningForm("Перейдите на лист!");
                        form.ShowDialog();
                    }
                    else {
                        XYZ position = new XYZ();
                        this.WindowState = FormWindowState.Minimized;
                        position = Data.UIdoc.Selection.PickPoint("Выберите точку вставки");
                        this.WindowState = FormWindowState.Normal;
                        foreach(Model model in ModelList){
                            if (model.LegendId != null)
                            {
                                Viewport viewport = Viewport.Create(Data.Doc, sheet.Id, model.LegendId, position);
                                position = position.Add(new XYZ(0.18, 0, 0));
                                
                                
                            }
                        }

                    }
                
                


                }
                catch { }
                trans.Commit();
            });
        }
    }
}
