using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.UploadObject;

public record UploadObjectCommand(
    string BucketKey,
    string ObjectKey,
    Stream Content,
    long ContentLength,
    string? ContentType) : IRequest<ObjectViewModel>;
