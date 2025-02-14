using Microsoft.EntityFrameworkCore;
using PassionProject.Interfaces;
using PassionProject.Models;
using Microsoft.CodeAnalysis;

namespace PassionProject.Services
{
	public class YarnService : IYarnService
	{
		private readonly AppDbContext _context;

		public YarnService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<YarnDto>> ListYarn()
		{
			//all projects
			List<YarnItems> YarnItems = await _context.YarnItems.ToListAsync();
			//Empty Dto for Yarn
			List<YarnDto> YarnDtos = new List<YarnDto>();
			foreach (YarnItems Yarn in YarnItems)
			{
				YarnDtos.Add(new YarnDto()
				{
					YarnId = Yarn.YarnId,
					YarnName = Yarn.YarnName,
					Weight = Yarn.Weight,
					Material = Yarn.Material,
					Colour = Yarn.Colour,
					DyeLot = Yarn.DyeLot

				});
			}
			return YarnDtos;
		}

		public async Task<YarnDto?> FindYarn(int id)
		{
			//include yarn item with one project
			var Yarn = await _context.YarnItems.FirstOrDefaultAsync(y => y.YarnId == id);

			//no project found
			if (Yarn == null)
			{
				return null;
			}

			//Create instance of YarnDto
			YarnDto YarnDto = new YarnDto()
			{
				YarnId = Yarn.YarnId,
				YarnName = Yarn.YarnName,
				Weight = Yarn.Weight,
				Material = Yarn.Material,
				Colour = Yarn.Colour,
				DyeLot = Yarn.DyeLot
			};
			return YarnDto;
		}
	

	//add in update option
		public async Task<ServiceResponse> UpdateYarn(YarnDto YarnDto)
		{
			ServiceResponse serviceResponse = new();

			//create an instance of yarn
			YarnItems Yarn = new YarnItems()
			{
				YarnId = YarnDto.YarnId,
				YarnName = YarnDto.YarnName,
				Weight = YarnDto.Weight,
				Material = YarnDto.Material,
				Colour = YarnDto.Colour,
				DyeLot = YarnDto.DyeLot
			};

			//flags that the item has been modified
			_context.Entry(Yarn).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
				serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
			}
			catch (DbUpdateConcurrencyException) 
			{
				serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
				serviceResponse.Messages.Add("An error occured while updating the record.");
			}

			return serviceResponse;
		}

		//Add in AddYarn
		public async Task<ServiceResponse> AddYarn(YarnDto YarnDto)
		{
			ServiceResponse serviceResponse = new();

			//create instance of new yarn
			YarnItems Yarn = new YarnItems()
			{
				YarnId = YarnDto.YarnId,
				YarnName = YarnDto.YarnName,
				Weight = YarnDto.Weight,
				Material = YarnDto.Material,
				Colour = YarnDto.Colour,
				DyeLot= YarnDto.DyeLot
			};

			try
			{
				_context.YarnItems.Add(Yarn);
				await _context.SaveChangesAsync();

				serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
				serviceResponse.CreatedId = Yarn.YarnId;
				
			}
			catch (Exception ex) 
			{
				serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
				serviceResponse.Messages.Add("There was an error adding to Yarn Items.");
				serviceResponse.Messages.Add(ex.Message);
			}
			return serviceResponse;
		}


		//Delete a Yarn from the db
		public async Task<ServiceResponse> DeleteYarn(int id)
		{
			ServiceResponse response = new();
			//Yarn Item must exist first!
			var Yarn = await _context.YarnItems.FindAsync(id);
			if (Yarn == null) 
			{
				response.Status = ServiceResponse.ServiceStatus.NotFound;
				response.Messages.Add("Yarn cannot be deleted because it does not exist.");

				return response;
			}

			try
			{
				_context.YarnItems.Remove(Yarn);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.Status = ServiceResponse.ServiceStatus.Error;
				response.Messages.Add("Error encountered while deleting yarn item.");
				return response;
			}
			response.Status = ServiceResponse.ServiceStatus.Deleted;

			return response;
		}
	}
}
