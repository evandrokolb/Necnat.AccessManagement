using Necnat.Shared.Models;
using System.Threading.Tasks;

namespace Necnat.Client.Interfaces
{
    public interface INecnatSessionService
    {
        public Task<MdUserAuthorizationReduced> GetUserAuthorizationAsync(int? applicationId = null);

        public Task RemoveUserAuthorizationAsync(int? applicationId = null);

        public Task RemoveSessionAsync();

        public bool HasRole(MdUserAuthorizationReduced userAuthorizationReduced, string roleCodeName);

        public bool HasRole(MdUserAuthorizationReduced userAuthorizationReduced, string roleCodeName, int componentTypeId, int componentId);

        public bool HasModuleFeature(MdUserAuthorizationReduced userAuthorizationReduced, string moduleCodeName, string featureCodeName);

        public bool HasModuleFeature(MdUserAuthorizationReduced userAuthorizationReduced, string moduleCodeName, string featureCodeName, int componentTypeId, int componentId);
    }
}
