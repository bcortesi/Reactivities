using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Content.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Content.Store.API.Services.Interfaces;
using Content.Store.API.Models;
using System.Net;

namespace Content.Store.API.Controllers
{
    [ApiController]
    [Route("contents/v1")]
    [Produces("application/json")]
    public class ContentController : ControllerBase
    {
        private readonly IContentStorageService _contentStorageService;

        public ContentController(IContentStorageService contentStorageService, IConfiguration configuration)
        {
            _contentStorageService = contentStorageService;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost()]
        [Route("Create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create(ContentModel model)
        {
            string recordId;
            HttpStatusCode statusCodeAWS;
            
            try
            {
                AWSResponse response = await _contentStorageService.AddAsync(model);
                recordId = response.Message;
                statusCodeAWS = response.Status;
                if (!statusCodeAWS.Equals(HttpStatusCode.OK))
                {
                   return StatusCode(500, response.Message);
                }
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
            return StatusCode(201, recordId);
        }

        [HttpPut()]
        [Route("Update")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(ContentModel model)
        {
            string recordId;
            HttpStatusCode statusCodeAWS;

            try
            {
                AWSResponse response = await _contentStorageService.UpdateAsync(model);
                recordId = response.Message;
                statusCodeAWS = response.Status;
                if (!statusCodeAWS.Equals(HttpStatusCode.OK))
                {
                    return StatusCode(500, response.Message);
                }
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
            return StatusCode(201, recordId);
        }

        [HttpGet()]
        [Route("GetContents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetContents()
        {
            try
            {                
                return Ok(await _contentStorageService.GetAllAsync());
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }            
        }

        [HttpGet()]
        [Route("GetContent/{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetContent(string Id)
        {
            try
            {
                return Ok(await _contentStorageService.GetAsync(Id));
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}