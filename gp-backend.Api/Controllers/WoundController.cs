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
                var response = await CallFlaskEndPoint(file);
                if (response == null)
                    return BadRequest();
                Disease diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(x => x.Name.Contains( response[0]));
                Disease diseasse_skin = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(x => x.Name.Contains( response[1]));


                if (diseasse == null && diseasse_skin == null)
                    return Ok(new BaseResponse(true, new List<string> { "Your Health is good"}, null));

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
                    Image = fileDescription
                };
                string diseaseName = "";
                string description = "";
                string risk = "";
                var prevention = new List<string>();
                if(diseasse is not null)
                {
                    wound.Disease.Add(diseasse);
                    diseaseName += diseasse.Name;
                    description += $"{diseaseName}: \n\n";
                    description += diseasse.Description + "\n\n";
                    prevention.Concat(diseasse.Preventions);
                    risk += diseasse.Risk;
                }
                if (diseasse_skin is not null)
                {
                    wound.Disease.Add(diseasse_skin);
                    diseaseName += " / " + diseasse_skin.Name;
                    description += $"{diseasse_skin.Name}: \n\n";
                    description += diseasse_skin.Description;
                    prevention.Concat(diseasse_skin.Preventions);
                    risk += "\n\n" + diseasse_skin.Risk;
                }

                
                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetWoundDetailsDto
                {
                    Id = wound.Id,
                    Type = wound.Type,
                    Location = "Location",
                    Name = diseaseName,
                    Description = description,
                    Image = Convert.ToBase64String(fileDescription.Content.Content),
                    Preventions = prevention,
                    Risk = risk,
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
            var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;
            var wounds = await _woundRepo.GetAllAsync(uid);

            var resultList = new List<GetWoundDto>();
            foreach(var wound in wounds)
            {
                string name = "";
                foreach (var item in wound.Disease)
                    name += item.Name + " / ";
                resultList.Add(new GetWoundDto
                {
                    Id = wound.Id,
                    file = Convert.ToBase64String( wound?.Image?.Content?.Content),
                    Type = wound.Type,
                    Name = name,
                    AddedDate = wound.UploadDate.ToShortDateString(),
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
                {
                    string diseaseName = "";
                    string description = "";
                    string risk = "";
                    var prevention = new List<string>();
                    foreach(var item in result.Disease)
                    {
                        diseaseName += item.Name + " / ";
                        description += $"{item.Name}: \n\n";
                        description += item.Description + "\n\n";
                        prevention.Concat(item.Preventions);
                        risk += item.Risk + "\n\n";
                    }

                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetWoundDetailsDto
                    {
                        Id = result.Id,
                        Description = description,
                        Name = diseaseName,
                        Location = result.Location,
                        Preventions = prevention,
                        Type = result.Type,
                        UploadDate = result.UploadDate,
                        Risk = risk
                    }));
                }
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
        private async Task<List<string>> CallFlaskEndPoint(IFormFile file)
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

                    return result.Output;
                }
            }
        }
    }
}
