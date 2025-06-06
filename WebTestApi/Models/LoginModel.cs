using System;
namespace TestWebApi.Models
{
    public record LoginModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}

