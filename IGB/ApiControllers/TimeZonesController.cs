using IGB.Data;
using IGB.Models.Admin;
using IGB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZonesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public TimeZonesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("AllTimeZones")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllCurriculums()
        {
            try
            {
                var TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(tz => new { tz.Id, tz.DisplayName }).ToList();

                if (TimeZones.Count > 0)
                {
                    var response = new
                    {
                        Response = "1",
                        TimeZones = TimeZones
                    };

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return Ok(jsonResponse);
                }
                else
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "No TimeZone Found"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);

                }
            }

            catch (Exception ex)
            {
                var Response = new
                {
                    Response = "0",
                    Error = "Something Went Wrong"
                };

                string jsonResponse = JsonConvert.SerializeObject(Response);
                return BadRequest(jsonResponse);
            }
        }
    }
}

