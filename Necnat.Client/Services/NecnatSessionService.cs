using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Necnat.Client.Interfaces;
using Necnat.Shared.HttpClients.NamHttpClients;
using Necnat.Shared.Models;
using Necnat.Shared.Settings;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.Client.Services
{
    public class NecnatSessionService : INecnatSessionService
    {
        protected ILocalStorageService _localStorage;
        protected NamHttpClient _namHttpClient;
        protected NecnatClientSettings _necnatClientSettings;
        protected NavigationManager _navigationManager;

        public NecnatSessionService(ILocalStorageService localStorage, NamHttpClient namHttpClient, NecnatClientSettings necnatClientSettings, NavigationManager navigationManager)
        {
            _localStorage = localStorage;
            _necnatClientSettings = necnatClientSettings;
            _namHttpClient = namHttpClient;
            _navigationManager = navigationManager;
        }

        public async Task<MdUserAuthorizationReduced> GetUserAuthorizationAsync(int? applicationId = null)
        {
            applicationId = applicationId ?? _necnatClientSettings.ApplicationId;

            if (!await _localStorage.ContainKeyAsync(_necnatClientSettings.UserAuthorizationName + ":" + applicationId))
            {
                var r = await _namHttpClient.AccessManagementAuthorizationAGetUserAuthorizationByApplicationId(_necnatClientSettings.AuthorizationApiEndPoint, (int)applicationId);
                if (r.IsSuccessStatusCode)
                    await _localStorage.SetItemAsync(_necnatClientSettings.UserAuthorizationName + ":" + applicationId, JsonConvert.DeserializeObject<MdUserAuthorizationReduced>(await r.Content.ReadAsStringAsync()));
                else
                {
                    await RemoveSessionAsync();
                    _navigationManager.NavigateTo("/MdlAccessManagement/Unauthorized?reason=CannotGetUserAuthorization");
                    return null;
                }
            }

            if (await _localStorage.ContainKeyAsync(_necnatClientSettings.SessionGroupName))
            {
                var dateTimeSessionExpired = await _localStorage.GetItemAsync<DateTime>(_necnatClientSettings.SessionGroupName);
                if (dateTimeSessionExpired < DateTime.Now)
                {
                    await RemoveUserAuthorizationAsync(applicationId);
                    await RemoveSessionAsync();
                    _navigationManager.NavigateTo("/MdlAccessManagement/Logout");
                    return null;
                }
            }
            await _localStorage.SetItemAsync(_necnatClientSettings.SessionGroupName, DateTime.Now.Add(_necnatClientSettings.SessionGroupLifeTime));

            return await _localStorage.GetItemAsync<MdUserAuthorizationReduced>(_necnatClientSettings.UserAuthorizationName + ":" + applicationId);
        }

        public async Task RemoveUserAuthorizationAsync(int? applicationId = null)
        {
            applicationId = applicationId ?? _necnatClientSettings.ApplicationId;
            await _localStorage.RemoveItemAsync(_necnatClientSettings.UserAuthorizationName + ":" + applicationId);
        }

        public async Task RemoveSessionAsync()
        {
            await _localStorage.RemoveItemAsync(_necnatClientSettings.SessionGroupName);
        }

        public bool HasRole(MdUserAuthorizationReduced userAuthorizationReduced, string roleCodeName)
        {
            return userAuthorizationReduced.AllowedRoleReducedList.Any(x => x.N == roleCodeName);
        }

        public bool HasRole(MdUserAuthorizationReduced userAuthorizationReduced, string roleCodeName, int componentTypeId, int componentId)
        {
            var n = userAuthorizationReduced.AllowedRoleReducedList.Where(x => x.N == roleCodeName).FirstOrDefault();
            if (n == null)
                return false;

            var j = userAuthorizationReduced.HierarchyComponentReducedJoinList.Where(x => x.J == n.J).FirstOrDefault();
            if (j == null)
                return false;

            return j.MdHierarchyComponentReducedList.Any(x =>
                    x.T == componentTypeId
                    && x.C == componentId);
        }

        public bool HasModuleFeature(MdUserAuthorizationReduced userAuthorizationReduced, string moduleCodeName, string featureCodeName)
        {
            var m = userAuthorizationReduced.AllowedModuleReducedList.Where(x => x.N == moduleCodeName).FirstOrDefault();
            if (m == null)
                return false;

            return userAuthorizationReduced.AllowedFeatureReducedList.Any(x => x.M == m.M && x.N == featureCodeName);
        }

        public bool HasModuleFeature(MdUserAuthorizationReduced userAuthorizationReduced, string moduleCodeName, string featureCodeName, int componentTypeId, int componentId)
        {
            var m = userAuthorizationReduced.AllowedModuleReducedList.Where(x => x.N == moduleCodeName).FirstOrDefault();
            if (m == null)
                return false;

            var n = userAuthorizationReduced.AllowedFeatureReducedList.Where(x => x.M == m.M && x.N == featureCodeName).FirstOrDefault();
            if (n == null)
                return false;

            var j = userAuthorizationReduced.HierarchyComponentReducedJoinList.Where(x => x.J == n.J).FirstOrDefault();
            if (j == null)
                return false;

            return j.MdHierarchyComponentReducedList.Any(x =>
                    x.T == componentTypeId
                    && x.C == componentId);
        }
    }
}
