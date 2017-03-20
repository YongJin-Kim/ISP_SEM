using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using SEC.GenericSupport.Mathematics;

namespace SEC.GUIelement
{
    public class ScalebarDrawer
    {
        public enum TickStyle
        {
            Ellipse,
            Rectangle,

        }

        private ScalebarDrawer() { }

        /// <summary>
        /// 미크론바의 이미지를 생성한다.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="backColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="egdeColor"></param>
        /// <param name="discription"></param>
        /// <param name="hvAlias"></param>
        /// <param name="magAlias"></param>
        /// <param name="dpiSystem"></param>
        /// <param name="realMagnificaion"></param>
        /// <param name="padding"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static Bitmap MakeMicronbar(Size area,
                                            Color backColor,
                                            Color foreColor,
                                            Color egdeColor,
                                            Font stringFont,
                                            string company,
                                            string discription,
                                            bool micronVoltage,
                                            string hvAlias,
                                            bool micronmag,
                                            string magAlias,
                                            double pixelWidth,
                                            double realMagnificaion,
                                            System.Windows.Forms.Padding padding,
                                            TickStyle style,
                                            bool microndetector,
                                            bool micronvac,
                                            bool microndate,
                                            bool microncompany,
                                            string date,
                                            Image dot,
                                            Image scalebar)
        {
            return MakeMicronbar(area, backColor, foreColor, egdeColor, stringFont, company, discription, micronVoltage, hvAlias, micronmag, magAlias, pixelWidth,
                                 realMagnificaion, padding, style, null, null, microndetector, micronvac, microndate, microncompany, date, dot, scalebar);
        }



