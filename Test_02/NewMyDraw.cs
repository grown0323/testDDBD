using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Test_02
{
    public partial class NewMyDraw : UserControl
    {
        float x1 = 0;
        float y1 = 0;

        //선택한 X,Y좌표
        public float x2 { get; set; }
        public float y2 { get; set; }

        public float tx2 = 0;
        public float ty2 = 0;

        #region 드래그 확대 시 사용할 변수
        public float DrgEgY = 0;

        float LineXind = 0;
        float LineYind = 0;

        #endregion



        //마우스 움직임 감지 좌표
        public float movingX { get; set; }
        public float movingY { get; set; }
        public string DragStartY {  get; set; }
        public string DragStartX { get; set; }
        public string DragEndX {  get; set; }
        public string DragEndY {  get; set; }
        public string NonMouseUpX {  get; set; }
        public string NonMouseUpY {  get; set; }

        //드래그 확대가 생겨 드래그 상자를 만드는 좌표 분화
        public string NonExpStartX {  get; set; }
        public string NonExpStartY { get; set; }
        public string NonExpEndX {  get; set; }
        public string NonExpEndY {  get; set; }

        public string PrevDrgX1 {  get; set; }
        public string PrevDrgY1 {  get; set; }

        public string PrevDrgX2 { get; set; }
        public string PrevDrgY2 {  get; set; }

        //회전 각도
        public int PicLotate {  get; set; }


        //실제 마우스 포인터 위치
        public float realX { get; set; }
        public float realY { get; set; }

        public List<string> points { get; set; }
        public List<string> Distance { get; set; }

        public List<string> DragInd { get; set; }

   

        public float SizeInfo {  get; set; }
        /*public float SizeDrgInfoX = 0;
        public float SizeDrgInfoY = 0;*/
        public float SizeDrgInfo = 0;
        public float SizeInfoX = 500;
        public float SizeInfoY = 500;

        public string selectedPoint { get; set; }


        public bool chk { get; set; }
        public bool SizeLock { get; set; }
        public bool isList { get; set; }
        public bool isHighL { get; set; }
        public bool isDist {get; set; }
        public bool isDrawRect { get; set; }
        public bool isCollectRange {  get; set; }
        public bool IsNewDrawMap {  get; set; }
        public bool CrdnChk { get; set; }
        public bool IsUnCheckedEnlg {  get; set; }

        public bool IsDragChked {  get; set; }
        public bool IsSizeDraw {  get; set; }
        public bool MoreDistchk { get; internal set; }

        //사이즈 재지정(확대)를 했는지. 이미 확대 된 상태인지. -> 확대 되어있다면 그 화면에서 다시 확대 할 것이기 때문


        private Bitmap bitmap;
        private readonly bool designMode;




        public NewMyDraw()
        {
            IsUnCheckedEnlg = false;
            isCollectRange = false;
            chk = false;
            selectedPoint = string.Empty;
            
            //거리
            isDist = false;

            //기본 사이즈 500 = -250~250 
            SizeInfo = 500;
            IsSizeDraw = true;
            SizeLock = false;
            

            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            bitmap = new Bitmap(this.Width,this.Height, PixelFormat.Format32bppPArgb);
        }
        protected override void OnResize(EventArgs e)
        {
            if (SizeLock == false)
            {
                SizeInfoX = Size.Width / 2;
                SizeInfoY = Size.Height / 2;

            }
            CreateBitmap();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            
            var data = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            this.DrawMap(bitmap, data, chk, isList);
            bitmap.UnlockBits(data);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }


        //맵 그리기
        public void DrawMap(Bitmap bitmap, BitmapData data, bool chk, bool isList)
        {
            x1 = bitmap.Width / 2;
            y1 = bitmap.Height / 2;
            
            var info = new SKImageInfo(bitmap.Width, bitmap.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            using (var surface = SKSurface.Create(info, data.Scan0, data.Stride))
            {
                
                surface.Canvas.Clear(Color.White.ToSKColor());

                DrawLayout(surface.Canvas);
                MoveingMouseEv(surface.Canvas);

                if (chk == true)
                {
                    if (isList == true)
                    {
                        foreach (string LocationPoints in points)
                        {
                            
                            string[] values = LocationPoints.Split(',');
                            x2 = float.Parse(values[0]);
                            y2 = float.Parse(values[1]);
                            tx2 = x2;
                            ty2 = y2;

                            if (PicLotate == 90 || PicLotate == -270)
                            {
                                var temp = x2;
                                x2 = y2;
                                y2 = -temp;
                            }
                            else if (PicLotate == -90 || PicLotate == 270)
                            {
                                var temp = x2;
                                x2 = -y2;
                                y2 = temp;
                            }
                            else if (PicLotate == -180 || PicLotate == 180)
                            {
                                x2 = -x2;
                                y2 = -y2;
                            }

                          
                            SKPaint skPen = new SKPaint() { Color = Color.Red.ToSKColor() };
                            DrawPointCircle(surface.Canvas, skPen);
                        }
                    }
                    else
                    {

                        tx2 = x2;
                        ty2 = y2;

                        if (PicLotate == 90 || PicLotate == -270)
                        {
                            var temp = tx2;
                            tx2 = -ty2;
                            ty2 = temp;
                        }
                        else if (PicLotate == -90 || PicLotate == 270)
                        {
                            var temp = tx2;
                            tx2 = ty2;
                            ty2 = -temp;
                        }
                        else if (PicLotate == -180 || PicLotate == 180)
                        {
                            tx2 = -tx2;
                            ty2 = -ty2;
                        }


                        SKPaint skPen = new SKPaint() { Color = Color.Red.ToSKColor() };
                        DrawPointCircle(surface.Canvas,skPen);
                    }
                    
                }
                if (isCollectRange == true)
                {
                    foreach (string DragPoints in DragInd)
                    {
                        
                        string[] values = DragPoints.Split(',');
                        x2 = float.Parse(values[0]);
                        y2 = float.Parse(values[1]);
                        tx2 = x2;
                        ty2 = y2;

                        if (PicLotate == 90 || PicLotate == -270)
                        {
                            var temp = x2;
                            x2 = y2;
                            y2 = -temp;
                        }
                        else if (PicLotate == -90 || PicLotate == 270)
                        {
                            var temp = x2;
                            x2 = -y2;
                            y2 = temp;
                        }
                        else if (PicLotate == -180 || PicLotate == 180)
                        {
                            x2 = -x2;
                            y2 = -y2;
                        }


                        HighlightedPoint(surface.Canvas,true);
                    }
                }

                if (isHighL == true)
                {
                    HighlightedPoint(surface.Canvas,false);
                }

                if (isDist == true)
                {
                    DistMark(surface.Canvas);
                }
                if (isDrawRect == true)
                {
                    DragPosition(surface.Canvas);
                }

                surface.Flush();
            }

        }

        //Distance 버튼 클릭 시
        private void DistMark(SKCanvas canvas)
        {
            foreach (string DistancePoints in Distance)
            {
                if (DistancePoints == ",")
                {
                    return;
                }
                string[] valuse = DistancePoints.Split(',');
                x2 = float.Parse(valuse[0]);
                y2 = float.Parse(valuse[1]);
                tx2 = x2;
                ty2 = y2;

                if (PicLotate == 90 || PicLotate == -270)
                {
                    var temp = x2;
                    x2 = y2;
                    y2 = -temp;
                }
                else if (PicLotate == -90 || PicLotate == 270)
                {
                    var temp = x2;
                    x2 = -y2;
                    y2 = temp;
                }
                else if (PicLotate == -180 || PicLotate == 180)
                {
                    x2 = -x2;
                    y2 = -y2;
                }

                SKPaint skPen = new SKPaint() { Color = Color.Blue.ToSKColor() };
                DrawPointCircle(canvas, skPen);


            }

            try
            {
                if (Distance.Count >= 2)
                {
                    string[] valuse1 = null;
                    string[] valuse2 = null;

                    for (int i = 0; i < Distance.Count; i++)
                    {
                        if (MoreDistchk == true)
                        {

                            if (Distance.Count == 2)
                            {
                                valuse1 = Distance[0].Split(',');
                                valuse2 = Distance[1].Split(',');

                            }
                            else if (Distance.Count >= 3)
                            {

                                if(i > 0)
                                {
                                    valuse1 = Distance[i - 1].Split(',');
                                }
                                else
                                {
                                    valuse1 = Distance[0].Split(',');
                                }

                                valuse2 = Distance[i].Split(',');

                            }
                        }
                        else
                        {
                            if (Distance.Count == 2)
                            {
                                valuse1 = Distance[0].Split(',');
                                valuse2 = Distance[1].Split(',');
                            }
                        }


                        /*if (Distance.Count == 2)
                        {
                            valuse1 = Distance[0].Split(',');
                            valuse2 = Distance[1].Split(',');
                        }
                        else if (Distance.Count >= 3)
                        {
                            valuse1 = Distance[i - 1].Split(',');
                            valuse2 = Distance[i].Split(',');
                        }*/




                        float rX1 = float.Parse(valuse1[0]);
                        float rY1 = float.Parse(valuse1[1]);
                        float rX2 = float.Parse(valuse2[0]);
                        float rY2 = float.Parse(valuse2[1]);

                        if (PicLotate == 90 || PicLotate == -270)
                        {
                            var temp1 = rX1;
                            var temp2 = rX2;

                            rX1 = rY1;
                            rX2 = rY2;

                            rY1 = -temp1;
                            rY2 = -temp2;
                        }
                        else if (PicLotate == -90 || PicLotate == 270)
                        {
                            var temp1 = rX1;
                            var temp2 = rX2;

                            rX1 = -rY1;
                            rX2 = -rY2;

                            rY1 = temp1;
                            rY2 = temp2;

                        }
                        else if (PicLotate == -180 || PicLotate == 180)
                        {
                            rX1 = -rX1;
                            rX2 = -rX2;
                            rY1 = -rY1;
                            rY2 = -rY2;
                        }
                        if (SizeDrgInfo != 0)
                        {
                            float DrX1 = 0;
                            float DrY1 = 0;
                            float DrX2 = 0;
                            float DrY2 = 0;

                            if (IsSizeDraw == true)
                            {
                                DrX1 = float.Parse(DragStartX);
                                DrY1 = float.Parse(DragStartY);
                                DrX2 = float.Parse(DragEndX);
                                DrY2 = DrgEgY;
                            }
                            else
                            {
                                DrX1 = float.Parse(PrevDrgX1);
                                DrY1 = float.Parse(PrevDrgY1);
                                DrX2 = float.Parse(PrevDrgX2);
                                DrY2 = float.Parse(PrevDrgY2);
                            }
                            // rX1의 위치가 DrX1(또는 DrX2)에서 부터 얼마나 떨어져있는지를 통하여 위치를 파악 / 무조건 rX1은 DrX1과 DrX2 사이에 있을 것이기 때문
                            if (DrX1 < DrX2)
                            {
                                rX1 = bitmap.Width * (rX1 - DrX1) / (SizeDrgInfo);
                            }
                            else
                            // DrX2가 DrX1보다 작을 때 (좌표 평면에서 끝 점이 좌측에 있을 때)
                            {
                                rX1 = bitmap.Width * (rX1 - DrX2) / (SizeDrgInfo);
                            }
                            // DrY1이 DrY2보다 '하단'에 있을 때.  (*실제 Form 내 좌표 판단 시, '좌측 상단'이 0,0 임을 인지. -> 중앙은 [0,0]이 아닌 Width/2,Height/2 이며, 중앙이 0,0임으로 Y가 내려갈 수록 값이 증가함
                            //그렇기에 최 하단인 Bitmap.Height를 시작으로 위로 거리만큼 -를 하여 좌표를 파악
                            if (DrY1 < DrY2)
                            {
                                rY1 = bitmap.Height - (bitmap.Height * (rY1 - DrY1) / (SizeDrgInfo));
                            }
                            else
                            {
                                rY1 = bitmap.Height - (bitmap.Height * (rY1 - DrY2) / (SizeDrgInfo));
                            }

                            // rX2 또한 rX1과 같이 위치를 파악한다.
                            if (DrX1 < DrX2)
                            {
                                rX2 = bitmap.Width * (rX2 - DrX1) / (SizeDrgInfo);
                            }
                            else
                            {
                                rX2 = bitmap.Width * (rX2 - DrX2) / (SizeDrgInfo);
                            }
                            //
                            if (DrY1 < DrY2)
                            {
                                rY2 = bitmap.Height - (bitmap.Height * (rY2 - DrY1) / (SizeDrgInfo));
                            }
                            else
                            {
                                rY2 = bitmap.Height - (bitmap.Height * (rY2 - DrY2) / (SizeDrgInfo));
                            }
                        }

                        /*float disX1 = x1 + (bitmap.Width * (rX1 / 10f) / (SizeInfo/10f));
                        float disY1 = y1 - (bitmap.Height * (rY1 / 10f) / (SizeInfo / 10f));
                        float disX2 = x1 + (bitmap.Width * (rX2 / 10f) / (SizeInfo / 10f));
                        float disY2 = y1 - (bitmap.Height * (rY2 / 10f) / (SizeInfo / 10f));*/

                        float disX1 = x1 + (bitmap.Width * (rX1 / 10f) / (SizeInfoX / 10f));
                        float disY1 = y1 - (bitmap.Height * (rY1 / 10f) / (SizeInfoY / 10f));
                        float disX2 = x1 + (bitmap.Width * (rX2 / 10f) / (SizeInfoX / 10f));
                        float disY2 = y1 - (bitmap.Height * (rY2 / 10f) / (SizeInfoY / 10f));

                        if (DrgEgY != 0)
                        {
                            disX1 = rX1;
                            disY1 = rY1;
                            disX2 = rX2;
                            disY2 = rY2;
                        }

                        double PowInst = (double.Parse(valuse2[0]) - double.Parse(valuse1[0])) * (double.Parse(valuse2[0]) - double.Parse(valuse1[0])) + (double.Parse(valuse1[1]) - double.Parse(valuse2[1])) * (double.Parse(valuse1[1]) - double.Parse(valuse2[1]));

                        double DisIndex = Math.Sqrt(PowInst);

                        canvas.DrawLine(disX1, disY1, disX2, disY2, new SKPaint() { Color = Color.DarkMagenta.ToSKColor() });
                        if (DisIndex.ToString("0.##") == "0")
                        {
                            
                        }
                        else
                        {
                            canvas.DrawText("[Distance : " + DisIndex.ToString("0.##") + "]", (disX2 + disX1) / 2, (disY1 + disY2) / 2, new SKPaint() { Color = Color.DarkCyan.ToSKColor() });
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                return;
            }

        }

        //더블클릭 강조 시
        private void HighlightedPoint(SKCanvas canvas,bool chk)
        {
            SKPaint skPen = new SKPaint() { Color = Color.Green.ToSKColor() };
            

            if (x2 > SizeInfoX / 2 || x2 < -SizeInfoX / 2 || y2 < -SizeInfoY / 2 || y2 > SizeInfoY / 2)
            {
               // 폼 사이즈를 조절 할 수 있게 되어 기능 삭제 
            }
            else
            {
                if (chk     == false)
                {
                    string[] values = selectedPoint.Split(',');
                    x2 = float.Parse(values[0]);
                    y2 = float.Parse(values[1]);
                    tx2 = x2;
                    ty2 = y2;


                    if (PicLotate == 90 || PicLotate == -270)
                    {
                        var temp = x2;
                        x2 = y2;
                        y2 = -temp;
                    }
                    else if (PicLotate == -90 || PicLotate == 270)
                    {
                        var temp = x2;
                        x2 = -y2;
                        y2 = temp;
                    }
                    else if (PicLotate == -180 || PicLotate == 180)
                    {
                        x2 = -x2;
                        y2 = -y2;
                    }

                }
                else
                {
                    skPen = new SKPaint() { Color = Color.BlueViolet.ToSKColor() };
                }
                float crdX = (x2 / 10f);
                float crdY = (y2 / 10f);
                float finX = x1 + (bitmap.Width * crdX / (SizeInfoX / 10f));
                float finY = y1 - (bitmap.Height * crdY / (SizeInfoY / 10f));
                float textX = finX;
                float textY = finY;

                if (SizeDrgInfo != 0)
                {
                    float DrX1 = 0;
                    float DrY1 = 0;
                    float DrX2 = 0;
                    float DrY2 = 0;

                    if (IsSizeDraw == true)
                    {
                        DrX1 = float.Parse(DragStartX);
                        DrY1 = float.Parse(DragStartY);
                        DrX2 = float.Parse(DragEndX);
                        DrY2 = DrgEgY;
                    }
                    else
                    {
                        DrX1 = float.Parse(PrevDrgX1);
                        DrY1 = float.Parse(PrevDrgY1);
                        DrX2 = float.Parse(PrevDrgX2);
                        DrY2 = float.Parse(PrevDrgY2);
                    }

                    if (DrX1 < DrX2)
                    {
                        finX = bitmap.Width * (x2 - DrX1) / (SizeDrgInfo);
                    }
                    else
                    {
                        finX = bitmap.Width * (x2 - DrX2) / (SizeDrgInfo);
                    }
                    if (DrY1 < DrY2)
                    {
                        finY = bitmap.Height - (bitmap.Height * (y2 - DrY1) / (SizeDrgInfo));
                    }
                    else
                    {
                        finY = bitmap.Height - (bitmap.Height * (y2 - DrY2) / (SizeDrgInfo));
                    }
                }



                //점 크기 조절 (최소 반지름 2
                if (SizeDrgInfo > 500)
                {
                    canvas.DrawCircle(finX, finY, 2, skPen);
                }
                else if (SizeDrgInfo > 400)
                {
                    canvas.DrawCircle(finX, finY, 3f, skPen);
                }
                else if (SizeDrgInfo > 300)
                {
                    canvas.DrawCircle(finX, finY, 3.5f, skPen);
                }
                else if (SizeDrgInfo > 150)
                {
                    canvas.DrawCircle(finX, finY, 4f, skPen);
                }
                else if (SizeDrgInfo > 80)
                {
                    canvas.DrawCircle(finX, finY, 4.5f, skPen);
                }
                else if (SizeDrgInfo > 0)
                {
                    canvas.DrawCircle(finX, finY, 5f, skPen);
                }
                else
                {
                    canvas.DrawCircle(finX, finY, 2, skPen);
                }


                if (finX > x1)
                {
                    textX = finX - 50;
                }
                else
                {
                    textX = finX + 15;
                }
                if (finY > y1)
                {
                    textY = finY - 10;
                }
                else
                {
                    textY = finY + 10;
                }
                canvas.DrawText(tx2 + ", " + ty2, textX, textY, skPen);
            }
            

        }

        //점 그리기
        private void DrawPointCircle(SKCanvas canvas,SKPaint skPen)
        {
            //좌표를 그려진 칸으로 위치시키기 위한 /10f  ( 49 = 그려진 칸 중에 4.9번째 칸 위치 )
            float crdX = (x2 / 10f);
            float crdY = (y2 / 10f);
            float finX = x1 + (bitmap.Width * crdX / (SizeInfoX / 10f)); //ex) 500칸으로 할 수 없으니 50칸으로 축소하기 위함의 /10f
            float finY = y1 - (bitmap.Height * crdY / (SizeInfoY / 10f));
            if (SizeDrgInfo != 0)
            {
                float DrX1 = 0;
                float DrY1 = 0;
                float DrX2 = 0;
                float DrY2 = 0;

                if (IsSizeDraw == true)
                {
                    DrX1 = float.Parse(DragStartX);
                    DrY1 = float.Parse(DragStartY);
                    DrX2 = float.Parse(DragEndX);
                    DrY2 = DrgEgY;
                }
                else
                {
                    DrX1 = float.Parse(PrevDrgX1);
                    DrY1 = float.Parse(PrevDrgY1);
                    DrX2 = float.Parse(PrevDrgX2);
                    DrY2 = float.Parse(PrevDrgY2);
                }
                if (DrX1 < DrX2)
                {
                    finX = bitmap.Width * (x2 - DrX1) / (SizeDrgInfo);
                }
                else
                {
                    finX = bitmap.Width * (x2 - DrX2) / (SizeDrgInfo);
                }
                if (DrY1 < DrY2)
                {
                    finY = bitmap.Height - (bitmap.Height * (y2 - DrY1) / (SizeDrgInfo));
                }
                else
                {
                    finY = bitmap.Height - (bitmap.Height * (y2 - DrY2) / (SizeDrgInfo));
                }
            
                
            }

            float textX = finX + 20;
            float textY = finY + 20;

            

            //상하가 반전됨 ( SkiSharp로 생성한 Rect의  Y는 아래로 내려가면 + 위로 올라가면 -가 됨)
            if (x2 > SizeInfoX/2 || x2 < -SizeInfoX / 2 || y2 < -SizeInfoY / 2 || y2 > SizeInfoY / 2)
            {
                //MessageBox.Show(string.Format("Out Of Index : [{0}, {1}]", x2, y2));
                x2 = 0;
                y2 = 0;
                //isDist = false;
                Form1.f.Clear_TextBox();
                return;
            }
            else
            {

                if (SizeDrgInfo > 500)
                {
                    canvas.DrawCircle(finX, finY, 2, skPen);
                }
                else if (SizeDrgInfo > 400)
                {
                    canvas.DrawCircle(finX, finY, 3f, skPen);
                }
                else if (SizeDrgInfo > 300)
                {
                    canvas.DrawCircle(finX, finY, 3.5f, skPen);
                }
                else if (SizeDrgInfo > 150)
                {
                    canvas.DrawCircle(finX, finY, 4f, skPen);
                }
                else if (SizeDrgInfo > 80)
                {
                    canvas.DrawCircle(finX,finY,4.5f, skPen);   
                }
                else if (SizeDrgInfo > 0)
                {
                    canvas.DrawCircle(finX, finY, 5f, skPen);
                }
                else
                {
                    canvas.DrawCircle(finX, finY, 2, skPen);
                }


                if (finX > x1)
                {
                    textX = finX - 50;
                }
                else
                {
                    textX = finX + 15;
                }
                if (finY > y1)
                {
                    textY = finY - 10;
                }
                else
                {
                    textY = finY + 10;
                }
                if (CrdnChk == true)
                {
                    canvas.DrawText(tx2 + ", " + ty2, textX, textY, skPen);
                }
                

            }
        }
        
        //화면 구성
        private void DrawLayout(SKCanvas canvas)
        {  
            float MapXSize = bitmap.Width;
            float MapYSize = bitmap.Height;

            CreateLine(canvas,MapXSize,MapYSize);
        }

        //화면 구성(선)
        private void CreateLine(SKCanvas canvas, float MapXSize, float MapYSize)
        {
            #region 회색 선

            //가로 선
            if (bitmap.Width > bitmap.Height)
            {
                for (int i = 1; i <= bitmap.Width / (MapYSize / 50); i++)
                {
                    canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    canvas.DrawLine(MapYSize * i / 50, 0, MapYSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    //canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    //canvas.DrawLine(MapXSize * i / 50, 0, MapXSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                }
            }
            else
            {
                for (int i = 1; i <= bitmap.Height / (MapYSize / 50); i++)
                {
                    canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    canvas.DrawLine(MapYSize * i / 50, 0, MapYSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    //canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    //canvas.DrawLine(MapXSize * i / 50, 0, MapXSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                }
            }

            #region 미사용
            /*for (int i = 1; i <= (bitmap.Width * (MapYSize / 50)); i++)
            {
                //canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                //canvas.DrawLine(MapYSize * i / 50, 0, MapYSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                
                //canvas.DrawText(""+((MapYSize / 50)), bitmap.Width / 2, bitmap.Height / 2, new SKPaint() { Color = Color.Black.ToSKColor() });
            }*/
            //미사용 (추가 필요)
            /*if (SizeInfo >= 500) {
                for (int i = 1; i <= 51; i++)
                {
                    canvas.DrawLine(0, MapYSize * i / 50, MapXSize, MapYSize * i / 50, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    canvas.DrawLine(MapXSize * i / 50, 0, MapXSize * i / 50, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                }
            }
            else if (SizeInfo >= 300 && SizeInfo < 500)
            {
                for (int i = 1; i <= 31; i++)
                {
                    canvas.DrawLine(0, MapYSize * i / 30, MapXSize, MapYSize * i / 30, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    canvas.DrawLine(MapXSize * i / 30, 0, MapXSize * i / 30, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                }
            }
            else if (SizeInfo >=1 && SizeInfo < 300)
            {
                for (int i = 1; i <= 21; i++)
                {
                    canvas.DrawLine(0, MapYSize * i / 10, MapXSize, MapYSize * i / 10, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                    canvas.DrawLine(MapXSize * i / 10, 0, MapXSize * i / 10, MapYSize, new SKPaint() { Color = Color.LightGray.ToSKColor() });
                }
            }*/
            #endregion
            #endregion

            #region 테두리 만들기
            canvas.DrawLine(0, 0, 0, MapYSize, new SKPaint() { Color = Color.Black.ToSKColor() });
            canvas.DrawLine(MapXSize, MapYSize, 0, MapYSize, new SKPaint() { Color = Color.Black.ToSKColor() });
            canvas.DrawLine(MapXSize, MapYSize, MapXSize, 0, new SKPaint() { Color = Color.Black.ToSKColor() });
            canvas.DrawLine(MapXSize, 0, 0, 0, new SKPaint() { Color = Color.Black.ToSKColor() });
            
            #endregion


            

            //드래그로 범위를 지정하지 않았을 때,
            if (IsNewDrawMap == false)
            {
                canvas.DrawText("[" + SizeInfoX / 2 + ",0]", MapXSize - 40, MapYSize / 2 - 3, new SKPaint() { Color = Color.Gray.ToSKColor() });
                canvas.DrawText("[0," + SizeInfoY / 2 + "]", MapXSize / 2 + 5, 13, new SKPaint() { Color = Color.Gray.ToSKColor() });

                //중앙 검은 선 찍기 0,0
                canvas.DrawLine(0, y1, MapXSize, y1, new SKPaint() { Color = Color.Black.ToSKColor() });
                canvas.DrawLine(x1, 0, x1, MapYSize, new SKPaint() { Color = Color.Black.ToSKColor() });
            }
            // 드래그로 범위를 지정했을 때
            else
            {
                try
                {

                    #region Drag 확대 범위 지정 시 사용할 변수
                    float DrgSizeInfo = 0;
                    float Ydist = 0;

                    //float DrgSizeInfoY = 0;

                    float DrX1 = 0;
                    float DrY1 = 0;
                    float DrX2 = 0;
                    float DrY2 = 0;

                    if (IsSizeDraw == true)
                    {
                        DrX1 = float.Parse(DragStartX);
                        DrY1 = float.Parse(DragStartY);
                        DrX2 = float.Parse(DragEndX);
                        DrY2 = DrgEgY;
                    }
                    else
                    {
                        DrX1 = float.Parse(PrevDrgX1);
                        DrY1 = float.Parse(PrevDrgY1);
                        DrX2 = float.Parse(PrevDrgX2);
                        DrY2 = float.Parse(PrevDrgY2);
                    }

                    if (DrX1 < DrX2)
                    {
                        DrgSizeInfo = DrX2 - DrX1; //가로 거리(정사각형이라 동일)
                    }
                    else
                    {
                        DrgSizeInfo = DrX1 - DrX2;
                    }

                    if (DragEndY != null)
                    {
                        if (DrY1 < float.Parse(DragEndY))
                        {

                            DrgEgY = DrY1 + DrgSizeInfo;
                            Ydist = DrY1 + DrgSizeInfo;
                        }
                        else
                        {
                            Ydist = DrY1 - DrgSizeInfo;
                            DrgEgY = DrY1 - DrgSizeInfo;
                        }
                    }
                    else if(PrevDrgY2 != null)
                    {
                        if (DrY1 < float.Parse(PrevDrgY2))
                        {

                            DrgEgY = DrY1 + DrgSizeInfo;
                            Ydist = DrY1 + DrgSizeInfo;
                        }
                        else
                        {
                            Ydist = DrY1 - DrgSizeInfo;
                            DrgEgY = DrY1 - DrgSizeInfo;
                        }
                    }
                    //정사각형을 맞추기 위해 X간의 거리 만큼 Y를 벌릴 것
                    

                    SizeDrgInfo = DrgSizeInfo;
                    /*SizeDrgInfoY = DrgSizeInfoY;*/


                    #endregion

                    //Y축
                    if (DrX1 < 0 && DrX2 > 0)
                    { //x1 + (bitmap.Width * crdX / (SizeInfo / 10f)); 얘는 중앙에서 좌표까지 이동할 때니까 바꿔서 써야함

                        float distDrgX = (0 - DrX1) / 10f;
                        LineXind = bitmap.Width * distDrgX / (DrgSizeInfo / 10f);

                        canvas.DrawLine(LineXind, 0, LineXind, MapYSize, new SKPaint() { Color = Color.Black.ToSKColor() });
                    }
                    else if (DrX2 < 0 && DrX1 > 0)
                    {
                        float distDrgX = (0 - DrX2) / 10f;
                        LineXind = bitmap.Width * distDrgX / (DrgSizeInfo / 10f);
                        canvas.DrawLine(LineXind, 0, LineXind, MapYSize, new SKPaint() { Color = Color.Black.ToSKColor() });
                    }

                    //X축
                    if (DrY1 < 0 && DrgEgY > 0)
                    {
                        float distDrgY = Ydist / 10f;
                        LineYind = bitmap.Height - (bitmap.Height - (bitmap.Height * (distDrgY / (DrgSizeInfo / 10f))));
                        canvas.DrawLine(0, LineYind, MapXSize, LineYind, new SKPaint() { Color = Color.Black.ToSKColor() });
                    }
                    else if (DrgEgY < 0 && DrY1 > 0)
                    {
                        float distDrgY = Ydist / 10f;
                        LineYind = bitmap.Height + (bitmap.Height * (distDrgY / (DrgSizeInfo / 10f)));
                        canvas.DrawLine(0, LineYind, MapXSize, LineYind, new SKPaint() { Color = Color.Black.ToSKColor() });
                    }


                    // => 점1(x1,y1)  점2(x2,DrgEgY) 



                    float TempX1 = 0;
                    float TempY1 = 0;
                    float TempX2 = 0;
                    float TempY2 = 0;
                    if (DrX1 < DrX2)
                    {
                        TempX1 = DrX1;
                        TempX2 = DrX2;
                    }
                    else
                    {
                        TempX1 = DrX2;
                        TempX2 = DrX1;
                    }

                    TempY1 = DrY1;
                    TempY2 = DrgEgY;

                    if (DrY1 < DrgEgY)
                    {
                        //중앙 상단
                        canvas.DrawText("[" + ((TempX2 + TempX1) / 2).ToString("0") + "," + TempY2.ToString("0") + "]", bitmap.Width / 2, 13, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //중앙 우측
                        canvas.DrawText("[" + TempX2.ToString("0") + "," + ((TempY2 + TempY1) / 2).ToString("0") + "]", bitmap.Width - 40, bitmap.Height / 2, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //좌상단
                        canvas.DrawText("[" + TempX1.ToString("0") + "," + TempY2.ToString("0") + "]", 20, 13, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //우하단
                        canvas.DrawText("[" + TempX2.ToString("0") + "," + TempY1.ToString("0") + "]", MapXSize - 60, MapYSize - 13, new SKPaint() { Color = Color.Gray.ToSKColor() });
                    }
                    else
                    {
                        //중앙 상단
                        canvas.DrawText("[" + ((TempX2 + TempX1) / 2).ToString("0") + "," + TempY1.ToString("0") + "]", bitmap.Width / 2, 13, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //중앙 우측
                        canvas.DrawText("[" + TempX2.ToString("0") + "," + ((TempY2 + TempY1) / 2).ToString("0") + "]", bitmap.Width - 60, bitmap.Height / 2, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //좌상단
                        canvas.DrawText("[" + TempX1.ToString("0") + "," + TempY1.ToString("0") + "]", 20, 13, new SKPaint() { Color = Color.Gray.ToSKColor() });

                        //우하단
                        canvas.DrawText("[" + TempX2.ToString("0") + "," + TempY2.ToString("0") + "]", MapXSize - 60, MapYSize - 13, new SKPaint() { Color = Color.Gray.ToSKColor() });
                    }

                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }


        //출력할 비트맵 생성
        void CreateBitmap()
        {
            if (bitmap == null || bitmap.Width != Width || bitmap.Height != Height)
            {
                FreeBitmap();
                bitmap = new Bitmap(this.Width,this.Height, PixelFormat.Format32bppPArgb);
                if (isList == true)
                {
                    Form1.f.MultiPinPointed();
                }
                else if (chk == true) 
                {
                    //Form1.f (메인 Form에서 생성)의 PointEv()를 실행하여 x2, y2의 값을 다시 받아옴 (ReSize 시,)
                    Form1.f.PointEv(x2,y2);

                }
                
                Invalidate();
                //Invalidate를 실행하면 이후 OnPaint가 호출되어 새로 그려짐 ( -> 새로 그린다 = chk 등의 값을 넘기면 처음과 다르게 그릴 수 있다                     
            }
        }

        //FreeBitmap
        void FreeBitmap()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }


        public void MoveingMouseEv(SKCanvas canvas)
        {
            string mX = movingX.ToString("0.#");
            string mY = movingY.ToString("0.#");

            if (PicLotate == 90 || PicLotate == -270)
            {
                var temp = mX;
                mX = "" + -float.Parse(mY);
                mY = ""+float.Parse(temp);
            }
            else if (PicLotate == -90 || PicLotate == 270)
            {
                var temp = mX;
                mX = "" + float.Parse(mY);
                mY = "" + -float.Parse(temp);
            }
            else if (PicLotate == -180 || PicLotate == 180)
            {
                mX = "" + -float.Parse(mX);
                mY = "" + -float.Parse(mY);
            }

            if (realX > x1)
            {
                canvas.DrawText(mX + "," + mY, realX - 60f, realY + 10f, new SKPaint() { Color = Color.Black.ToSKColor() });
            }
            else
            {
                canvas.DrawText(mX + "," + mY, realX + 10f, realY + 10f, new SKPaint() { Color = Color.Black.ToSKColor() });
            }
        

            
        }

        void DragPosition(SKCanvas canvas)
        {
            try
            {

                float ax2 = 0;
                float ay2 = 0;

                SKPaint pen = new SKPaint() { Color = Color.YellowGreen.ToSKColor() };
                pen.StrokeWidth = 2;
                SKColor halfTransparentBlue = SKColors.Yellow.WithAlpha(0x10);

                SKPaint RecPen = new SKPaint() { Color = halfTransparentBlue };


                // 그려진 bit맵 크기 안에서 비율을 맞게 할 필요가 있음 -- 안하면, 처음 500x500 사이즈로 좌표가 잡힘 
                float ax1 = x1 + (bitmap.Width * (float.Parse(NonExpStartX) / 10f) / (SizeInfoX / 10f));
                float ay1 = y1 - (bitmap.Height * (float.Parse(NonExpStartY) / 10f) / (SizeInfoY / 10f));
                

                if (NonExpEndX == null && DrgEgY ==0)
                {
                    ax2 = x1 + (bitmap.Width * (float.Parse(NonMouseUpX) / 10f) / (SizeInfoX / 10f));
                    ay2 = y1 - (bitmap.Height * (float.Parse(NonMouseUpY) / 10f) / (SizeInfoY / 10f));
                }
                else if (NonExpEndX != null && DrgEgY ==0)
                {
                    ax2 = x1 + (bitmap.Width * (float.Parse(NonExpEndX) / 10f) / (SizeInfoX / 10f));
                    ay2 = y1 - (bitmap.Height * (float.Parse(NonExpEndY) / 10f) / (SizeInfoY / 10f));
                }
                else if (DrgEgY != 0)
                {
                    
                    ax1 = float.Parse(NonExpStartX);
                    ay1 = float.Parse(NonExpStartY);
                    if (NonExpEndX == null)
                    {
                        ax2 = float.Parse(NonMouseUpX);
                        ay2 = float.Parse(NonMouseUpY);
                    }
                    else
                    {
                        ax2 = float.Parse(NonExpEndX);
                        ay2 = float.Parse(NonExpEndY);
                    }
                }

                SKRect RectStrc = new SKRect(ax1, ay1, ax2, ay2);

                //확대 범위를 지정했을때.
                if (IsDragChked == true && IsUnCheckedEnlg == false)
                {
                    
                    if (ax1 < ax2)
                    {
                        if (ay1 < ay2)
                        {
                            ay2 = ay1 + (ax2 - ax1);
                        }
                        else
                        {
                            ay2 = ay1 - (ax2 - ax1);
                        }
                    }
                    else
                    {
                        if (ay1 < ay2)
                        {
                            ay2 = ay1 + (ax1 - ax2);
                        } 
                        else
                        {
                            ay2 = ay1 - (ax1 - ax2);
                        }
                    }

                    canvas.DrawLine(ax1, ay1, ax2, ay1, pen);
                    canvas.DrawLine(ax2, ay1, ax2, ay2, pen);
                    canvas.DrawLine(ax2, ay2, ax1, ay2, pen);
                    canvas.DrawLine(ax1, ay2, ax1, ay1, pen);
                }
                else
                {
                    canvas.DrawRect(RectStrc, RecPen);
                    canvas.DrawLine(ax1, ay1, ax2, ay1, pen);
                    canvas.DrawLine(ax2, ay1, ax2, ay2, pen);
                    canvas.DrawLine(ax2, ay2, ax1, ay2, pen);
                    canvas.DrawLine(ax1, ay2, ax1, ay1, pen);
                }


            }
            catch (Exception ex)
            {
                return;
            }


        }

    }



    public static class Extention
    {
        public static SKColor ToSKColor(this Color color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }
}
