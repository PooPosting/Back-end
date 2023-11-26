using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Authorization;
using PooPosting.Application.Mappers;
using PooPosting.Application.Models;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Entities;
using PooPosting.Domain.Enums;
using PooPosting.Domain.Exceptions;

namespace PooPosting.Application.Services
{
    public class CommentService
        (PictureDbContext dbContext,
            IAccountContextService accountContextService,
            IAuthorizationService authorizationService
            )
        : ICommentService
    {
        public async Task<PagedResult<CommentDto>> GetByPictureId(int picId, Query query)
        {
            var commentsQuery = dbContext.Comments
                .Where(c => c.PictureId == picId)
                .OrderByDescending(c => c.Id);

            var comments = await commentsQuery
                .ProjectToDto()
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var totalComments = await commentsQuery.CountAsync();

            return new PagedResult<CommentDto>(
                comments,
                query.PageNumber,
                query.PageSize,
                totalComments
            );
        }

        public async Task<CommentDto> Create(int picId, string text)
        {
            var picture = await dbContext.Pictures.FirstOrDefaultAsync(p => p.Id == picId);
            if (picture == null) throw new NotFoundException();

            var comment = new Comment()
            {
                AccountId = accountContextService.GetAccountId(),
                PictureId = picId,
                Text = text,
            };

            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();

            var commentDto = await dbContext.Comments
                .Include(c => c.Account)
                .Where(c => c.Id == comment.Id)
                .ProjectToDto()
                .FirstOrDefaultAsync();

            return commentDto!; 
        }

        public async Task<CommentDto> Update(int commId, string text)
        {
            await AuthorizeCommentOperation(commId, ResourceOperation.Update, "You can't modify a comment you didn't add");

            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);
            if (comment == null) throw new NotFoundException();

            comment.Text = text;
            await dbContext.SaveChangesAsync();

            return comment.MapToDto();
        }

        public async Task<bool> Delete(int commId)
        {
            await AuthorizeCommentOperation(commId, ResourceOperation.Delete, "You have no rights to delete this comment");

            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);
            if (comment == null) throw new NotFoundException();

            comment.IsDeleted = true;
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task AuthorizeCommentOperation(int commId, ResourceOperation operation, string message)
        {
            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);

            if (comment == null) throw new NotFoundException();
            var user = accountContextService.User;
            var authorizationResult = await authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation));
            if (!authorizationResult.Succeeded) throw new ForbidException(message);
        }
    }
}