        public static Bitmap MakeMicronbar(Size area,
                                    Color backColor,
                                    Color foreColor,
                                    Color egdeColor,
                                    Font stringFont,
                                    string company,
                                    string discription,
                                    bool micronvoltage,
                                    string hvAlias,
                                    bool micronmag,
                                    string magAlias,
                                    double pixelWidth,
                                    double realMagnificaion,
                                    System.Windows.Forms.Padding padding,
                                    TickStyle style,
                                    string etcStr,
                                    string vacStr,
                                    bool microndetector,
                                    bool micronvac,
                                    bool microndate,
                                    bool microncompany,
                                    string date,
                                    Image dot,
                                    Image scalebar)
        {
            System.Diagnostics.Trace.Assert(pixelWidth != 0);

            // 이미지 생성 및 초기화
            Bitmap bm = new Bitmap(area.Width, area.Height);



            Graphics g = Graphics.FromImage(bm);
            g.Clear(backColor);

            RectangleF scalebarRect = new RectangleF(0, 0, 0, 0);

            RectangleF companyRect = new RectangleF(0, 0, 0, 0);
            RectangleF com_graph = new RectangleF(0, 0, 0, 0);

            RectangleF desciptorRect = new RectangleF(0, 0, 0, 0);
            RectangleF info_graph = new RectangleF(0, 0, 0, 0);

            RectangleF hvRect = new RectangleF(0, 0, 0, 0);
            RectangleF hv_graph = new RectangleF(0, 0, 0, 0);

            RectangleF etcRect = new RectangleF(0, 0, 0, 0);
            RectangleF etc_graph = new RectangleF(0, 0, 0, 0);

            RectangleF vacRect = new RectangleF(0, 0, 0, 0);
            RectangleF vac_graph = new RectangleF(0, 0, 0, 0);

            RectangleF magRect = new RectangleF(0, 0, 0, 0);
            RectangleF mag_graph = new RectangleF(0, 0, 0, 0);

            RectangleF MonthRect = new RectangleF(0, 0, 0, 0);
            RectangleF Month_graph = new RectangleF(0, 0, 0, 0);

            RectangleF dateRect = new RectangleF(0, 0, 0, 0);
            RectangleF date_graph = new RectangleF(0, 0, 0, 0);

            RectangleF ScaleBarRect1 = new RectangleF(0, 0, 0, 0);





            #region 각 종류별 표시 영역 결정
            if (area.Width < 600)
            {	// Scale Bar만 표시
                desciptorRect = new RectangleF(0, 0, 0, 0);
                hvRect = new RectangleF(0, 0, 0, 0);
                magRect = new RectangleF(0, 0, 0, 0);
                scalebarRect = new RectangleF(padding.Left, padding.Top, area.Width - padding.Horizontal, area.Height - padding.Vertical);
            }
            else if (area.Width < 800)
            { // Descriptor, Scale Bar
                //desciptorRect = new RectangleF((micronArea.Width / 20), micronBlank / 2, (micronArea.Width / 4), micronArea.Height - micronBlank);
                desciptorRect = new RectangleF(padding.Left, padding.Top, ((area.Width - padding.Horizontal * 2) / 4), area.Height - padding.Vertical);
                hvRect = new RectangleF(0, 0, 0, 0);
                magRect = new RectangleF(0, 0, 0, 0);
                scalebarRect = new RectangleF(desciptorRect.Right + padding.Horizontal, padding.Top, ((area.Width - padding.Horizontal * 2) * 3 / 4), area.Height - padding.Vertical);
            }
            else
            { // Discriptor, HV, Mag, Scale Bar
                if (etcStr != null)
                {
                    float disWidth = (area.Width - padding.Vertical * 4);
                    float disHeight = area.Height - padding.Vertical;

                    Point newPadding = new Point(5, (int)(disHeight / 2));

                    int count = 2;



                    if (microndate)
                    {
                        //Month_graph = new RectangleF(newPadding.X + 10, padding.Top, 20, 40);
                        //MonthRect = new RectangleF(Month_graph.Right + 3, Month_graph.Top, ((disWidth / 2) / 5) * 2, Month_graph.Height);

                        date_graph = new RectangleF(newPadding.X + 10, newPadding.Y + 5, 20, 20);
                        dateRect = new RectangleF(date_graph.Right + 3, date_graph.Top, ((disWidth / 2) / 5) * 2, date_graph.Height);

                        newPadding = new Point((int)dateRect.Right, (int)dateRect.Top);



                    }

                    //if (microncompany)
                    //{
                    //    com_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                    //    companyRect = new RectangleF(com_graph.Right + 3, com_graph.Top, ((disWidth / 2) / 6) * count, com_graph.Height);
                    //    count = 1;
                    //    newPadding = new Point((int)companyRect.Right, (int)companyRect.Top);
                    //}


                    if (micronvoltage)
                    {
                        hv_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                        hvRect = new RectangleF(hv_graph.Right + 3, hv_graph.Top, ((disWidth / 2) / 5) * 1, hv_graph.Height);
                        count = 1;
                        newPadding = new Point((int)hvRect.Right, (int)hvRect.Top);

                    }

                    if (micronmag)
                    {
                        mag_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                        magRect = new RectangleF(mag_graph.Right + 3, mag_graph.Top, ((disWidth / 2) / 5) * 1, mag_graph.Height);

                        newPadding = new Point((int)magRect.Right, (int)magRect.Top);
                        count = 1;
                    }

                    if (micronvac)
                    {
                        vac_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                        vacRect = new RectangleF(vac_graph.Right + 3, vac_graph.Top, ((disWidth / 2) / 5) * 2, vac_graph.Height);

                        newPadding = new Point((int)vacRect.Right, (int)vacRect.Top);
                        count = 1;
                    }

                    if (microndetector)
                    {
                        etc_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                        etcRect = new RectangleF(etc_graph.Right + 3, etc_graph.Top, ((disWidth / 2) / 5) * 1, etc_graph.Height);
                        newPadding = new Point((int)etcRect.Right, (int)etcRect.Top);
                        count = 1;
                    }

                    if (microncompany)
                    {
                        com_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                        companyRect = new RectangleF(com_graph.Right + 3, com_graph.Top, ((disWidth / 2) / 5) * 2, com_graph.Height);
                        count = 1;
                        newPadding = new Point((int)companyRect.Right, (int)companyRect.Top);
                    }








                    //info_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                    //desciptorRect = new RectangleF(info_graph.Right + 3, info_graph.Top, ((disWidth / 2) / 6), info_graph.Height);

                    ScaleBarRect1 = new RectangleF((disWidth / 2) - (500 / 2) + 30, padding.Top, 500, 15);
                    //info_graph = new RectangleF(newPadding.X + 10, newPadding.Y, 20, 20);
                    //desciptorRect = new RectangleF(info_graph.Right + 3, info_graph.Top, ((disWidth / 2) / 6), info_graph.Height);

                    //ScaleBarRect1 = new RectangleF((disWidth / 3) + (disWidth / 4) + 30 , padding.Top + 14, 500, 13);


                }
                else
                {
                    float disWidth = (area.Width - padding.Vertical * 4);
                    float disHeight = area.Height - padding.Vertical;
                    desciptorRect = new RectangleF(padding.Left, padding.Top, disWidth * 3 / 10, disHeight);
                    hvRect = new RectangleF(desciptorRect.Right + padding.Horizontal, padding.Top, disWidth / 10, disHeight);
                    magRect = new RectangleF(hvRect.Right + padding.Horizontal, padding.Top, disWidth / 10, disHeight);
                    scalebarRect = new RectangleF(magRect.Right + padding.Horizontal, padding.Top, disWidth / 2, disHeight);
                }
            }
            #endregion

            #region 그리기
            Brush textBrush = new SolidBrush(foreColor);
            Brush edgeBrush = new SolidBrush(egdeColor);

            if (companyRect.Width > 0)
            {
                if (dot != null)
                {
                    //g.DrawImage(dot, com_graph);
                }

                DrawEdgeString(company, g, companyRect, textBrush, edgeBrush, stringFont);
            }

            if (hvRect.Width > 0)
            {


                if (dot != null)
                {
                    //g.DrawImage(dot, hv_graph);
                }
                DrawEdgeString(hvAlias, g, hvRect, textBrush, edgeBrush, stringFont);
            }

            if (etcRect.Width > 0)
            {
                if (dot != null)
                {
                    //g.DrawImage(dot, etc_graph);
                }
                DrawEdgeString(etcStr, g, etcRect, textBrush, edgeBrush, stringFont);
            }

            if (vacRect.Width > 0)
            {
                if (dot != null)
                {
                    //g.DrawImage(dot, vac_graph);
                }
                DrawEdgeString(vacStr, g, vacRect, textBrush, edgeBrush, stringFont);
            }


            if (magRect.Width > 0)
            {
                if (dot != null)
                {
                    //g.DrawImage(dot, mag_graph);
                }
                DrawEdgeString(magAlias, g, magRect, textBrush, edgeBrush, stringFont);
            }

            if (dateRect.Width > 0)
            {
                if (dot != null)
                {
                    //g.DrawImage(dot, date_graph);
                }

                //string date = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

                //string[] day = date.Split(' ');

                //DrawEdgeString(day[0], g, MonthRect, textBrush, edgeBrush, stringFont);
                DrawEdgeString(date, g, dateRect, textBrush, edgeBrush, stringFont);
            }

            if (desciptorRect.Width > 0)
            {
                if (dot != null && discription != "")
                {
                    g.DrawImage(dot, info_graph);
                }

                DrawEdgeString(discription, g, desciptorRect, textBrush, edgeBrush, stringFont);
            }

            //if (ScaleBarRect1.Width > 0)
            //{
            //    if (scalebar != null)
            //    {
            //        g.DrawImage(scalebar, ScaleBarRect1);
            //    }
            //}

            if (scalebar != null)
            {
                DrawTicks(realMagnificaion, pixelWidth, g, ScaleBarRect1, style, textBrush, edgeBrush, stringFont, scalebar);
            }




            textBrush.Dispose();
            edgeBrush.Dispose();
            #endregion

            return bm;
        }

