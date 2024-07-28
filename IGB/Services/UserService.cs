using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using IGB.Data;

namespace IGB.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public string LoggedInUserId { get; private set; }
        public string LoggedInUserRole { get; private set; }
        public string LoggedInGuardianStudentId { get; private set; }

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {
            try
            {

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context;
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

            InitializeUserInfo();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InitializeUserInfo()
        {
            try
            {
                var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                if (userId != null)
                {
                    LoggedInUserId = userId;

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var RoleId = Context.ApplicationUserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).FirstOrDefault();

                        LoggedInUserRole = Context.ApplicationRoles.Where(x => x.Id == RoleId).Select(x => x.Name).FirstOrDefault();

                        if (LoggedInUserRole == "Guardian")
                        {
                            LoggedInGuardianStudentId = Context.ApplicationUsers.Where(x => x.Id == userId).Select(x => x.StudentId).FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
           
        }
    }
}
