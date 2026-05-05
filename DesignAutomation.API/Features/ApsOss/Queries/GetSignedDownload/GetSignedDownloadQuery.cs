using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.GetSignedDownload;

public record GetSignedDownloadQuery(string BucketKey, string ObjectKey, int? MinutesExpiration) : IRequest<SignedDownloadViewModel>;
