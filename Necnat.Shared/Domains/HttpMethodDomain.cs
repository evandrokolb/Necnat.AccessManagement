using System.Collections.Generic;
using System.Linq;

namespace Necnat.Shared.Domains
{
    public enum EnHttpMethod
	{
		GET = 1,
		POST = 2,
		PUT = 3,
		DELETE = 4,
		HEAD = 5,
		OPTIONS = 6,
		PATCH = 7
	}

	public class HttpMethod
	{
		public int? Id { get; set; }
		public string Name { get; set; }
	}

	public static class HttpMethodDomain
	{
		public static List<HttpMethod> GetAll()
		{
			var l = new List<HttpMethod>();

			var e = new HttpMethod();
			e.Id = 1;
			e.Name = "GET";
			l.Add(e);

			e = new HttpMethod();
			e.Id = 2;
			e.Name = "POST";
			l.Add(e);

			e = new HttpMethod();
			e.Id = 3;
			e.Name = "PUT";
			l.Add(e);

			e = new HttpMethod();
			e.Id = 4;
			e.Name = "DELETE";
			l.Add(e);

			return l;
		}

		public static List<HttpMethod> GetAllWithSelect(string selectName = "Select")
		{
			var l = GetAll();
			l.Add(new HttpMethod { Id = null, Name = selectName });

			return l;
		}

		public static HttpMethod GetById(int id)
		{
			return GetAll().First(x => x.Id == id);
		}

		public static bool ValidId(int id)
		{
			return GetAll().Any(x => x.Id == id);
		}

		public static HttpMethod GetByName(string name)
		{
			return GetAll().First(x => x.Name == name);
		}

		public static bool ValidName(string name)
		{
			return GetAll().Any(x => x.Name == name);
		}
	}
}
