using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace MOSTplugin.SkittingWall
{
    public partial class SkittinWallForm : System.Windows.Forms.Form
    {
        private RevitTask revitTask;
        List<WallType> _wallTypes = new List<WallType>();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
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


        public SkittinWallForm()
        {
            revitTask = new RevitTask();    
            InitializeComponent();
            _wallTypes = (from elem in new FilteredElementCollector(Data.Doc).OfClass(typeof(WallType))
                                               let type = elem as WallType
                                               where type.LookupParameter("Группа модели")?.AsString() == "Отделка"
                                               select type).ToList();
            comboBox1.DataSource = (from elem in _wallTypes select elem.Name).ToList();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));



            this.Show();
        }
        
        async private void button1_Click(object sender, EventArgs e)
        {
            await revitTask.Run(app =>
            {
                bool start = true;
                Logic a = new Logic();
                // Add Your Code Here
                Transaction tx = new Transaction(Data.Doc,"go");
                float offsetValue;
                float WallHeight;
                if (!float.TryParse(offset_value.Text, out offsetValue)) {

                    MessageBox.Show("Смещение должно содержать число. Проверьте!");
                    if (start)
                    {
                        
                        start = false;
                    }
                }
                if (!float.TryParse(textBox_WallHeight.Text, out WallHeight))
                {
                    MessageBox.Show("Высота должна содержать число. Проверьте!");
                    if(start)
                        start = false;  
                }


                if (start) { 
                    a.RoomFinish(Data.UIdoc, tx, _wallTypes[comboBox1.SelectedIndex], offsetValue, WallHeight, checkBox_JoinWalls.Checked);
                
                }
                /*Data.UIapp.Application.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(Logic.FailuresProcessing);
                using (TransactionGroup txg = new TransactionGroup(Data.Doc))
                {
                    using (Transaction tx = new Transaction(Data.Doc))
                    {
                        txg.Start(Tools.LangResMan.GetString("roomFinishes_transactionName", Tools.Cult));


                        Logic a = new Logic();
                        // Add Your Code Here
                        a.RoomFinish(Data.UIdoc, tx);
                        try
                        {
                            txg.Start(Tools.LangResMan.GetString("roomFinishes_transactionName", Tools.Cult));


                            Logic a = new Logic();
                            // Add Your Code Here
                            a.RoomFinish(Data.UIdoc, tx);

                            if (tx.GetStatus() == TransactionStatus.RolledBack)
                            {
                                txg.RollBack();
                            }
                            else
                            {
                                txg.Assimilate();
                            }


                            //Unsubscribe to the FailuresProcessing Event
                            Data.UIapp.Application.FailuresProcessing -= Logic.FailuresProcessing;
                            // Return Success
                            return Result.Succeeded;
                        }
                        catch (Autodesk.Revit.Exceptions.OperationCanceledException exceptionCanceled)
                        {
                            Data.Message = exceptionCanceled.Message;
                            if (tx.HasStarted())
                            {
                                tx.RollBack();
                            }
                            //Unsubscribe to the FailuresProcessing Event
                            Data.UIapp.Application.FailuresProcessing -= Logic.FailuresProcessing;
                            return Autodesk.Revit.UI.Result.Cancelled;
                        }
                        catch (ErrorMessageException errorEx)
                        {
                            // checked exception need to show in error messagebox
                            Data.Message = errorEx.Message;
                            if (tx.HasStarted())
                            {
                                tx.RollBack();
                            }
                            //Unsubscribe to the FailuresProcessing Event
                            Data.UIapp.Application.FailuresProcessing -= Logic.FailuresProcessing;
                            return Autodesk.Revit.UI.Result.Failed;
                        }
                        catch (Exception ex)
                        {
                            // unchecked exception cause command failed
                            Data.Message = Tools.LangResMan.GetString("roomFinishes_unexpectedError", Tools.Cult) + ex.Message;
                            //Trace.WriteLine(ex.ToString());
                            if (tx.HasStarted())
                            {
                                tx.RollBack();
                            }
                            //Unsubscribe to the FailuresProcessing Event
                            Data.UIapp.Application.FailuresProcessing -= Logic.FailuresProcessing;
                            return Autodesk.Revit.UI.Result.Failed;
                        }
                    }
                }*/






            });

            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SkittinWallForm_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            base.WndProc(ref msg);
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

        private void radioButton_JoinWalls_CheckedChanged(object sender, EventArgs e)
        {
            
           
        }

        
    }
}
