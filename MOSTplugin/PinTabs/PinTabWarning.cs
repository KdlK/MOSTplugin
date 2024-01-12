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
    public partial class PinTabWarning : Form
    {
        public PinTabWarning()
        {
            
            InitializeComponent();
            this.label1.Text = "ВНИМАНИЕ!!! Есть незакрепленные элементы!";
            this.Refresh();


        }



       
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
