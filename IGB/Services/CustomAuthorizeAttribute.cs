using Microsoft.AspNetCore.Authorization;

namespace IGB.Services
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {
            try 
            { 
                Policy = "ApiUser"; 
            } 
            catch
            {
                throw;
            }
            
        }
    }
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            try
            {
                _next = next;
            }
            catch (Exception)
            {

                throw;
            }            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"Response\":0, \"Error\":\"Not Authorized\"}");

                }
            }
            catch (Exception)
            {

                throw;
            }           
        }
    }

}
