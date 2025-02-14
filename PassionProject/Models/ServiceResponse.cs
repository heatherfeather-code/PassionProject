namespace PassionProject.Services
{
	public class ServiceResponse
	{
		//The type of response for direct data manipulations (Create,Delete,Update)

		public enum ServiceStatus { NotFound, Created, Updated, Deleted, Error }

		public ServiceStatus Status { get; set; }

		public int CreatedId { get; set; }

		//ServiceResponse packages gives more infomrmation. ie logic and validation errors. 
		public List<string> Messages { get; set; } = new List<string>();
	}
}
