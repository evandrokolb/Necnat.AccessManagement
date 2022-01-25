using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.Server.Data.DataSeeds
{
    public partial class DataSeed
    {
        public async Task FeatureDataSeedAsync(List<FeatureDataSeed> featureDataSeedList = null)
        {
            if (featureDataSeedList == null)
                featureDataSeedList = DefaultFeatureDataSeed.GetFeatureDataSeedList();

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var controllerList = await _context.Controller
                    .Include(x => x.ApiList)
                    .ToListAsync();

                foreach (var iController in controllerList)
                {
                    foreach (var iFeatureDataSeed in featureDataSeedList)
                    {
                        foreach (var iApiName in iFeatureDataSeed.ApiNameList)
                        {
                            var api = iController.ApiList.Where(x => x.Name == iApiName).FirstOrDefault();
                            if (api != null)
                            {
                                var featureId = await AddFeatureAsync(iController.ModuleId, iController.Name + "." + iFeatureDataSeed.FeatureName);
                                await AddFeatureApiAsync(featureId, api.Id);
                            }
                        }
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public class FeatureDataSeed
    {
        public FeatureDataSeed()
        {
            ApiNameList = new HashSet<string>();
        }

        public string FeatureName { get; set; }
        public ICollection<string> ApiNameList { get; set; }
    }

    public static class DefaultFeatureDataSeed
    {
        public static List<FeatureDataSeed> GetFeatureDataSeedList()
        {
            var l = new List<FeatureDataSeed>();

            var e = new FeatureDataSeed();
            e.FeatureName = "Read";
            e.ApiNameList.Add("List");
            e.ApiNameList.Add("AList");
            e.ApiNameList.Add("GetById");
            e.ApiNameList.Add("AGetById");
            l.Add(e);

            e = new FeatureDataSeed();
            e.FeatureName = "Insert";
            e.ApiNameList.Add("Insert");
            e.ApiNameList.Add("AInsert");
            l.Add(e);

            e = new FeatureDataSeed();
            e.FeatureName = "Update";
            e.ApiNameList.Add("Update");
            e.ApiNameList.Add("AUpdate");
            l.Add(e);

            e = new FeatureDataSeed();
            e.FeatureName = "Delete";
            e.ApiNameList.Add("Delete");
            e.ApiNameList.Add("ADelete");
            l.Add(e);

            return l;
        }
    }
}
