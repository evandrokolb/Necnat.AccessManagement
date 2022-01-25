using Necnat.Shared.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Necnat.Shared.Models
{
    public class MdUserAuthorization
    {
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
        public List<MdAllowedRole> AllowedRoleList { get; set; }
        public List<MdAllowedModuleFeature> AllowedModuleFeatureList { get; set; }
        public MdUserAuthorizationReduced ToReduced()
        {
            var mdReduced = new MdUserAuthorizationReduced();
            mdReduced.UserId = UserId;
            mdReduced.ApplicationId = ApplicationId;
            mdReduced.AllowedRoleReducedList = new List<MdAllowedRoleReduced>();
            mdReduced.AllowedFeatureReducedList = new List<MdAllowedFeatureReduced>();
            mdReduced.AllowedModuleReducedList = new List<MdAllowedModuleReduced>();
            mdReduced.HierarchyComponentReducedJoinList = new List<MdHierarchyComponentReducedJoin>();

            var iHierarchy = 1;
            var dictHierarchy = new Dictionary<int, ICollection<MdHierarchyComponent>>();

            //Role
            if (AllowedRoleList != null)
                foreach (var iAllowedRole in AllowedRoleList)
                {
                    //Hierarchy
                    int? hierarchyComponentId = null;
                    foreach (var iKeyValue in dictHierarchy)
                        if (EnumerableUtil.ScrambledEquals(iKeyValue.Value, iAllowedRole.HierarchyComponentList))
                        {
                            hierarchyComponentId = iKeyValue.Key;
                            break;
                        }

                    if (hierarchyComponentId == null)
                    {
                        dictHierarchy.Add(iHierarchy, iAllowedRole.HierarchyComponentList);
                        hierarchyComponentId = iHierarchy;
                        iHierarchy++;
                    }

                    //Role
                    mdReduced.AllowedRoleReducedList.Add(new MdAllowedRoleReduced { R = iAllowedRole.RoleId, N = iAllowedRole.RoleCodeName, J = iHierarchy });
                    if (!mdReduced.HierarchyComponentReducedJoinList.Any(x => x.J == iHierarchy))
                    {
                        var j = new MdHierarchyComponentReducedJoin();
                        j.J = iHierarchy;
                        j.MdHierarchyComponentReducedList = new List<MdHierarchyComponentReduced>();

                        foreach (var iHierarchyComponent in iAllowedRole.HierarchyComponentList)
                            j.MdHierarchyComponentReducedList.Add(new MdHierarchyComponentReduced { H = iHierarchyComponent.HierarchyId, T = iHierarchyComponent.ComponentTypeId, C = iHierarchyComponent.ComponentId });

                        mdReduced.HierarchyComponentReducedJoinList.Add(j);
                    }
                }

            //Feature
            if (AllowedModuleFeatureList != null)
                foreach (var iAllowedFeature in AllowedModuleFeatureList)
                {
                    //Hierarchy
                    int? hierarchyComponentId = null;
                    foreach (var iKeyValue in dictHierarchy)
                        if (EnumerableUtil.ScrambledEquals(iKeyValue.Value, iAllowedFeature.HierarchyComponentList))
                        {
                            hierarchyComponentId = iKeyValue.Key;
                            break;
                        }

                    if (hierarchyComponentId == null)
                    {
                        dictHierarchy.Add(iHierarchy, iAllowedFeature.HierarchyComponentList);
                        hierarchyComponentId = iHierarchy;
                        iHierarchy++;
                    }

                    //Feature
                    mdReduced.AllowedFeatureReducedList.Add(new MdAllowedFeatureReduced { M = iAllowedFeature.ModuleId, F = iAllowedFeature.FeatureId, N = iAllowedFeature.FeatureCodeName, J = iHierarchy });

                    if (!mdReduced.AllowedModuleReducedList.Any(x => x.M == iAllowedFeature.ModuleId))
                        mdReduced.AllowedModuleReducedList.Add(new MdAllowedModuleReduced { M = iAllowedFeature.ModuleId, N = iAllowedFeature.ModuleCodeName });

                    if (!mdReduced.HierarchyComponentReducedJoinList.Any(x => x.J == iHierarchy))
                    {
                        var j = new MdHierarchyComponentReducedJoin();
                        j.J = iHierarchy;
                        j.MdHierarchyComponentReducedList = new List<MdHierarchyComponentReduced>();

                        foreach (var iHierarchyComponent in iAllowedFeature.HierarchyComponentList)
                            j.MdHierarchyComponentReducedList.Add(new MdHierarchyComponentReduced { H = iHierarchyComponent.HierarchyId, T = iHierarchyComponent.ComponentTypeId, C = iHierarchyComponent.ComponentId });

                        mdReduced.HierarchyComponentReducedJoinList.Add(j);
                    }
                }

            return mdReduced;
        }
    }
}
