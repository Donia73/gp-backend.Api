using gp_backend.Api.Dtos;
using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using gp_backend.Api.Dtos.Wound;

namespace gp_backend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WoundController : ControllerBase
    {
        private readonly IGenericRepo<Wound> _woundRepo;
        private readonly ILogger<WoundController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepo<Disease> _diseaseRepo;
        public WoundController(IGenericRepo<Wound> woundRepo, ILogger<WoundController> logger,
        IGenericRepo<Disease> diseaseRepo, UserManager<ApplicationUser> userManager)
        {
            _woundRepo = woundRepo;
            _logger = logger;
            _userManager = userManager;
            _diseaseRepo = diseaseRepo;
        }

        // add
        [RequestSizeLimit(1000_000_000)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // extract the file description
                int wound_id = await CallFlaskEndPoint(file);
                string burnDegree = "";
                Disease diseasse = new();
                if (wound_id == 1)
                {
                    diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(d => d.Name == "first degree burn");
                }
                else if (wound_id == 2)
                {
                    diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(d => d.Name == "second degree burn");
                }
                else if (wound_id == 3)
                {
                    diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(d => d.Name == "third degree burn");
                }

                var fileDescription = GetDescription(file);
                var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

                var user = await _userManager.FindByIdAsync(uid);

                var wound = new Wound
                {
                    Type = "Type",
                    Location = "Location",
                    UploadDate = DateTime.Now.Date,
                    Advice = "Advice",
                    User = user,
                    ApplicationUserId = user.Id,
                    Image = fileDescription,
                    Disease = diseasse
                };

                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetWoundDetailsDto
                {
                    Id = wound.Id,
                    Type = "Type",
                    Location = "Location",
                    Name = diseasse.Name,
                    Description = diseasse.Description,
                    Image = Convert.ToBase64String(fileDescription.Content.Content),
                    Preventions = diseasse.Preventions,
                    Risk = diseasse.Risk,
                    UploadDate = wound.UploadDate
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { ex.Message }, null));
            }
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            var wounds = await _woundRepo.GetAllAsync(user.Id);

            var resultList = new List<GetWoundDto>();
            foreach(var wound in wounds)
            {
                resultList.Add(new GetWoundDto
                {
                    Id = wound.Id,
                    file = Convert.ToBase64String( wound.Image.Content.Content),
                    Type = wound.Type,
                    Location = wound.Location,
                    AddedDate = wound.UploadDate
                });
            }
            if(resultList.Count > 0)
                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, resultList));
            else
                return Ok(new BaseResponse(true, new List<string> { "History Empty" }, resultList));
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

            var result = new Wound();
            if (id > 0)
            {
                result = await _woundRepo.GetByIdAsync(id);
                if (result != null)
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetWoundDetailsDto
                    {
                        Id = result.Id,
                        Description = result.Disease.Description,
                        Name = result.Disease.Name,
                        Location = result.Location,
                        Preventions = result.Disease.Preventions,
                        Type = result.Type,
                        UploadDate = result.UploadDate,
                        Risk = result.Disease.Risk
                    }));
                else
                    return NotFound();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }    
        /*
         Methods
         */
        private FileDescription GetDescription(IFormFile file)
        {
            byte[] fileBytes;

            using (var fs = file.OpenReadStream())
            {
                using (var sr = new BinaryReader(fs))
                {
                    fileBytes = sr.ReadBytes((int)file.Length);
                }
            }
            var fileContent = new FileContent
            {
                Content = fileBytes
            };
            return new FileDescription
            {
                Content = fileContent,
                ContentType = file.ContentType,
                ContentDisposition = file.ContentDisposition,
            };
        }
        private async Task<int> CallFlaskEndPoint(IFormFile file)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ml-deploy-production.up.railway.app");
                using(var content = new MultipartFormDataContent())
                {
                    var fileStream = file.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "file", file.FileName);

                    var response = await client.PostAsync("/predict", content);

                    var result = await response.Content.ReadFromJsonAsync<WoundIdDto>();

                    return result.Output[0];
                }
            }
        }
    }
}
