namespace DesignAutomation.API.Features.ApsOss.ViewModels;

public class ObjectListViewModel
{
    public IReadOnlyList<ObjectViewModel> Items { get; set; } = Array.Empty<ObjectViewModel>();
    public string? Next { get; set; }
}
