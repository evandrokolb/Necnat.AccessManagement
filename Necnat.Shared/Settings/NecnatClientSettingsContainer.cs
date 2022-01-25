using System;

namespace Necnat.Shared.Settings
{
    public class NecnatClientSettingsContainer
    {
        public NecnatClientSettings NecnatClientSettings { get; set; }
    }

    public class NecnatClientSettings
    {
        public int ApplicationId { get; set; }
        public string ApplicationUrl { get; set; }
        public string AuthorizationApiEndPoint { get; set; } = "Api/v1.0/AccessManagement/Authorization/AGetFeatureWithHierarchyByApplicationId?applicationId=";
        public string SessionGroupName { get; set; } = "NecnatShared";
        public TimeSpan SessionGroupLifeTime { get; set; } = new TimeSpan(1, 0, 0);
        public string UserAuthorizationName { get; set; } = "NecnatUserAuthorization";
    }
}
