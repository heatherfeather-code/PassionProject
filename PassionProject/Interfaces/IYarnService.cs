using PassionProject.Models;
using PassionProject.Services;
namespace PassionProject.Interfaces
{
	public interface IYarnService
	{
		//base CRUD

		Task<IEnumerable<YarnDto>> ListYarn();
		Task<YarnDto?> FindYarn(int id);

		Task<ServiceResponse> UpdateYarn(YarnDto YarnDto);

		Task<ServiceResponse> AddYarn(YarnDto YarnDto);

		Task<ServiceResponse> DeleteYarn (int id);


	}
}
