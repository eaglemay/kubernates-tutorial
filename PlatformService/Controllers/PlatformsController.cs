using AutoMapper;
using FlatformService.Data;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");
            var platformItems = _repository.GetAllPlatForms();
                                  
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
             Console.WriteLine("--> Getting Platform....");
            var platformItem = _repository.GetPlatformById(id);

            if(platformItem != null)
            {                 
              return Ok(_mapper.Map<PlatformReadDto>(platformItem));           
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine("--> Create Platform....");

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            }

            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);         
        }
    }
}