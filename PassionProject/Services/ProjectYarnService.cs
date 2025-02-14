using PassionProject.Interfaces;
using PassionProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace PassionProject.Services
{
	public class ProjectYarnService :IProjectYarnService
	{
		private readonly AppDbContext _context;

		public ProjectYarnService (AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProjectYarnDto>> ListProjectYarn()
		{
			//All Project Yarn
			List<ProjectYarnBridge> ProjectYarn = await _context.ProjectYarn.ToListAsync();
			List<ProjectYarnDto> ProjectYarnDtos = new List<ProjectYarnDto>();
			foreach(ProjectYarnBridge ProjectYarnBridge in ProjectYarn)
			{
				//creates a new instance of ProjectYarn Dto and add to list. 
				ProjectYarnDtos.Add(new ProjectYarnDto()
				{
					ProjectYarnId = ProjectYarnBridge.ProjectYarnId,
					ProjectId = ProjectYarnBridge.ProjectId,
					YarnId = ProjectYarnBridge.YarnId,
					QuantityUsed = ProjectYarnBridge.QuantityUsed,
					//Projects = ProjectYarnBridge.Projects
				});
			}
			return ProjectYarnDtos;
		}

		public async Task<ProjectYarnDto?> FindProjectYarn(int id)
		{
			//include joins projectyarn with project and yarn
			var ProjectYarn = await _context.ProjectYarn.FirstOrDefaultAsync(pj => pj.ProjectId == id);

			// no project found
			if (ProjectYarn == null)
			{
				return null;
			}
			//create instance of CraftProjectDto
			ProjectYarnDto ProjectYarnDto = new ProjectYarnDto()
			{
				ProjectYarnId = ProjectYarn.ProjectYarnId,
				ProjectId = ProjectYarn.ProjectId,
				YarnId = ProjectYarn.YarnId,
				QuantityUsed = ProjectYarn.QuantityUsed,
				//Projects = ProjectYarn.Projects

			};
			return ProjectYarnDto;
		}

		//Update ProjectYarn
		public async Task<ServiceResponse> UpdateProjectYarn(ProjectYarnDto ProjectYarnDto)
		{
			ServiceResponse serviceResponse = new();

			//create instance of project
			ProjectYarnBridge ProjectYarn = new ProjectYarnBridge()
			{
				ProjectYarnId = ProjectYarnDto.ProjectYarnId,
				ProjectId = ProjectYarnDto.ProjectId,
				YarnId = ProjectYarnDto.YarnId,
				QuantityUsed = ProjectYarnDto.QuantityUsed,
				//Projects = ProjectYarnDto.Projects
			};
			//flags that the object has changed
			_context.Entry(ProjectYarn).State = EntityState.Modified;

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

		//Add in AddProjectYarn

		public async Task<ServiceResponse> AddProjectYarn(ProjectYarnDto ProjectYarnDto)
		{
			ServiceResponse serviceResponse = new();

			//create instance of ProjectYarn
			ProjectYarnBridge ProjectYarn = new ProjectYarnBridge()
			{
				ProjectYarnId = ProjectYarnDto.ProjectYarnId,
				ProjectId = ProjectYarnDto.ProjectId,
				YarnId = ProjectYarnDto.YarnId,
				QuantityUsed = ProjectYarnDto.QuantityUsed,
				//Projects = ProjectYarnDto.Projects
			};

			try
			{
				_context.ProjectYarn.Add(ProjectYarn);
				await _context.SaveChangesAsync();

				serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
				serviceResponse.CreatedId = ProjectYarn.ProjectYarnId;
			}
			catch (Exception ex)
			{
				serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
				serviceResponse.Messages.Add("There was an error adding to the Project Yarn Table.");
				serviceResponse.Messages.Add(ex.Message);
			}

			return serviceResponse;
		}

		//add Delete ProjectYarn
		public async Task<ServiceResponse> DeleteProjectYarn(int id)
		{
			ServiceResponse response = new();
			//Project item must exist first
			var ProjectYarns = await _context.ProjectYarn.FindAsync(id);
			if (ProjectYarns == null)
			{
				response.Status = ServiceResponse.ServiceStatus.NotFound;
				response.Messages.Add("Project Yarn cannot be deleted because it does not exist.");
				return response;
			}

			try
			{
				_context.ProjectYarn.Remove(ProjectYarns);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.Status = ServiceResponse.ServiceStatus.Error;
				response.Messages.Add("Error encountered while deleting the Project Yarn info.");
				return response;
			}
			response.Status = ServiceResponse.ServiceStatus.Deleted;

			return response;
		}


	}
}
