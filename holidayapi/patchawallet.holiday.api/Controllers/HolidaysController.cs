using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/holidays")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidaysController(IHolidayService service)
        {
            _service = service;
        }

        [HttpGet("{pageIndex}/{pageSize}")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(List<HolidayVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10, string search = "")
        {
            try
            {
                var result = await _service.GetAllAsync(pageIndex, pageSize, search);
                return Ok(result);
            }
            catch (QueryHolidayException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(HolidayVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (QueryHolidayException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(HolidayVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(HolidayInserOrUpdateVM request)
        {
            try
            {
                var result = await _service.PostAsync(request);
                return Ok(result);
            }
            catch (HolidayAlreadyCadastredException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PostHolidayException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(HolidayVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put(HolidayInserOrUpdateVM request)
        {
            try
            {
                var result = await _service.UpdateAsync(request);
                return Ok(result);
            }
            catch (HolidayAlreadyCadastredException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PutHolidayException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(List<HolidayVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(result);
            }
            catch (QueryHolidayException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
