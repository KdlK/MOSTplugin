using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOSTplugin.Notifications
{
    public partial class WarningForm : Form
    {
        public WarningForm()
        {
            
            InitializeComponent();
            this.label1.Text = "Что - то пошло не так = (\nОбратитесь к ТИМ-Специалисту";
            this.Refresh();
        }
        public WarningForm(string message)
        {

            InitializeComponent();
            this.label1.Text =message;
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
