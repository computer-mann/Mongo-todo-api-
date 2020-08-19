
using Microsoft.Extensions.Options;
using ToDoApi.Infrastructure.Settings;

namespace ToDoApi.Infrastructure.Helper
{
    public class JwtHelper
    {
        private readonly JwtSettings jwtSettings;
        public JwtHelper(IOptions<JwtSettings> options)
        {
            jwtSettings=options.Value;
        }
    }
}