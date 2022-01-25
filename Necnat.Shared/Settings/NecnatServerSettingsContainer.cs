namespace Necnat.Shared.Settings
{
    public class NecnatServerSettingsContainer
    {
        public NecnatServerSettings NecnatServerSettings { get; set; }
    }

    public class NecnatServerSettings
    {
        public int ApplicationId { get; set; }
        public string AccessManagementConnectionString { get; set; }
        public ApiRouteSettings ApiRouteSettings { get; set; }
    }

    public class ApiRouteSettings
    {
        public string ApiPerfix { get; set; } = "Api/";
        public int ModuleOrder { get; set; } = 1;
        public int ControllerOrder { get; set; } = 2;
        public int ApiNameOrder { get; set; } = 3;
        public int ApiVersionOrder { get; set; } = 0;
        public string NoModuleDefault { get; set; } = "Legacy";
        public string NoApiVersionDefault { get; set; } = "v1.0";
    }
}
