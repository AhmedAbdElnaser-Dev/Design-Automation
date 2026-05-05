namespace DesignAutomation.API.Services.Aps.Models;

public class CreateBucketApsRequest
{
    public string BucketKey { get; set; } = default!;
    public string PolicyKey { get; set; } = "transient";
}
