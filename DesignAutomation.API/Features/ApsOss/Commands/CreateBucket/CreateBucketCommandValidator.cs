using System.Text.RegularExpressions;
using FluentValidation;

namespace DesignAutomation.API.Features.ApsOss.Commands.CreateBucket;

public class CreateBucketCommandValidator : AbstractValidator<CreateBucketCommand>
{
    private static readonly Regex BucketKeyPattern = new("^[a-z0-9._-]{3,128}$", RegexOptions.Compiled);
    private static readonly HashSet<string> AllowedPolicies = new(StringComparer.OrdinalIgnoreCase) { "transient", "temporary", "persistent" };

    public CreateBucketCommandValidator()
    {
        RuleFor(x => x.BucketKey)
            .NotEmpty()
            .Must(k => BucketKeyPattern.IsMatch(k))
            .WithMessage("BucketKey must be 3-128 chars of lowercase letters, digits, '.', '_', '-'.");

        RuleFor(x => x.PolicyKey)
            .Must(p => p is null || AllowedPolicies.Contains(p))
            .WithMessage("PolicyKey must be 'transient', 'temporary', or 'persistent'.");
    }
}
