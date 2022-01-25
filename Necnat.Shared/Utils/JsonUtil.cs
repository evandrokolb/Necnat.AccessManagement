using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Necnat.Shared.Utils
{
    public static class JsonUtil
    {
        public static T Clone<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static List<T> RemakeSelectedRecordList<T>(List<T> recordList, List<T> selectedRecordList)
        {
            var l = new List<T>();
            foreach (var iSelectedRecord in selectedRecordList)
            {
                var substitute = recordList.Where(x => JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(iSelectedRecord)).FirstOrDefault();
                if (substitute != null)
                {
                    l.Remove(selectedRecordList.Where(x => JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(iSelectedRecord)).First());
                    l.Add(substitute);
                }
                else
                    l.Add(iSelectedRecord);
            }

            return l;
        }
    }
}
