using System.ComponentModel.DataAnnotations;
namespace PassionProject.Models
{
	public class YarnItems
	{
		[Key]
		public int YarnId { get; set; }

		public required string YarnName { get; set; }

		public required string Material {  get; set; }

		public required string Weight { get; set; }

		public required string Colour { get; set; }

		public string? DyeLot { get; set; }

	}

	public class YarnDto
	{
		//Data Transfer Object for Yarn
		public int YarnId { get; set; }
		public required string YarnName { get; set; }
		public string? Material { get; set; }

		public required string Weight { get; set; }

		public required string Colour { get; set; }

		public string? DyeLot { get; set; }




	}
}
