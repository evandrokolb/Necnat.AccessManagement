using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Necnat.Shared.Domains;
using Necnat.Shared.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.Server.Data.DataSeeds
{
    public partial class DataSeed
    {
        public async Task ApiDataSeedAsync()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var apiRouteList = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
                x => x.AttributeRouteInfo.Template.StartsWith(_necnatServerSettings.ApiRouteSettings.ApiPerfix)
                && x.GetType() == typeof(ControllerActionDescriptor)).ToList();

                foreach (ControllerActionDescriptor iApiRoute in apiRouteList)
                {
                    var split = iApiRoute.AttributeRouteInfo.Template.Substring(_necnatServerSettings.ApiRouteSettings.ApiPerfix.Length).Split('/');

                    //Module
                    var moduleCodeName = _necnatServerSettings.ApiRouteSettings.ModuleOrder > -1 ? split[_necnatServerSettings.ApiRouteSettings.ModuleOrder] : _necnatServerSettings.ApiRouteSettings.NoModuleDefault;
                    var moduleId = await AddModuleAsync(_necnatServerSettings.ApplicationId, moduleCodeName);

                    //Controller
                    var controllerCodeName = split[_necnatServerSettings.ApiRouteSettings.ControllerOrder];
                    var controllerId = await AddControllerAsync(moduleId, controllerCodeName);

                    //Api
                    var apiCodeName = split[_necnatServerSettings.ApiRouteSettings.ApiNameOrder];

                    var apiHttpMethodId = GetHttpMethodId(iApiRoute);
                    if (apiHttpMethodId == null)
                        continue;

                    var apiVersion = GetApiVersion(iApiRoute, split[_necnatServerSettings.ApiRouteSettings.ApiVersionOrder], _necnatServerSettings);
                    if (string.IsNullOrEmpty(apiVersion))
                        continue;

                    await AddApiAsync(controllerId, apiCodeName, (int)apiHttpMethodId, apiVersion);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private int? GetHttpMethodId(ControllerActionDescriptor controllerActionDescriptor)
        {
            var httpMethodActionConstraintList = controllerActionDescriptor.ActionConstraints.Where(x => x.GetType() == typeof(HttpMethodActionConstraint)).ToList();

            if (httpMethodActionConstraintList.Count() != 1)
                return null;
            if (((HttpMethodActionConstraint)httpMethodActionConstraintList.First()).HttpMethods.Count() != 1)
                return null;

            return HttpMethodDomain.GetByName(((HttpMethodActionConstraint)httpMethodActionConstraintList.First()).HttpMethods.First()).Id;
        }

        private string GetApiVersion(ControllerActionDescriptor controllerActionDescriptor, string splitApiVersion, NecnatServerSettings _necnatServerSettings)
        {
            var apiVersion = _necnatServerSettings.ApiRouteSettings.NoApiVersionDefault;
            if (_necnatServerSettings.ApiRouteSettings.ApiVersionOrder > -1)
            {
                if (!splitApiVersion.Contains("{version:apiVersion}"))
                    apiVersion = splitApiVersion;
                else
                {
                    var apiVersionAttributeList = controllerActionDescriptor.EndpointMetadata.Where(x => x.GetType() == typeof(ApiVersionAttribute)).ToList();
                    if (apiVersionAttributeList.Count() != 1)
                        return null;
                    if (((ApiVersionAttribute)apiVersionAttributeList.First()).Versions.Count() != 1)
                        return null;
                    apiVersion = splitApiVersion.Replace("{version:apiVersion}", ((ApiVersionAttribute)apiVersionAttributeList.First()).Versions.First().ToString());
                }
            }

            return apiVersion;
        }
    }
}
