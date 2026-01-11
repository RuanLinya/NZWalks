using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NZWalks.API.CustomActionFilters;


namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper) 
        { 
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetAll()
        {
            // var regionsDomain = await dbContext.Regions.ToListAsync();

            // Get data from database - domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Convert domain models to DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // Get api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
    
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        // Post To Create new Region
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            if (ModelState.IsValid)
            {  //Convert DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Save to Database
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Convert Domain Model to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }

            return BadRequest(ModelState);
              
        }

        //Update Region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Get region from database
            // var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomainModel = await regionRepository.UpdateAsync(id, mapper.Map<Region>(updateRegionRequestDto));

            //If not found, return NotFound
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert to DTO
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete Region

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
