using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            // Call Repository to persist the data
            await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
            // Save to database

        }
    }
}
