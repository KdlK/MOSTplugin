namespace MOSTplugin.WindowManager
{
    partial class ManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_Table = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegendSatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.pnl_TopBar = new System.Windows.Forms.Panel();
            this.pnl_BottomBar = new System.Windows.Forms.Panel();
            this.pnl_RightBar = new System.Windows.Forms.Panel();
            this.pnl_DGV = new System.Windows.Forms.Panel();
            this.pnl_Menu = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_PutOnSheet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Table)).BeginInit();
            this.pnl_TopBar.SuspendLayout();
            this.pnl_DGV.SuspendLayout();
            this.pnl_Menu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_Table
            // 
            this.dgv_Table.AllowUserToAddRows = false;
            this.dgv_Table.AllowUserToDeleteRows = false;
            this.dgv_Table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Mark,
            this.Count,
            this.LegendSatus});
            this.dgv_Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Table.Location = new System.Drawing.Point(0, 0);
            this.dgv_Table.Name = "dgv_Table";
            this.dgv_Table.ShowRowErrors = false;
            this.dgv_Table.Size = new System.Drawing.Size(615, 291);
            this.dgv_Table.TabIndex = 0;
            // 
            // Type
            // 
            this.Type.HeaderText = "Тип";
            this.Type.Name = "Type";
            // 
            // Mark
            // 
            this.Mark.HeaderText = "Марка";
            this.Mark.Name = "Mark";
            // 
            // Count
            // 
            this.Count.HeaderText = "Количество";
            this.Count.Name = "Count";
            // 
            // LegendSatus
            // 
            this.LegendSatus.HeaderText = "Легенда";
            this.LegendSatus.Name = "LegendSatus";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Создать легенды";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(122, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 37);
            this.button2.TabIndex = 3;
            this.button2.Text = "Перемаркировать ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(72, 178);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(44, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "Проставить размеры";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(5, 55);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 38);
            this.button4.TabIndex = 5;
            this.button4.Text = "Создать изображение";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(863, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(28, 22);
            this.btn_Close.TabIndex = 6;
            this.btn_Close.Text = "X";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // pnl_TopBar
            // 
            this.pnl_TopBar.BackColor = System.Drawing.Color.LightGray;
            this.pnl_TopBar.Controls.Add(this.btn_Close);
            this.pnl_TopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_TopBar.Location = new System.Drawing.Point(0, 0);
            this.pnl_TopBar.Name = "pnl_TopBar";
            this.pnl_TopBar.Size = new System.Drawing.Size(894, 31);
            this.pnl_TopBar.TabIndex = 7;
            this.pnl_TopBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnl_TopBar_MouseMove);
            // 
            // pnl_BottomBar
            // 
            this.pnl_BottomBar.BackColor = System.Drawing.Color.LightGray;
            this.pnl_BottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_BottomBar.Location = new System.Drawing.Point(0, 322);
            this.pnl_BottomBar.Name = "pnl_BottomBar";
            this.pnl_BottomBar.Size = new System.Drawing.Size(894, 24);
            this.pnl_BottomBar.TabIndex = 8;
            // 
            // pnl_RightBar
            // 
            this.pnl_RightBar.BackColor = System.Drawing.Color.LightGray;
            this.pnl_RightBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnl_RightBar.Location = new System.Drawing.Point(863, 31);
            this.pnl_RightBar.Name = "pnl_RightBar";
            this.pnl_RightBar.Size = new System.Drawing.Size(31, 291);
            this.pnl_RightBar.TabIndex = 9;
            // 
            // pnl_DGV
            // 
            this.pnl_DGV.Controls.Add(this.dgv_Table);
            this.pnl_DGV.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnl_DGV.Location = new System.Drawing.Point(248, 31);
            this.pnl_DGV.Name = "pnl_DGV";
            this.pnl_DGV.Size = new System.Drawing.Size(615, 291);
            this.pnl_DGV.TabIndex = 10;
            // 
            // pnl_Menu
            // 
            this.pnl_Menu.BackColor = System.Drawing.Color.LightGray;
            this.pnl_Menu.Controls.Add(this.panel1);
            this.pnl_Menu.Controls.Add(this.button3);
            this.pnl_Menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Menu.Location = new System.Drawing.Point(0, 31);
            this.pnl_Menu.Margin = new System.Windows.Forms.Padding(10);
            this.pnl_Menu.Name = "pnl_Menu";
            this.pnl_Menu.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.pnl_Menu.Size = new System.Drawing.Size(248, 291);
            this.pnl_Menu.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btn_PutOnSheet);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 101);
            this.panel1.TabIndex = 6;
            // 
            // btn_PutOnSheet
            // 
            this.btn_PutOnSheet.Location = new System.Drawing.Point(122, 57);
            this.btn_PutOnSheet.Name = "btn_PutOnSheet";
            this.btn_PutOnSheet.Size = new System.Drawing.Size(99, 37);
            this.btn_PutOnSheet.TabIndex = 6;
            this.btn_PutOnSheet.Text = "Разместить на листе";
            this.btn_PutOnSheet.UseVisualStyleBackColor = true;
            this.btn_PutOnSheet.Click += new System.EventHandler(this.btn_PutOnSheet_Click);
            // 
            // WindowsManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 346);
            this.Controls.Add(this.pnl_Menu);
            this.Controls.Add(this.pnl_DGV);
            this.Controls.Add(this.pnl_RightBar);
            this.Controls.Add(this.pnl_BottomBar);
            this.Controls.Add(this.pnl_TopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Table)).EndInit();
            this.pnl_TopBar.ResumeLayout(false);
            this.pnl_DGV.ResumeLayout(false);
            this.pnl_Menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Table;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LegendSatus;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Panel pnl_TopBar;
        private System.Windows.Forms.Panel pnl_BottomBar;
        private System.Windows.Forms.Panel pnl_RightBar;
        private System.Windows.Forms.Panel pnl_DGV;
        private System.Windows.Forms.Panel pnl_Menu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_PutOnSheet;
    }
}