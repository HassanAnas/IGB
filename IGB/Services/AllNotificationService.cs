using IGB.Data;
using Microsoft.AspNetCore.SignalR;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using IGB.Hubs;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using IGB.Models;
using IGB.Shared;

namespace IGB.Services
{
    public class AllNotificationService : IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ApplicationDbContext context;
        private readonly IHubContext<AllNotificationHub> hub;
        private readonly SqlTableDependency<AllNotification> dependency;
        private const int MaxRetryAttempts = 3;
        private readonly TimeSpan PauseBetweenFailures = TimeSpan.FromSeconds(2);
        private readonly UserService UserService;
        private readonly string connectionString;
        private string? UserId;
        private string? UserRole;
        private string? GuardianStudentId;

        public AllNotificationService(ApplicationDbContext context, IHubContext<AllNotificationHub> hub, IConfiguration configuration, IServiceScopeFactory scopeFactory, UserService UserService)
        {
            try
            {

                this.context = context;
                this.hub = hub;

                this.UserService = UserService;
                this.scopeFactory = scopeFactory;
                connectionString = configuration.GetConnectionString("DefaultConnection");

                UserId = UserService.LoggedInUserId;
                UserRole = UserService.LoggedInUserRole;
                GuardianStudentId = UserService.LoggedInGuardianStudentId;

                //var connectionString = "Data Source=DESKTOP-LUKDDU1;Initial Catalog=IGB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
                dependency = new SqlTableDependency<AllNotification>(connectionString, "AllNotifications");
                dependency.OnChanged += Dependency_OnChanged;
                StartDependencyWithRetry();

            }
            catch (Exception)
            {

                throw;
            }
        }
 

        private void Dependency_OnChanged(object sender, RecordChangedEventArgs<AllNotification> e)
        {
            try
            {



                UserId = UserService.LoggedInUserId;
                UserRole = UserService.LoggedInUserRole;
                GuardianStudentId = UserService.LoggedInGuardianStudentId;

                if (UserRole == "Admin")
                {

                    var adminnotifications = GetAdmin();
                    hub.Clients.All.SendAsync("RefreshAdmin", adminnotifications);
                }
                else if (UserRole == "Tutor")
                {

                    var tutornotifications = GetTutor();
                    hub.Clients.All.SendAsync("RefreshTutor", tutornotifications);
                }
                else if (UserRole == "Student" || UserRole == "Guardian")
                {

                    var studentnotifications = GetStudent();
                    hub.Clients.All.SendAsync("RefreshStudent", studentnotifications);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void StartDependencyWithRetry()
        {
            try
            {

                var attempts = 0;
                while (true)
                {
                    try
                    {
                        dependency.Start();
                        break; // Success! Break out of the loop.
                    }
                    catch (SqlException ex)
                    {
                        attempts++;
                        if (attempts == MaxRetryAttempts)
                            throw; // Re-throw the exception if we've reached the max retry attempts.
                        Task.Delay(PauseBetweenFailures).Wait();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllNotification> GetAdmin()
        {
            try
            {

                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    return Context.AllNotifications
                    .Where(x => x.ForAdmin == true && x.IsReadByAdmin == false)
                    .Include(x => x.NewApplicationUsers)
                    .Include(x => x.TutorApplicationUsers)
                    .Include(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.TutorFeedbacks)
                    .Include(x => x.StudentFeedbacks)
                              .OrderByDescending(n => n.Id)
                               .Skip(0)
                              .Take(10)
                              .ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public long GetAdminCount()
        {
            try
            {
                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    return Context.AllNotifications
                                     .Include(x => x.NewApplicationUsers)
                    .Include(x => x.TutorApplicationUsers)
                    .Include(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.TutorFeedbacks)
                    .Include(x => x.StudentFeedbacks)
                    .Where(x => x.ForAdmin == true && x.IsReadByAdmin == false).Count();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllNotification> GetTutor()
        {
            try
            {

                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    return Context.AllNotifications
                    .Where(x => x.ForTutor == true && x.IsReadByTutor == false && x.ForTutorId == UserId)
                             .Include(x => x.NewApplicationUsers)
                    .Include(x => x.TutorApplicationUsers)
                    .Include(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.TutorFeedbacks)
                    .Include(x => x.StudentFeedbacks)
                              .OrderByDescending(n => n.Id)
                               .Skip(0)
                              .Take(10)
                              .ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public long GetTutorCount()
        {
            try
            {

                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    return Context.AllNotifications
                                     .Include(x => x.NewApplicationUsers)
                    .Include(x => x.TutorApplicationUsers)
                    .Include(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.TutorFeedbacks)
                    .Include(x => x.StudentFeedbacks)
                 .Where(x => x.ForTutor == true && x.IsReadByTutor == false && x.ForTutorId == UserId).Count();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllNotification> GetStudent()
        {
            try
            {

                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (UserRole == "Guardian")
                    {
                        return Context.AllNotifications
      .Where(x => x.ForStudent == true && x.IsReadByStudent == false && x.ForStudentId == GuardianStudentId)
               .Include(x => x.NewApplicationUsers)
       .Include(x => x.TutorApplicationUsers)
       .Include(x => x.StudentApplicationUsers)
       .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
       .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
       .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
       .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
       .Include(x => x.TutorFeedbacks)
       .Include(x => x.StudentFeedbacks)
                 .OrderByDescending(n => n.Id)
                  .Skip(0)
                 .Take(10)
                 .ToList();
                    }
                    else
                    {
                        return Context.AllNotifications
.Where(x => x.ForStudent == true && x.IsReadByStudent == false && x.ForStudentId == UserId)
.Include(x => x.NewApplicationUsers)
.Include(x => x.TutorApplicationUsers)
.Include(x => x.StudentApplicationUsers)
.Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
.Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
.Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
.Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
.Include(x => x.TutorFeedbacks)
.Include(x => x.StudentFeedbacks)
.OrderByDescending(n => n.Id)
.Skip(0)
.Take(10)
.ToList();
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public long GetStudentCount()
        {
            try
            {

                using (var scope1 = scopeFactory.CreateScope())
                {
                    var Context = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (UserRole == "Guardian")
                    {
                        return Context.AllNotifications
                                 .Include(x => x.NewApplicationUsers)
                .Include(x => x.TutorApplicationUsers)
                .Include(x => x.StudentApplicationUsers)
                .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                .Include(x => x.TutorFeedbacks)
                .Include(x => x.StudentFeedbacks)
                     .Where(x => x.ForStudent == true && x.IsReadByStudent == false && x.ForStudentId == GuardianStudentId).Count();
                    }
                    else
                    {
                        return Context.AllNotifications
                              .Include(x => x.NewApplicationUsers)
             .Include(x => x.TutorApplicationUsers)
             .Include(x => x.StudentApplicationUsers)
             .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
             .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
             .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
             .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
             .Include(x => x.TutorFeedbacks)
             .Include(x => x.StudentFeedbacks)
                  .Where(x => x.ForStudent == true && x.IsReadByStudent == false && x.ForStudentId == UserId).Count();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Dispose()
        {
            try
            {
                dependency.Stop();
                dependency.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}