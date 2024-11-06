using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> GetAll([FromQuery] PaginationParams paginationParams) {
        return Ok(await _productService.GetAllProductsAsync(paginationParams));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdProduct = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _productService.UpdateProductAsync(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}



public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Product price is required") ]
    [Range(1, double.MaxValue)]
    public decimal Price { get; set; }
}