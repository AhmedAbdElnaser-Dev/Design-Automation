namespace DesignAutomation.API.Features.ApsOss.ViewModels;

public class BucketViewModel
{
    public string BucketKey { get; set; } = default!;
    public string? BucketOwner { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? PolicyKey { get; set; }
}
