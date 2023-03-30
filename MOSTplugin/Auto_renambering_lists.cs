using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Document = Autodesk.Revit.DB.Document;
using Form = System.Windows.Forms.Form;

namespace MOSTplugin
{
    public partial class Auto_renambering_lists : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private RevitTask revit_task;
        Document doc;
        UIDocument uidoc;

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
        public Auto_renambering_lists(Document Doc, UIDocument UIdoc)
        {
            doc = Doc;
            revit_task = new RevitTask();
            uidoc = UIdoc;
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.Show();
        }

        private void Auto_renambering_lists_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            renumber(textBox1.Text);


        }

        //1‪‫‪
        async private void renumber(string start_position) {


            await revit_task.Run(app =>
            {
                Transaction t = new Transaction(doc, "go");
                t.Start("go");
                ICollection<ElementId> SelectedElementIds = uidoc.Selection.GetElementIds();
                List<Element> SelectedElements = (from ElementId el in SelectedElementIds select doc.GetElement(el)).Where(el => el.Category.Id.IntegerValue == ((int)BuiltInCategory.OST_Sheets)).ToList();
                List<Element> AllElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).ToElements().ToList();
                List<String> AllElements_values = (from Element elem in AllElements select elem.LookupParameter("Номер листа").AsString()).ToList();
                
            
                List < Element > sorted_elements = SelectedElements.OrderBy(el => el.LookupParameter("Номер листа").AsString().Split('\u202A')[0].Length).ThenBy(c=>c.LookupParameter("Номер листа").AsString()).ToList();
                string StaticPart ="";
                int StaticPartLength = 0;
                string DynamicPart = "";
                int DynamicPartLength = 0;
                int DynamicPart_int=0;


                for (int i = start_position.Length - 1; i >= 0; i--)
                {

                    if (char.IsDigit(start_position[i]))
                    {
                        DynamicPartLength++;
                    }
                    else
                    {

                        break;
                    }
                }
                StaticPartLength = start_position.Length - DynamicPartLength;
                if (StaticPartLength > 0)
                    StaticPart = start_position.Substring(0, StaticPartLength);

                if (StaticPartLength != start_position.Length)
                {
                    for (int i = StaticPartLength; i <= start_position.Length - 1; i++)
                    {
                        if (char.IsDigit(start_position[i]) && start_position[i] != '0')
                        {
                            DynamicPart_int = int.Parse(start_position.Substring(i));
                            break;

                        }
                    }
                }
                else
                {
                    DynamicPart_int = 0;
                }

                foreach (Element el in sorted_elements) {
                    string value = "";
                    DynamicPart = DynamicPart_int.ToString();
                    if (DynamicPartLength>0)
                    {
                        while (DynamicPart.Length < DynamicPartLength) DynamicPart = "0" + DynamicPart;
                    }
                    value = StaticPart + DynamicPart + '\u202A';
                    //while (AllElements_values.Contains(value) && !el.LookupParameter("Номер листа").AsString().Contains("\u202A"))


                    while (true) {
                        
                        if (AllElements_values.Contains(value)) 
                            value = value + '\u202A';
                        else
                            break;

                    }
                    el.LookupParameter("Номер листа").Set(value);
                    DynamicPart_int++;



                }



                t.Commit();
            });
            


        }
        private void add_unicode_symbol(List<Element> elements) {
            foreach (Element el in elements)
            {
                if (el.LookupParameter("Номер листа") == null)
                    elements.Remove(el);
                else if (!el.LookupParameter("Номер листа").AsString().Contains("\u202A"))
                    el.LookupParameter("Номер листа").Set(el.LookupParameter("Номер листа").AsString() + '\u202A');

            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Auto_renambering_lists_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            base.WndProc(ref msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
