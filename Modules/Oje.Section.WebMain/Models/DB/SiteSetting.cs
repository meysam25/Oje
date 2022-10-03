using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            AutoAnswerOnlineChatMessages = new();
            ContactUses = new();
            FooterExteraLinks = new();
            FooterGroupExteraLinks = new();
            LoginBackgroundImages = new();
            LoginDescrptions = new();
            OurObjects = new();
            Pages = new();
            PageLeftRightDesigns = new();
            PageLeftRightDesignItems = new();
            PageManifests = new();
            PageManifestItems = new();
            PageSliders = new();
            ShortLinks = new();
            TopMenus = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<AutoAnswerOnlineChatMessage> AutoAnswerOnlineChatMessages { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ContactUs> ContactUses { get; set; }
        [InverseProperty("SiteSetting")]
        public List<FooterExteraLink> FooterExteraLinks { get; set; }
        [InverseProperty("SiteSetting")]
        public List<FooterGroupExteraLink> FooterGroupExteraLinks { get; set; }
        [InverseProperty("SiteSetting")]
        public List<LoginBackgroundImage> LoginBackgroundImages { get; set; }
        [InverseProperty("SiteSetting")]
        public List<LoginDescrption> LoginDescrptions { get; set; }
        [InverseProperty("SiteSetting")]
        public List<OurObject> OurObjects { get; set; }
        [InverseProperty("SiteSetting")]
        public List<Page> Pages { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PageLeftRightDesign> PageLeftRightDesigns { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PageLeftRightDesignItem> PageLeftRightDesignItems { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PageManifest> PageManifests { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PageManifestItem> PageManifestItems { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PageSlider> PageSliders { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ShortLink> ShortLinks { get; set; }
        [InverseProperty("SiteSetting")]
        public List<TopMenu> TopMenus { get; set; }
    }
}
