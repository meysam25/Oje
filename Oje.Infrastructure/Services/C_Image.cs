using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class C_Image
    {
        public static Bitmap ResizeBitmap(Bitmap input, Size BitmapSize)
        {
            int OrginalWidth = input.Width;
            int OrginalHeight = input.Height;

            int NewWidth = BitmapSize.Width;
            int NewHeight = BitmapSize.Height;

            int ResultWidth = 0;
            int ResultHeight = 0;

            ResultWidth = NewWidth;
            ResultHeight = Convert.ToInt32(OrginalHeight * (Convert.ToDouble(NewWidth)) / (Convert.ToDouble(OrginalWidth)));

            if (ResultHeight > BitmapSize.Height)
            {
                ResultWidth = Convert.ToInt32(OrginalWidth * (Convert.ToDouble(NewHeight)) / (Convert.ToDouble(OrginalHeight)));
                ResultHeight = NewHeight;
            }

            Bitmap ResultBitmap = new Bitmap(ResultWidth, ResultHeight);

            using (Graphics g = Graphics.FromImage(ResultBitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(input, 0, 0, ResultWidth, ResultHeight);
            }

            return ResultBitmap;
        }

        public static Bitmap ResizeBitmap2(Bitmap input, Size BitmapSize)
        {
            int OrginalWidth = input.Width;
            int OrginalHeight = input.Height;

            int NewWidth = BitmapSize.Width;
            int NewHeight = BitmapSize.Height;

            int ResultWidth = 0;
            int ResultHeight = 0;

            ResultWidth = NewWidth;
            ResultHeight = Convert.ToInt32(OrginalHeight * (Convert.ToDouble(NewWidth)) / (Convert.ToDouble(OrginalWidth)));

            if (ResultHeight < BitmapSize.Height)
            {
                ResultWidth = Convert.ToInt32(OrginalWidth * (Convert.ToDouble(NewHeight)) / (Convert.ToDouble(OrginalHeight)));
                ResultHeight = NewHeight;
            }

            Bitmap ResultBitmap = new Bitmap(ResultWidth, ResultHeight);

            using (Graphics g = Graphics.FromImage(ResultBitmap))
            {
                g.DrawImage(input, 0, 0, ResultWidth, ResultHeight);
            }

            return ResultBitmap;
        }

        public static Bitmap ResizeBitmapJustWith(Bitmap input, Size BitmapSize)
        {
            int OrginalWidth = input.Width;
            int OrginalHeight = input.Height;

            int NewWidth = BitmapSize.Width;
            int NewHeight = BitmapSize.Height;

            int ResultWidth = 0;
            int ResultHeight = 0;

            ResultWidth = NewWidth;
            ResultHeight = Convert.ToInt32(OrginalHeight * (Convert.ToDouble(NewWidth)) / (Convert.ToDouble(OrginalWidth)));

            Bitmap ResultBitmap = new Bitmap(ResultWidth, ResultHeight);

            using (Graphics g = Graphics.FromImage(ResultBitmap))
            {
                g.DrawImage(input, 0, 0, ResultWidth, ResultHeight);
            }

            input.Dispose();
            input = null;

            return ResultBitmap;
        }

        public static Bitmap ResizeJustHeight(Bitmap input, Size BitmapSize)
        {
            int OrginalWidth = input.Width;
            int OrginalHeight = input.Height;

            int NewWidth = BitmapSize.Width;
            int NewHeight = BitmapSize.Height;

            int ResultWidth = 0;
            int ResultHeight = 0;

            ResultWidth = Convert.ToInt32(OrginalWidth * (Convert.ToDouble(NewHeight)) / (Convert.ToDouble(OrginalHeight)));
            ResultHeight = NewHeight;

            Bitmap ResultBitmap = new Bitmap(ResultWidth, ResultHeight);

            using (Graphics g = Graphics.FromImage(ResultBitmap))
            {
                g.DrawImage(input, 0, 0, ResultWidth, ResultHeight);
            }

            input.Dispose();
            input = null;

            return ResultBitmap;
        }
    }
}
