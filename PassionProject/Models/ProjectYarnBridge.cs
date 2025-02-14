using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PassionProject.Models;

namespace PassionProject.Models
{
	public class ProjectYarnBridge
	{
		[Key]
		public int ProjectYarnId { get; set; }


		//foreign key
		[ForeignKey("Projects")]
		public int ProjectId { get; set; }

		[ForeignKey("Yarn")]
		public int YarnId { get; set; } 
		public float QuantityUsed { get; set; }

		
		public virtual Projects Projects { get; set; }
	}

	public class ProjectYarnDto
	{
		//Data Transfer Object
		public int ProjectYarnId { get; set; }

		public int ProjectId { get; set; }

		public int YarnId { get; set; }

		public float QuantityUsed { get; set; }

		public virtual Projects Projects { get; set; }
	}
}
