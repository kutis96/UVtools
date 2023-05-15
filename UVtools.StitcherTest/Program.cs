using System;
using UVtools.Core.FileFormats;
using UVtools.Core;
using Mono.Options;

namespace UVtools.StitcherTest
{
    public class Program
    {

        // A bit fugly, but...
        static float bottomLayerExposureSeconds = 20;
        static float layerExposureSeconds = 3;
        static ushort bottomLayerCount = 6;
        static float layerHeight = 0.05F;
        static float liftHeight = 5F;
        static float bottomLiftHeight = 5F;
        static float bottomLiftHeight2 = 5F;
        static float liftSpeed = 1;
        static float liftSpeed2 = 3;
        static float bottomLiftSpeed = 1;
        static float bottomLiftSpeed2 = 3;
        static float bottomLightOffDelay = 1F;
        static float lightOffDelay = 1F;

        static String sourcePath = null;
        static String targetPath = null;


        public static void Main(string[] args)
        {

            bool showHelp = false;

            var options = new OptionSet()
            {
                {"h|?|help|Help", "Shows something almost, but not quite, helpful.", v => showHelp = v != null },
                {"blc|BottomLayerCount=", "Number of bottom layers. Integer.", (ushort v) => bottomLayerCount = v },
                {"blet|BottomLayerExposureTime=", "Exposure time of the bottom layers. In seconds, accepts decimals.", (float v) => bottomLayerExposureSeconds = v },
                {"let|LayerExposureTime=", "Exposure time of subsequent layers. In seconds, accepts decimals.", (float v) => layerExposureSeconds = v },
                {"lah|LayerHeight=", "Layer height. In millimeters, accepts decimals.", (float v) => layerHeight = v},
                {"blih|BottomLiftHeight=", "Bottom layer lift height. In millimeters, accepts decimals.", (float v) => liftHeight = v },
                {"blih2|BottomLiftHeight2=", "Bottom layer lift height 2. Got no clue either. In millimeters, accepts decimals.", (float v) => bottomLiftHeight = v },
                {"lih|LiftHeight=", "Lift height of subsequent layers. In millimeters, accepts decimals.", (float v) => bottomLiftHeight2 = v },
                {"blis|BottomLiftSpeed=", "Lift speed of bottom layers. In mm/s. Accepts decimals.", (float v) => bottomLiftSpeed = v },
                {"blis2|BottomLiftSpeed2=", "Lift speed 2 of bottom layers. In mm/s. Accepts decimals.", (float v) => bottomLiftSpeed2 = v },
                {"lis|LiftSpeed=", "Lift speed of subsequent layers. In mm/s. Accepts decimals.", (float v) => liftSpeed = v },
                {"lis2|LiftSpeed2=", "Lift speed 2 of subsequent layers. In mm/s. Accepts decimals.", (float v) => liftSpeed2 = v },
                {"blod|BottomLightOffDelay=", "Bottom layer light off delay.  In seconds, accepts decimals.", (float v) => bottomLightOffDelay = v},
                {"lod|LightOffDelay=", "Bottom layer light off delay.  In seconds, accepts decimals.", (float v) => lightOffDelay = v},
                {"src|SourcePath=", "Source file path.", v => sourcePath = v},
                {"tgt|TargetPath=", "Target file path.", v => targetPath = v}
            };

            options.Parse(args);

            if(sourcePath == null || !(System.IO.Directory.Exists(sourcePath) || System.IO.File.Exists(sourcePath)) )
            {
                Console.Error.WriteLine("Source path must be specified and pointing to a directory containing PNG source files or a single PNG file to process.");
            }

            if(targetPath == null)
            {
                Console.Error.WriteLine("Target path must be specified and containing a file extension specifying the printer file format.");
            }

            if (showHelp || sourcePath == null || targetPath == null) {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }


        FromLayers(sourcePath, targetPath);

            /*
            foreach (string dir in Directory.EnumerateDirectories("H:\\heightmaps\\sliced\\composite"))
            {
                Console.WriteLine("Processing " + dir); 
                MakeMap(dir, "H:\\heightmaps\\sliced\\export");
            }
            */
        }

        public static void FromLayers(string sourcePath, string targetFile)
        {

            Console.WriteLine(targetFile);

            string[] pngFiles;

            if (System.IO.Directory.Exists(sourcePath))
            {
                pngFiles = System.IO.Directory.GetFiles(sourcePath, "*.png");
            }
            else
            {
                pngFiles = new string[]{sourcePath};
            }

            Console.WriteLine("Found " + pngFiles.Length + " PNG images representing layer images in " + sourcePath);

            if (pngFiles.Length == 0)
                return;

            Array.Sort(pngFiles);

            FileFormat outputFileFormat = FileFormat.FindByExtensionOrFilePath(targetFile, true);
            outputFileFormat.FileFullPath = targetFile; //ffs

            outputFileFormat.ExposureTime = layerExposureSeconds;
            outputFileFormat.BottomLayerCount = bottomLayerCount;
            outputFileFormat.BottomExposureTime = bottomLayerExposureSeconds;
            outputFileFormat.LiftHeight = liftHeight;
            outputFileFormat.BottomLiftHeight = bottomLiftHeight;
            outputFileFormat.BottomLiftHeight2 = bottomLiftHeight2;
            outputFileFormat.LiftSpeed = liftSpeed;
            outputFileFormat.LiftSpeed2 = liftSpeed2;
            outputFileFormat.BottomLiftSpeed = bottomLiftSpeed;
            outputFileFormat.BottomLiftSpeed2 = bottomLiftSpeed2;
            outputFileFormat.BottomLightOffDelay = bottomLightOffDelay;
            outputFileFormat.LightOffDelay = lightOffDelay;
            outputFileFormat.LayerHeight = layerHeight;

            outputFileFormat.LayerManager.Init((uint)0);
            foreach (string pngFile in pngFiles)
            {
                Console.WriteLine("Processing " + pngFile);
                FileFormat pngFileFormat = FileFormat.FindByExtensionOrFilePath(pngFile, true);

                pngFileFormat.Decode(pngFile);

                Layer layer = pngFileFormat.FirstLayer;

                layer.LayerMat = layer.LayerMat.T();

                layer.ExposureTime = outputFileFormat.ExposureTime;
                layer.LiftSpeed = outputFileFormat.LiftSpeed;
                layer.LiftSpeed2 = outputFileFormat.LiftSpeed2;
                layer.LiftHeight = outputFileFormat.LiftHeight;
                layer.LiftHeight2 = outputFileFormat.LiftHeight2;

                outputFileFormat.LayerManager
                    .Add(layer);
            }

            outputFileFormat.Encode(targetFile);
        }
    }
}
