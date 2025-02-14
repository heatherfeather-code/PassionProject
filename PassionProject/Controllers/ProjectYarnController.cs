using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;

namespace PassionProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectYarnController : Controller
	{
		private readonly IProjectYarnService _ProjectYarnService;

		//dependency injection of service interfaces
		public ProjectYarnController(IProjectYarnService ProjectYarnService)
		{
			_ProjectYarnService = ProjectYarnService;
		}

		/// <summary>
		/// Returns a list of Project Yarn Bridge
		/// </summary>
		/// <returns>
		/// 200 Ok
		/// </returns>
		/// <example>
		/// GET: api/ProjectYarn/List ->[{ProjectYarnDto}, {ProjectYarnDto},..]
		/// </example>
		[HttpGet(template: "List")]
		public async Task<ActionResult<IEnumerable<ProjectYarnDto>>> ListProjectYarn()
		{
			IEnumerable<ProjectYarnDto> ProjectYarnDtos = await _ProjectYarnService.ListProjectYarn();
			return Ok (ProjectYarnDtos);
		}

		/// <summary>
		/// Returns a single Project Yarn specified by {id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 200 Ok
		/// {ProjectYarnDto}
		/// OR:
		/// 404 Not Found
		/// </returns>
		/// <example>
		/// GET: api/ProjectYarn/Find/1 -> {ProjectYarnDto}
		/// </example>
		[HttpGet(template: "Find/{id}")]
		public async Task<ActionResult<ProjectYarnDto>> FindProjectYarn(int id)
		{
			var ProjectYarn = await _ProjectYarnService.FindProjectYarn(id);

			//If the ProjectYarn couldnt be located return a 404 Error
			if (ProjectYarn == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(ProjectYarn);
			}
		}

		/// <summary>
		/// Updates a Project Yarn if necessary 
		/// (Note: This is also a bridge table and doesnt have a ton of content,
		/// but still an option if needed)
		/// </summary>
		/// <param name="id"></param>
		/// <param name="ProjectYarnDto"></param>
		/// <returns></returns>
		/// <example>
		/// PUT: api/ProjectYarn/Update/2
		/// Request Headers: Content-Type: application/json
		/// Request Body: {ProjectYarnDto}
		/// ->
		/// Response Code: 204 No Content
		/// </example>
		[HttpPut(template:"Update/{id}")]
		public async Task<ActionResult> UpdateProjectYarn(int id, ProjectYarnDto ProjectYarnDto)
		{
			//{id} in URL must match the id in the POST body
			if(id != ProjectYarnDto.ProjectYarnId)
			{
				return BadRequest();
			}

			ServiceResponse response = await _ProjectYarnService.UpdateProjectYarn(ProjectYarnDto);

			if(response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}
			// Status = Updated
			return NoContent();
		}

		/// <summary>
		/// Adds a ProjectYarn to the System
		/// </summary>
		/// <param name="ProjectYarnDto"></param>
		/// <returns>
		/// 201 Created -> api/ProjectYarn/Find/{ID}
		/// OR:
		/// 404 Not Found
		/// </returns>
		/// <example>
		/// POST: api/ProjectYarn/Add
		/// Request header: content-type: application/json
		/// request body: {ProjectYarnDto}
		/// </example>
		[HttpPost(template:"Add")]
		public async Task<ActionResult<ProjectYarnBridge>> AddProjectYarn(ProjectYarnDto ProjectYarnDto)
		{
			ServiceResponse response = await _ProjectYarnService.AddProjectYarn(ProjectYarnDto);

			if (response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}
			return Created($"api/ProjectYarn/FindProjectYarn/{response.CreatedId}", ProjectYarnDto);
		}

		
		/// <summary>
		/// Deletes an instance of Project Yarn
		/// </summary>
		/// <param name="id"> ID for the Project Yarn item to be delted.</param>
		/// <returns>
		/// 204 No Content 
		/// OR
		/// 404 Not Found
		/// </returns>
		/// <example>
		/// DELETE: api/ProjectYarn/Delete/1 -> Response Code: 204 No Content
		/// </example>
		[HttpDelete("Delete/{id}")]
		public async Task<ActionResult> DeleteProjectYarn(int id)
		{
			ServiceResponse response = await _ProjectYarnService.DeleteProjectYarn(id);

			if (response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error) 
			{
				return StatusCode(500, response.Messages);
			}
			return NoContent();
		}
	}	 
}
