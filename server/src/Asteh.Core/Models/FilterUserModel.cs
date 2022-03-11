using System.ComponentModel.DataAnnotations;

namespace Asteh.Core.Models
{
	public class FilterUserModel
	{
		public string? Name { get; set; }
		public string? TypeName { get; set; }
		public string? BeginDate { get; set; }
		public string? EndDate { get; set; }
	}
}
