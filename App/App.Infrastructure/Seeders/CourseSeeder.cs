using App.Domain.Entities;
using App.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Seeders
{
    internal class CourseSeeder : ICourseSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseSeeder(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync() && !_dbContext.Courses.Any())
            {
                var educator = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == "educator");

                if (educator != null)
                {
                    var courses = GetCourses(educator.Id);
                    await _dbContext.Courses.AddRangeAsync(courses);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Course> GetCourses(string educatorId)
        {
            return new List<Course>
            {
                new() { Name = "Introduction to React", Description = "Learn the basics of React.js and build modern web applications.", Category = "Web Development", Price = 20, CreatedBy= educatorId },
                new() { Name = "Mastering JavaScript", Description = "Deep dive into JavaScript fundamentals and advanced concepts.", Category = "Web Development", Price = 25, CreatedBy= educatorId  },
                new() { Name = "Advanced CSS Techniques", Description = "Take your CSS skills to the next level with modern techniques.", Category = "Web Development", Price = 18, CreatedBy= educatorId  },
                new() { Name = "Full-Stack Web Development", Description = "Learn to build complete applications using front-end and back-end technologies.", Category = "Web Development", Price = 35, CreatedBy= educatorId  },
                new() { Name = "Mobile App Development with Flutter", Description = "Develop cross-platform mobile apps using Flutter framework.", Category = "Mobile Development", Price = 30, CreatedBy= educatorId  },
                new() { Name = "iOS Development with Swift", Description = "Build iOS applications with Swift programming language.", Category = "Mobile Development", Price = 40, CreatedBy= educatorId  },
                new() { Name = "Android Development with Kotlin", Description = "Learn how to develop Android apps using Kotlin.", Category = "Mobile Development", Price = 28, CreatedBy= educatorId  },
                new() { Name = "Game Development with Unity", Description = "Create 2D and 3D games using the Unity game engine.", Category = "Game Development", Price = 50, CreatedBy= educatorId  },
                new() { Name = "Beginner's Guide to Unreal Engine", Description = "Start your journey with game development using Unreal Engine.", Category = "Game Development", Price = 40, CreatedBy= educatorId  },
                new() { Name = "AI Programming with Python", Description = "Learn artificial intelligence concepts and implement them in Python.", Category = "Programming", Price = 45 , CreatedBy = educatorId},
                new() { Name = "Data Science with R", Description = "Analyze data using R programming language and create predictive models.", Category = "Data Science", Price = 50, CreatedBy= educatorId  },
                new() { Name = "Machine Learning Fundamentals", Description = "Understand the principles of machine learning and apply them to real-world problems.", Category = "Data Science", Price = 55, CreatedBy= educatorId  },
                new() { Name = "Web Scraping with Python", Description = "Learn how to extract data from websites using Python and BeautifulSoup.", Category = "Programming", Price = 22, CreatedBy= educatorId  },
                new() { Name = "Cloud Computing with AWS", Description = "Get hands-on experience with AWS and learn cloud computing fundamentals.", Category = "Cloud Computing", Price = 60, CreatedBy= educatorId  },
                new() { Name = "DevOps Essentials", Description = "Learn the essentials of DevOps and how to implement CI/CD pipelines.", Category = "Cloud Computing", Price = 45, CreatedBy= educatorId  },
                new() { Name = "Digital Marketing Strategy", Description = "Learn strategies to grow a brand using online marketing channels.", Category = "Marketing", Price = 30, CreatedBy= educatorId  },
                new() { Name = "SEO Mastery", Description = "Master SEO techniques to rank higher in search engines and increase web traffic.", Category = "Marketing", Price = 25, CreatedBy= educatorId  },
                new() { Name = "Content Creation for Social Media", Description = "Learn how to create and market content across social media platforms.", Category = "Marketing", Price = 20, CreatedBy= educatorId  },
                new() { Name = "Introduction to Cybersecurity", Description = "Understand the basics of cybersecurity and how to protect data and networks.", Category = "IT & Security", Price = 40, CreatedBy= educatorId  },
                new() { Name = "Ethical Hacking and Penetration Testing", Description = "Learn the skills to become an ethical hacker and secure web applications.", Category = "IT & Security", Price = 55, CreatedBy= educatorId  }


            };
        }
    }
}
