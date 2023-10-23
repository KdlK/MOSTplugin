using Autodesk.Revit.DB;
using MOSTplugin.SkittingWall;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace MOSTplugin.LintelBeam
{
    public partial class LintelBeam : Form
    {

        
        ErrorReview Errors;
        private RevitTask revitTask;
        List<Model> ModelList = new List<Model>();
        List<ViewSection> ViewSectionList = new List<ViewSection>();
        public static List<Element> Views = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views).ToList();

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        


            

        public LintelBeam()
        {
            
            revitTask = new RevitTask();
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.comboBox1.Items.AddRange(Params.FormatSizes.Keys.Cast<object>().ToArray());
            this.comboBox1.SelectedIndex = 0;
            Params.selectedImageSize = this.comboBox1.SelectedItem.ToString();
            
            refresh();
            this.Show();
        }

        async private void btn_Check_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                
                
                Errors = new ErrorReview(ModelList);
                btn_CreateSectionView.Enabled = Errors.NoSection_btn_enabled;
                btn_RefreshSectionView.Enabled = Errors.SectionInvalid_btn_enabled;
               
                label_NoSection.Text = Errors.NoSectionReview;
                label_NoSection.ForeColor = Errors.NoSectionColor;

                label_SectionInvalid.Text = Errors.SectionInvalidReview;
                label_SectionInvalid.ForeColor = Errors.SectionInvalidColor;
                


                refresh();

            });
        }
        
        private void Check()
        {
            
            
                


        }

        private void refresh() {
            Params.selectedImageSize = this.comboBox1.SelectedItem.ToString();
            ModelList = Logic.CreateModelList().OrderBy(g => g.code).ToList();
            dataGridView1.Rows.Clear();
            string list_value = comboBox1.SelectedItem.ToString();
            dataGridView1.RowTemplate.Height = Params.FormatSizes[list_value].height;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Columns[5].Width = Params.FormatSizes[list_value].width;
            
            for (int i = 0; i < ModelList.Count; i++) {
                dataGridView1.Rows.Add(false,ModelList[i].code, ModelList[i].Mark, ModelList[i].count, ModelList[i].SectionStatus, new Bitmap(ModelList[i].Image_bitmap), ModelList[i].ImageFilePath);
                
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        async private void btn_CreateSection_Click(object sender, EventArgs e)
        {
           
            await revitTask.Run(app =>
            {
                
                Transaction t = new Transaction(Data.Doc,"go");
                t.Start();
                
                foreach (Model model in Errors.NoSectionList) {
                    
                    Logic.CreateSectionView(model);
                    
                }
                t.Commit();
            });
            btn_CreateSectionView.Enabled = false;
            
        }

        async private void btn_CreateImegeView_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                
                foreach (Model model in ModelList) {
                    Logic.CreateViewImage(model);
                    Logic.SaveViewImage(model); 
                    

                }
                

            });
        }

       

        private async void btn_RefreshSectionView_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {

                Transaction t = new Transaction(Data.Doc, "go");
                t.Start();

                foreach (Model model in Errors.SectionInvalidList)
                {

                    Logic.CreateSectionView(model);

                }
                t.Commit();
            });
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                
                
           

        }

        private async void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                int index = dataGridView1.CurrentCell.RowIndex;
                Model model = ModelList[index];
                await revitTask.Run(app =>
                {
                    Data.UIdoc.ActiveView = model.Section;
                });
            }
        }

        private void LintelBeam_Load(object sender, EventArgs e)
        {

        }

        private void label_SectionInvalid_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form F_settings = new Settings();
            F_settings.ShowDialog();
        }

        private async void btn_Remark_Click(object sender, EventArgs e)
        {
            if (Rbtn_Mark_Auto.Checked) {
                int counter = 1;
                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    row.Cells[2].Value = "ПР-" + counter.ToString();
                    counter++;
                }
            
            }
            await revitTask.Run(app =>
            {
                Transaction t = new Transaction(Data.Doc,"Обновление марки");
                t.Start();
                for (int i = 0; i < ModelList.Count(); i++) {
                    ModelList[i].Mark = dataGridView1[2, i].Value.ToString();
                }
                t.Commit();


            });

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void скопироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Clipboard.SetText(dataGridView1.CurrentCell.Value.ToString());
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridView1.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridView1, relativeMousePosition);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool checkedstatus = true;
            if (!checkBox1.Checked)
                checkedstatus = false;
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = checkedstatus;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.Equals(true)) //3 is the column number of checkbox
                {
                    row.Selected = true;
                    
                    
                }
                else
                    row.Selected = false;
            }

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
            
                if (dataGridView1.Rows[e.RowIndex].Selected.Equals(true)) //3 is the column number of checkbox
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                }
            
        }
    }
}
