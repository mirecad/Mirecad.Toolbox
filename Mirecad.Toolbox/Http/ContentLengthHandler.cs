using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mirecad.Toolbox.Http
{
    /// <summary>
    /// Adds Content-Length header base of length of actual content.
    /// </summary>
    public class ContentLengthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var stream = await request.Content.ReadAsStreamAsync())
            {
                request.Content.Headers.Add(@"Content-Length", stream.Length.ToString());
            }
            return await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
