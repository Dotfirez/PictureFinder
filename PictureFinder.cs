using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dotfire
{
    public class PictureFinder
    {
        /// <summary>
        /// 查找局部图片的方法
        /// </summary>
        /// <param name="FindResulte">返回查找的结果，相似度大于设定的FoundRate比例</param>
        /// <param name="Source">原始大图片</param>
        /// <param name="Smallpic">原始大图片的局部小图片</param>
        /// <param name="OriginalFindResulte">返回原始的查找结果</param>
        /// <param name="FindAll">是否相同所有相同的多个子图片</param>
        /// <param name="FoundRate">查找到相同像素的百分比条件，1表示100%完全相同，默认值为0.90f表示90%的相似度</param>
        /// <param name="ExcludeColor">默认排除查找的颜色为黑色0xFF和白色0xFFFFFFFF(BGRA)</param>
        /// <param name="error">颜色的容错值，0~255，0表示没有色差，255表示最大色差</param>
        /// <param name="step">查找像素的步长，默认为1</param>
        /// <param name="deeppercent">取出多少百分比相同颜色进行比较，以提高速度，0~1f</param>
        /// <returns>是否找到了局部小图片</returns>
        public static bool FindSmallPic(out Dictionary<Point, float> FindResulte, Bitmap Source, Bitmap Smallpic, out Dictionary<Point, float> OriginalFindResulte, bool FindAll = false, float FoundRate = 0.90f, uint[] ExcludeColor = default, byte error = 20, int step = 1, float deeppercent = 0.1f)
        {
            int w1 = Source.Width;
            int h1 = Source.Height;
            int w = Smallpic.Width;
            int h = Smallpic.Height;
            if (step == default || step < 1)
                step = 1;

            if (ExcludeColor == default)
                ExcludeColor = new uint[] { 0xFF, 0xFFFFFFFF };//黑色和白色，最后一个字节是255表示是透明度
            Dictionary<uint, uint> colorcountsmall;
            Dictionary<uint, uint> colorcountbig;
            Dictionary<uint, List<Point>> ColorPointListSmall = GetBitmapColorPointList(Smallpic, out colorcountsmall);
            Dictionary<uint, List<Point>> ColorPointListBig = GetBitmapColorPointList(Source, out colorcountbig);
            ColorPointListBig = ColorPointListBig.Where(x => ColorPointListSmall.ContainsKey(x.Key) && !ExcludeColor.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            //有时小图不一定是原大图中的，要进行颜色的相互筛选
            ColorPointListSmall = ColorPointListSmall.Where(x => ColorPointListBig.ContainsKey(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            Dictionary<uint, double> count = new Dictionary<uint, double>();
            foreach (var v in ColorPointListSmall.Keys)
            {
                double vsmall = Math.Sqrt(colorcountsmall[v]);
                double vbig = Math.Sqrt(colorcountbig[v]);
                double cunt = vsmall * vbig;
                count.Add(v, cunt);
            }
            if (count.Count == 0)
            {
                FindResulte = null;
                OriginalFindResulte = null;
                GC.Collect();
                return false;
            }
            colorcountsmall = null;
            colorcountbig = null;
            count = count.OrderBy(x => x.Value).ToDictionary(p => p.Key, p => p.Value);
            int deep = (int)(count.Count* deeppercent);// (int)(count.Last().Value * deeppercent);
            uint[] corlortofind = count.OrderBy(x => x.Value).Take(deep).Select(x => x.Key).ToArray();

            count = null;
            ColorPointListBig = ColorPointListBig.Where(x => corlortofind.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            ColorPointListSmall = ColorPointListSmall.Where(x => corlortofind.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            corlortofind = null;
            Dictionary<Point, List<Point>> PointsFound = new Dictionary<Point, List<Point>>();
            foreach (var vv in ColorPointListSmall)
            {
                foreach (var v in vv.Value)
                {
                    List<Point> points = ColorPointListBig.Where(x => x.Key == vv.Key).Select(x => x.Value).First();
                    PointsFound.Add(v, points);
                }
            }
            ColorPointListBig = null;
            ColorPointListSmall = null;
            FindResulte = new Dictionary<Point, float>();

            foreach (var vp in PointsFound)
            {
                foreach (var p in vp.Value)
                {
                    float percent = 0;
                    float BeFind = 0;
                    int startY = p.Y - vp.Key.Y;
                    int startX = p.X - vp.Key.X;
                    if (startX < 0 || startY < 0 || startX > w1 - w || startY > h1 - h)
                        continue;
                    Point start = new Point(startX, startY);
                    if (FindResulte.ContainsKey(start))
                    {
                        continue;
                    }
                    float AllDot = 0;
                    for (int j = 0; j < h; j += step)
                    {
                        for (int i = 0; i < w; i += step)
                        {
                            AllDot++;
                            Color vsmal = Smallpic.GetPixel(i, j);
                            Color vsource = Source.GetPixel(i + startX, j + startY);
                            if (ColorSimilarity(vsmal, vsource) < error)
                            {
                                BeFind++;
                            }
                        }
                    }
                    percent = BeFind / AllDot;
                    FindResulte.Add(start, percent);
                }
            }

            OriginalFindResulte = FindResulte.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            FindResulte = OriginalFindResulte.Where(x => x.Value >= FoundRate).ToDictionary(x => x.Key, x => x.Value);
            bool ret = FindResulte.Count > 0;
            PointsFound = null;
            //GC.Collect();
            return ret;
        }
        /// <summary>
        /// 两颜色的RGB值的差的平均值
        /// </summary>
        /// <param name="a">颜色a</param>
        /// <param name="b">颜色b</param>
        /// <returns>两颜色的RGB值的差平均值</returns>
        private static byte ColorSimilarity(Color a, Color b)
        {
            //int Ea = (a.A - b.A);
            byte Er = (byte)Math.Abs(a.R - b.R);
            byte Eg = (byte)Math.Abs(a.G - b.G);
            byte Eb = (byte)Math.Abs(a.B - b.B);
            byte e = (byte)((Er + Eg + Eb) / 3);
            return e;
        }

        /// <summary>
        /// 查找局部图片的方法.
        /// </summary>
        /// <param name="FindResulte">返回查找的结果，相似度大于设定的FoundRate比例</param>
        /// <param name="Source">原始大图片</param>
        /// <param name="Smallpic">原始大图片的局部小图片</param>
        /// <param name="FoundRate">查找到相同像素的百分比条件，1表示100%完全相同，默认值为0.90f表示90%的相似度</param>
        /// <param name="ExcludeColor">默认排除查找的颜色为白色0xFFFFFFFF</param>
        /// <returns>是否找到了局部小图片</returns>
        public static bool FindSmallPic(out Dictionary<Point, float> FindResulte, Bitmap Source, Bitmap Smallpic, float FoundRate = 0.90f, uint[] ExcludeColor = default)
        {
            int WidthBig = Source.Width;
            int HeightBig = Source.Height;
            int WidthSmall = Smallpic.Width;
            int HeightSmall = Smallpic.Height;
            //查找的对比像素的步长，越大越快，但会降低精度，速度提升并不明显。
            int Step = 1;
            //比对多少种颜色，越少越快
            int ColorDeep = 1;
            if (ExcludeColor == default)
                ExcludeColor = new uint[] { 0xFFFFFFFF };
            Dictionary<uint, uint> colorcountsmall;
            Dictionary<uint, uint> colorcountbig;
            Dictionary<uint, List<Point>> ColorPointListSmall = GetBitmapColorPointList(Smallpic, out colorcountsmall);
            Dictionary<uint, List<Point>> ColorPointListBig = GetBitmapColorPointList(Source, out colorcountbig);
            ColorPointListBig = ColorPointListBig.Where(x => ColorPointListSmall.ContainsKey(x.Key) && !ExcludeColor.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            //有时小图不一定是原大图中的，要进行颜色的相互筛选
            ColorPointListSmall = ColorPointListSmall.Where(x => ColorPointListBig.ContainsKey(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            Dictionary<uint, double> count = new Dictionary<uint, double>();
            foreach (var v in ColorPointListSmall.Keys)
            {
                double vsmall = Math.Sqrt(colorcountsmall[v]);
                double vbig = Math.Sqrt(colorcountbig[v]);
                double allcount = vsmall * vbig;
                count.Add(v, allcount);
            }
            if (count.Count == 0)
            {
                FindResulte = null;
                return false;
            }

            colorcountsmall = null;
            colorcountbig = null;
            count = count.OrderBy(x => x.Value).ToDictionary(p => p.Key, p => p.Value);

            uint[] corlortofind = count.OrderBy(x => x.Value).Take(ColorDeep).Select(x => x.Key).ToArray();
            count = null;
            ColorPointListBig = ColorPointListBig.Where(x => corlortofind.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            ColorPointListSmall = ColorPointListSmall.Where(x => corlortofind.Contains(x.Key)).ToDictionary(p => p.Key, p => p.Value);
            Dictionary<Point, List<Point>> PointsFound = new Dictionary<Point, List<Point>>();
            foreach (var vv in ColorPointListSmall)
            {
                foreach (var v in vv.Value)
                {
                    List<Point> points = ColorPointListBig.Where(x => x.Key == vv.Key).Select(x => x.Value).First();
                    PointsFound.Add(v, points);
                }
            }
            ColorPointListBig = null;
            ColorPointListSmall = null;
            FindResulte = new Dictionary<Point, float>();
            Dictionary<Point, Rectangle> RectanglesFound = new Dictionary<Point, Rectangle>();
            foreach (var vp in PointsFound)
            {
                foreach (var p in vp.Value)
                {
                    float percent = 0;
                    float BeFind = 0;
                    int startY = p.Y - vp.Key.Y;
                    int startX = p.X - vp.Key.X;
                    if (startX < 0 || startY < 0 || startX > WidthBig - WidthSmall || startY > HeightBig - HeightSmall)
                        continue;
                    Point start = new Point(startX, startY);
                    if (FindResulte.ContainsKey(start))
                    {
                        continue;
                    }
                    float AllDot = 0;
                    for (int j = 0; j < HeightSmall; j += Step)
                    {
                        for (int i = 0; i < WidthSmall; i += Step)
                        {
                            AllDot++;
                            int vsmal = Smallpic.GetPixel(i, j).ToArgb();
                            int vsource = Source.GetPixel(i + startX, j + startY).ToArgb();
                            if (vsmal == vsource)
                            {
                                BeFind++;
                            }
                        }
                    }
                    percent = BeFind / AllDot;
                    FindResulte.Add(start, percent);
                }
            }
            Dictionary<Point, float> vlist, vlist2;
            vlist = FindResulte.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            vlist2 = vlist.Where(x => x.Value >= FoundRate).ToDictionary(x => x.Key, x => x.Value);
            FindResulte = vlist2;
            vlist=null; vlist2=null;
            //GC.Collect();
            return FindResulte.Count > 0;
        }
        public static Dictionary<uint, List<Point>> GetBitmapColorPointList(Bitmap obitmap, out Dictionary<uint, uint> ColorCount)
        {
            Bitmap bitmap = (Bitmap)obitmap.Clone();
            int parWidth = bitmap.Width;
            int parHeight = bitmap.Height;
            var parData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var byteArraryPar = new byte[parData.Stride * parData.Height];
            Marshal.Copy(parData.Scan0, byteArraryPar, 0, parData.Stride * parData.Height);
            var iMax = parData.Height;//行
            var jMax = parData.Width;//列
            int Depth = 4;//Format32bppArgb的颜色深度
            Dictionary<uint, List<Point>> dir = new Dictionary<uint, List<Point>>();
            ColorCount = new Dictionary<uint, uint>();
            for (int i = 0; i < iMax - 1; i++)
            {
                for (int j = 0; j < jMax - 1; j++)
                {
                    //大图x，y坐标处的颜色值
                    //int x = j, y = i;
                    int parIndex = i * parWidth * Depth + j * Depth;
                    uint c = 0;
                    byte a = byteArraryPar[parIndex + 3];
                    byte r = byteArraryPar[parIndex + 2];
                    byte g = byteArraryPar[parIndex + 1];
                    byte b = byteArraryPar[parIndex + 0];
                    c = (uint)((b << 24) | (g << 16) | (r << 8) | a);
                    if (dir.ContainsKey(c))
                    {
                        dir[c].Add(new Point(j, i));
                        ColorCount[c]++;
                    }
                    else
                    {
                        List<Point> list = new List<Point>
                        {
                            new Point(j, i)
                        };
                        dir.Add(c, list);
                        ColorCount.Add(c, 1);
                    }
                }
            }
            bitmap.UnlockBits(parData);
            bitmap = null;
            //GC.Collect();
            return dir;
        }

        public static Bitmap DrawRectangle(Bitmap d, List<Rectangle> recs, Color color = default, float penwidth = 1f)
        {
            if (color == default) color = Color.Red;
            Bitmap vb = (Bitmap)d.Clone();
            Graphics g = Graphics.FromImage(vb);
            Pen pen = new Pen(color, penwidth);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            foreach (var v in recs)
            {
                g.DrawRectangle(pen, v);
            }
            g.Dispose();
            return vb;
        }
        public static Bitmap DrawRectangle(Bitmap d, Rectangle rec, Color color = default)
        {
            if (color == default) color = Color.Red;
            Bitmap vb = (Bitmap)d.Clone();
            Pen pen = new Pen(color, 0.1f);
            Graphics g = Graphics.FromImage(vb);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawRectangle(pen, rec);
            g.Dispose();
            return vb;
        }
    }
}
