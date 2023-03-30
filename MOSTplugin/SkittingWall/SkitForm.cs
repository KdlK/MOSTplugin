using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a = System.Windows.Forms;

namespace MOSTplugin.SkittingWall
{
    public partial class SkitForm : a.Form
    {
        public SkitForm()
        {
            InitializeComponent();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Element> rooms = SkittingWall_Class.roomCollector();
            List<Element> walls = SkittingWall_Class.SkittingWallCollector();


        }
    }
}
