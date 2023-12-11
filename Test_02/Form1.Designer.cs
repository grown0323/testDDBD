namespace Test_02
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Xindex = new System.Windows.Forms.TextBox();
            this.Yindex = new System.Windows.Forms.TextBox();
            this.AddBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.AllpinBtn = new System.Windows.Forms.Button();
            this.ClrBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MoreDistchk = new System.Windows.Forms.CheckBox();
            this.listReset = new System.Windows.Forms.Button();
            this.crdCheck = new System.Windows.Forms.CheckBox();
            this.Zoom_chk = new System.Windows.Forms.CheckBox();
            this.sizeInfo = new System.Windows.Forms.TextBox();
            this.sizeInfoBtn = new System.Windows.Forms.Button();
            this.lotate = new System.Windows.Forms.TextBox();
            this.DrgCheckBox = new System.Windows.Forms.CheckBox();
            this.SlcDelBtn = new System.Windows.Forms.Button();
            this.DselectedList = new System.Windows.Forms.ListBox();
            this.DisBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.newMyDraw1 = new Test_02.NewMyDraw();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "X 좌표";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 26);
            this.button1.TabIndex = 2;
            this.button1.Text = "Pin Point";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Y 좌표";
            // 
            // Xindex
            // 
            this.Xindex.Location = new System.Drawing.Point(51, 9);
            this.Xindex.Name = "Xindex";
            this.Xindex.Size = new System.Drawing.Size(185, 21);
            this.Xindex.TabIndex = 0;
            // 
            // Yindex
            // 
            this.Yindex.Location = new System.Drawing.Point(51, 37);
            this.Yindex.Name = "Yindex";
            this.Yindex.Size = new System.Drawing.Size(185, 21);
            this.Yindex.TabIndex = 1;
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(27, 202);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(65, 26);
            this.AddBtn.TabIndex = 6;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(27, 96);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(227, 100);
            this.listBox1.TabIndex = 7;
            this.listBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseClick);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // AllpinBtn
            // 
            this.AllpinBtn.Location = new System.Drawing.Point(27, 234);
            this.AllpinBtn.Name = "AllpinBtn";
            this.AllpinBtn.Size = new System.Drawing.Size(227, 34);
            this.AllpinBtn.TabIndex = 8;
            this.AllpinBtn.Text = "ALL_Pin";
            this.AllpinBtn.UseVisualStyleBackColor = true;
            this.AllpinBtn.Click += new System.EventHandler(this.AllPinBtn_Click);
            // 
            // ClrBtn
            // 
            this.ClrBtn.Location = new System.Drawing.Point(23, 777);
            this.ClrBtn.Name = "ClrBtn";
            this.ClrBtn.Size = new System.Drawing.Size(227, 35);
            this.ClrBtn.TabIndex = 9;
            this.ClrBtn.Text = "RESET";
            this.ClrBtn.UseVisualStyleBackColor = true;
            this.ClrBtn.Click += new System.EventHandler(this.ClrBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(98, 202);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 26);
            this.button2.TabIndex = 10;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.MoreDistchk);
            this.panel1.Controls.Add(this.listReset);
            this.panel1.Controls.Add(this.crdCheck);
            this.panel1.Controls.Add(this.Zoom_chk);
            this.panel1.Controls.Add(this.sizeInfo);
            this.panel1.Controls.Add(this.sizeInfoBtn);
            this.panel1.Controls.Add(this.lotate);
            this.panel1.Controls.Add(this.DrgCheckBox);
            this.panel1.Controls.Add(this.SlcDelBtn);
            this.panel1.Controls.Add(this.DselectedList);
            this.panel1.Controls.Add(this.DisBtn);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.ClrBtn);
            this.panel1.Controls.Add(this.AllpinBtn);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.AddBtn);
            this.panel1.Controls.Add(this.Yindex);
            this.panel1.Controls.Add(this.Xindex);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(257, 824);
            this.panel1.TabIndex = 6;
            // 
            // MoreDistchk
            // 
            this.MoreDistchk.AutoSize = true;
            this.MoreDistchk.Location = new System.Drawing.Point(25, 432);
            this.MoreDistchk.Name = "MoreDistchk";
            this.MoreDistchk.Size = new System.Drawing.Size(79, 16);
            this.MoreDistchk.TabIndex = 21;
            this.MoreDistchk.Text = "Distance+";
            this.MoreDistchk.UseVisualStyleBackColor = true;
            this.MoreDistchk.CheckedChanged += new System.EventHandler(this.MoreDistchk_CheckedChanged);
            // 
            // listReset
            // 
            this.listReset.Location = new System.Drawing.Point(27, 369);
            this.listReset.Name = "listReset";
            this.listReset.Size = new System.Drawing.Size(223, 23);
            this.listReset.TabIndex = 20;
            this.listReset.Text = "Selected List Clear";
            this.listReset.UseVisualStyleBackColor = true;
            this.listReset.Click += new System.EventHandler(this.listReset_Click);
            // 
            // crdCheck
            // 
            this.crdCheck.AutoSize = true;
            this.crdCheck.Location = new System.Drawing.Point(135, 453);
            this.crdCheck.Name = "crdCheck";
            this.crdCheck.Size = new System.Drawing.Size(85, 16);
            this.crdCheck.TabIndex = 19;
            this.crdCheck.Text = "Coordinate";
            this.crdCheck.UseVisualStyleBackColor = true;
            this.crdCheck.CheckedChanged += new System.EventHandler(this.crdCheck_CheckedChanged);
            // 
            // Zoom_chk
            // 
            this.Zoom_chk.AutoSize = true;
            this.Zoom_chk.Location = new System.Drawing.Point(135, 431);
            this.Zoom_chk.Name = "Zoom_chk";
            this.Zoom_chk.Size = new System.Drawing.Size(89, 16);
            this.Zoom_chk.TabIndex = 18;
            this.Zoom_chk.Text = "Zoom_Drag";
            this.Zoom_chk.UseVisualStyleBackColor = true;
            this.Zoom_chk.CheckedChanged += new System.EventHandler(this.Zoom_chk_CheckedChanged);
            // 
            // sizeInfo
            // 
            this.sizeInfo.Location = new System.Drawing.Point(19, 680);
            this.sizeInfo.Name = "sizeInfo";
            this.sizeInfo.Size = new System.Drawing.Size(100, 21);
            this.sizeInfo.TabIndex = 17;
            // 
            // sizeInfoBtn
            // 
            this.sizeInfoBtn.Location = new System.Drawing.Point(19, 711);
            this.sizeInfoBtn.Name = "sizeInfoBtn";
            this.sizeInfoBtn.Size = new System.Drawing.Size(92, 25);
            this.sizeInfoBtn.TabIndex = 16;
            this.sizeInfoBtn.Text = "Size Change";
            this.sizeInfoBtn.UseVisualStyleBackColor = true;
            this.sizeInfoBtn.Click += new System.EventHandler(this.sizeInfoBtn_Click);
            // 
            // lotate
            // 
            this.lotate.Location = new System.Drawing.Point(135, 475);
            this.lotate.Name = "lotate";
            this.lotate.ReadOnly = true;
            this.lotate.Size = new System.Drawing.Size(100, 21);
            this.lotate.TabIndex = 15;
            // 
            // DrgCheckBox
            // 
            this.DrgCheckBox.AutoSize = true;
            this.DrgCheckBox.Location = new System.Drawing.Point(135, 409);
            this.DrgCheckBox.Name = "DrgCheckBox";
            this.DrgCheckBox.Size = new System.Drawing.Size(81, 16);
            this.DrgCheckBox.TabIndex = 14;
            this.DrgCheckBox.Text = "Drag_Hold";
            this.DrgCheckBox.UseVisualStyleBackColor = true;
            this.DrgCheckBox.CheckedChanged += new System.EventHandler(this.DrgCheckBox_CheckedChanged);
            // 
            // SlcDelBtn
            // 
            this.SlcDelBtn.Location = new System.Drawing.Point(25, 402);
            this.SlcDelBtn.Name = "SlcDelBtn";
            this.SlcDelBtn.Size = new System.Drawing.Size(75, 23);
            this.SlcDelBtn.TabIndex = 12;
            this.SlcDelBtn.Text = "Del_All";
            this.SlcDelBtn.UseVisualStyleBackColor = true;
            this.SlcDelBtn.Click += new System.EventHandler(this.SlcDelBtn_Click);
            // 
            // DselectedList
            // 
            this.DselectedList.FormattingEnabled = true;
            this.DselectedList.ItemHeight = 12;
            this.DselectedList.Location = new System.Drawing.Point(28, 274);
            this.DselectedList.Name = "DselectedList";
            this.DselectedList.Size = new System.Drawing.Size(223, 88);
            this.DselectedList.TabIndex = 11;
            this.DselectedList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DselectedList_MouseClick);
            this.DselectedList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DselectedList_MouseDoubleClick);
            // 
            // DisBtn
            // 
            this.DisBtn.Location = new System.Drawing.Point(179, 202);
            this.DisBtn.Name = "DisBtn";
            this.DisBtn.Size = new System.Drawing.Size(75, 26);
            this.DisBtn.TabIndex = 6;
            this.DisBtn.Text = "Distance";
            this.DisBtn.UseVisualStyleBackColor = true;
            this.DisBtn.Click += new System.EventHandler(this.DisBtn_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.newMyDraw1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(257, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(824, 824);
            this.panel2.TabIndex = 7;
            // 
            // newMyDraw1
            // 
            this.newMyDraw1.AutoSize = true;
            this.newMyDraw1.chk = false;
            this.newMyDraw1.CrdnChk = false;
            this.newMyDraw1.Distance = null;
            this.newMyDraw1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newMyDraw1.DragEndX = null;
            this.newMyDraw1.DragEndY = null;
            this.newMyDraw1.DragInd = null;
            this.newMyDraw1.DragStartX = null;
            this.newMyDraw1.DragStartY = null;
            this.newMyDraw1.isCollectRange = false;
            this.newMyDraw1.isDist = false;
            this.newMyDraw1.IsDragChked = false;
            this.newMyDraw1.isDrawRect = false;
            this.newMyDraw1.isHighL = false;
            this.newMyDraw1.isList = false;
            this.newMyDraw1.IsNewDrawMap = false;
            this.newMyDraw1.IsSizeDraw = true;
            this.newMyDraw1.IsUnCheckedEnlg = false;
            this.newMyDraw1.Location = new System.Drawing.Point(0, 0);
            this.newMyDraw1.movingX = 0F;
            this.newMyDraw1.movingY = 0F;
            this.newMyDraw1.Name = "newMyDraw1";
            this.newMyDraw1.NonExpEndX = null;
            this.newMyDraw1.NonExpEndY = null;
            this.newMyDraw1.NonExpStartX = null;
            this.newMyDraw1.NonExpStartY = null;
            this.newMyDraw1.NonMouseUpX = null;
            this.newMyDraw1.NonMouseUpY = null;
            this.newMyDraw1.PicLotate = 0;
            this.newMyDraw1.points = null;
            this.newMyDraw1.PrevDrgX1 = null;
            this.newMyDraw1.PrevDrgX2 = null;
            this.newMyDraw1.PrevDrgY1 = null;
            this.newMyDraw1.PrevDrgY2 = null;
            this.newMyDraw1.realX = 0F;
            this.newMyDraw1.realY = 0F;
            this.newMyDraw1.selectedPoint = "";
            this.newMyDraw1.Size = new System.Drawing.Size(824, 824);
            this.newMyDraw1.SizeInfo = 500F;
            this.newMyDraw1.SizeLock = false;
            this.newMyDraw1.TabIndex = 5;
            this.newMyDraw1.x2 = 0F;
            this.newMyDraw1.y2 = 0F;
            this.newMyDraw1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.newMyDraw1_MouseClick);
            this.newMyDraw1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.newMyDraw1_MouseDoubleClick);
            this.newMyDraw1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.newMyDraw1_MouseDown);
            this.newMyDraw1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.newMyDraw1_MouseMove);
            this.newMyDraw1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.newMyDraw1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 824);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Xindex;
        private System.Windows.Forms.TextBox Yindex;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button AllpinBtn;
        private System.Windows.Forms.Button ClrBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private NewMyDraw newMyDraw1;
        private System.Windows.Forms.Button DisBtn;
        private System.Windows.Forms.ListBox DselectedList;
        private System.Windows.Forms.Button SlcDelBtn;
        private System.Windows.Forms.CheckBox DrgCheckBox;
        private System.Windows.Forms.TextBox lotate;
        private System.Windows.Forms.TextBox sizeInfo;
        private System.Windows.Forms.Button sizeInfoBtn;
        private System.Windows.Forms.CheckBox Zoom_chk;
        private System.Windows.Forms.CheckBox crdCheck;
        private System.Windows.Forms.Button listReset;
        private System.Windows.Forms.CheckBox MoreDistchk;
    }
}

