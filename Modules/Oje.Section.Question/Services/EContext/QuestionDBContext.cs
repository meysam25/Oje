using Microsoft.EntityFrameworkCore;
using Oje.Section.Question.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Question.Services.EContext
{
    public class QuestionDBContext : DbContext
    {
        public DbSet<YourQuestion> YourQuestions { get; set; }

        public QuestionDBContext(DbContextOptions<QuestionDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
    }
}
