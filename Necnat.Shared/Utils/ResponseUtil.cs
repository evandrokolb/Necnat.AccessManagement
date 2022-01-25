using Necnat.Shared.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Necnat.Shared.Utils
{
    public class ResponseUtil
    {
        public static async Task<T> KeyOf<T>(HttpResponseMessage httpResponseMessage)
        {
            return JsonConvert.DeserializeObject<dynamic>(await httpResponseMessage.Content.ReadAsStringAsync()).key;
        }

        public static async Task<string> MessageOf(HttpResponseMessage httpResponseMessage)
        {
            return JsonConvert.DeserializeObject<dynamic>(await httpResponseMessage.Content.ReadAsStringAsync()).message;
        }

        public static async Task<T> DataOfFilterObject<T>(HttpResponseMessage httpResponseMessage)
        {
            var fo = await httpResponseMessage.Content.ReadFromJsonAsync<MdFilterObject>();
            return JsonConvert.DeserializeObject<T>(fo.Data.ToString());
        }
    }
}
