namespace DesignAutomation.API.Features.ApsOss.ViewModels;

public class ObjectViewModel
{
    public string BucketKey { get; set; } = default!;
    public string ObjectKey { get; set; } = default!;
    public string ObjectId { get; set; } = default!;
    public long Size { get; set; }
    public string? ContentType { get; set; }
    public string? Location { get; set; }
}
