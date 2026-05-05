using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.GetBucket;

public record GetBucketQuery(string BucketKey) : IRequest<BucketViewModel>;
