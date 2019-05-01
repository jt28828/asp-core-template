using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DotnetCoreWebApiTemplate.Middleware.Auth
{
    public class MyAuthenticationOptions : AuthenticationSchemeOptions,
        IPostConfigureOptions<MyAuthenticationOptions>
    {
        public void PostConfigure(string name, MyAuthenticationOptions options)
        {
        }
    }
}