using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    public Task<PagedResult<Product>> GetAllProductsAsync(PaginationParams paginationParams);
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(CreateProductDto product);
    Task UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
}

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Product>> GetAllProductsAsync(PaginationParams paginationParams) {
        var query = _context.Products.AsQueryable();

        int totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.Page - 1) * paginationParams.Size)
            .Take(paginationParams.Page)
            .ToListAsync();

        return new PagedResult<Product>(
            data,
            totalCount,
            paginationParams.Page,
            paginationParams.Size
        );
    }

    public async Task<Product> GetProductByIdAsync(int id) => await _context.Products.FindAsync(id);

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        var product = new Product { Name = productDto.Name, Price = productDto.Price };
        var createdProduct = _context.Products.Add(product).Entity;
        await _context.SaveChangesAsync();
        return createdProduct;
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null) throw new Exception("Product not found");

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
