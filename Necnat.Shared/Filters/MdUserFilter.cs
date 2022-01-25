using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
	public class MdUserFilter : MdFilter<string>
	{
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
	}
}
