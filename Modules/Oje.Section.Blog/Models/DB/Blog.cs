using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Blog.Models.DB
{
    [Table("Blogs")]
    public class Blog: IEntityWithSiteSettingId
    {
        public Blog()
        {
            BlogLastLikeAndViews = new List<BlogLastLikeAndView>();
            BlogTagBlogs = new List<BlogTagBlog>();
            BlogReviews = new List<BlogReview>();
            BlogOwnBlogs = new List<BlogBlog>();
            BlogRelatedBlogs = new List<BlogBlog>();
        }

        [Key]
        public long Id { get; set; }
        public int BlogCategoryId { get; set; }
        [ForeignKey("BlogCategoryId")]
        [InverseProperty("Blogs")]
        public BlogCategory BlogCategory { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Summery { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublisheDate { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("Blogs")]
        public User CreateUser { get; set; }
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        [MaxLength(200)]
        public string ImageUrl600 { get; set; }
        [MaxLength(200)]
        public string ImageUrl200 { get; set; }
        [MaxLength(200)]
        public string ImageUrl50 { get; set; }
        [MaxLength(400)]
        public string VideoUrl { get; set; }
        [MaxLength(200)]
        public string SoundUrl { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("Blogs")]
        public SiteSetting SiteSetting { get; set; }


        [InverseProperty("Blog")]
        public List<BlogLastLikeAndView> BlogLastLikeAndViews { get; set; }
        [InverseProperty("Blog")]
        public List<BlogTagBlog> BlogTagBlogs { get; set; }
        [InverseProperty("Blog")]
        public List<BlogReview> BlogReviews { get; set; }
        [InverseProperty("BlogOwn")]
        public List<BlogBlog> BlogOwnBlogs { get; set; }
        [InverseProperty("BlogRelated")]
        public List<BlogBlog> BlogRelatedBlogs { get; set; }
    }
}
