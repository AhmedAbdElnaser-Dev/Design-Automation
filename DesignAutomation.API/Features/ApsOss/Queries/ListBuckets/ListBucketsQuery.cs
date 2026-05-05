using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.ListBuckets;

public record ListBucketsQuery(string? Region, int? Limit, string? StartAt) : IRequest<BucketListViewModel>;
