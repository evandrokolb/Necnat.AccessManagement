using System.Collections.Generic;
using System.Linq;

namespace Necnat.Shared.Domains
{
    public enum EnFilterType
	{
		CONSIDER_NULL_EQUALS = 1,
		CONSIDER_NULL_CONTAINS = 2,
		DISREGARD_NULL_EQUALS = 3,
		DISREGARD_NULL_CONTAINS = 4
	}

	public class FilterType
	{
		public int? Id { get; set; }
		public string Name { get; set; }
	}

	public static class FilterTypeDomain
	{
		public static List<FilterType> GetAll()
		{
			var l = new List<FilterType>();

			var e = new FilterType();
			e.Id = 1;
			e.Name = "Consider Null Equals";
			l.Add(e);

			e = new FilterType();
			e.Id = 2;
			e.Name = "Consider Null Constains";
			l.Add(e);

			e = new FilterType();
			e.Id = 3;
			e.Name = "Disregard Null Equals";
			l.Add(e);

			e = new FilterType();
			e.Id = 4;
			e.Name = "Disregard Null Constains";
			l.Add(e);

			return l;
		}

		public static List<FilterType> GetAllWithSelect(string selectName = "Select")
		{
			var l = GetAll();
			l.Add(new FilterType { Id = null, Name = selectName });

			return l;
		}

		public static FilterType GetById(int id)
		{
			return GetAll().First(x => x.Id == id);
		}

		public static bool ValidId(int id)
		{
			return GetAll().Any(x => x.Id == id);
		}

		public static FilterType GetByName(string name)
		{
			return GetAll().First(x => x.Name == name);
		}

		public static bool ValidName(string name)
		{
			return GetAll().Any(x => x.Name == name);
		}
	}
}
