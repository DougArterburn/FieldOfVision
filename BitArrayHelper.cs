using System.Collections;
using System.Drawing;
using System.Text;
using System.Xml.Schema;

namespace ASignInSpace
{
    public static class BitArrayHelper
    {


        public const string Header64String = "0000011010010000001010000100000101000100100010000100010010001000";
        public const string Footer64String = "1111100000111110000000000000000000000000000000000000000000000000";
        public const string Header68String = "11110000011010010000001010000100000101000100100010000100010010001000";
        public const string Footer68String = "11111000001111100000000000000000000000000000000000000000000000000000";
        public const string Header72String = "111111110000011010010000001010000100000101000100100010000100010010001000";
        public const string Footer72String = "111110000011111000000000000000000000000000000000000000000000000000001111";
        public const string Header80String = "11111111111111110000011010010000001010000100000101000100100010000100010010001000";
        public const string Footer80String = "11111000001111100000000000000000000000000000000000000000000000000000111111111111";
        public const string HeaderFooter144String = $"11111111{Header64String}{Footer64String}00001111";

        public const char One = '1';
        public const char Zero = '0';

        static public BitArray GetStarmap()
        {
            var starmap = GetStarmapBytes();
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            return dataBitsIn;
        }

        static public BitArray GetSigns()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.Signs.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            return dataBitsIn;
        }

