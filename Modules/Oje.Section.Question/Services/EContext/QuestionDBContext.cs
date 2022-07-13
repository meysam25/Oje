using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.Question.Models.DB;

namespace Oje.Section.Question.Services.EContext
{
    public class QuestionDBContext : MyBaseDbContext
    {
        public DbSet<YourQuestion> YourQuestions { get; set; }
        public DbSet<UserRegisterFormYourQuestion> UserRegisterFormYourQuestions { get; set; }
        public DbSet<UserRegisterForm> UserRegisterForms { get; set; }
        public DbSet<ProposalFormYourQuestion> ProposalFormYourQuestions { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }

        public QuestionDBContext(DbContextOptions<QuestionDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