        //private static void DrawTicks(double realMagnificaion, double pixelWidth, Graphics g, RectangleF scalebarRect, TickStyle style, Brush textBrush, Brush edgeBrush, Font strFont)
        private static void DrawTicks(double realMagnificaion, double pixelWidth, Graphics g, RectangleF scalebarRect, TickStyle style, Brush textBrush, Brush edgeBrush, Font strFont, Image scaleBar)
        {
            SizeF tickSmall = new SizeF(strFont.Size / 3, strFont.Size / 2);
            SizeF tickLarge = new SizeF(strFont.Size * 2 / 3, strFont.Size);

            #region 실제 그릴 거리 측정
            //double phisicalLength = ((double)scalebarRect.Width - tickLarge.Width) * pixelWidth / realMagnificaion;
            double phisicalLength = ((double)scalebarRect.Width - tickLarge.Width) * pixelWidth;

            Engineer scale = new Engineer(phisicalLength);
            scale = Engineer.ApproximateMantissa(scale, new double[] { 1, 2, 3, 5 });

            phisicalLength = scale.Value;

            // 실제로 그릴 pixels
            //float drawLength = (float)(phisicalLength * realMagnificaion / pixelWidth);
            float drawLength = (float)(phisicalLength / pixelWidth);
            #endregion

            #region tick 위치 결정

            const int tickCnt = 11;

            float tickStart = ((float)scalebarRect.Width - drawLength) / 2;
            //float tickEnd = tickStart + drawLength;
            float tickEnd = drawLength;
            float tickY = scalebarRect.Y + scalebarRect.Height / 8 - tickLarge.Height;
            string str = "0";



            if (tickY < 0) { tickY = 0; }

            PointF[] pntsTicks = CalTicksLocation(tickStart, tickEnd, tickY, tickCnt);
            g.DrawImage(scaleBar, scalebarRect.Left + tickStart / 2, scalebarRect.Top, tickEnd, tickCnt);
            //g.DrawString(str, stringFont, textBrush, rectDisplay, style);
            //g.DrawString(str, strFont, textBrush, new PointF(scalebarRect.Left + tickStart / 2 -10, scalebarRect.Top + 20));
            #endregion

            #region Tick 그리기
            SmoothingMode oriSM = g.SmoothingMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Action<Brush, RectangleF> tdd;
            //Action<Brush,Image> tdd;

            switch (style)
            {
                case TickStyle.Ellipse:
                    tdd = g.FillEllipse;
                    break;
                case TickStyle.Rectangle:
                    tdd = g.FillRectangle;
                    break;
                default:
                    throw new ArgumentException("Undefined TickStyle");
            }

            //tdd = g.fil;

            SizeF tickSize;

            for (int i = 0; i < tickCnt; i++)
            {
                if (i % (tickCnt - 1) > 0)
                {
                    tickSize = tickSmall;
                }
                else
                {
                    tickSize = tickLarge;
                }

                RectangleF rect = new RectangleF(pntsTicks[i].X - tickSize.Width / 2f + scalebarRect.Left, pntsTicks[i].Y + 14 /*- tickSize.Height / 2F + scalebarRect.Top*/, tickSize.Width, tickSize.Height);

                //rect.Offset(1, 1);
                //g.DrawImage(scaleBar, rect);
                ////tdd(edgeBrush, rect);
                //rect.Offset(0, -2);
                //g.DrawImage(scaleBar, rect);
                ////tdd(edgeBrush, rect);

                //rect.Offset(-2, 0);
                //g.DrawImage(scaleBar, rect);
                ////tdd(edgeBrush, rect);

                //rect.Offset(0, 2);
                //g.DrawImage(scaleBar, rect);
                ////tdd(edgeBrush, rect);

                //rect.Offset(1, -1);
                //g.DrawImage(scaleBar, rect);
                //tdd(textBrush, rect);


                //g.DrawImage(scaleBar, rect);



            }
            //g.DrawImage(scaleBar, scalebarRect);




            //tdd(edgeBrush, scaleBar);



            g.SmoothingMode = oriSM;
            #endregion

            #region Scale
            //RectangleF scaleRect = new RectangleF(scalebarRect.X, scalebarRect.Y + tickLarge.Height, scalebarRect.Width, scalebarRect.Height - tickLarge.Height);
            //RectangleF scaleRect = new RectangleF(scalebarRect.Left + tickStart / 2 - 10, scalebarRect.Top + 20);
            //RectangleF scaleRect = new RectangleF((scalebarRect.Left + 100) - tickStart, scalebarRect.Top + 20, scalebarRect.Width, scalebarRect.Height + tickLarge.Height);
            RectangleF scaleRect = new RectangleF(scalebarRect.Left - tickStart, scalebarRect.Top + 7, scalebarRect.Width, scalebarRect.Height + tickLarge.Height);
            DrawScale(g, strFont, scale, scaleRect, textBrush, edgeBrush);
            //g.DrawString(str, strFont, textBrush, new PointF(scalebarRect.Left + tickStart / 2 - 7, scalebarRect.Top + 12));
            DrawEdgeString(str, g, new RectangleF(scalebarRect.Left + tickStart / 2 - 7, scalebarRect.Top + 12, scalebarRect.Width, scalebarRect.Height + tickLarge.Height), textBrush, edgeBrush, strFont);


            #endregion

        }

