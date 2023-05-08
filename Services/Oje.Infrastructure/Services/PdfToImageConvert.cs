using Ghostscript.NET.Processor;
using System.Collections.Generic;

namespace Oje.Infrastructure.Services
{
    public static class PdfToImageConvert
    {
        public static void Convert(string inputPath, string outPutPath)
        {
            //Ghostscript.NET..GeneratePageThumb(inputPath, outPutPath, 1, 200, 350);

            string inputFile = inputPath;
            string outputFile = outPutPath;

            int pageFrom = 1;
            int pageTo = 1;

            using (GhostscriptProcessor ghostscript = new GhostscriptProcessor())
            {
                //ghostscript.Processing += new GhostscriptProcessorProcessingEventHandler(ghostscript_Processing);

                List<string> switches = new List<string>();
                switches.Add("-empty");
                switches.Add("-dSAFER");
                switches.Add("-dBATCH");
                switches.Add("-dNOPAUSE");
                switches.Add("-dNOPROMPT");
                switches.Add("-dFirstPage=" + pageFrom.ToString());
                switches.Add("-dLastPage=" + pageTo.ToString());
                switches.Add("-sDEVICE=png16m");
                switches.Add("-r96");
                switches.Add("-dTextAlphaBits=4");
                switches.Add("-dGraphicsAlphaBits=4");
                switches.Add(@"-sOutputFile=" + outputFile);
                switches.Add(@"-f");
                switches.Add(inputFile);

                ghostscript.Process(switches.ToArray());
            }

            ////Initialize HTML to PDF converter with Blink rendering engine 
            //HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);


            //BlinkConverterSettings settings = new BlinkConverterSettings();
            //if (cookies != null && cookies.Count > 0)
            //{
            //    foreach (var cookie in cookies)
            //    {
            //        settings.Cookies.Add(cookie.Key, cookie.Value);
            //    }
            //}

            //settings.Cookies.Add("ignoreCIP", "true".Encrypt2());

            ////Set the BlinkBinaries folder path 
            //settings.BlinkPath = Path.Combine(GlobalConfig.WebHostEnvironment.ContentRootPath, "BlinkBinariesWindows");

            ////Assign Blink settings to HTML converter
            //htmlConverter.ConverterSettings = settings;

            //var document = htmlConverter.ConvertToImage(Url);

            //File.WriteAllBytes(outpotFile, document.ImageData);
        }
    }
}
