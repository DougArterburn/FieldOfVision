using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASignInSpace
{
    public class FieldOfVision
    {
        public enum CommonMapsEnum { Other, StarMap, Signs, TwoByTwoCellsAll, Header64, Footer64 };

        public enum FunctionEnum { HeaderFunction, FooterFunction };

        public CommonMapsEnum MapSelected
        {
            get;
            set;
        } = CommonMapsEnum.Header64;

        public FunctionEnum FunctionSelected
        {
            get;
            set;
        } = FunctionEnum.HeaderFunction;

        /*
            * The most common values for FinalImage dimensions are 8X8 or 256X256. 
            * For the starmap, 512X128 is sometimes interesting or 128X128 if you want to overlay pixels
            */
        public int FinalImageWidth
        {
            get;
            set;
        } = 8;

        public int FinalImageHeight
        {
            get;
            set;
        } = 8;

        public int Width
        {
            get;
            set;
        } = 8;

        public int Height
        {
            get;
            set;
        } = 8;

        /*
         * FocusWidth must be less than or equal to FieldOfVisionWidth
         * FocusWidth must be a factor of FieldOfVisionWidth
         */
        public int FocusWidth
        {
            get;
            set;
        } = 2;

        /*
        * FieldOfVisionWidth must be less than or equal to SegmentWidth
        * FieldOfVisionWidth must be a factor of SegmentWidth
        */
        public int FieldOfVisionWidth
        {
            get;
            set;
        } = 4;


        /*
         * SegmentWidth must be less than or equal to Width
         * SegmentWidth must be a factor of Width
         */
        public int SegmentWidth
        {
            get;
            set;
        } = 4;


        /*
         * SegmentHeight must be less than or equal to Height
         * SegmentHeight must be a factor of Height
         */
        public int SegmentHeight
        {
            get;
            set;
        } = 4;


        public void Run(BitArray MapIn = null)
        {
            int SegmentLength = SegmentWidth * SegmentHeight;

            BitArray map = null;

            switch (MapSelected)
            {
                case CommonMapsEnum.Other:
                    if (MapIn == null)
                    {
                        throw new Exception("Please pass the map");
                    }
                    map = (BitArray)MapIn.Clone();
                    break;
                case CommonMapsEnum.StarMap:
                    map = BitArrayHelper.GetStarmap();
                    break;
                case CommonMapsEnum.Header64:
                    map = BitArrayHelper.CreateBitArray(BitArrayHelper.Header64String);
                    break;
                case CommonMapsEnum.Footer64:
                    map = BitArrayHelper.CreateBitArray(BitArrayHelper.Footer64String);
                    break;
                case CommonMapsEnum.Signs:
                    map = BitArrayHelper.GetSigns();
                    break;
                case CommonMapsEnum.TwoByTwoCellsAll:
                    map = BitArrayHelper.TwoByTwoCellsAll();
                    break;
            }

            if (Width * Height != map.Length)
            {
                throw new Exception("Width * Height must equal map.length");
            }

            if (SegmentWidth > Width)
            {
                throw new Exception("SegmentWidth must be <= Width");
            }

            if (SegmentHeight > Height)
            {
                throw new Exception("SegmentHeight must be <= Height");
            }

            if (FieldOfVisionWidth >= SegmentWidth)
            {
                throw new Exception("FieldOfVisionWidth must be < SegmentWidth");
            }

            if (FocusWidth > FieldOfVisionWidth)
            {
                throw new Exception("FocusWidth must be <= FieldOfVisionWidth");
            }


            int x = 0, y = 0;

            bool[,] mapOutBool = new bool[Width, Height];
            bool[,] mapBool = BitArrayHelper.CreateBoolArray(map, Width, Height);

            for (y = 0; y < Height; y += SegmentHeight)
            {
                for (x = 0; x < Width; x += SegmentWidth)
                {
                    BitArray segment = new BitArray(SegmentLength);
                    int segIdx = 0;
                    for (int yy = 0; yy < SegmentHeight; yy++)
                    {
                        for (int xx = 0; xx < SegmentWidth; xx++)
                        {
                            segment[segIdx++] = mapBool[x + xx, y + yy];
                        }
                    }

                    bool[,] segmentBool = null;

                    switch (FunctionSelected)
                    {
                        case FunctionEnum.HeaderFunction:
                            segmentBool = BitArrayHelper.CreateBoolArray(HeaderFunction(segment, FocusWidth, FieldOfVisionWidth), SegmentWidth, SegmentHeight);
                            break;
                        case FunctionEnum.FooterFunction:
                            segmentBool = BitArrayHelper.CreateBoolArray(FooterFunction(segment, FocusWidth, FieldOfVisionWidth), SegmentWidth, SegmentHeight);
                            break;
                    }

                    for (int yy = 0; yy < SegmentHeight; yy++)
                    {
                        for (int xx = 0; xx < SegmentWidth; xx++)
                        {
                            mapOutBool[x + xx, y + yy] = segmentBool[xx, yy];
                        }
                    }

                }
            }

            Draw d = new Draw();
            d.FilenamePrefix = "FieldOfVision";
            d.WriteBinFile = false;
            d.Width = FinalImageWidth;
            d.Height = FinalImageHeight;
            var mapB = BitArrayHelper.CreateBitArray(mapOutBool, Width, Height);
            d.DrawBitArray(mapB);
        }

        BitArray HeaderFunction(BitArray mapIn, int focusWidth, int fieldOfVisionWidth)
        {
            int len = mapIn.Length;
            BitArray mapOut = new BitArray(len);
            int mapIdx = 0;
            for (int start = 0; start < fieldOfVisionWidth; start += focusWidth)
            {
                for (int i = start; i < len; i += fieldOfVisionWidth * 2)
                {
                    for (int j = 0; j < focusWidth; j++)  //this first gives shooter
                    {
                        mapOut[mapIdx++] = mapIn[i + j];
                    }
                    for (int j = i + fieldOfVisionWidth, c = 0; c < focusWidth; j++, c++)
                    {
                        mapOut[mapIdx++] = mapIn[j];
                    }

                }
            }
            return mapOut;
        }

        BitArray FooterFunction(BitArray mapIn, int focusWidth, int fieldOfVisionWidth)
        {
            int len = mapIn.Length;
            BitArray mapOut = new BitArray(len);
            int mapIdx = 0;
            int oncountIn = 0, oncountOut = 0;
            int[] indexList = new int[len];

            for (int i = 0; i < len; i++)
            {
                if (mapIn[i])
                {
                    oncountIn++;
                }
            }

            for (int start = 0; start < fieldOfVisionWidth; start += focusWidth)
            {
                for (int i = 0; i < len / 2; i += fieldOfVisionWidth)
                {
                    for (int j = 0; j < focusWidth; j++)
                    {
                        indexList[mapIdx] = i + j + start;
                        mapOut[mapIdx++] = mapIn[i + j + start];
                    }
                    for (int j = len - (i + fieldOfVisionWidth), c = 0; c < focusWidth; j++, c++)
                    {
                        indexList[mapIdx] = j + start;
                        mapOut[mapIdx++] = mapIn[j + start];
                    }

                }
            }

            for (int i = 0; i < len; i++)
            {
                if (mapOut[i])
                {
                    oncountOut++;
                }
            }

            if (oncountIn != oncountOut)
            {
                bool stop = true;
            }

            return mapOut;

        }


    }
}
