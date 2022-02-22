using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("INFO: Getting platforms...");

            var platforms = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }

        [HttpGet("{id}", Name = nameof(GetPlatformById))]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platform = _repository.GetPlatformById(id);

            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platform = _mapper.Map<Platform>(platformCreateDTO);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDTO.Id }, platformReadDTO);
        }
    }
}
