using Necnat.AccessManagement.Server.Interfaces;
using Necnat.Shared.Domains;
using Necnat.Shared.Filters;
using Necnat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Server.Data.Services.Modules.MdlAccessManagement
{
    public class UserService : INecnatAccessManagementService
    {
        public List<MdUser> GetAll()
        {
            var l = new List<MdUser>();

            var e = new MdUser();
            e.Id = "1";
            e.Name = "alice";
            l.Add(e);

            e = new MdUser();
            e.Id = "11";
            e.Name = "bob";
            l.Add(e);

            return l;
        }

        public MdUser GetById(string id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public MdFilterObject Filter(MdUserFilter filter, bool isSupport = false)
        {
            var q = GetAll()
                .Where(x => ((string.IsNullOrWhiteSpace(filter.NameFilter) && (filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS))
                        || (string.IsNullOrWhiteSpace(filter.NameFilter) && x.Name == null)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_EQUALS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_EQUALS) && x.Name == filter.NameFilter)
                        || ((filter.NameFilterType == (int)EnFilterType.CONSIDER_NULL_CONTAINS || filter.NameFilterType == (int)EnFilterType.DISREGARD_NULL_CONTAINS) && x.Name.Contains(filter.NameFilter)))
                    && (filter.WithoutIdList == null || filter.WithoutIdList.Count < 1 || !filter.WithoutIdList.Contains(x.Id))
                );

            if (isSupport)
                q = q.Select(x => new MdUser { Id = x.Id, Name = x.Name });

            if ((!string.IsNullOrWhiteSpace(filter.OrderBy)) && filter.OrderBy.EndsWith("asc"))
                q = q.OrderBy(filter.OrderBy.Split(' ')[0]);
            else if ((!string.IsNullOrWhiteSpace(filter.OrderBy)) && filter.OrderBy.EndsWith("desc"))
                q = q.OrderByDescending(filter.OrderBy.Split(' ')[0]);
            else
                q = q.OrderBy(x => x.Id);

            var op = new MdFilterObject();

            if (filter.IsPaging)
            {
                op.Total = q.Count();
                q = q.Skip(filter.Skip).Take(filter.Take);
            }

            var lData = q.ToList();

            if (!filter.IsPaging)
                op.Total = lData.Count;

            op.Data = lData;
            return op;
        }
    }
}
