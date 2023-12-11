using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_02
{
    public partial class Form1 : Form
    {

        #region 확대 범위 재지정 시, 변수
        string prevX1 = string.Empty;
        string prevY1 = string.Empty;
        string prevX2 = string.Empty;
        string prevY2 = string.Empty;   
/*        string nextX1 = string.Empty;
        string nextY1 = string.Empty;
        string nextX2 = string.Empty;
        string nextY2 = string.Empty;*/
        #endregion


        //ListBox 값을 저장할 List
        List<string> points = new List<string>();

        //거리 측정에 쓸 List
        List<string> DisList = new List<string>();

        List<string> RangeList = new List<string>();


        //form 시작 시 컨트롤 기본 사이즈 (
        private const int OriginalWidth = 824;
        private const int OriginalHeight = 824;

        bool ClickEv = false;
        bool DrgChk = false;

        bool Zoomchk = false;
        bool ZoomRangeSelected = false;
        bool ChkEv = false;
        bool CrdnChk = false;
        bool MoveDrg = false;
        bool DistMarkMove = false;
        bool SizeLock = false;

        float SizeOfX = 500;
        float SizeOfY = 500;
            
        string DragStartX;
        string DragStartY;
        string DragEndX;
        string DragEndY;
        string MvStartX;
        string MvStartY;
        string MvEndX;
        string MvEndY;


        int MvDisMore;
        int AldMvDis;


        int selectedIndex = -1;
        float ClickX;
        float ClickY;
        float Xind;
        float Yind;
        public static Form1 f;

        string MoveX;
        string MoveY;
        string MvDelPointX;
        string MvDelPointY;

        float currentWidth = 0;
        float currentHeight =0;
        public Form1()
        {
            InitializeComponent();
            f = this;
            this.newMyDraw1.MouseWheel += MouseWheelEvent;
            SizeOfX = newMyDraw1.Size.Width;
            SizeOfY = newMyDraw1.Size.Height;
            //this.SizeChanged += panel2_SizeChanged;
            //this.sizeInfoBtn.Click += panel2_SizeChanged;
            //this.MouseClick -= panel2_SizeChanged;
        }

        //마우스 휠을 굴릴 때, 동작할 이벤트 (속성에 없어 새로 작성)
        private void MouseWheelEvent(object sender, MouseEventArgs e)
        {
            //델타 (마우스를 굴리는 방향// 0보다 큼 = 위로 굴림)
            if (e.Delta > 0)
            {
                this.newMyDraw1.PicLotate += 90;
            }
            //0보다 작음 = 아래로 굴림
            else if (e.Delta < 0)
            {
                this.newMyDraw1.PicLotate += -90;
                
            }
            if (this.newMyDraw1.PicLotate >= 360 || this.newMyDraw1.PicLotate <= -360)
            {
                this.newMyDraw1.PicLotate = 0;
            }
            lotate.Text = "" + this.newMyDraw1.PicLotate;

            if (Zoom_chk.Checked == false)
            {
                #region 드래그 사라지게
                this.newMyDraw1.DragStartX = null;
                this.newMyDraw1.DragStartY = null;
                this.newMyDraw1.DragEndX = null;
                this.newMyDraw1.DragEndY = null;
                this.newMyDraw1.isDrawRect = false;
                #endregion
            }


            this.newMyDraw1.Invalidate();
        }

        //Pin Point (점 하나만 찍어보기 버튼 -> 다중 점 List는 유지되나, 마킹된 다중 점은 bitmap 이미지에서 사라짐)
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Xindex.Text == "")
                {
                    Xind = 0;
                }
                else
                {
                    Xind = float.Parse(Xindex.Text);
                }
                if (Yindex.Text == "")
                {
                    Yind = 0;
                }
                else
                {
                    Yind = float.Parse(Yindex.Text);
                }
                PointEv(Xind, Yind);
                this.newMyDraw1.isDrawRect=false;
                RangeList.Clear();
                DselectedList.DataSource = null;
                DselectedList.DataSource = RangeList;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Index Error : " + ex);
                Clear_TextBox();
                return;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Xindex.Text == "")
                {
                    Xind = 0;
                }
                else
                {
                    Xind = float.Parse(Xindex.Text);
                }
                if (Yindex.Text == "")
                {
                    Yind = 0;
                }
                else
                {
                    Yind = float.Parse(Yindex.Text);
                }


                points.Add(Xind + "," + Yind);
                points = points.Distinct().ToList();
                listBox1.DataSource = null;
                listBox1.DataSource = points;
                Clear_TextBox();
                selectedIndex = 0;
                this.newMyDraw1.isList = false;
                this.newMyDraw1.isDist = false;

            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Index Error : ");
                Clear_TextBox();
                return;
            }
        }

        //다중 점 찍는 AllPin 버튼 클릭 시 -> MultiPinPointed()로 넘어감)
        private void AllPinBtn_Click(object sender, EventArgs e)
        {
            MultiPinPointed();
            
        }


        //마우스 우클릭, 회전 등 다양한 요소에 사용 (Pin Point를 눌렀을 때, 작동하는 NewMyDraw1의 DrawCircle 이벤트를 반복하여 찍음)  
        public void MultiPinPointed()
        {
            
            listBox1.SelectedItem = Xindex.Text + "," + Yindex.Text;
            if (RangeList.Contains(Xindex.Text + "," + Yindex.Text))
            {
                DselectedList.SelectedItem = Xindex.Text + "," + Yindex.Text;
            }
            this.newMyDraw1.chk = true;
            this.newMyDraw1.points = points; 
            this.newMyDraw1.isList = true;
            if (DrgChk == true)
            {
                SearchRangePoint(ChkEv);
            }
            this.newMyDraw1.Invalidate();
            this.newMyDraw1.Update();
        }

        #region 전부 삭제 (화면 초기화)
        private void ClrBtn_Click(object sender, EventArgs e)
        {
            ClearEv();
        }

        private void ClearEv()
        {
            this.WindowState = FormWindowState.Normal;
            sizeInfo.Text = "";
            PanelSizeIndexing(false);
            this.newMyDraw1.SizeLock = false;
            SizeLock = false;
            Width = Width + 1;
            Width = Width - 1;
            points.Clear();
            RangeList.Clear();
            this.newMyDraw1.isHighL = false;
            this.newMyDraw1.isList = false;
            this.newMyDraw1.chk = false;
            this.newMyDraw1.isDist = false;
            DisList.Clear();
            listBox1.DataSource = null;
            listBox1.DataSource = points;
            DselectedList.DataSource = null;
            DselectedList.DataSource = RangeList;
            selectedIndex = -1;
            this.newMyDraw1.DragStartX = null;
            this.newMyDraw1.DragStartY = null;
            this.newMyDraw1.DragEndX = null;
            this.newMyDraw1.DragEndY = null;
            this.newMyDraw1.isDrawRect = false;
            ChkEv = false;
            this.newMyDraw1.IsNewDrawMap = false;
            this.newMyDraw1.IsDragChked = false;
            /*Zoomchk = false;*/
            ZoomRangeSelected = false;
            Zoom_chk.Checked = false;
            this.newMyDraw1.SizeDrgInfo = 0;
            this.newMyDraw1.IsSizeDraw = true;
            this.newMyDraw1.DrgEgY = 0;
            this.newMyDraw1.IsUnCheckedEnlg = false;
            this.newMyDraw1.Invalidate();
        }
        #endregion

        private void Marked_Box(MouseEventArgs e,int EvNum)
        {

            //selectedIndex = 마우스를 통해 ListBox1에서 선택한 요소 (상위의 listBox1_MouseClick 또는 MouseDoubleClick 에서 선택
            //값을 넘겨줌)
            if (selectedIndex != -1)
            {

                if (this.newMyDraw1.isList == true)
                {
                    string SelectedPoint = string.Empty;
                    if (EvNum == 0)
                    {
                        this.newMyDraw1.isHighL = true;
                        SelectedPoint = listBox1.Items[selectedIndex] as string;

                        if (RangeList.Contains(SelectedPoint))
                        {
                            DselectedList.SelectedItem = SelectedPoint;
                        }
                        else
                        {
                            RangeList.Add(SelectedPoint);
                            DselectedList.DataSource = null;
                            DselectedList.DataSource = RangeList;
                            DselectedList.SelectedItem = SelectedPoint;
                        }
                    }
                    else if (EvNum == 1)
                    {
                        SelectedPoint = DselectedList.Items[selectedIndex] as string;
                        listBox1.SelectedItem = SelectedPoint;
                    }
          
                    this.newMyDraw1.selectedPoint = SelectedPoint;
                    this.newMyDraw1.isHighL = true;
                }

            }
            this.newMyDraw1.Invalidate();
        }
        

        
        //delete
        private void button2_Click(object sender, EventArgs e)
        {

            //listBox에서 요소를 선택하였을 때 , 
            if (selectedIndex != -1)
            {
                string SelectedPoint = string.Empty;
                SelectedPoint = listBox1.Items[selectedIndex] as string;
                if (ClickEv == true)
                {
                    SelectedPoint = Xindex.Text + "," + Yindex.Text;
                    points.Remove(SelectedPoint);

                }
                else
                {
                    points.RemoveAt(selectedIndex);
                }
                if (RangeList.Contains(SelectedPoint))
                {
                    SelectedPoint = DselectedList.Items[selectedIndex] as string;
                    if (ClickEv == true)
                    {
                        SelectedPoint = Xindex.Text + "," + Yindex.Text;
                        RangeList.Remove(SelectedPoint);
                    }
                    else
                    {
                        RangeList.Remove(SelectedPoint);
                    }
                }
                

                
                listBox1.DataSource = null;
                listBox1.DataSource = points;
                DselectedList.DataSource = null;
                DselectedList.DataSource = RangeList;
                this.newMyDraw1.isHighL = false;
                if (DisList.Contains(SelectedPoint))
                {
                    DisList.Remove(SelectedPoint);
                    this.newMyDraw1.Distance = DisList;
                }
/*                else if (DisList.Contains(SelectedPoint))
                {
                    DisList.Remove(SelectedPoint);
                    this.newMyDraw1.Distance = DisList;
                }*/
                ClickEv = false;
            


                if (points.Count == 0)
                {
                    selectedIndex = -1;
                }

            }
            this.newMyDraw1.Invalidate();
        }

        public void Clear_TextBox()
        {
                Xindex.Text = string.Empty;
                Yindex.Text = string.Empty;
        }

        private void DisBtn_Click(object sender, EventArgs e)
        {
            if (selectedIndex != -1)
            {
                string SelectedPoint = string.Empty;
                if (MoreDistchk.Checked == false)
                {
                    this.newMyDraw1.MoreDistchk = false;
                    if (DisList.Count >= 2)
                    {
                        DisList.Clear();
                        this.newMyDraw1.Distance = DisList;

                    }
                }
                else
                {
                    this.newMyDraw1.MoreDistchk = true;
                }
                if (ClickEv == false)
                {
                    SelectedPoint = Xindex.Text + "," + Yindex.Text;
                    if (points.Contains(SelectedPoint) == false)
                    {
                        return;

                    }
                    listBox1.SelectedItem = SelectedPoint;
                    if (RangeList.Contains(SelectedPoint))
                    {
                        DselectedList.SelectedItem = SelectedPoint;
                    }
                }
                else
                {
                    SelectedPoint = Xindex.Text + "," + Yindex.Text;
                }
                if (DisList.Contains(SelectedPoint))
                {
                    AldMvDis = DisList.IndexOf(SelectedPoint);
                }
                DisList.Add(SelectedPoint);
                this.newMyDraw1.Distance = DisList;

                this.newMyDraw1.isHighL = false;
                this.newMyDraw1.isDist = true;
                //ClickEv = false;


                this.newMyDraw1.Invalidate();

            }
        }

        private void selectedListValue(Point point, int EvNum)
        {
            try
            {
                string SelectedPoint = string.Empty;
                if (EvNum == 0)
                {
                    selectedIndex = listBox1.IndexFromPoint(point);
                    SelectedPoint = listBox1.Items[selectedIndex] as string;
                    if (RangeList.Contains(SelectedPoint))
                    {
                        DselectedList.SelectedItem = SelectedPoint;
                    }
                }
                else if (EvNum == 1)
                {
                    selectedIndex = DselectedList.IndexFromPoint(point);
                    SelectedPoint = DselectedList.Items[selectedIndex] as string;
                    listBox1.SelectedItem = SelectedPoint;
                }

                string[] valuse = SelectedPoint.Split(',');
                Xindex.Text = valuse[0];
                Yindex.Text = valuse[1];
            }
            catch(Exception ex)
            {
                return;
            }
        }


        private void SearchRangePoint(bool Evchk)
        {
            RangeList.Clear();

            float x1 = -2550;
            float x2 = -2550;
            float y1 = -2550;
            float y2 = -2550;

            if (Evchk == true)
            {
                try {
                    x1 = float.Parse(this.newMyDraw1.DragStartX);
                    y1 = float.Parse(this.newMyDraw1.DragStartY);

                    x2 = float.Parse(this.newMyDraw1.DragEndX);
                    y2 = float.Parse(this.newMyDraw1.DragEndY);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else
            {
                try
                {
                    x1 = float.Parse(this.newMyDraw1.DragStartX);
                    y1 = float.Parse(this.newMyDraw1.DragStartY);

                    x2 = float.Parse(this.newMyDraw1.DragEndX);
                    y2 = float.Parse(this.newMyDraw1.DragEndY);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            foreach (string tempRangeList in points)
            {
                string[] valuse = tempRangeList.Split(',');
                float Selcx = float.Parse(valuse[0]);
                float Selcy = float.Parse(valuse[1]);
                if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
                {
                    var temp = Selcx;
                    Selcx = Selcy;
                    Selcy = -temp;
                }
                else if (this.newMyDraw1.PicLotate == -180 || this.newMyDraw1.PicLotate == 180)
                {
                    Selcx = -Selcx;
                    Selcy = -Selcy;
                }
                else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
                {
                    var temp = Selcx;
                    Selcx = -Selcy;
                    Selcy = temp;
                }
                #region 0도일때
                //////////////////////
                if (x1 < x2)
                {
                    if (y1 < y2)
                    {
                        if (Selcx > x1 & Selcx < x2 & Selcy > y1 & Selcy < y2)
                        {
                            RangeList.Add(valuse[0] + "," + valuse[1]);
                        }
                    }
                    else
                    {
                        if (Selcx > x1 & Selcx < x2 & Selcy < y1 & Selcy > y2)
                        {
                            RangeList.Add(valuse[0] + "," + valuse[1]);
                        }
                    }
                }
                else
                {
                    if (y1 < y2)
                    {
                        if (Selcx < x1 & Selcx > x2 & Selcy > y1 & Selcy < y2)
                        {
                            RangeList.Add(valuse[0] + "," + valuse[1]);
                        }
                    }
                    else
                    {
                        if (Selcx < x1 & Selcx > x2 & Selcy < y1 & Selcy > y2)
                        {
                            RangeList.Add(valuse[0] + "," + valuse[1]);
                        }
                    }
                }
                /////////////////////
                #endregion

            }

            this.newMyDraw1.isCollectRange = true;
            this.newMyDraw1.DragInd = RangeList;
            DselectedList.DataSource = null;
            DselectedList.DataSource = RangeList;

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;
            selectedListValue(point, 0);
        }


        #region 리스트 박스 더블클릭
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;
            selectedIndex = listBox1.IndexFromPoint(point);

            Marked_Box(e, 0);
        }
        #endregion
        private void DselectedList_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;
            selectedListValue(point,1);
        }

        private void DselectedList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;
            selectedIndex = DselectedList.IndexFromPoint(point);

            Marked_Box(e,1);
        }



        public void PointEv(float Xind, float Yind)
        {
            try
            {
                this.newMyDraw1.isList = false;
                this.newMyDraw1.x2 = Xind;
                this.newMyDraw1.y2 = Yind;
                this.newMyDraw1.chk = true;
                this.newMyDraw1.isHighL = false;

                //화면을 새로 그려 ->  OnPaint를 실행.
                this.newMyDraw1.Invalidate();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wrong Index Error : " + e);
                Clear_TextBox();
                return;
            }
        }

        private void SlcDelBtn_Click(object sender, EventArgs e)
        {
            if (RangeList != null)
            {
                foreach (string DelList in RangeList)
                {
                    points.Remove(DelList);
                    if (DisList.Contains(DelList))
                    {
                        this.newMyDraw1.isDist = false;
                    }
                    ClickEv = false;
                }
                RangeList.Clear();
                listBox1.DataSource = null;
                listBox1.DataSource = points;

                DselectedList.DataSource = null;
                DselectedList.DataSource = RangeList;

                this.newMyDraw1.Invalidate();

                

            }
        }

        private void DrgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DrgChk == true)
            {
                DrgChk = false;
                if (Zoomchk == false)
                {
                    this.newMyDraw1.IsUnCheckedEnlg = true;
                }
            }
            else
            {
                Zoom_chk.Checked = false;
                DrgChk = true;
                RangeList.Clear();
                DselectedList.DataSource = null;
                DselectedList.DataSource = RangeList;
                this.newMyDraw1.IsSizeDraw = true;
                this.newMyDraw1.IsUnCheckedEnlg = true;
                
                //확대 된 상황이라면 확대를 할 때 사용했던 Drag값들을 초기화. 안 할 시, 휠을 클릭하면 저장된 값이 바로 나와버림.
                this.newMyDraw1.DragStartX = null;
                this.newMyDraw1.DragStartY = null;
                this.newMyDraw1.DragEndY = null;
                this.newMyDraw1.DragEndX = null;
            }

            this.newMyDraw1.IsSizeDraw = false;
            this.newMyDraw1.isDrawRect = false;
            
            
            this.newMyDraw1.Invalidate();
        }

        #region 콘트롤을 정사각형 유지 시
        /*private void panel2_SizeChanged(object s, EventArgs e)
        {
            currentWidth = panel2.Width;
            currentHeight = panel2.Height;

            float widthRatio = (float)currentWidth / OriginalWidth;
            float HeightRatio = (float)currentHeight / OriginalHeight;

            this.newMyDraw1.MaximumSize = new Size((int)panel2.Height, (int)panel2.Height);

            this. Height = (int)(OriginalHeight * widthRatio);
        }*/
        #endregion

        private void PanelSizeIndexing(bool chk)
        {
            
            if (chk == false)
            {
                this.newMyDraw1.MaximumSize = new Size(0,0);
                SizeOfX = newMyDraw1.Size.Width;
                SizeOfY = newMyDraw1.Size.Height;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;

                //사이즈를 줄 때 SizeOfX와 SizeOfY가 각자 내에서 /2를 하기 때문에 *2를 한 값을 준다
                this.newMyDraw1.MaximumSize = new Size((int)panel2.Height, (int)panel2.Height);
                this.newMyDraw1.SizeLock = true;
                try
                {
                    this.newMyDraw1.SizeInfo = float.Parse(sizeInfo.Text);
                }
                catch
                {
                    return;
                }
                //SizeOfX = float.Parse(sizeInfo.Text);
                //SizeOfY = float.Parse(sizeInfo.Text);
                this.newMyDraw1.Invalidate();
            }
        }

        private void newMyDraw1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;

            

            string Xrnd = "0";
            string Yrnd = "0";
            if (SizeOfX > 10 || SizeOfY > 10)
            {
                Xrnd = ClickX.ToString("0");
                Yrnd = ClickY.ToString("0");
            }
            else
            {
                Xrnd = ClickX.ToString("0.#");
                Yrnd = ClickY.ToString("0.#");
            }
            /*if(e.Button == MouseButtons.Left)
            {
                if (points.Contains(Xrnd + "," + Yrnd) == false)
                {
                    this.newMyDraw1.DragInd=null;
                }
            }*/
            if (e.Button == MouseButtons.Right)
            {
                this.newMyDraw1.isHighL = false;

                if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
                {
                    var temp = Xrnd;
                    Xrnd = "" + -float.Parse(Yrnd);
                    Yrnd = temp;
                }
                else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
                {
                    var temp = Xrnd;
                    Xrnd = Yrnd;
                    Yrnd = "" + -float.Parse(temp);
                }
                else if (Math.Abs(this.newMyDraw1.PicLotate) == 180)
                {
                    Xrnd = "" + -float.Parse(Xrnd);
                    Yrnd = "" + -float.Parse(Yrnd);
                }

                points.Add(Xrnd + "," + Yrnd);
                points = points.Distinct().ToList();
                listBox1.DataSource = null;
                listBox1.DataSource = points;
                string SelectedPoint = Xrnd + "," + Yrnd;
                listBox1.SelectedItem = SelectedPoint;
                if (RangeList.Contains(SelectedPoint))
                {
                    DselectedList.SelectedItem = SelectedPoint;
                }
                selectedIndex = 0;
                this.newMyDraw1.selectedPoint = Xrnd + "," + Yrnd;
                Xindex.Text = Xrnd;
                Yindex.Text = Yrnd;
                ClickEv = true;

                //PointEv(float.Parse(Xrnd), float.Parse(Yrnd));
                MultiPinPointed();
                if (DrgChk == false)
                {
                    if (Zoomchk == false)
                    {
                        this.newMyDraw1.DragStartX = null;
                        this.newMyDraw1.DragStartY = null;
                        this.newMyDraw1.DragEndY = null;
                        this.newMyDraw1.DragEndX = null;
                    }
                }
                else
                {
                    this.newMyDraw1.isDrawRect = true;
                    SelectedPoint = Xrnd + "," + Yrnd;
                    listBox1.SelectedItem = SelectedPoint;
                    if (RangeList.Contains(SelectedPoint))
                    {
                        DselectedList.SelectedItem = SelectedPoint;
                    }
                }


            }
            else if (e.Button == MouseButtons.Middle)
            {
                //제자리를 클릭하면 (확대 취소)
                if (DragStartX == this.DragEndX && this.DragEndY == DragStartY)
                {
                    this.newMyDraw1.IsUnCheckedEnlg = false;
                    this.WindowState = FormWindowState.Normal;
                    sizeInfo.Text = "";
                    SizeLock = false;
                    this.newMyDraw1.SizeLock = false;
                    this.newMyDraw1.IsNewDrawMap = false;
                    ZoomRangeSelected = false;
                    Zoom_chk.Checked = false;
                    //this.newMyDraw1.SizeDrgInfo = 0;
                    this.newMyDraw1.IsSizeDraw = true;
                    PanelSizeIndexing(false);
                    Width = Width + 1;
                    Zoomchk = false;
                    this.newMyDraw1.Invalidate();
                }

            }

        }

        private void newMyDraw1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.newMyDraw1.chk = false;
            Point point = e.Location;
            ClickX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / (SizeOfX / 10f)) * 10;
            ClickY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / (SizeOfY / 10f)) * 10;


            string Xrnd = ClickX.ToString("0.#");
            string Yrnd = ClickY.ToString("0.#");

            PointEv(float.Parse(Xrnd), float.Parse(Yrnd));

            if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
            {
                var temp = Xrnd;
                Xrnd = "" + -float.Parse(Yrnd);
                Yrnd = temp;
            }
            else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
            {
                var temp = Xrnd;
                Xrnd = Yrnd;
                Yrnd = "" + -float.Parse(temp);
            }
            else if (Math.Abs(this.newMyDraw1.PicLotate) == 180)
            {
                Xrnd = "" + -float.Parse(Xrnd);
                Yrnd = "" + -float.Parse(Yrnd);
            }

            Xindex.Text = Xrnd;
            Yindex.Text = Yrnd;

            RangeList.Clear();
            
            DselectedList.DataSource = null;
            DselectedList.DataSource = RangeList;
            this.newMyDraw1.isDrawRect = false;


        }


        private void newMyDraw1_MouseMove(object sender, MouseEventArgs e)
        {
            if (SizeLock == false)
            {
                SizeOfX = newMyDraw1.Size.Width / 2;
                SizeOfY = newMyDraw1.Size.Height / 2;
            }
            Point point = e.Location;
            float moveX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / SizeOfX);
            float moveY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / SizeOfY);
            float passX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / SizeOfX);
            float passY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / SizeOfY);
            float realX = point.X;
            float realY = point.Y;
            

            //확대할 범위가 선택된 이후에 움직일 때 좌표를 확인하기 위하여 ZoomRangeSelected를 사용함. (움직일 때 변화된 좌표, 오른쪽 클릭 시 바뀐 좌표로 찍는 등)
            if (ZoomRangeSelected == true)
            {
                if (Zoomchk == true)
                {
                    try
                    {
                        if (float.Parse(newMyDraw1.DragStartX) < float.Parse(newMyDraw1.DragEndX))
                        {
                            if (float.Parse(newMyDraw1.DragStartY) > newMyDraw1.DrgEgY)
                            {

                                float crX = float.Parse(newMyDraw1.DragEndX) - float.Parse(newMyDraw1.DragStartX);  //거리
                                                                                                                    //       최좌측의 실제 좌표 = 0이기 때문에 + 더 작은 x 좌표(이하, x1) 를 계산해주어 결과로 그려진 bitmap의 좌측변 x좌표를 x1으로 만들어 준다.
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.DragStartX));
                                float crY = float.Parse(newMyDraw1.DragStartY) - newMyDraw1.DrgEgY;
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.DragStartY));
                            }
                            else
                            {
                                float crX = float.Parse(newMyDraw1.DragEndX) - float.Parse(newMyDraw1.DragStartX);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.DragStartX));
                                float crY = -float.Parse(newMyDraw1.DragStartY) + newMyDraw1.DrgEgY;
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - newMyDraw1.DrgEgY);
                            }

                        }
                        else
                        {
                            if (float.Parse(newMyDraw1.DragStartY) > newMyDraw1.DrgEgY)
                            {

                                float crX = -float.Parse(newMyDraw1.DragEndX) + float.Parse(newMyDraw1.DragStartX);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.DragEndX));
                                float crY = float.Parse(newMyDraw1.DragStartY) - newMyDraw1.DrgEgY;
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.DragStartY));
                            }
                            else
                            {
                                float crX = -float.Parse(newMyDraw1.DragEndX) + float.Parse(newMyDraw1.DragStartX);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.DragEndX));
                                float crY = -float.Parse(newMyDraw1.DragStartY) + newMyDraw1.DrgEgY;
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - newMyDraw1.DrgEgY);

                                
                            }
                        }
                    }
                    catch (Exception) { return; }
                }
                else
                {
                    try
                    {
                        if (float.Parse(newMyDraw1.PrevDrgX1) < float.Parse(newMyDraw1.PrevDrgX2))
                        {
                            if (float.Parse(newMyDraw1.PrevDrgY1) > float.Parse(newMyDraw1.PrevDrgY2))
                            {

                                float crX = float.Parse(newMyDraw1.PrevDrgX2) - float.Parse(newMyDraw1.PrevDrgX1);  //거리
                                                                                                                    //       최좌측의 실제 좌표 = 0이기 때문에 + 더 작은 x 좌표(이하, x1) 를 계산해주어 결과로 그려진 bitmap의 좌측변 x좌표를 x1으로 만들어 준다.
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.PrevDrgX1));
                                float crY = float.Parse(newMyDraw1.PrevDrgY1) - float.Parse(newMyDraw1.PrevDrgY2);
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.PrevDrgY1));
                            }
                            else
                            {
                                float crX = float.Parse(newMyDraw1.PrevDrgX2) - float.Parse(newMyDraw1.PrevDrgX1);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.PrevDrgX1));
                                float crY = -float.Parse(newMyDraw1.PrevDrgY1) + float.Parse(newMyDraw1.PrevDrgY2);
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.PrevDrgY2));
                            }

                        }
                        else
                        {
                            if (float.Parse(newMyDraw1.PrevDrgY1) > float.Parse(newMyDraw1.PrevDrgY2))
                            {

                                float crX = -float.Parse(newMyDraw1.PrevDrgX2) + float.Parse(newMyDraw1.PrevDrgX1);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.PrevDrgX2));
                                float crY = float.Parse(newMyDraw1.PrevDrgY1) - newMyDraw1.DrgEgY;
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.PrevDrgY1));
                            }
                            else
                            {
                                float crX = -float.Parse(newMyDraw1.PrevDrgX2) + float.Parse(newMyDraw1.PrevDrgX1);  //거리
                                moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(newMyDraw1.PrevDrgX2));
                                float crY = -float.Parse(newMyDraw1.PrevDrgY1) + float.Parse(newMyDraw1.PrevDrgY2);
                                moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(newMyDraw1.PrevDrgY2));


                            }
                        }
                    }
                    catch (Exception) { return; }
                }
            }
            //확대 된 상태일 때
            else if (this.newMyDraw1.DrgEgY != 0)
            {
                
                try
                {
                    if (float.Parse(prevX1) < float.Parse(prevX2))
                    {

                        if (float.Parse(prevY1) > float.Parse(prevY2))
                        {

                            float crX = float.Parse(prevX2) - float.Parse(prevX1);  //거리
                            moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(prevX1));
                            float crY = float.Parse(prevY1) - float.Parse(prevY2);
                            moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(prevY1));
                        }
                        else
                        {
                            float crX = float.Parse(prevX2) - float.Parse(prevX1);  //거리
                            moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(prevX1));
                            float crY = -float.Parse(prevY1) + float.Parse(prevY2);
                            moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(prevY2));
                        }

                    }
                    else
                    {
                        if (float.Parse(prevY1) > float.Parse(prevY2))
                        {
                            //
                            float crX = -float.Parse(prevX2) + float.Parse(prevX1);  //거리
                            moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(prevX2));
                            float crY = float.Parse(prevY1) - float.Parse(prevY2);
                            moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(prevY1));
                        }
                        else
                        {

                            float crX = -float.Parse(prevX2) + float.Parse(prevX1);  //거리
                            moveX = (point.X / (newMyDraw1.Width / crX) + float.Parse(prevX2));
                            float crY = -float.Parse(prevY1) + float.Parse(prevY2);
                            moveY = -(point.Y / (newMyDraw1.Height / crY) - float.Parse(prevY2));


                        }
                    }
                }
                catch (Exception) { return; }
            
            }

            ClickX = moveX;
            ClickY = moveY;
            

            passX = moveX;
            passY = moveY;
            if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
            {
                var temp = moveX;
                moveX = -moveY;
                moveY = temp;

            }
            else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
            {
                var temp = moveX;
                moveX = moveY;
                moveY = -temp;

            }
            else if (this.newMyDraw1.PicLotate == -180 || this.newMyDraw1.PicLotate == 180)
            {
                moveX = -moveX;
                moveY = -moveY;

            }

            #region 드래그 이동 시,
            MoveX = moveX.ToString("0");
            MoveY = moveY.ToString("0");

            if (MoveDrg == true)
            {

                if (e.Button == MouseButtons.Left)
                {

                    points.Remove(MvDelPointX + "," + MvDelPointY);
                    if (DselectedList.Items.Count > 0 == false)
                    {
                        if (DisList.Contains(MvDelPointX + "," + MvDelPointY))
                        {
                            MvDisMore=DisList.IndexOf(MvDelPointX + "," + MvDelPointY);
                            DisList.Remove(MvDelPointX + "," + MvDelPointY);
                            this.newMyDraw1.Distance = DisList;
                            DistMarkMove = true;
                            
                        }
                    }
                    listBox1.DataSource = null;
                    listBox1.DataSource = points;

                    string SelectedPoint = MoveX + "," + MoveY;

                    if (RangeList.Count == 0)
                    {
                        listBox1.SelectedItem = SelectedPoint;
                        if (RangeList.Contains(SelectedPoint))
                        {
                            DselectedList.SelectedItem = SelectedPoint;
                        }
                    }
                    else
                    {
                        DselectedList.SelectedItem = SelectedPoint;
                        listBox1.SelectedItem = SelectedPoint;
                    }
                    this.newMyDraw1.selectedPoint = MoveX + "," + MoveY;
                    this.newMyDraw1.isHighL = true;
                    if (DselectedList.Items.Count > 0)
                    {
                        this.newMyDraw1.isHighL = false;
                    }
                    MoveDrg = true;


                    this.newMyDraw1.Invalidate();

                }
            }
            #endregion

            this.newMyDraw1.movingX = passX;
            this.newMyDraw1.movingY = passY;

            this.newMyDraw1.realX = realX;
            this.newMyDraw1.realY = realY;

            #region 드래그 범위 지정 (확대) 용
            if (Zoomchk == false)
            {
                if ((e.Button == MouseButtons.Middle))
                {
                    if (ZoomRangeSelected == true)
                    { 
                        Xindex.Text = moveX.ToString("0.0");
                        Yindex.Text = moveY.ToString("0.0");
                        this.newMyDraw1.isDrawRect = true;
                        this.newMyDraw1.NonMouseUpX = "" + point.X;
                        this.newMyDraw1.NonMouseUpY = "" + point.Y;
                    }
                    else
                    {
                        /*if (prevX1 != null && prevY1 != null)
                        {
                            this.newMyDraw1.DragStartX = prevX1;
                            this.newMyDraw1.DragStartY = prevY1;
                            this.newMyDraw1.DragEndX = prevX2;
                            this.newMyDraw1.DragEndY = prevY2;
                        }**/
                        Xindex.Text = moveX.ToString("0.#");
                        Yindex.Text = moveY.ToString("0.#");
                        this.newMyDraw1.isDrawRect = true;
                        this.newMyDraw1.NonMouseUpX = "" + passX;
                        this.newMyDraw1.NonMouseUpY = "" + passY;
                    }
                }
            }
            else
            {
                if ((e.Button == MouseButtons.Middle))
                {
                    if (this.newMyDraw1.DrgEgY == 0)
                    {
                        Xindex.Text = moveX.ToString("0.0");
                        Yindex.Text = moveY.ToString("0.0");
                        this.newMyDraw1.isDrawRect = true;
                        this.newMyDraw1.NonMouseUpX = "" + passX;
                        this.newMyDraw1.NonMouseUpY = "" + passY;
                    }
                    
                    else
                    {
                        Xindex.Text = moveX.ToString("0.0");
                        Yindex.Text = moveY.ToString("0.0");
                        this.newMyDraw1.isDrawRect = true;
                        this.newMyDraw1.NonMouseUpX = "" + point.X;
                        this.newMyDraw1.NonMouseUpY = "" + point.Y;
                    }
                }
            }
            #endregion
            this.newMyDraw1.Invalidate();

        }



        private void newMyDraw1_MouseDown(object sender, MouseEventArgs e)
        {
            //this.newMyDraw1.isDrawRect = true;
            if (e.Button == MouseButtons.Left)
            {
                Point point = e.Location;

                string Xrnd = "0";
                string Yrnd = "0";
                if (SizeOfX > 10 || SizeOfY>10)
                {
                    Xrnd = ClickX.ToString("0");
                    Yrnd = ClickY.ToString("0");
                }
                else
                {
                    Xrnd = ClickX.ToString("0.#");
                    Yrnd = ClickY.ToString("0.#");
                }

                if (DselectedList.Items.Count > 0)
                {
                    MvStartX = Xrnd;
                    MvStartY = Yrnd;

                }

                #region 

                if (e.Button == MouseButtons.Left)
                {
                    
                    if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
                    {
                        var temp = float.Parse(Xrnd);
                        Xrnd = "" + -float.Parse(Yrnd);
                        Yrnd = "" + temp;
                    }
                    else if (this.newMyDraw1.PicLotate == 180 || this.newMyDraw1.PicLotate == -180)
                    {
                        Xrnd = "" + -float.Parse(Xrnd);
                        Yrnd = "" + -float.Parse(Yrnd);
                    }
                    else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
                    {
                        var temp = float.Parse(Xrnd);
                        Xrnd = "" + float.Parse(Yrnd);
                        Yrnd = "" + -temp;
                    }

                    Xindex.Text = Xrnd;
                    Yindex.Text = Yrnd;

                    if (points.Contains(Xrnd + "," + Yrnd))
                    {
                        string SelectedPoint = Xrnd + "," + Yrnd;

                        if (RangeList.Count == 0)
                        {
                            listBox1.SelectedItem = SelectedPoint;
                            
                        }
                        else
                        {
                            DselectedList.SelectedItem = SelectedPoint;
                            listBox1.SelectedItem = SelectedPoint;
                            if (RangeList.Contains(SelectedPoint)==false)
                            {
                                RangeList.Add(SelectedPoint);
                                DselectedList.DataSource = null;
                                DselectedList.DataSource = RangeList;
                            }
                            else
                            {
                                DselectedList.DataSource = null;
                                RangeList.Clear();
                                DselectedList.DataSource = RangeList;
                            }

                        }

                        this.newMyDraw1.selectedPoint = Xrnd + "," + Yrnd;
                        this.newMyDraw1.isHighL = true;

                        
                        MvDelPointX = Xrnd;
                        MvDelPointY = Yrnd;

                        ClickEv = true;
                        MoveDrg = true;

                        this.newMyDraw1.Invalidate();

                    }
                    else
                    {
                        this.newMyDraw1.isHighL = false;
                    }
                }
                #endregion
            }
            #region 마우스 휠 조작시 (드래그 범위지정 등)
            if ((e.Button == MouseButtons.Middle))
            {
                

                this.newMyDraw1.isDrawRect = true;
                if (Zoomchk == false)
                {
                    //this.newMyDraw1.SizeDrgInfo = 0;
                    if(this.newMyDraw1.IsUnCheckedEnlg == true)
                    {
                        //this.newMyDraw1.IsNewDrawMap = true;
                    }

                }
                else  //한 번 확대 범위를 지정한 상태에서 재지정 시,
                {
                    if (this.newMyDraw1.DrgEgY != 0 && DrgChk!=true)
                    {
                        prevX1 = this.newMyDraw1.DragStartX;
                        prevY1 = this.newMyDraw1.DragStartY;
                        prevX2 = this.newMyDraw1.DragEndX;
                        prevY2 = "" + this.newMyDraw1.DrgEgY;
                        this.newMyDraw1.IsNewDrawMap = true;
                    }
                    
                
                }
                this.newMyDraw1.DragStartX = null;
                this.newMyDraw1.DragStartY = null;
                this.newMyDraw1.DragEndX = null;
                this.newMyDraw1.DragEndY = null;
                this.newMyDraw1.NonMouseUpX = null;
                this.newMyDraw1.NonMouseUpY = null;
                this.newMyDraw1.NonExpEndX = null;
                this.newMyDraw1.NonExpEndY = null;
                this.newMyDraw1.NonExpStartX = null;
                this.newMyDraw1.NonExpStartY = null;
                this.newMyDraw1.isDrawRect = true;
                //ZoomRangeSelected = false;



                Point point = e.Location;


                float moveX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / (SizeOfX ));
                float moveY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / (SizeOfY ));

                if (this.newMyDraw1.DrgEgY!=0)
                {
                    if (Zoomchk == true)
                    {
                        
                        try
                        {
                            float x1 = float.Parse(prevX1);
                            float x2 = float.Parse(prevX2);
                            float y1 = float.Parse(prevY1);
                            float y2 = float.Parse(prevY2);

                            if (x1 < x2)
                            {
                                if (y1 > y2)
                                {

                                    float crX = x2 - x1;  //거리

                                    moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                    float crY = y1 - y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                }
                                else
                                {
                                    float crX = x2 - x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                    float crY = -y1 + y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);
                                }

                            }
                            else
                            {
                                if (y1 > y2)
                                {

                                    float crX = -x2 + x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                    float crY = y1 - y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                }
                                else
                                {

                                    float crX = -x2 + x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                    float crY = -y1 + y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);


                                }
                            }
                        }
                        catch (Exception) { return; }
                    }
                    else
                    {
                        try
                        {
                            float x1 = float.Parse(newMyDraw1.PrevDrgX1);
                            float x2 = float.Parse(newMyDraw1.PrevDrgX2);
                            float y1 = float.Parse(newMyDraw1.PrevDrgY1);
                            float y2 = float.Parse(newMyDraw1.PrevDrgY2);

                            if (x1 < x2)
                            {
                                if (y1 > y2)
                                {

                                    float crX = x2 - x1;  //거리

                                    moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                    float crY = y1 - y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                }
                                else
                                {
                                    float crX = x2 - x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                    float crY = -y1 + y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);
                                }

                            }
                            else
                            {
                                if (y1 > y2)
                                {

                                    float crX = -x2 + x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                    float crY = y1 - y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                }
                                else
                                {

                                    float crX = -x2 + x1;  //거리
                                    moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                    float crY = -y1 + y2;
                                    moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);


                                }
                            }
                        }
                        catch (Exception) { return; }
                    }
                }
                

                string Xrnd = moveX.ToString("0.#");
                string Yrnd = moveY.ToString("0.#");
                Xindex.Text = Xrnd;
                Yindex.Text = Yrnd;

                DragStartX = Xrnd;
                DragStartY = Yrnd;
                if (prevX1 != null && prevY1 != null)
                {
                    this.newMyDraw1.DragStartX = prevX1;
                    this.newMyDraw1.DragStartY = prevY1;
                    this.newMyDraw1.DragEndX = prevX2;
                    this.newMyDraw1.DragEndY = prevY2;
                }

                //this.newMyDraw1.DragStartX = Xrnd;
                //this.newMyDraw1.DragStartY = Yrnd;

                if (this.newMyDraw1.DrgEgY != 0)
                {
                    this.newMyDraw1.NonExpStartX = point.X.ToString("0.#");
                    this.newMyDraw1.NonExpStartY = point.Y.ToString("0.#");
                }
                else
                {
                    this.newMyDraw1.NonExpStartX = Xrnd;
                    this.newMyDraw1.NonExpStartY = Yrnd;
                }

                MultiPinPointed();
                
                if (Zoomchk == true)
                {
                    this.newMyDraw1.IsDragChked = true;
                }
            }
            #endregion

        }

        private void newMyDraw1_MouseUp(object sender, MouseEventArgs e)
        {
            string MvDistPoint = string.Empty;
            if (e.Button == MouseButtons.Left)
            {
                

                string Xrnd = "0";
                string Yrnd = "0";
                if (SizeOfX > 10 || SizeOfY > 10)
                {
                    Xrnd = ClickX.ToString("0");
                    Yrnd = ClickY.ToString("0");
                }
                else
                {
                    Xrnd = ClickX.ToString("0.#");
                    Yrnd = ClickY.ToString("0.#");
                }

                if (DselectedList.Items.Count > 0)
                {
                    MvEndX = Xrnd;
                    MvEndY = Yrnd;
                }

                // 다중 이동 시, 움직인 거리 (x1에서 x2로 이동했을 때의 이동된 거리 만큼 다중 선택 점들이 같이 옮겨짐) -> Distance의 Dist가 아님
                float MvDisX = 0;
                float MvDisY = 0;
                if (MvStartX != null && MvEndX != null)
                {

                    MvDisX = float.Parse(MvEndX) - float.Parse(MvStartX);
                    MvDisY = float.Parse(MvEndY) - float.Parse(MvStartY);
                    if (this.newMyDraw1.PicLotate == 90 || this.newMyDraw1.PicLotate == -270)
                    {
                        var temp = MvDisX;
                        MvDisX = -MvDisY;
                        MvDisY = temp;
                    }
                    else if (this.newMyDraw1.PicLotate == -180 || this.newMyDraw1.PicLotate == 180)
                    {
                        MvDisX = -MvDisX;
                        MvDisY = -MvDisY;
                    }
                    else if (this.newMyDraw1.PicLotate == -90 || this.newMyDraw1.PicLotate == 270)
                    {
                        var temp = MvDisX;
                        MvDisX = MvDisY;
                        MvDisY = -temp;
                    }


                }


                try
                {
                    if (DselectedList.Items.Count > 0)
                    {

                        List<string> TempList = new List<string>();
                        foreach (string Changed_RangeList in RangeList)
                        {
                            //원래 위치 + 이동 할 거리 = 이동 된 위치
                            string[] values = Changed_RangeList.Split(',');
                            string TempValX = float.Parse(values[0]) + MvDisX + "";
                            string TempValY = float.Parse(values[1]) + MvDisY + "";


                            if (DisList.Contains(Changed_RangeList))
                            {
                                MvDisMore = DisList.IndexOf(Changed_RangeList);
                                DisList.Remove(Changed_RangeList);
                                MvDistPoint = TempValX + "," + TempValY;
                                DisList.Insert(MvDisMore,MvDistPoint);
                                DistMarkMove = true;
                            }
                            // 이동된 범위가 밖으로 나가지면
                            /*if (float.Parse(TempValX) > this.newMyDraw1.SizeInfo / 2 || float.Parse(TempValX) < -this.newMyDraw1.SizeInfo / 2 ||
                            float.Parse(TempValY) < -this.newMyDraw1.SizeInfo / 2 || float.Parse(TempValY) > this.newMyDraw1.SizeInfo / 2)
                            { 
                                //points 리스트에서 삭제
                                points.Remove(Changed_RangeList);
                            }
                            else*/

                            {
                                TempList.Add(TempValX + "," + TempValY);
                                points.Remove(Changed_RangeList);
                                points.Add(TempValX + "," + TempValY);
                            }

                        }
                        RangeList = TempList;

                        listBox1.DataSource = null;
                        listBox1.DataSource = points;
                        DselectedList.DataSource = null;
                        DselectedList.DataSource = RangeList;
                        this.newMyDraw1.DragInd = RangeList;



                        if (DistMarkMove == true)
                        {
                            
                            this.newMyDraw1.Distance = DisList;
                            DistMarkMove = false;
                        }
                }

                }
                catch (Exception ex)
                {
                    return;
                }
                    //마우스를 떼었을 때, 선택한 요소 초기화 및 리스트 박스 최신화
                if (MoveDrg == true)
                {
                    if (DselectedList.Items.Count > 0 == false)
                    {
                        points.Add(MoveX + "," + MoveY);
                        listBox1.DataSource = null;
                        listBox1.DataSource = points;

                        if (DistMarkMove == true)
                        {
                            DisList.Insert(MvDisMore,MoveX + "," + MoveY);
                            this.newMyDraw1.Distance = DisList;
                            DistMarkMove = false;
                        }

                        MoveDrg = false;

                        MoveX = string.Empty;
                        MoveY = string.Empty;
                        this.newMyDraw1.isHighL = true;
                    }
                    else
                    {
                        
                        this.newMyDraw1.isHighL = false;
                    }
                    
                }
            }

            #region 드래그 List범위지정 안했을 시,
            if (Zoomchk == false)
            {
                if ((e.Button == MouseButtons.Middle))
                {
                    if (ZoomRangeSelected == false)
                    {
                        Point point = e.Location;
                        float moveX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / (SizeOfX / 10f)) * 10;
                        float moveY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / (SizeOfY / 10f)) * 10;



                        string Xrnd = moveX.ToString("0.#");
                        string Yrnd = moveY.ToString("0.#");

                        Xindex.Text = Xrnd;
                        Yindex.Text = Yrnd;

                        this.newMyDraw1.DragStartX = DragStartX;
                        this.newMyDraw1.DragStartY = DragStartY;
                        this.newMyDraw1.DragEndX = Xindex.Text;
                        this.newMyDraw1.DragEndY = Yindex.Text;
                        DragEndX = this.newMyDraw1.DragStartX;
                        DragEndY = this.newMyDraw1.DragStartY;

                        this.newMyDraw1.NonExpEndX = Xrnd;
                        this.newMyDraw1.NonExpEndY = Yrnd;

                        ChkEv = true;
                        SearchRangePoint(ChkEv);

                        if (DrgChk == false)
                        {
                            this.newMyDraw1.isDrawRect = false;
                        }
                        else
                        {
                            this.newMyDraw1.isDrawRect = true;
                        }
                        Clear_TextBox();
                    }
                    //드래그로 확대 범위를 정한 상태일 때
                    else
                    {
                        Point point = e.Location;
                        float moveX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / (SizeOfX / 10f)) * 10;
                        float moveY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / (SizeOfY / 10f)) * 10;

                        if (this.newMyDraw1.DrgEgY != 0)
                        {

                            try
                            {
                                float x1 = float.Parse(newMyDraw1.PrevDrgX1);
                                float x2 = float.Parse(newMyDraw1.PrevDrgX2);
                                float y1 = float.Parse(newMyDraw1.PrevDrgY1);
                                float y2 = float.Parse(newMyDraw1.PrevDrgY2);

                                if (x1 < x2)
                                {
                                    if (y1 > y2)
                                    {

                                        float crX = x2 - x1;  //거리
                                                              //       최좌측의 실제 좌표 = 0이기 때문에 + 더 작은 x 좌표(이하, x1) 를 계산해주어 결과로 그려진 bitmap의 좌측변 x좌표를 x1으로 만들어 준다.
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                        float crY = y1 - y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                    }
                                    else
                                    {
                                        float crX = x2 - x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                        float crY = -y1 + y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);
                                    }

                                }
                                else
                                {
                                    if (y1 > y2)
                                    {

                                        float crX = -x2 + x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                        float crY = y1 - y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                    }
                                    else
                                    {

                                        float crX = -x2 + x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                        float crY = -y1 + y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);


                                    }
                                }
                            }
                            catch (Exception) { return; }
                        }

                        string Xrnd = moveX.ToString("0.#");
                        string Yrnd = moveY.ToString("0.#");


                        this.newMyDraw1.DragStartX = DragStartX;
                        this.newMyDraw1.DragStartY = DragStartY;


                        //this.newMyDraw1.DragStartX = DragStartX;
                        //this.newMyDraw1.DragStartY = DragStartY;


                        Xindex.Text = Xrnd;
                        Yindex.Text = Yrnd;

                        DragEndX = Xindex.Text;
                        DragEndY = Yindex.Text;

                        if (this.newMyDraw1.DrgEgY == 0)
                        {
                            this.newMyDraw1.DragEndX = Xindex.Text;
                            this.newMyDraw1.DragEndY = Yindex.Text;
                            this.newMyDraw1.NonExpEndX = Xrnd;
                            this.newMyDraw1.NonExpEndY = Yrnd;
                        }
                        else
                        {


                            this.newMyDraw1.NonExpEndX = point.X.ToString("0.#");
                            this.newMyDraw1.NonExpEndY = point.Y.ToString("0.#");


                            /*this.newMyDraw1.DragStartX = this.newMyDraw1.NonExpStartX;
                            this.newMyDraw1.DragStartY = this.newMyDraw1.NonExpStartY;
                            this.newMyDraw1.DragEndX = this.newMyDraw1.NonExpEndX;
                            this.newMyDraw1.DragEndY = this.newMyDraw1.NonExpEndY;*/

                            this.newMyDraw1.DragEndX = Xindex.Text;
                            this.newMyDraw1.DragEndY = Yindex.Text;
                        }

                        ChkEv = true;
                        SearchRangePoint(ChkEv);

                        Clear_TextBox();

                        if (DrgChk == true)
                        {
                            this.newMyDraw1.isDrawRect = true;
                        }
                        else
                        {
                            this.newMyDraw1.isDrawRect = false;
                        }
                        this.newMyDraw1.IsNewDrawMap = true;
                        ZoomRangeSelected = true;
                    }
                    
                }
            }
            #endregion
            // Zoom Check는 되어있지만 아직 범위를 지정하지 않은 상태
            else
            {
                if ((e.Button == MouseButtons.Middle))
                {

                    Point point = e.Location;
                    float moveX = (point.X - (newMyDraw1.Width / 2f)) / (newMyDraw1.Width / (SizeOfX / 10f)) * 10;
                    float moveY = -(point.Y - (newMyDraw1.Height / 2f)) / (newMyDraw1.Height / (SizeOfY / 10f)) * 10;

                    if (this.newMyDraw1.DrgEgY != 0)
                    {
                            try
                            {
                                float x1 = float.Parse(newMyDraw1.DragStartX);
                                float x2 = float.Parse(newMyDraw1.DragEndX);
                                float y1 = float.Parse(newMyDraw1.DragStartY);
                                float y2 = newMyDraw1.DrgEgY;

                                if (this.newMyDraw1.DrgEgY != 0)
                                {
                                    x1 = float.Parse(prevX1);
                                    x2 = float.Parse(prevX2);
                                    y1 = float.Parse(prevY1);
                                    y2 = float.Parse(prevY2);
                                }

                                if (x1 < x2)
                                {
                                    if (y1 > y2)
                                    {

                                        float crX = x2 - x1;  //거리
                                                              //       최좌측의 실제 좌표 = 0이기 때문에 + 더 작은 x 좌표(이하, x1) 를 계산해주어 결과로 그려진 bitmap의 좌측변 x좌표를 x1으로 만들어 준다.
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                        float crY = y1 - y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                    }
                                    else
                                    {
                                        float crX = x2 - x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x1);
                                        float crY = -y1 + y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);
                                    }

                                }
                                else
                                {
                                    if (y1 > y2)
                                    {

                                        float crX = -x2 + x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                        float crY = y1 - y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y1);
                                    }
                                    else
                                    {

                                        float crX = -x2 + x1;  //거리
                                        moveX = (point.X / (newMyDraw1.Width / crX) + x2);
                                        float crY = -y1 + y2;
                                        moveY = -(point.Y / (newMyDraw1.Height / crY) - y2);

                                    }
                                }
                            }
                            catch (Exception) { return; }
                        
                    }

                    string Xrnd = moveX.ToString("0.#");
                    string Yrnd = moveY.ToString("0.#");

                    //ChkEv = true;
                    //SearchRangePoint(ChkEv);

                    if (DragStartX == Xrnd && DragStartY == Yrnd)
                    {
                        this.newMyDraw1.IsDragChked = false;
                        this.newMyDraw1.SizeDrgInfo = 0;
                        this.newMyDraw1.IsNewDrawMap = false;
                        ZoomRangeSelected = false;
                        this.newMyDraw1.DrgEgY = 0;
                        this.newMyDraw1.Invalidate();
                    }
                    else
                    {
                        this.newMyDraw1.DragStartX = DragStartX;
                        this.newMyDraw1.DragStartY = DragStartY;
                        

                        Xindex.Text = Xrnd;
                        Yindex.Text = Yrnd;

                        DragEndX = Xindex.Text;
                        DragEndY = Yindex.Text;

                        if (this.newMyDraw1.DrgEgY == 0)
                        {
                            this.newMyDraw1.DragEndX = Xindex.Text;
                            this.newMyDraw1.DragEndY = Yindex.Text;
                            this.newMyDraw1.NonExpEndX = Xrnd;
                            this.newMyDraw1.NonExpEndY = Yrnd;
                        }
                        else
                        {


                            this.newMyDraw1.NonExpEndX = point.X.ToString("0.#");
                            this.newMyDraw1.NonExpEndY = point.Y.ToString("0.#");


                            /*this.newMyDraw1.DragStartX = this.newMyDraw1.NonExpStartX;
                            this.newMyDraw1.DragStartY = this.newMyDraw1.NonExpStartY;
                            this.newMyDraw1.DragEndX = this.newMyDraw1.NonExpEndX;
                            this.newMyDraw1.DragEndY = this.newMyDraw1.NonExpEndY;*/

                            this.newMyDraw1.DragEndX = Xindex.Text;
                            this.newMyDraw1.DragEndY = Yindex.Text;
                        }
                        
                        Clear_TextBox();

                        this.newMyDraw1.isDrawRect = false;
                        this.newMyDraw1.IsNewDrawMap = true;
                        ZoomRangeSelected = true;
                    }
                    
                }
            }
            
        }


        private void sizeInfoBtn_Click(object sender, EventArgs e)
        {
            SizeLock = true;
            RangeList.Clear();
            DselectedList.DataSource = null;
            DselectedList.DataSource = RangeList;
            PanelSizeIndexing(true);
            /*ClearEv();*/
            float SizeInfo = 500;
            try
            {
                if (sizeInfo.Text == string.Empty || float.Parse(sizeInfo.Text) < 2)
                {
                    MessageBox.Show(" Out Of Index. Minimum Size = 2 ");
                    sizeInfo.Text = "" + 500;
                    SizeOfX = newMyDraw1.Size.Width;
                    SizeOfY = newMyDraw1.Size.Height;
                }
                else
                {
                    SizeInfo = float.Parse(sizeInfo.Text);
                    /*if ((float.Parse(sizeInfo.Text) % 10 == 0) == false)
                    {

                    }
                    else
                    {
                        SizeOf = float.Parse(sizeInfo.Text);
                    }*/
                    SizeOfX = float.Parse(sizeInfo.Text);
                    SizeOfY = float.Parse(sizeInfo.Text);

                    foreach (var SizeInList in points.ToList())
                    {
                        string[] values = SizeInList.Split(',');
                        float Qx = float.Parse(values[0]);
                        float Qy = float.Parse(values[1]);

                        if (Qx > SizeInfo / 2 || Qx < -SizeInfo / 2 || Qy < -SizeInfo / 2 || Qy > SizeInfo / 2)
                        {
                            points.Remove(SizeInList);
                        }
                    }
                    listBox1.DataSource = null;
                    listBox1.DataSource = points;
                }
            }
            catch (Exception ex)
            {
                SizeInfo = 500;
                /*if ((float.Parse(sizeInfo.Text) % 10 == 0) == false)
                {

                }
                else
                {
                    SizeOf = float.Parse(sizeInfo.Text);
                }*/
                SizeOfX = 500;
                SizeOfY = 500;

                foreach (var SizeInList in points.ToList())
                {
                    string[] values = SizeInList.Split(',');
                    float Qx = float.Parse(values[0]);
                    float Qy = float.Parse(values[1]);

                    if (Qx > SizeInfo / 2 || Qx < -SizeInfo / 2 || Qy < -SizeInfo / 2 || Qy > SizeInfo / 2)
                    {
                        points.Remove(SizeInList);
                    }
                }
                listBox1.DataSource = null;
                listBox1.DataSource = points;
            }
            
            //this.WindowState = FormWindowState.Normal;
            this.newMyDraw1.SizeInfo = SizeInfo;

            this.newMyDraw1.SizeInfoX = SizeInfo;
            this.newMyDraw1.SizeInfoY = SizeInfo;
            SizeOfX = SizeInfo;
            SizeOfY = SizeInfo;

            //윈폼 사이즈 조절을 위해 = Dock으로 채워버린 newMyDraw1은 직접적인 사이즈 조절이 불가능하고 폼으로 사이즈를 조절하여야 함.
            //전체 폼 사이즈 - 현재 Control 사이즈 = Control 외의 바깥 외곽의 크기
            //외곽 크기 + 주려고 하는 Size = 원하는 폼 사이즈

            //int tempWidth = this.Width - this.newMyDraw1.Width;
            //int tempHeight = this.Height - this.newMyDraw1.Height;
            //this.Width = tempWidth + ((int)SizeInfo*2);
            //this.Height = tempHeight + ((int)SizeInfo*2);


            this.newMyDraw1.Invalidate();
        }

        private void Zoom_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (Zoomchk == true)
            {
                //this.newMyDraw1.IsDragChked = false;
                
                Zoomchk = false;
                /*if (DrgChk == false)
                {
                    this.newMyDraw1.IsSizeDraw = false;
                    this.newMyDraw1.PrevDrgX1 = this.newMyDraw1.DragStartX;
                    this.newMyDraw1.PrevDrgY1 = this.newMyDraw1.DragStartY;
                    this.newMyDraw1.PrevDrgX2 = this.newMyDraw1.DragEndX;
                    this.newMyDraw1.PrevDrgY2 = "" + this.newMyDraw1.DrgEgY;
                }*/
                //ZoomRangeSelected = false;
                this.newMyDraw1.IsSizeDraw = false;
                /*this.newMyDraw1.DrgEgY = 0;
                prevX1 = null;
                prevX2 = null;
                prevY1 = null;
                prevY2 = null;
                this.newMyDraw1.SizeDrgInfo = 0;
                this.newMyDraw1.IsNewDrawMap = false;*/

                this.newMyDraw1.IsSizeDraw = false;
                this.newMyDraw1.PrevDrgX1 = this.newMyDraw1.DragStartX;
                this.newMyDraw1.PrevDrgY1 = this.newMyDraw1.DragStartY;
                this.newMyDraw1.PrevDrgX2 = this.newMyDraw1.DragEndX;
                this.newMyDraw1.PrevDrgY2 = "" + this.newMyDraw1.DrgEgY;
                this.newMyDraw1.IsUnCheckedEnlg = true;

                this.newMyDraw1.Invalidate();
                

            }
            else
            {
                //확대 -> 취소 -> 다시 확대 시
                if (ZoomRangeSelected == true)
                {
                    ZoomRangeSelected = false;
                }
                
                this.newMyDraw1.IsUnCheckedEnlg = false;
                DrgCheckBox.Checked = false;
                this.newMyDraw1.DragStartX = null;
                this.newMyDraw1.DragStartY = null;
                this.newMyDraw1.DragEndX = null;
                this.newMyDraw1.DragEndY = null;
                this.newMyDraw1.NonMouseUpX = null;
                this.newMyDraw1.NonMouseUpY = null;
                this.newMyDraw1.isDrawRect = false;
                this.newMyDraw1.IsSizeDraw = true;
                this.newMyDraw1.IsDragChked = false;
                this.newMyDraw1.SizeDrgInfo = 0;
                this.newMyDraw1.IsNewDrawMap = false;
                this.newMyDraw1.DrgEgY = 0;

                
                prevX1 = null;
                prevX2 = null;
                prevY1 = null;
                prevY2 = null;
                

                Zoomchk = true;
                this.newMyDraw1.Invalidate();
            }
        }

        private void crdCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (CrdnChk == true)
            {
                CrdnChk = false;
            }
            else
            {
                CrdnChk = true;
            }
            this.newMyDraw1.CrdnChk = CrdnChk;
            this.newMyDraw1.Invalidate();
        }

        private void listReset_Click(object sender, EventArgs e)
        {
            RangeList.Clear();
            DselectedList.DataSource = null;
            DselectedList.DataSource = RangeList;
            this.newMyDraw1.Invalidate();
        }

        private void MoreDistchk_CheckedChanged(object sender, EventArgs e)
        {
            if(MoreDistchk.Checked == false && DisList.Count >= 3)
            {
                DisList.Clear();
                this.newMyDraw1.Distance = DisList;
                this.newMyDraw1.Invalidate();
            }
        }
    }
}
