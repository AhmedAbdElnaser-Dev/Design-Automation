using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.ListObjects;

public record ListObjectsQuery(string BucketKey, int? Limit, string? StartAt) : IRequest<ObjectListViewModel>;
