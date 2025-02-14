using PassionProject.Interfaces;
//using PassionProject.Migrations;
using PassionProject.Models;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using Microsoft.CodeAnalysis;



namespace PassionProject.Services
{
	public class ProjectService : IProjectService 
	{
		private readonly AppDbContext _context;

		public ProjectService (AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CraftProjectDto>> ListProjects()
		{
			//all Projects
			List<Projects> Projects = await _context.Projects.ToListAsync();
			//empty list of data transfer object ProjectDto
			List<CraftProjectDto> ProjectDtos = new List<CraftProjectDto>();
			foreach (Projects Project in Projects)
			{
				//Create new instance of Project Dto, add to list
				ProjectDtos.Add(new CraftProjectDto()
				{
					ProjectId = Project.ProjectId,
					ProjectName = Project.ProjectName,
					CraftType = Project.CraftType,

				});
			}
			return ProjectDtos;
		}
		public async Task<CraftProjectDto?> FindProject (int id)
		{
			//include will join project item with one project, yarn and project yarn
			var Project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);

			// no project found
			if (Project == null) 
			{
				return null;
			}
			//create instance of CraftProjectDto
			CraftProjectDto CraftProjectDto = new CraftProjectDto()
			{
				ProjectId = Project.ProjectId,
				ProjectName = Project.ProjectName,
				CraftType = Project.CraftType,
				Source = Project.Source,
				Materials = Project.Materials,

			};
			return CraftProjectDto;
		}

		//Add in update
		public async Task<ServiceResponse> UpdateProject(CraftProjectDto CraftProjectDto)
		{
			ServiceResponse serviceResponse = new();

			//Create Instance of Project
			Projects Project = new Projects()
			{
				ProjectId = CraftProjectDto.ProjectId,
				ProjectName = CraftProjectDto.ProjectName,
				CraftType = CraftProjectDto.CraftType,
				Source = CraftProjectDto.Source,
				Status = CraftProjectDto.Status,
				Materials = CraftProjectDto.Materials,
				Description = CraftProjectDto.Description
			};

			//flags that the object has changed
			_context.Entry(Project).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
			}
			catch (DbUpdateConcurrencyException)
			{
				serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
				serviceResponse.Messages.Add("An Error occured while updating the record.");
			}

			return serviceResponse;
		}
		//add in addProject
		public async Task<ServiceResponse> AddProject(CraftProjectDto CraftProjectDto)
		{
			ServiceResponse serviceResponse = new();

			//create instance of Project
			Projects Project = new Projects()
			{
				ProjectName = CraftProjectDto.ProjectName,
				CraftType = CraftProjectDto.CraftType,
				Source = CraftProjectDto.Source,
				Status = CraftProjectDto.Status,
				Materials = CraftProjectDto.Materials,
				Description = CraftProjectDto.Description
			};

			try
			{
				_context.Projects.Add(Project);
				await _context.SaveChangesAsync();

				serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
				serviceResponse.CreatedId = Project.ProjectId;
			}
			catch (Exception ex)
			{
				serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
				serviceResponse.Messages.Add("There was an error adding the Project.");
				serviceResponse.Messages.Add(ex.Message);
			}

			return serviceResponse;
		}

		//add Delete Project
		public async Task<ServiceResponse> DeleteProject(int id)
		{
			ServiceResponse response = new();
			//Project item must exist first
			var Project = await _context.Projects.FindAsync(id);
			if (Project == null)
			{
				response.Status = ServiceResponse.ServiceStatus.NotFound;
				response.Messages.Add("Project cannot be deleted because it does not exist.");
				return response;
			}

			try
			{
				_context.Projects.Remove(Project);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.Status = ServiceResponse.ServiceStatus.Error;
				response.Messages.Add("Error encountered while deleting the project.");
				return response;
			}
			response.Status = ServiceResponse.ServiceStatus.Deleted;

			return response;
		}

	
	}
}
