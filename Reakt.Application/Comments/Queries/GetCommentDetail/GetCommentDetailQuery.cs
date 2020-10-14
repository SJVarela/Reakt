using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Queries.GetCommentDetail
{
    public class GetCommentDetailQuery : IRequest<Comment>
    {
        public long Id { get; set; }
    }
}