using System.Linq;

namespace MOSTplugin.LintelBeam
{
    partial class LintelBeam
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ItemSelection = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Check = new System.Windows.Forms.Button();
            this.btn_CreateSectionView = new System.Windows.Forms.Button();
            this.btn_CreateViewImage = new System.Windows.Forms.Button();
            this.btn_Remark = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label_NoSection = new System.Windows.Forms.Label();
            this.btn_RefreshSectionView = new System.Windows.Forms.Button();
            this.label_SectionInvalid = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.Rbtn_Mark_Auto = new System.Windows.Forms.RadioButton();
            this.Rbtn_Mark_Manual = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.скопироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.LightGray;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemSelection,
            this.Code,
            this.Mark,
            this.Count,
            this.SectionStatus,
            this.ImageColumn,
            this.Column1});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 120;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(732, 573);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // ItemSelection
            // 
            this.ItemSelection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ItemSelection.FillWeight = 77.92208F;
            this.ItemSelection.HeaderText = "";
            this.ItemSelection.Name = "ItemSelection";
            this.ItemSelection.Width = 20;
            // 
            // Code
            // 
            this.Code.FillWeight = 105.5195F;
            this.Code.HeaderText = "Код";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // Mark
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Mark.DefaultCellStyle = dataGridViewCellStyle3;
            this.Mark.FillWeight = 105.5195F;
            this.Mark.HeaderText = "Марка";
            this.Mark.Name = "Mark";
            // 
            // Count
            // 
            this.Count.FillWeight = 105.5195F;
            this.Count.HeaderText = "Кол-во";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // SectionStatus
            // 
            this.SectionStatus.FillWeight = 105.5195F;
            this.SectionStatus.HeaderText = "Разрез";
            this.SectionStatus.Name = "SectionStatus";
            this.SectionStatus.ReadOnly = true;
            this.SectionStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ImageColumn
            // 
            this.ImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ImageColumn.HeaderText = "Изображение";
            this.ImageColumn.MinimumWidth = 10;
            this.ImageColumn.Name = "ImageColumn";
            this.ImageColumn.ReadOnly = true;
            this.ImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ImageColumn.Width = 83;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Visible = false;
            // 
            // btn_Check
            // 
            this.btn_Check.Location = new System.Drawing.Point(12, 5);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(125, 33);
            this.btn_Check.TabIndex = 1;
            this.btn_Check.Text = "Запустить проверку";
            this.btn_Check.UseVisualStyleBackColor = true;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // btn_CreateSectionView
            // 
            this.btn_CreateSectionView.Enabled = false;
            this.btn_CreateSectionView.Location = new System.Drawing.Point(7, 14);
            this.btn_CreateSectionView.Name = "btn_CreateSectionView";
            this.btn_CreateSectionView.Size = new System.Drawing.Size(121, 30);
            this.btn_CreateSectionView.TabIndex = 2;
            this.btn_CreateSectionView.Text = "Создать разрезы";
            this.btn_CreateSectionView.UseVisualStyleBackColor = true;
            this.btn_CreateSectionView.Click += new System.EventHandler(this.btn_CreateSection_Click);
            // 
            // btn_CreateViewImage
            // 
            this.btn_CreateViewImage.Location = new System.Drawing.Point(138, 14);
            this.btn_CreateViewImage.Name = "btn_CreateViewImage";
            this.btn_CreateViewImage.Size = new System.Drawing.Size(129, 30);
            this.btn_CreateViewImage.TabIndex = 3;
            this.btn_CreateViewImage.Text = "Создать изображение";
            this.btn_CreateViewImage.UseVisualStyleBackColor = true;
            this.btn_CreateViewImage.Click += new System.EventHandler(this.btn_CreateImegeView_Click);
            // 
            // btn_Remark
            // 
            this.btn_Remark.Location = new System.Drawing.Point(7, 54);
            this.btn_Remark.Name = "btn_Remark";
            this.btn_Remark.Size = new System.Drawing.Size(105, 40);
            this.btn_Remark.TabIndex = 2;
            this.btn_Remark.Text = "Переопределить марку";
            this.btn_Remark.UseVisualStyleBackColor = true;
            this.btn_Remark.Click += new System.EventHandler(this.btn_Remark_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(177, 54);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 38);
            this.button3.TabIndex = 3;
            this.button3.Text = "Перейти к разрезу";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btn_CreateImegeView_Click);
            // 
            // label_NoSection
            // 
            this.label_NoSection.AutoSize = true;
            this.label_NoSection.Location = new System.Drawing.Point(14, 10);
            this.label_NoSection.Name = "label_NoSection";
            this.label_NoSection.Size = new System.Drawing.Size(0, 13);
            this.label_NoSection.TabIndex = 4;
            // 
            // btn_RefreshSectionView
            // 
            this.btn_RefreshSectionView.Enabled = false;
            this.btn_RefreshSectionView.Location = new System.Drawing.Point(7, 50);
            this.btn_RefreshSectionView.Name = "btn_RefreshSectionView";
            this.btn_RefreshSectionView.Size = new System.Drawing.Size(121, 28);
            this.btn_RefreshSectionView.TabIndex = 5;
            this.btn_RefreshSectionView.Text = "Обновить разрез";
            this.btn_RefreshSectionView.UseVisualStyleBackColor = true;
            this.btn_RefreshSectionView.Click += new System.EventHandler(this.btn_RefreshSectionView_Click);
            // 
            // label_SectionInvalid
            // 
            this.label_SectionInvalid.AutoSize = true;
            this.label_SectionInvalid.Location = new System.Drawing.Point(14, 31);
            this.label_SectionInvalid.Name = "label_SectionInvalid";
            this.label_SectionInvalid.Size = new System.Drawing.Size(0, 13);
            this.label_SectionInvalid.TabIndex = 4;
            this.label_SectionInvalid.Click += new System.EventHandler(this.label_SectionInvalid_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 57);
            this.panel1.TabIndex = 7;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.LightGray;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(937, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(111, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.btn_Check);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(319, 573);
            this.panel2.TabIndex = 8;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 424);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 8;
            this.listBox1.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 525);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Настройки";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.Rbtn_Mark_Auto);
            this.panel7.Controls.Add(this.Rbtn_Mark_Manual);
            this.panel7.Controls.Add(this.btn_Remark);
            this.panel7.Controls.Add(this.button3);
            this.panel7.Location = new System.Drawing.Point(12, 293);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(300, 106);
            this.panel7.TabIndex = 6;
            // 
            // Rbtn_Mark_Auto
            // 
            this.Rbtn_Mark_Auto.AutoSize = true;
            this.Rbtn_Mark_Auto.Checked = true;
            this.Rbtn_Mark_Auto.Location = new System.Drawing.Point(7, 8);
            this.Rbtn_Mark_Auto.Name = "Rbtn_Mark_Auto";
            this.Rbtn_Mark_Auto.Size = new System.Drawing.Size(132, 17);
            this.Rbtn_Mark_Auto.TabIndex = 5;
            this.Rbtn_Mark_Auto.TabStop = true;
            this.Rbtn_Mark_Auto.Text = "Сквозная нумерация";
            this.Rbtn_Mark_Auto.UseVisualStyleBackColor = true;
            // 
            // Rbtn_Mark_Manual
            // 
            this.Rbtn_Mark_Manual.AutoSize = true;
            this.Rbtn_Mark_Manual.Enabled = false;
            this.Rbtn_Mark_Manual.Location = new System.Drawing.Point(7, 31);
            this.Rbtn_Mark_Manual.Name = "Rbtn_Mark_Manual";
            this.Rbtn_Mark_Manual.Size = new System.Drawing.Size(118, 17);
            this.Rbtn_Mark_Manual.TabIndex = 4;
            this.Rbtn_Mark_Manual.Text = "Ручная нумерация";
            this.Rbtn_Mark_Manual.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.label_NoSection);
            this.panel6.Controls.Add(this.label_SectionInvalid);
            this.panel6.Location = new System.Drawing.Point(12, 44);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(300, 135);
            this.panel6.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.btn_CreateViewImage);
            this.panel5.Controls.Add(this.btn_CreateSectionView);
            this.panel5.Controls.Add(this.btn_RefreshSectionView);
            this.panel5.Location = new System.Drawing.Point(12, 185);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(300, 86);
            this.panel5.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightGray;
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(319, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(732, 573);
            this.panel3.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 555);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(732, 18);
            this.panel4.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1057, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.скопироватьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 26);
            // 
            // скопироватьToolStripMenuItem
            // 
            this.скопироватьToolStripMenuItem.Name = "скопироватьToolStripMenuItem";
            this.скопироватьToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.скопироватьToolStripMenuItem.Text = "Скопировать";
            this.скопироватьToolStripMenuItem.Click += new System.EventHandler(this.скопироватьToolStripMenuItem_Click);
            // 
            // LintelBeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1081, 630);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1080, 630);
            this.Name = "LintelBeam";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.LintelBeam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_Check;
        private System.Windows.Forms.Button btn_CreateSectionView;
        private System.Windows.Forms.Button btn_CreateViewImage;
        
        private System.Windows.Forms.Button btn_Remark;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label_NoSection;
        private System.Windows.Forms.Button btn_RefreshSectionView;
        private System.Windows.Forms.Label label_SectionInvalid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton Rbtn_Mark_Auto;
        private System.Windows.Forms.RadioButton Rbtn_Mark_Manual;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem скопироватьToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ItemSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionStatus;
        private System.Windows.Forms.DataGridViewImageColumn ImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.CheckBox checkBox1;
        
    }
}