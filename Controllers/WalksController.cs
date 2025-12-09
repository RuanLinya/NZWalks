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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await walkRepository.GetAllAsync();   
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetAllAsync();   
            if (walksDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            // Call Repository to persist the data
            var updatedWalkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
            if (updatedWalkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(updatedWalkDomainModel));
            // Save to database

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Call Repository to delete the data
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
            // Save to database
        }
    }
}