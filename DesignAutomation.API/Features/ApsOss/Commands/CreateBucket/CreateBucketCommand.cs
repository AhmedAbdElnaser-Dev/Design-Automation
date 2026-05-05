using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.CreateBucket;

public record CreateBucketCommand(string BucketKey, string? PolicyKey, string? Region) : IRequest<BucketViewModel>;
