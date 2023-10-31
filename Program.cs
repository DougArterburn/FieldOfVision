
using ASignInSpace;

FieldOfVision fov = new FieldOfVision();



/*
 * 
 * Random test to be sure that effects are just artifacts of my program
 */
fov.MapSelected = FieldOfVision.CommonMapsEnum.Other;
fov.FunctionSelected = FieldOfVision.FunctionEnum.HeaderFunction;
fov.FinalImageWidth = 8;
fov.FinalImageHeight = 8;
fov.Width = 8;
fov.Height = 8;
fov.SegmentWidth = 2;
fov.SegmentHeight = 2;
fov.FocusWidth = 1;
fov.FieldOfVisionWidth = 2;

fov.Run(BitArrayHelper.CreateBitArray("0000100010000101001010000101110000010100100000010000100000000010"));

/*
 * These parameters produce the "space invaders" image from the 64 bit header
 * 
 */
//fov.MapSelected = FieldOfVision.CommonMapsEnum.Header64;
//fov.FunctionSelected = FieldOfVision.FunctionEnum.HeaderFunction;
//fov.FinalImageWidth = 8;
//fov.FinalImageHeight = 8;
//fov.Width = 8;
//fov.Height = 8;
//fov.SegmentWidth = 8;
//fov.SegmentHeight = 8;
//fov.FocusWidth = 2;
//fov.FieldOfVisionWidth = 4;
//fov.Run();

/*
 * These parameters produce the only other interesting image I have found from the 64 bit header
 * 
 */
//fov.MapSelected = FieldOfVision.CommonMapsEnum.Header64;
//fov.FunctionSelected = FieldOfVision.FunctionEnum.HeaderFunction;
//fov.FinalImageWidth = 8;
//fov.FinalImageHeight = 8;
//fov.Width = 8;
//fov.Height = 8;
//fov.SegmentWidth = 4;
//fov.SegmentHeight = 4;
//fov.FocusWidth = 2;
//fov.FieldOfVisionWidth = 4;
//fov.Run();


/*
 * These parameters produce the 1+2+3+4=10 image from the 64 bit footer
 * 
 */
//fov.MapSelected = FieldOfVision.CommonMapsEnum.Footer64;
//fov.FunctionSelected = FieldOfVision.FunctionEnum.FooterFunction;
//fov.FinalImageWidth = 8;
//fov.FinalImageHeight = 8;
//fov.Width = 8;
//fov.Height = 8;
//fov.SegmentWidth = 4;
//fov.SegmentHeight = 4;
//fov.FocusWidth = 2;
//fov.FieldOfVisionWidth = 4;
//fov.Run();