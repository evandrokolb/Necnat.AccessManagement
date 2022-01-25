using System.Net.Http;

namespace Necnat.Shared.HttpClients.NamHttpClients
{
    public partial class NamHttpClient
    {
        public HttpClient HttpClient { get; set; }

        public NamHttpClient(HttpClient client)
        {
            HttpClient = client;
        }
    }
}