        private static PointF[] CalTicksLocation(float tickStart, float tickEnd, float tickY, int cnt)
        {
            PointF[] ticks = new PointF[cnt];

            float length = tickEnd - tickStart;

            for (int i = 0; i < cnt; i++)
            {
                ticks[i].X = tickStart + length * i / (cnt - 1);
                ticks[i].Y = tickY;
            }

            return ticks;
        }

        private static void DrawEdgeString(string str, Graphics g, RectangleF rectDisplay, Brush textBrush, Brush edgeBrush, Font stringFont)
        {
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            style.LineAlignment = StringAlignment.Center;

            RectangleF rect = rectDisplay;
            rect.Offset(1, 1);
            g.DrawString(str, stringFont, edgeBrush, rect, style);
            rect.Offset(0, -2);
            g.DrawString(str, stringFont, edgeBrush, rect, style);
            rect.Offset(-2, 0);
            g.DrawString(str, stringFont, edgeBrush, rect, style);
            rect.Offset(0, 2);
            g.DrawString(str, stringFont, edgeBrush, rect, style);

            g.DrawString(str, stringFont, textBrush, rectDisplay, style);

        }

        private static void DrawScale(
                        Graphics g,
                        Font font,
                        Engineer scale,
                        RectangleF bounds,
                        Brush textBrush,
                        Brush edgeBrush)
        {
            string[] unitTable = { " km", " m", " mm", " um", " nm", " pm", " fm" };
            int unitIndex = 0;
            double value = 0;

            for (int i = 0; i < unitTable.Length; i++)
            {
                int max = (i - 2) * -3;
                int min = (i - 1) * -3;

                if (min <= scale.Exponent && scale.Exponent < max)
                {
                    unitIndex = i;
                    value = scale.Mantissa * Math.Pow(10, scale.Exponent - min);
                    //Trace.WriteLine("Mantissa : "+scale.Mantissa.ToString());
                    break;
                }
            }

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            format.LineAlignment = StringAlignment.Far;

            string drawStr = value.ToString("0") + unitTable[unitIndex];




            bounds.Offset(1, 1);
            g.DrawString(drawStr, font, edgeBrush, bounds, format);
            bounds.Offset(0, -2);
            g.DrawString(drawStr, font, edgeBrush, bounds, format);
            bounds.Offset(-2, 0);
            g.DrawString(drawStr, font, edgeBrush, bounds, format);
            bounds.Offset(0, 2);
            g.DrawString(drawStr, font, edgeBrush, bounds, format);
            bounds.Offset(1, -1);
            g.DrawString(drawStr, font, textBrush, bounds, format);
        }



    }
}
