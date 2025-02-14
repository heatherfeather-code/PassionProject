using PassionProject.Models;
using PassionProject.Services;

namespace PassionProject.Interfaces
{
	public interface IProjectService
	{
		//base CRUD
		Task<IEnumerable<CraftProjectDto>> ListProjects();

		Task<CraftProjectDto?> FindProject(int id);

		Task<ServiceResponse> UpdateProject(CraftProjectDto CraftProjectDto);

		Task<ServiceResponse> AddProject(CraftProjectDto CraftprojectDto);

		Task<ServiceResponse> DeleteProject(int id);

	
	}


}
