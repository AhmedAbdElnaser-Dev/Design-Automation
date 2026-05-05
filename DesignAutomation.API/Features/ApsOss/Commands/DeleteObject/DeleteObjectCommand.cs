using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.DeleteObject;

public record DeleteObjectCommand(string BucketKey, string ObjectKey) : IRequest<Unit>;
