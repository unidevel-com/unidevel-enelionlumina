using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Unidevel.EnelionLumina.Model;

namespace Unidevel.EnelionLumina
{
    public class EnelionLuminaChargerClient : IEnelionLuminaChargerClient
    {
        private string? _sessionCookieValue = null;
        private string? _hostName = null;

        public async Task LoginAsync(string hostname, string userName, string password)
        {
            if (_sessionCookieValue == null) throw new InvalidOperationException("Logout first.");

            _hostName = hostname;

            var httpClient = new HttpClient();
            var credentials = new LoginRequest() { UserName = userName, Password = password };

            var postResult = await httpClient.PostAsJsonAsync($"http://{_hostName}/api/users/login", credentials);
            var response = await postResult.Content.ReadAsStringAsync();

            Console.WriteLine(response);

            var setCookieHeaders = postResult.Headers.GetValues("Set-Cookie");

            foreach (var setCookieHeader in setCookieHeaders)
            {
                var cookieNameValues = setCookieHeader.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var cookieNameValue in cookieNameValues)
                {
                    var cookieNameValueSplit = cookieNameValue.Split('=', 2);
                    if (cookieNameValueSplit.Length == 2)
                    {
                        if (cookieNameValueSplit[0] == "session")
                        {
                            if (_sessionCookieValue == null) _sessionCookieValue = cookieNameValueSplit[1]; else throw new InvalidOperationException("Multiple session values found.");
                        }
                    }
                }
            }
            if (_sessionCookieValue == null) throw new InvalidOperationException("Session cookie not found.");

            Console.WriteLine(_sessionCookieValue);
        }

        public Task LogoutAsync()
        {
            _hostName = null;
            _sessionCookieValue = null;

            return Task.CompletedTask;
        }

        public async Task SetMainsAsync(Phases phasesLimit, int currentLimitAmp)
        {
            var httpClient = new HttpClient();

            var mainsRequest = new MainsRequest() { CurrentLimitAmp = currentLimitAmp, PhasesLimit = phasesLimit };

            JsonContent request = JsonContent.Create(mainsRequest);
            request.Headers.Add("cookie", "session=" + _sessionCookieValue);

            var mainsResult = await httpClient.PatchAsync($"http://{_hostName}/api/charger/mains", request);
            mainsResult.EnsureSuccessStatusCode();

            _ = await mainsResult.Content.ReadFromJsonAsync<MainsResponse>();
        }
    }
}