        static public BitArray TwoByTwoCellsAll()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.TwoByTwoAll.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            return dataBitsIn;
        }

        static public byte[] GetStarmapBytes()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.data17square.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            return starmap;
        }

        static public BitArray GetStarmapData17()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.data17.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            return dataBitsIn;
        }

        static public BitArray CreateBitArray(string s, bool reverse = false)
        {
            BitArray ba = new BitArray(s.Length);
            ba.SetAll(false);

            if (reverse)
            {
                for (int i = s.Length - 1, j = 0; i >= 0; i--, j++)
                {
                    if (s[j] == '0') ba[i] = false;
                    else ba[i] = true;
                }
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '0') ba[i] = false;
                    else ba[i] = true;
                }
            }
            return ba;
        }

        static public int D(Point p1, Point p2)
        {
            int xDistance = Math.Abs(p1.X - p2.X);
            int yDistance = Math.Abs(p1.Y - p2.Y);
            int d = (xDistance * xDistance) + (yDistance * yDistance);
            return d;
        }
        static public bool Compare(BitArray a1, BitArray a2)
        {
            if (a2 == null) return false;

            string s1 = ConvertToString(a1);
            string s2 = ConvertToString(a2);

            if (a1.Length < a2.Length)
            {
                return s2.Contains(s1);
            }
            else if (a1.Length > a2.Length)
            {
                return s1.Contains(s2);
            }

            else
            {
                return s1 == s2;
            }
        }

        static public string ConvertToString(BitArray a1)
        {
            StringBuilder s = new StringBuilder(a1.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i]) s.Append('1');
                else s.Append("0");
            }
            return s.ToString();
        }

        static public string ConvertToString(int v)
        {
            StringBuilder s = new StringBuilder();
            while (v != 0)
            {
                if (v % 2 == 1)
                {
                    s.Insert(0, '1');
                }
                else
                {
                    s.Insert(0, '0');
                }
                v /= 2;
            }

            return s.ToString().TrimStart('0');
        }

        static public BitArray CreateBitArray(byte[] data)
        {
            BitArray ba = new BitArray(data.Length * 8);    // definitly MSB-first

            for (int iByte = 0; iByte < data.Length; iByte++)
            {
                byte aByte = data[iByte];
                for (int iBit = 0; iBit < 8; iBit++)
                {
                    bool aBit = (((aByte << iBit) & 0x80) == 0x80);   // MSB-first
                    ba[iByte * 8 + iBit] = aBit;
                }
            }
            return ba;
        }

        static public BitArray CreateBitArray(bool[,] arrayIn, int width, int height)
        {
            var output = new BitArray(width * height);
            int outputIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    output[outputIndex++] = arrayIn[x, y];
                }
            }
            return output;
        }

        static public bool[,] CreateBoolArray(BitArray gridIn, int width, int height)
        {
            var Grid = new bool[width, height];
            int x = 0, y = 0;
            for (int i = 0; i < gridIn.Length; i++)
            {
                Grid[x, y] = gridIn[i];
                x++;
                if (x == width)
                {
                    x = 0;
                    y++;
                    if (y == height)
                    {
                        y = 0;
                    }
                }
            }
            return Grid;
        }

        static public byte[] CreateByteArray(BitArray ba)
        {
            byte[] data = new byte[ba.Length / 8];
            int iByte = 0, iBit = 0;
            while (iByte < data.Length)
            {
                byte aByte = 0;
                for (int j = 0; j < 8; j++)
                {
                    aByte <<= 1;
                    if (ba[iBit])
                    {
                        aByte += 1;
                    }
                    iBit++;
                }
                data[iByte] = aByte;
                iByte++;
            }
            return data;
        }

        static public byte[] CreateByteArray(string s)
        {
            byte[] data = new byte[s.Length / 8];
            int iByte = 0, iBit = 0;
            while (iByte < data.Length)
            {
                byte aByte = 0;
                for (int j = 0; j < 8; j++)
                {
                    aByte <<= 1;
                    if (s[iBit] == One)
                    {
                        aByte += 1;
                    }
                    iBit++;
                }
                data[iByte] = aByte;
                iByte++;
            }
            return data;
        }

        static public BitArray ShiftLeft(BitArray ba, int count, bool fill = false)
        {
            BitArray newArray = new BitArray(ba);
            int len = ba.Length;
            for (int i = 0; i < len - count; i++)
            {
                newArray[i] = ba[i + count];
            }

            for (int i = 0, j = len - count; i < count; i++, j++)
            {
                newArray[j] = fill;
            }

            return newArray;
        }
        static public BitArray RotateLeft(BitArray ba, int count, bool fill = false)
        {
            BitArray holdBits = new BitArray(count);
            BitArray newArray = new BitArray(ba);
            int len = ba.Length;
            for (int i = 0; i < count; i++)
            {
                holdBits[i] = ba[i];
            }

            newArray = ShiftLeft(newArray, count, fill);

            for (int i = 0, j = len - count; i < holdBits.Length; i++, j++)
            {
                newArray[j] = holdBits[i];
            }

            return newArray;
        }

        static public BitArray ShiftRight(BitArray ba, int count, bool fill = false)
        {
            BitArray newArray = new BitArray(ba);
            int len = ba.Length;
            for (int i = len - 1; i > count - 1; i--)
            {
                newArray[i] = ba[i - count];
            }

            for (int i = 0; i < count; i++)
            {
                newArray[i] = fill;
            }

            return newArray;
        }
        static public BitArray RotateRight(BitArray ba, int count, bool fill = false)
        {
            BitArray holdBits = new BitArray(count);
            BitArray newArray = new BitArray(ba);
            int len = ba.Length;
            for (int i = 0, j = len - count; i < count; i++, j++)
            {
                holdBits[i] = ba[j];
            }

            newArray = ShiftRight(newArray, count, fill);

            for (int i = 0; i < holdBits.Length; i++)
            {
                newArray[i] = holdBits[i];
            }

            return newArray;
        }

        static public bool[,] GetBlock(bool[,] gridIn, Point startPoint, int blockWidth, int blockHeight)
        {

            var output = new bool[blockWidth, blockHeight];
            for (int y = startPoint.Y, yy = 0; yy < blockHeight; y++, yy++)
            {
                for (int x = startPoint.X, xx = 0; xx < blockWidth; x++, xx++)
                {
                    output[xx, yy] = gridIn[x, y];
                }
            }
            return output;
        }

        static public void PutBlock(bool[,] targetGrid, bool[,] sourceGrid, Point startPoint)
        {
            int blockWidth = sourceGrid.GetUpperBound(0) + 1;
            int blockHeight = sourceGrid.GetUpperBound(1) + 1;
            for (int y = startPoint.Y, yy = 0; yy < blockHeight; y++, yy++)
            {
                for (int x = startPoint.X, xx = 0; xx < blockWidth; x++, xx++)
                {
                    targetGrid[x, y] = sourceGrid[xx, yy];
                }
            }
        }

        static public Point convertToCoordiate(int idx, int width, int height)
        {
            var p = new Point(idx % width, idx / height);
            return p;
        }

        static public int convertToIndex(Point p, int width, int height)
        {
            int idx = (p.X % width) + (p.Y * height);
            return idx;
        }

        static public bool IsPrime(int n)
        {
            if (n == 2 || n == 3)
                return true;

            if (n <= 1 || n % 2 == 0 || n % 3 == 0)
                return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        static public bool IsSemiPrime(int num)
        {
            int cnt = 0;

            for (int i = 2; cnt < 2 && i * i <= num; ++i)
            {
                while (num % i == 0)
                {
                    num /= i; ++cnt; // Increment count
                                     // of prime numbers
                }
            }

            // If number is greater than 1, add it to
            // the count variable as it indicates the
            // number remain is prime number
            if (num > 1)
            {
                ++cnt;
            }
            

            // Return '1' if count is equal to '2' else
            // return '0'
            return cnt == 2;
        }


        static public string ReverseString(string s)
        {
            char[] a = new char[s.Length];
            s.CopyTo(0, a, 0, a.Length);
            Array.Reverse<char>(a);
            return new string(a);
        }

    }



}
