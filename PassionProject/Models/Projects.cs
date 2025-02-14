using System.ComponentModel.DataAnnotations;
namespace PassionProject.Models

{
	public class Projects
	{
		[Key]
		public int ProjectId { get; set; }

		public required string ProjectName { get; set; }

		public required string Source { get; set; }

		public required string CraftType { get; set; }

		public DateOnly DateCreated { get; set; }

		public required string Status { get; set; }

		public required string Description { get; set; }

		public required string Materials { get; set; }
	}
	public class CraftProjectDto
	{
		//Data Transfer Object
		public int ProjectId { get; set; }
		public string? ProjectName { get; set; }
		public string? CraftType { get; set; }
		public string? Source { get; set; }
		public string? Status { get; set; }
		public string? Materials { get; set; }
		public string? Description { get; set; }
	}
}
