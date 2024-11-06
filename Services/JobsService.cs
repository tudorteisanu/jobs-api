using Microsoft.EntityFrameworkCore;

public interface IJobsService
{
    public Task<PagedResult<Job>> GetAllJobsAsync(PaginationParams paginationParams, string? searchString = null);
    Task<Job> GetJobByIdAsync(int id);
    Task<Job> CreateJobAsync(Job dto);
    Task UpdateJobAsync(int id, Job product);
    Task PartialUpdateJobAsync(int id, UpdateJobDto product);
    Task DeleteJobAsync(int id);
}

public class JobsService : IJobsService
{
    private readonly AppDbContext _context;

    public JobsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Job>> GetAllJobsAsync(PaginationParams paginationParams, string? searchString) {
        var query = _context.Jobs.AsQueryable();

        if (searchString != null) {
            query = query.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
        }

        int totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.Page - 1) * paginationParams.Size)
            .Take(paginationParams.Size)
            .ToListAsync();

        return new PagedResult<Job>(
            data,
            totalCount,
            paginationParams.Page,
            paginationParams.Size
        );
    }

    public async Task<Job> GetJobByIdAsync(int id) => await _context.Jobs.FindAsync(id);

    public async Task<Job> CreateJobAsync(Job productDto)
    {
        var createdProduct = _context.Jobs.Add(productDto).Entity;
        await _context.SaveChangesAsync();
        return createdProduct;
    }

    public async Task UpdateJobAsync(int id, Job product)
    {
        var existingProduct = await _context.Jobs.FindAsync(id);
        if (existingProduct == null) throw new Exception("Job not found");

        // existingProduct.Name = product.Name;
        // existingProduct.Price = product.Price;
        await _context.SaveChangesAsync();
    }

    public async Task PartialUpdateJobAsync(int id, UpdateJobDto product)
    {
        var existingProduct = await _context.Jobs.FindAsync(id);
        
        if (existingProduct == null) {
            throw new Exception("Job not found");
        }

        if (product.Title != null) {
            existingProduct.Title = product.Title;
        }

        // existingProduct.Name = product.Name;
        // existingProduct.Price = product.Price;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Job not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
