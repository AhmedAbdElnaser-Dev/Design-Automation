namespace DesignAutomation.API.Features.ApsOss.ViewModels;

public class BucketListViewModel
{
    public IReadOnlyList<BucketViewModel> Items { get; set; } = Array.Empty<BucketViewModel>();
    public string? Next { get; set; }
}
