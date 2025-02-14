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
using Microsoft.IdentityModel.Tokens;


namespace PassionProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class YarnController : ControllerBase
	{
		private readonly IYarnService _YarnService;
		//create IYarnService page

		//dependency injection of service interfaces
		public YarnController(IYarnService YarnService)
		{
			_YarnService = YarnService;
		}

		/// <summary>
		/// Returns a list of Yarn Items
		/// </summary>
		/// <returns>
		/// 200 OK
		/// </returns>
		/// <example>
		/// GET: api/Yarn/List -> [{YarnDto},{YarnDto},...]
		/// </example>
		[HttpGet(template: "List")]
		public async Task<ActionResult<IEnumerable<YarnDto>>> ListYarn()
		{
			//Empty list of data for DTO YarnDto
			IEnumerable<YarnDto> YarnDtos = await _YarnService.ListYarn();
			return Ok(YarnDtos);
		}

		/// <summary>
		/// Returns a single Yarn Item specified by the {id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// 200 Ok
		/// {YarnDto}
		/// or
		/// 404 Not Found
		/// </returns>
		/// <example>
		/// GET: api/Yarn/Find/1 -> {YarnDto}
		/// </example>
		[HttpGet(template:"Find/{id}")]

		public async Task<ActionResult<YarnDto>> FindYarn(int id)
		{
			var Yarn = await _YarnService.FindYarn(id);

			//if Yarn could not be located return a 404 error
			if (Yarn == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(Yarn);
			}

		}
		/// <summary>
		/// Updates a Yarn Item
		/// </summary>
		/// <param name="id"></param>
		/// <param name=""></param>
		/// <returns></returns>
		[HttpPut(template:"Update/{id}")]
		public async Task<ActionResult> UpdateYarn(int id, YarnDto YarnDto)
		{
			//{id} in URL must match the id in the POST body
			if(id != YarnDto.YarnId)
			{
				return BadRequest();
			}
			if (string.IsNullOrEmpty(YarnDto.DyeLot))
			{
				return BadRequest("DyeLot is required and cannot be null");
			}

			ServiceResponse response = await _YarnService.UpdateYarn(YarnDto);

			if (response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if (response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}

			//Status = Updated
			return NoContent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="YarnDto"></param>
		/// <returns></returns>
		[HttpPost(template:"Add")]
		public async Task<ActionResult<YarnItems>> AddYarnItem(YarnDto YarnDto)
		{
			ServiceResponse response = await _YarnService.AddYarn(YarnDto);

			if(response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if(response.Status == ServiceResponse.ServiceStatus.Error)
			{
				return StatusCode(500, response.Messages);
			}
			return Created($"api/Yarn/FindYarn/{response.CreatedId}", YarnDto);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("Delete/{id}")]
		public async Task<ActionResult> DeleteYarn(int id)
		{
			ServiceResponse response = await _YarnService.DeleteYarn(id);

			if (response.Status == ServiceResponse.ServiceStatus.NotFound)
			{
				return NotFound(response.Messages);
			}
			else if(response.Status == ServiceResponse.ServiceStatus.Error) 
			{
				return StatusCode(500, response.Messages);
			}
			return NoContent();

		}
	}
}
