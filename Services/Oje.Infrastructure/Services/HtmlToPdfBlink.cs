using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Oje.Infrastructure.Services
{
    public static class HtmlToPdfBlink
    {
        public static byte[] Convert(string Url, IRequestCookieCollection cookies)
        {
            //Initialize HTML to PDF converter with Blink rendering engine 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);


            BlinkConverterSettings settings = new BlinkConverterSettings();
            if (cookies != null && cookies.Count > 0)
            {
                foreach(var cookie in cookies)
                {
                    settings.Cookies.Add(cookie.Key, cookie.Value);
                }
            }

            //Set the BlinkBinaries folder path 
            settings.BlinkPath = Path.Combine(GlobalConfig.WebHostEnvironment.ContentRootPath, "BlinkBinariesWindows");

            //Assign Blink settings to HTML converter
            htmlConverter.ConverterSettings = settings;
           

            //Convert URL to PDF
            using (PdfDocument document = htmlConverter.Convert(Url))
            {
                //Saving the PDF to the MemoryStream
                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
