namespace DesignAutomation.API.Features.ApsOss.ViewModels;

public class SignedDownloadViewModel
{
    public string? Status { get; set; }
    public string? Url { get; set; }
    public long Size { get; set; }
    public string? Sha1 { get; set; }
    public string? ContentType { get; set; }
    public DateTime ExpiresAt { get; set; }
}
