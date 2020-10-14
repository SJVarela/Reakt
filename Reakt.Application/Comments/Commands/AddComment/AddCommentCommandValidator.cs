using FluentValidation;

namespace Reakt.Application.Comments.Commands.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(c => c.Comment).NotNull();
            RuleFor(c => c.Comment.Message)
                .NotNull()
                .NotEmpty();
        }
    }
}