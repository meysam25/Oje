using Oje.Section.WebMain.Interfaces;
using System;
using System.Collections.Generic;

namespace Oje.Section.WebMain.Models.View
{
    public class PageWebVM
    {
        public PageWebVM()
        {
            Items = new ();
            PageWebSliderVMs = new();
        }

        public long id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string stColorCode { get; set; }
        public string summery { get; set; }
        public string mainImage { get; set; }
        public string mainImageSmall { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
        public string url { get; set; }
        public DateTime createDate { get; set; }

        public List<object> Items { get; set; }
        public List<PageWebSliderVM> PageWebSliderVMs { get; set; }
    }
}
