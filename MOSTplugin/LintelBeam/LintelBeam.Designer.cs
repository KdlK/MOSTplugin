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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Check = new System.Windows.Forms.Button();
            this.btn_CreateSectionView = new System.Windows.Forms.Button();
            this.btn_CreateViewImage = new System.Windows.Forms.Button();
            this.SectionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Mark,
            this.Count,
            this.SectionStatus});
            this.dataGridView1.Location = new System.Drawing.Point(270, 73);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(482, 283);
            this.dataGridView1.TabIndex = 0;
            // 
            // Code
            // 
            this.Code.HeaderText = "Код";
            this.Code.Name = "Code";
            // 
            // Mark
            // 
            this.Mark.HeaderText = "Марка";
            this.Mark.Name = "Mark";
            // 
            // Count
            // 
            this.Count.HeaderText = "Кол-во";
            this.Count.Name = "Count";
            // 
            // btn_Check
            // 
            this.btn_Check.Location = new System.Drawing.Point(12, 54);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(143, 23);
            this.btn_Check.TabIndex = 1;
            this.btn_Check.Text = "Запустить проверку";
            this.btn_Check.UseVisualStyleBackColor = true;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // btn_CreateSectionView
            // 
            this.btn_CreateSectionView.Location = new System.Drawing.Point(13, 84);
            this.btn_CreateSectionView.Name = "btn_CreateSectionView";
            this.btn_CreateSectionView.Size = new System.Drawing.Size(142, 23);
            this.btn_CreateSectionView.TabIndex = 2;
            this.btn_CreateSectionView.Text = "Создать разрезы";
            this.btn_CreateSectionView.UseVisualStyleBackColor = true;
            this.btn_CreateSectionView.Click += new System.EventHandler(this.btn_CreateSection_Click);
            // 
            // btn_CreateViewImage
            // 
            this.btn_CreateViewImage.Location = new System.Drawing.Point(13, 114);
            this.btn_CreateViewImage.Name = "btn_CreateViewImage";
            this.btn_CreateViewImage.Size = new System.Drawing.Size(142, 23);
            this.btn_CreateViewImage.TabIndex = 3;
            this.btn_CreateViewImage.Text = "Создать изображение";
            this.btn_CreateViewImage.UseVisualStyleBackColor = true;
            this.btn_CreateViewImage.Click += new System.EventHandler(this.btn_CreateImegeView_Click);
            // 
            // SectionStatus
            // 
            this.SectionStatus.HeaderText = "Разрез";
            this.SectionStatus.Name = "SectionStatus";
            // 
            // LintelBeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_CreateViewImage);
            this.Controls.Add(this.btn_CreateSectionView);
            this.Controls.Add(this.btn_Check);
            this.Controls.Add(this.dataGridView1);
            this.Name = "LintelBeam";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.Button btn_CreateSectionView;
        private System.Windows.Forms.Button btn_CreateViewImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionStatus;
    }
}