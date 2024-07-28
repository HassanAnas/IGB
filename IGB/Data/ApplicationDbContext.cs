

using IGB.Models;
using IGB.Models.Admin;
using IGB.Models.Feedback;
using IGB.Models.Tutor;
using IGB.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IGB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeCourse> GradeCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTopic> CourseTopics { get; set; }
        public DbSet<TutorSpeciality> TutorSpecialitys { get; set; }
        public DbSet<TutorEducation> TutorEducations { get; set; }
        public DbSet<TutorExperience> TutorExperiences { get; set; }
        public DbSet<TutorDocument> TutorDocuments { get; set; }
        public DbSet<AdminDocument> AdminDocuments { get; set; }
        public DbSet<CourseBooking> CourseBookings { get; set; }
        public DbSet<LessonBooking> LessonBookings { get; set; }
 
        public DbSet<StudentCredit> StudentCredits { get; set; }
        public DbSet<AllNotification> AllNotifications { get; set; }

        public DbSet<TutorFeedback> TutorFeedbacks { get; set; }
        public DbSet<StudentFeedback> StudentFeedbacks { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method to ensure any default behavior is preserved
            base.OnModelCreating(modelBuilder);

            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {

            var defaultUser = new ApplicationUser
            {
                Id = "b9055214-c3b3-4f05-b383-b6e601042b35",
                SecurityStamp = "5f175fb0-7da8-42b7-ad22-fdc734ada25a",
                ConcurrencyStamp = "6947db82-43e2-4974-8820-275aa5302715",
                UserName = "superadmin@gmail.com",
                FirstName = "SuperAdmin",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                PasswordHash = "AQAAAAEAACcQAAAAEBy+nQwjwzI0Obd1HeJIb/VfZArTILlvoj8Nd0iHd0HY4y8pgav9ZvLVDx4gTcnoiw==",
                EmailConfirmed = true,
                IsSuperAdmin=true,
                RoleName="SuperAdmin",
                IsUpdated=true

            };

            modelBuilder.Entity<ApplicationUser>().HasData(defaultUser);




            var roles = new List<ApplicationRole>
                {
                    new ApplicationRole { ConcurrencyStamp="738ede22-b7d9-475a-bb28-5a5058350a37",Id="ad98681d-5abb-4df9-9a9d-4797888220ac",Name = "SuperAdmin" },
                    new ApplicationRole { ConcurrencyStamp="cdfbb1eb-c26c-4662-897a-649775ef8bd3",Id="de8dc073-416f-4716-89be-b1c48c9f9de1",Name = "Admin" },
                         new ApplicationRole { ConcurrencyStamp = "29b705bc-f203-4401-bb9b-b052456a3449", Id = "2c98e431-4a03-4999-807d-d7ac6ebedac9",  Name = "Tutor" },
                new ApplicationRole { ConcurrencyStamp="aa48445e-1746-4525-b94d-b26283394355",Id="feb14f16-e7ac-43bf-9bd2-e7de8d1ba71d",Name = "Student" },
                new ApplicationRole { ConcurrencyStamp = "59f8dcf7-b7e8-4731-97c4-fb8652ed0f51", Id = "a2204359-8a88-49c9-867f-a2dd15e3f7c8",  Name = "Guardian" }
                };

            modelBuilder.Entity<ApplicationRole>().HasData(roles);

            var userrole = new List<ApplicationUserRole>
                {
                    new ApplicationUserRole { UserId="b9055214-c3b3-4f05-b383-b6e601042b35",RoleId="ad98681d-5abb-4df9-9a9d-4797888220ac"},
               };

            modelBuilder.Entity<ApplicationUserRole>().HasData(userrole);

        }
    }
}
