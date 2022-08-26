using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Identity.Client;

namespace Mirecad.Toolbox.Http
{
    public class AzureAdAppMessageHandler : HttpClientHandler
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string[] _scopes;

        private readonly object _lock = new object();

        private string _bearerToken;
        private DateTimeOffset _tokenExpiresOnUtc = DateTimeOffset.MinValue;

        public AzureAdAppMessageHandler(string resourceId)
        {
            _scopes = new[] { resourceId + "/.default" };
        }

        public AzureAdAppMessageHandler(IConfidentialClientApplication appCredential, string resourceId)
        {
            _app = appCredential;
            _scopes = new[] { resourceId + "/.default" };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (ShouldRegenerateToken())
            {
                lock (_lock)
                {
                    ObtainBearerTokenAsync().GetAwaiter().GetResult();
                }                
            }
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", "Bearer " + _bearerToken);
            return await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task ObtainBearerTokenAsync()
        {
            if (_app is null)
            {
                await ObtainDefaultCredentialTokenAsync();
                return;
            }
            await ObtainClientAppTokenAsync();
        }

        private async Task ObtainClientAppTokenAsync()
        {
            var authenticationResult = await _app.AcquireTokenForClient(_scopes)
                .ExecuteAsync();
            _tokenExpiresOnUtc = authenticationResult.ExpiresOn;
            _bearerToken = authenticationResult.AccessToken;
        }

        private async Task ObtainDefaultCredentialTokenAsync()
        {
            var credential = new DefaultAzureCredential();
            var token = await credential.GetTokenAsync(new TokenRequestContext(_scopes));
            _tokenExpiresOnUtc = token.ExpiresOn;
            _bearerToken = token.Token.Replace("Bearer ", "");
        }

        private bool ShouldRegenerateToken()
        {
            var tokenExpireInSeconds = (_tokenExpiresOnUtc - DateTimeOffset.Now).TotalSeconds;
            return tokenExpireInSeconds < 60;
        }
    }
}