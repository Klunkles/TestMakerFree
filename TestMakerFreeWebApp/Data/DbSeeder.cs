using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMakerFreeWebApp.Data.Models;

namespace TestMakerFreeWebApp.Data
{
    public class DbSeeder
    {
        #region Public Methods
        /// <summary>
        /// Generate data in the database if none exists
        /// </summary>
        /// <param name="dbContext"></param>
        public static void Seed(ApplicationDbContext dbContext)
        {
            //Create default Users (if there are none)
            if (!dbContext.Users.Any())
            {
                CreateUsers(dbContext);
            }

            //Create default quizzes (if there are none) together with their set of Q&A
            if (!dbContext.Quizzes.Any())
            {
                CreateQuizzes(dbContext);
            }
        }
        #endregion

        #region Seed Methods
        /// <summary>
        /// Create admin and test user accounts
        /// </summary>
        /// <param name="dbContext">Connection to the database</param>
        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            //local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //Create the "Admin" ApplicationUser account (if it doesn't already exist)
            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            //Insert the admin user into the database
            dbContext.Users.Add(user_Admin);

#if DEBUG
            //create some sample registered user accounts (if they don't exist already)
            var user_Ryan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Solice = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Vodan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            //Insert sample registered users into the database
            dbContext.Users.AddRange(user_Ryan, user_Solice, user_Vodan);
#endif
            //Commit the changes to the database
            dbContext.SaveChanges();
        }

        private static void CreateQuizzes(ApplicationDbContext dbContext)
        {
            //local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //retrieve the admin user, which we'll use as default author.
            var authorId = dbContext.Users.Where(user => string.Equals(user.UserName, "Admin"))
                .FirstOrDefault()
                .Id;

#if DEBUG
            //create 47 sample quizzes with auto-generated data
            var num = 47;
            for (int i = 1; i <= num; i++)
            {
                CreateSampleQuiz(dbContext, i, authorId, num - i, 3, 3, 3, createdDate.AddDays(-num));
            }
#endif

            //Create 3 more quizzes with better descriptive data
            EntityEntry<Quiz> starwars = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Are you more Light or Dark side of the Force?",
                Text = @"Choose wisely you must, young padawan: " +
                "this test will prove if your will is strong enough " +
                "to adhere to the principles of the light side of the Force " +
                "or if you're fated to embrace the dark side. " +
                "No you want to become a true JEDI, you can't possibly miss this!",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Quiz> generations = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "GenX, GenY or Genz?",
                Description = "Find out what decade most represents you",
                Text = @"Do you feel confortable in your generation? " +
                "What year should you have been born in?" +
                "Here's a bunch of questions that will help you to find out!",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Quiz> anime = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Attack On Titan personality test",
                Text = @"Do you relentlessly seek revenge like Eren? " +
                           "Are you willing to put your like on the " +
                           "stake to protect your friends like Mikasa ? " +
                           "Would you trust your fighting skills like Levi " +
                           "or rely on your strategies and tactics like Arwin? " +
                           "Unveil your true self with this Attack On Titan " +
                           "personality test!",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            //Commit changes in database   
            dbContext.SaveChanges();
        }
        #endregion

        #region Utility Methods
        private static void CreateSampleQuiz(
            ApplicationDbContext dbContext,
            int num,
            string authorId,
            int viewCount,
            int numberOfQuestions,
            int numberOfAnswersPerQuestion,
            int numberOfResults,
            DateTime createdDate
            )
        {
            var quiz = new Quiz()
            {
                UserId = authorId,
                Title = $"Quiz {num} Title",
                Description = $"This is a sample description quiz {num}",
                Text = "This is a sample quiz created by the DbSeeder class for " +
                "testing purposes. All the questions, answers and results are auto-generated as well",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };

            dbContext.Quizzes.Add(quiz);
            dbContext.SaveChanges();

            for(int i = 0; i < numberOfQuestions; i++)
            {
                var question = new Question()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample question created by the dbSeeder class for testing purposes." +
                    "All the child answers are auto-generated as well.",
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                };

                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                for(int j = 0; j < numberOfAnswersPerQuestion; j++)
                {
                    var answers = dbContext.Answers.Add(new Answer()
                    {
                        QuestionId = question.Id,
                        Text = "This is a sample answer created by the DbSeeder class for testing purposes",
                        Value = j,
                        CreatedDate = createdDate,
                        LastModifiedDate = createdDate
                    });
                }
            }

            for(int i = 0; i < numberOfResults; i++)
            {
                dbContext.Results.Add(new Result()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample result created by the DbSeeder class for testing purposes.",
                    MinValue = 0,
                    //max value should be equal to answers number * max answer value
                    MaxValue = numberOfAnswersPerQuestion * 2,
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                });
            }
            dbContext.SaveChanges();
        }
        #endregion
    }
}
