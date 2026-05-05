using FluentValidation;

namespace DesignAutomation.API.Features.ApsOss.Commands.UploadObject;

public class UploadObjectCommandValidator : AbstractValidator<UploadObjectCommand>
{
    public UploadObjectCommandValidator()
    {
        RuleFor(x => x.BucketKey).NotEmpty();
        RuleFor(x => x.ObjectKey).NotEmpty();
        RuleFor(x => x.ContentLength).GreaterThan(0);
        RuleFor(x => x.Content).NotNull();
    }
}
