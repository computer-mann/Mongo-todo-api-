namespace ToDoApi.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string Issuer {get;set;}
        public string Audience { get; set; }
        public string SignInKey { get; set; }
    }
}