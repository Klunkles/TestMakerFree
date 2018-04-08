using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMakerFreeWebApp.Data.Models;

namespace TestMakerFreeWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        #endregion

        #region Methods
        /// <summary>
        /// Generate table structures, automatic identity insertions, and 
        /// the one-many constraints
        /// </summary>
        /// <param name="modelBuilder">The model Builders</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("Users_t");
            modelBuilder.Entity<ApplicationUser>().HasMany(user => user.Quizzes).WithOne(quiz => quiz.User);

            modelBuilder.Entity<Quiz>().ToTable("Quizzes_t");
            modelBuilder.Entity<Quiz>().Property(quiz => quiz.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Quiz>().HasOne(quiz => quiz.User).WithMany(user => user.Quizzes);
            modelBuilder.Entity<Quiz>().HasMany(quiz => quiz.Questions).WithOne(question => question.Quiz);

            modelBuilder.Entity<Question>().ToTable("Questions_t");
            modelBuilder.Entity<Question>().Property(question => question.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>().HasOne(question => question.Quiz).WithMany(quiz => quiz.Questions);
            modelBuilder.Entity<Question>().HasMany(question => question.Answers).WithOne(answer => answer.Question);

            modelBuilder.Entity<Answer>().ToTable("Answer_t");
            modelBuilder.Entity<Answer>().Property(answer => answer.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Answer>().HasOne(answer => answer.Question).WithMany(question => question.Answers);

            modelBuilder.Entity<Result>().ToTable("Result_t");
            modelBuilder.Entity<Result>().Property(result => result.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Result>().HasOne(result => result.Quiz).WithMany(quiz => quiz.Results);
        }
        #endregion

        #region Properties
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Result> Results { get; set; }
        #endregion
    }
}
