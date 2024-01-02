namespace MetLifeInsurance
{
    public class SessionExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        public SessionExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            if (context.Session.GetString("UserName") == null)
            {
                context.Response.Redirect("/Account/Login");
            }
            else
            {
                await _next(context);
            }
          
        }
    }
}
