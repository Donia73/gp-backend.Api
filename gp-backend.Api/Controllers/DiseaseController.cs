using gp_backend.Api.Dtos;
using gp_backend.Api.Dtos.Disease;
using gp_backend.Api.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gp_backend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly IGenericRepo<Disease> _diseaseRepo;
        private readonly ILogger<DiseaseController> _logger;
        public DiseaseController(IGenericRepo<Disease> diseaseRepo, ILogger<DiseaseController> logger,
        UserManager<ApplicationUser> userManager)
        {
            _diseaseRepo = diseaseRepo;
            _logger = logger;
        }

        // Add
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddDiseaseDto model)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }
            var result = new Disease {
                Description = model.Description,
                Name = model.Name,
                Preventions = model.Preventions,
                Risk = model.Risk
            };
            var resultadd = await _diseaseRepo.InsertAsync(result);
            await _diseaseRepo.SaveAsync();

            return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetDiseaseDetailsDto
            {
                Id = resultadd.Id,
                Description = resultadd.Description,
                Name = resultadd.Name,
                Preventions = resultadd.Preventions,
                Risk = resultadd.Risk
            }));
        }

        [HttpGet("get-id")]
        public async Task<IActionResult> GetById(int id)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }
            if(id > 0)
            {
                var result = await _diseaseRepo.GetByIdAsync(id);
                if (result != null)
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetDiseaseDetailsDto
                    {
                        Id = id,
                        Description = result.Description,
                        Name = result.Name,
                        Preventions = result.Preventions,
                        Risk = result.Risk
                    }));
                else
                    return NotFound();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _diseaseRepo.GetAllAsync("");
            var diseases = new List<GetAllDiseaseDto>();

            foreach(var dis in result)
            {
                diseases.Add(new GetAllDiseaseDto { Id = dis.Id, Name = dis.Name });
            }

            return Ok(new BaseResponse(true, new List<string> { "Success" }, diseases));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("dis-delete")]
        public async Task<IActionResult> Delete(int id)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }
            if (id > 0)
            {
                await _diseaseRepo.DeleteAsync(id);
                await _diseaseRepo.SaveAsync();
                return NoContent();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }
    }
}
