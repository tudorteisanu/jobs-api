public class Job
{
    public int Id { get; set; }
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