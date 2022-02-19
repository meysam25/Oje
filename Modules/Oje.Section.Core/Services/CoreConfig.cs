using Microsoft.AspNetCore.Builder;
using System;

namespace Oje.Section.Core.Services
{
    public static class CoreConfig
    {
        public static StaticFileOptions GetStaticFileOptions()
        {
            return new StaticFileOptions
            {
                OnPrepareResponse = content =>
                {
                    bool addExteras = false;
                    TimeSpan maxAge = new TimeSpan(365, 0, 0, 0);
                    if (content.File.Name.EndsWith(".js.gz"))
                    {
                        content.Context.Response.Headers["Content-Type"] = "text/javascript";
                        addExteras = true;
                    }
                    else if (content.File.Name.EndsWith(".css.gz"))
                    {
                        addExteras = true;
                        content.Context.Response.Headers["Content-Type"] = "text/css";
                    }
                    else if (content.File.Name.EndsWith(".svg.gz"))
                    {
                        addExteras = true;
                        content.Context.Response.Headers["Content-Type"] = "image/svg+xml";
                    }
                    //else if (content.File.Name.EndsWith(".ico.gz"))
                    //{
                    //    addExteras = true;
                    //    content.Context.Response.Headers["Content-Type"] = "image/x-icon";
                    //}
                    else if (content.File.Name.EndsWith(".png"))
                    {
                        content.Context.Response.Headers["Content-Type"] = "image/png";
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                    }
                    else if (content.File.Name.EndsWith(".svg"))
                    {
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                        content.Context.Response.Headers["Content-Type"] = "image/svg+xml";
                    }
                    else if (content.File.Name.EndsWith(".woff2"))
                    {
                        content.Context.Response.Headers["Content-Type"] = "font/woff2";
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                    }
                    else if (content.File.Name.EndsWith(".jpg"))
                    {
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                        content.Context.Response.Headers["Content-Type"] = "image/jpg";
                    }
                    else if (content.File.Name.EndsWith(".webp"))
                    {
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                        content.Context.Response.Headers["Content-Type"] = "image/webp";
                    }

                    if (addExteras == true)
                    {
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                        content.Context.Response.Headers["Content-Encoding"] = "gzip";
                    }
                }
            };
        }
    }
}
