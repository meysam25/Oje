﻿using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class Captcha
    {
        public static List<CaptchaResult> CaptchaResults { get; set; } = new List<CaptchaResult>();
        const string Letters = "012346789ABCDEFGHJKLMNPRTUVWXYZ";

        public static CaptchaResult Generate()
        {
            CaptchaResult result = GenerateCaptchaImage(278, 40, GenerateCaptchaCode());

            CaptchaResults.Add(result);

            RemoveExpiredCode();

            return result;
        }

        public static bool ValidateCaptchaCode(string userInputCaptcha, Guid Uid)
        {
            bool result = false;
            lock (CaptchaResults)
            {
                var foundItem = CaptchaResults.Where(t => t.CaptchaCode.ToLower() == (userInputCaptcha + "").ToLower() && t.Uid == Uid).FirstOrDefault();
                if (foundItem != null && (DateTime.Now - foundItem.Timestamp).TotalMinutes < 5)
                    result = true;
                if (foundItem != null)
                    CaptchaResults.Remove(foundItem);
            }
            return result;
        }

        public static byte[] GetImageCaptch(Guid guid)
        {
            lock (CaptchaResults)
            {
                var foundItem = CaptchaResults.Where(t => t.Uid == guid).FirstOrDefault();
                if (foundItem != null && (DateTime.Now - foundItem.Timestamp).TotalMinutes < 5)
                    return foundItem.CaptchaByteData;
            }

            return null;
        }

        public static void RemoveExpiredCode()
        {
            lock (CaptchaResults)
            {
                DateTime dtNow = DateTime.Now;
                CaptchaResults = CaptchaResults.Where(t => (dtNow - t.Timestamp).TotalMinutes < 5).ToList();
            }
        }

        public static string GenerateCaptchaCode()
        {
            Random rand = new();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            if (OperatingSystem.IsWindows())
            {
                using (Bitmap baseMap = new(width, height))
                using (Graphics graph = Graphics.FromImage(baseMap))
                {
                    Random rand = new();

                    graph.Clear(GetRandomLightColor());

                    DrawCaptchaCode();
                    DrawDisorderLine();
                    AdjustRippleEffect();

                    using (MemoryStream ms = new())
                    {
                        baseMap.Save(ms, ImageFormat.Jpeg);

                        return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };
                    }



                    static int GetFontSize(int imageWidth, int captchCodeCount)
                    {
                        var averageSize = imageWidth / captchCodeCount;

                        averageSize -= 30;

                        return Convert.ToInt32(averageSize);
                    }

                    Color GetRandomDeepColor()
                    {
                        int redlow = 160, greenLow = 100, blueLow = 160;
                        return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                    }

                    Color GetRandomLightColor()
                    {
                        int low = 180, high = 255;

                        int nRend = rand.Next(high) % (high - low) + low;
                        int nGreen = rand.Next(high) % (high - low) + low;
                        int nBlue = rand.Next(high) % (high - low) + low;

                        return Color.FromArgb(nRend, nGreen, nBlue);
                    }

                    void DrawCaptchaCode()
                    {
                        SolidBrush fontBrush = new(Color.Black);
                        int fontSize = GetFontSize(width, captchaCode.Length);

                        Font font = new(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                        for (int i = 0; i < captchaCode.Length; i++)
                        {
                            fontBrush.Color = GetRandomDeepColor();

                            int shiftPx = fontSize / 6;

                            float x = (i + 2) * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                            int maxY = height - fontSize;
                            if (maxY < 0) maxY = 0;
                            float y = rand.Next(0, maxY);

                            graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                        }
                    }

                    void DrawDisorderLine()
                    {
                        Pen linePen = new(new SolidBrush(Color.Black), 3);
                        for (int i = 0; i < rand.Next(3, 5); i++)
                        {
                            linePen.Color = GetRandomDeepColor();

                            Point startPoint = new(rand.Next(0, width), rand.Next(0, height));
                            Point endPoint = new(rand.Next(0, width), rand.Next(0, height));
                            graph.DrawLine(linePen, startPoint, endPoint);
                        }
                    }

                    void AdjustRippleEffect()
                    {
                        short nWave = 6;
                        int nWidth = baseMap.Width;
                        int nHeight = baseMap.Height;

                        Point[,] pt = new Point[nWidth, nHeight];

                        for (int x = 0; x < nWidth; ++x)
                        {
                            for (int y = 0; y < nHeight; ++y)
                            {
                                var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                                var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                                var newX = x + xo;
                                var newY = y + yo;

                                if (newX > 0 && newX < nWidth)
                                {
                                    pt[x, y].X = (int)newX;
                                }
                                else
                                {
                                    pt[x, y].X = 0;
                                }


                                if (newY > 0 && newY < nHeight)
                                {
                                    pt[x, y].Y = (int)newY;
                                }
                                else
                                {
                                    pt[x, y].Y = 0;
                                }
                            }
                        }

                        Bitmap bSrc = (Bitmap)baseMap.Clone();

                        BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        int scanline = bitmapData.Stride;

                        IntPtr scan0 = bitmapData.Scan0;
                        IntPtr srcScan0 = bmSrc.Scan0;

                        unsafe
                        {
                            byte* p = (byte*)(void*)scan0;
                            byte* pSrc = (byte*)(void*)srcScan0;

                            int nOffset = bitmapData.Stride - baseMap.Width * 3;

                            for (int y = 0; y < nHeight; ++y)
                            {
                                for (int x = 0; x < nWidth; ++x)
                                {
                                    var xOffset = pt[x, y].X;
                                    var yOffset = pt[x, y].Y;

                                    if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                                    {
                                        if (pSrc != null)
                                        {
                                            p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                            p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                            p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                        }
                                    }

                                    p += 3;
                                }
                                p += nOffset;
                            }
                        }

                        baseMap.UnlockBits(bitmapData);
                        bSrc.UnlockBits(bmSrc);
                        bSrc.Dispose();
                    }
                }
            }
            else
                throw new NotImplementedException("GenerateCaptchaImage");

        }
    }
}
