using System.Text.Json.Serialization;

namespace Unidevel.EnelionLumina.Model
{
    class LoginRequest
    {
        [JsonPropertyName("username")] public string UserName { get; set; }
        [JsonPropertyName("password")] public string Password { get; set; }

        public LoginRequest(string userName, string password)
        {
            ArgumentNullException.ThrowIfNull(userName);
            ArgumentNullException.ThrowIfNull(password);

            UserName = userName;
            Password = password;
        }
    }
}
