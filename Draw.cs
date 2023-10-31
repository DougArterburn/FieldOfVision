using System.Drawing;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASignInSpace
{
    public class Draw
    {

        public struct CircleDef
        {
            public int Radius
            {
                get;
                set;
            }

            public Point Center
            {
                get;
                set;
            }
        }
        public int Width
        {
            get;
            set;
        } = 256;

        public int Height
        {
            get;
            set;
        } = 256;

        public bool WriteBinFile
        {
            get;
            set;
        } = false;

        public bool WriteCoordinatesFile
        {
            get;
            set;
        } = false;

        public int FileNumber
        {
            get;
            set;
        } = 0;

        public string FilenamePrefix
        {
            get;
            set;
        } = @$"Default";

        public Point[] PlotAddressList
        {
            get;
            set;
        } = Array.Empty<Point>();


        public Point[] DrawLinesPoints
        {
            get;
            set;
        } = Array.Empty<Point>();

        public Brush[] ColorMap
        {
            get;
            set;
        } = Array.Empty<Brush>();

        public Color BackgroundColor
        {
            get;
            set;
        } = Color.Black;

        public CircleDef[] Circles
        {
            get;
            set;
        } = Array.Empty<CircleDef>();

        public Rectangle[] Rectangles
        {
            get;
            set;
        } = Array.Empty<Rectangle>();

        public void DrawStarMap()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.data17square.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            DrawBitArray(dataBitsIn);
        }

        public void DrawFile(string filename)
        {
            var dataBytesIn = File.ReadAllBytes(filename);
            var dataBitsIn = BitArrayHelper.CreateBitArray(dataBytesIn);
            DrawBitArray(dataBitsIn);
        }

        public void DrawBitArray(BitArray data)
        {
            Bitmap dialImage = new Bitmap(Width, Height);
            int x = 0, y = 0;
            Graphics g = Graphics.FromImage(dialImage);
            g.Clear(BackgroundColor);


            for (int i = 0; i < DrawLinesPoints.Length; i += 2)
            {
                if (i + 1 < DrawLinesPoints.Length)
                {
                    g.DrawLine(Pens.Blue, DrawLinesPoints[i], DrawLinesPoints[i + 1]);
                }
            }

            for (int i = 0; i < Circles.Length; i++)
            {
                Point location = new Point(Circles[i].Center.X - Circles[i].Radius, Circles[i].Center.Y - Circles[i].Radius);
                Size size = new Size(Circles[i].Radius, Circles[i].Radius);
                Rectangle rect = new Rectangle(location, size * 2);
                g.DrawEllipse(Pens.Blue, rect);
            }

            for (int i = 0; i < Rectangles.Length; i++)
            {
                g.DrawRectangle(Pens.Blue, Rectangles[i]);
            }

            for (int i = 0; i < data.Length; i++)
            {

                if (data[i])
                {
                    if (ColorMap.Length == data.Length)
                    {
                        g.FillRectangle(ColorMap[i], x, y, 1, 1);
                    }
                    else if (dialImage.GetPixel(x, y).Name == BackgroundColor.ToArgb().ToString("x"))
                    {
                        g.FillRectangle(Brushes.White, x, y, 1, 1);
                    }
                }

                x++;
                if (x == dialImage.Width)
                {
                    x = 0;
                    y++;
                    if (y == Height)
                    {
                        y = 0;
                    }
                }

            }


            foreach (var p in PlotAddressList)
            {
                g.FillRectangle(Brushes.Blue, p.X, p.Y, 1, 1);
            }

            if (WriteBinFile)
            {
                File.WriteAllBytes($"{FilenamePrefix}-{FileNumber}.bin", BitArrayHelper.CreateByteArray(data));
            }

            dialImage.Save($"{FilenamePrefix}-{FileNumber}.png", System.Drawing.Imaging.ImageFormat.Png);

            StringBuilder coordinatesCSV = new StringBuilder("Index,x,y\r\n");

            FileNumber++;
            int onCount = 0;
            for (int yy = 0; yy < Height; yy++)
            {
                for (int xx = 0; xx < Width; xx++)
                {
                    if (dialImage.GetPixel(xx, yy).Name != BackgroundColor.ToArgb().ToString("x"))
                    {
                        onCount++;
                        if (WriteCoordinatesFile)
                        {
                            coordinatesCSV.Append($"{yy*Height+xx},{xx},{yy}\r\n");
                        }
                    }
                }
            }
            Console.WriteLine($"On count = {onCount}");

            if (WriteCoordinatesFile)
            {
                File.WriteAllText($"{FilenamePrefix}-{FileNumber}.csv", coordinatesCSV.ToString());
            }
        }

        public void CreateRandomMap()
        {
            Width = 256;
            Height = 256;
            BitArray randomMap = new BitArray(Width * Height);
            Random r = new Random();
            for (int i = 0; i < 625; i++)
            {
                int idx;

            tryAgain:

                idx = (int)(r.NextDouble() * randomMap.Length);

                if (i % 5 == 0)
                {
                    if (randomMap[idx]) goto tryAgain;
                    randomMap[idx] = true;
                    continue;
                }

                bool hit = false;
                while (!hit)
                {
                    idx = (int)(r.NextDouble() * randomMap.Length);
                    Point p = BitArrayHelper.convertToCoordiate(idx, Width, Height);
                    if (p.X >= 42 && p.X <= 42 + 41 && p.Y >= 34 && p.Y <= 34 + 41)
                    {
                        hit = true;
                    }
                    if (p.X >= 22 && p.X <= 22 + 41 && p.Y >= 125 && p.Y <= 125 + 41)
                    {
                        hit = true;
                    }
                    if (p.X >= 135 && p.X <= 135 + 41 && p.Y >= 60 && p.Y <= 60 + 41)
                    {
                        hit = true;
                    }
                    if (p.X >= 176 && p.X <= 176 + 41 && p.Y >= 138 && p.Y <= 138 + 41)
                    {
                        hit = true;
                    }
                    if (p.X >= 78 && p.X <= 78 + 41 && p.Y >= 167 && p.Y <= 167 + 41)
                    {
                        hit = true;
                    }
                    if (randomMap[idx])
                    {
                        hit = false;
                    }
                }
                randomMap[idx] = true;
                if (idx == 0x2841)
                {
                    bool yahoo = true;
                }
            }

            FilenamePrefix = "Random";
            WriteBinFile = true;
            DrawBitArray(randomMap);

        }

    }
}
