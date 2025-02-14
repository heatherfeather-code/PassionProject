using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;
//using PassionProject.DTOs;


namespace PassionProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly IProjectService _ProjectService;

		//dependency injection of service interfaces
		public ProjectController(IProjectService ProjectService)
		{
			_ProjectService = ProjectService;
		}

		/// <summary>
		/// Returns a list of Projects
		/// </summary>
		/// <returns>
		/// 200 OK
		/// </returns>
		/// <example>
		/// GET: api/Project/List -> [{ProjectDto}, {ProjectDto},...]
		/// </example>
		[HttpGet(template: "List")]
		public async Task<ActionResult<IEnumerable<CraftProjectDto>>> ListProjects()
		{
			//empty list of data transfer object CraftProjectDto
			IEnumerable<CraftProjectDto> CraftProjectDtos = await _ProjectService.ListProjects();

			return Ok(CraftProjectDtos);
		}

		/// <summary>
		/// Returns a single Project specified by the {id}
		/// </summary>
		/// <param name="id"> The Project Id</param>
		/// <returns>
		/// 200 Ok
		/// {CraftProjectDto}
		/// or
		/// 404 Not Found
		/// </returns>
		/// <example>
		/// GET: api/Project/Find/1 -> {ProjectDto}
		/// </example>
		[HttpGet(template: "Find/{id}")]
		public async Task<ActionResult<CraftProjectDto>> FindProject(int id)
		{
			var Project = await _ProjectService.FindProject(id);

			//If the Project couldn't be located return a 404 error
			if (Project == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(Project);
			}
		}
		/// <summary>
		/// Updates a Project
		/// </summary>
		/// <param name = "id" > The Id of the Project to Update</param>
		/// <param name = "CraftProjectDto" > The required information to update the Project (Materials, Status..etc)</param>
		/// <returns>
		/// 400 Bad request
		/// or
		/// 404 Not Found
		/// or
		/// 204 No Content
		/// </returns>
		/// <example>
		/// PUT: api/Project/Update/2
		/// Request Headers: Content-Type: application/json
		/// Request Body: { CraftProjectDto}
		/// ->
		/// Response Code: 204 No Content
		/// </example>
		[HttpPut(template: "Update/{id}")]
		public async Task<ActionResult> UpdateProject(int id, CraftProjectDto CraftProjectDto)
		{
			//{id} in URL must match the id in the POST body
			if (id != CraftProjectDto.ProjectId)
			{
				return BadRequest();
			}
			ServiceResponse response = await _ProjectService.UpdateProject(CraftProjectDto);

			if (response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}

			//Status = updated
			return NoContent();
		}

		/// <summary>
		/// Adds a Project to the system
		/// </summary>
		/// <param name="CraftProjectDto"></param>
		/// <returns>
		/// 201 created
		/// Location: api/Project/Find/{ProjectId}
		/// OR
		/// 404 Not found
		/// </returns>
		/// <example>
		/// POST: api/Project/Add
		/// Request Headers: Content-type: application/json
		/// Request Body: {CraftProjectDto}
		/// </example>
		[HttpPost(template:"Add")]
		public async Task<ActionResult<Projects>> AddProject(CraftProjectDto CraftProjectDto)
		{
			ServiceResponse response = await _ProjectService.AddProject(CraftProjectDto);

			if(response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}
			return Created($"api/Project/FindProject/{response.CreatedId}",CraftProjectDto);
		}


		/// <summary>
		/// Deletes a Project
		/// </summary>
		/// <param name="id"> The id of the project to be deleted </param>
		/// <returns>
		/// 204 No Content
		/// Or:
		/// 404 Not Found
		/// </returns>
		///<example>
		///DELETE: api/Project/Delete/10 -> Response Code: 204 No Content
		/// </example> 

		[HttpDelete("Delete/{id}")]
		public async Task<ActionResult> DeleteProject(int id)
		{
			ServiceResponse response = await _ProjectService.DeleteProject(id);

			if(response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound();
			}
			else if(response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}
			return NoContent();
		}
	}
}
