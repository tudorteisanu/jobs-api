using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobsService _service;

    public JobsController(IJobsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Job>>> GetAll([FromQuery] PaginationParams paginationParams, [FromQuery] string? search) {
        return Ok(await _service.GetAllJobsAsync(paginationParams, search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetJobByIdAsync(id);

        if (product == null) {
            return NotFound();
        }
        
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Job dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdProduct = await _service.CreateJobAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Job job)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.UpdateJobAsync(id, job);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(int id, UpdateJobDto job) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        await _service.PartialUpdateJobAsync(id, job);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteJobAsync(id);
        return NoContent();
    }
}



public class CreateJobDto
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Company { get; set; }
    public required string Location { get; set; }
    public required string Industry { get; set; }
    public required string JobType { get; set; }
    public required string postDate { get; set; }
    public required string expiryDate { get; set; }
    public required string externalUrl { get; set; }
    public int[] salaryRange { get; set; } = [];
    public string[] keywords { get; set; } = [];
}

public class UpdateJobDto {
    public string? Title { get; set; }
}