using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Form = System.Windows.Forms.Form;
using TextBox = System.Windows.Forms.TextBox;

namespace MOSTplugin
{
    public partial class Rename_views : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        Document doc;
        UIDocument uidoc;
        private RevitTask revitTask;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public Rename_views(Document Doc,UIDocument UIdoc)
        {
            InitializeComponent();
            doc = Doc;
            uidoc = UIdoc;
            revitTask = new RevitTask();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            /*SetRoundedShape(panel1, 30);
            SetRoundedShape(panel2, 30);
            SetRoundedShape(panel3, 30);
            SetRoundedShape(panel4, 30);*/
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            rename_views();
       
        }

        private async void rename_views() {
            int warnings = 0;
            string param_name = string.Empty;
            await revitTask.Run(app =>
            {
                ICollection<ElementId> elementIds = uidoc.Selection.GetElementIds();
                Transaction t = new Transaction(doc, "param");
                t.Start("param");
                foreach (ElementId elemID in elementIds)
                {
                    
                    Element el = doc.GetElement(elemID);

                    if (el.LookupParameter("Имя вида") != null || el.LookupParameter("Имя листа") != null)
                    {
                        param_name = "Имя вида";
                        if (el.LookupParameter("Имя листа") != null && el.LookupParameter("Имя листа").IsReadOnly != true)  
                            param_name = "Имя листа";

                        string name = el.LookupParameter(param_name).AsString();
                        if (textBox_serchfor.Text != "" && textBox_replaceon.Text != "")
                        {
                            name = textBox_prefix.Text + name.Replace(textBox_serchfor.Text, textBox_replaceon.Text) + textBox_suffix.Text;
                        }
                        else if (textBox_serchfor.Text != "" && textBox_replaceon.Text == "") {
                            name = textBox_prefix.Text + name.Replace(textBox_serchfor.Text, "") + textBox_suffix.Text;

                        }
                        else
                        {
                            name = textBox_prefix.Text + name + textBox_suffix.Text;
                        }
                        el.LookupParameter(param_name)?.Set(name);
                        }
                    
                    else
                    {
                        warnings++;
                    }

                }

                if (warnings > 0) this.Close();
                else label6.Text = "Все прошло успешно!";
                
                t.Commit();
            });



        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void textBox1_change_text(string text_in) {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            base.WndProc(ref msg);
        }

        private void panel1_BackColorChanged(object sender, EventArgs e)
        {

        }
        static void SetRoundedShape(System.Windows.Forms.Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.AddLine(0, control.Height - radius, 0, radius);
            path.AddArc(0, 0, radius, radius, 180, 90);
            control.Region = new Region(path);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            //Application.Exit();
        }

        private void textBox_suffix_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_serchfor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Tab))
            {
                textBox_replaceon.Focus();
            }
        }

        private void textBox_replaceon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Tab))
            {
                textBox_prefix.Focus();
            }
        }

        private void textBox_prefix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Tab))
            {
                textBox_suffix.Focus();
            }
        }

        private void textBox_serchfor_KeyUp(object sender, KeyEventArgs e)
        {
            int start = textBox_serchfor.SelectionStart;
            int length = textBox_serchfor.SelectionLength;
            
            if (e.Control && e.KeyCode == Keys.C)
            {
                //textBox_serchfor.Text.Substring(start,length).Copy();
            }
            if (e.KeyData == (Keys.Control | Keys.V))
                (sender as TextBox).Paste();
            if (e.KeyData == (Keys.Control | Keys.C))
                (sender as TextBox).Copy();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void Rename_views_Load(object sender, EventArgs e)
        {

        }

        private void textBox_replaceon_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
