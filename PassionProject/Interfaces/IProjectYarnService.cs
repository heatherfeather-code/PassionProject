using PassionProject.Models;
using PassionProject.Services;
namespace PassionProject.Interfaces
{
	public interface IProjectYarnService
	{
		// Base CRUD
		Task<IEnumerable<ProjectYarnDto>> ListProjectYarn();

		Task<ProjectYarnDto?> FindProjectYarn(int id);

		Task<ServiceResponse> UpdateProjectYarn(ProjectYarnDto ProjectYarnDto);

		Task<ServiceResponse> AddProjectYarn(ProjectYarnDto ProjectYarnDto);

		Task<ServiceResponse> DeleteProjectYarn(int id);

	}
}
