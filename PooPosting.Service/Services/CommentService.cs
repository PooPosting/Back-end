using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PooPosting.Data.DbContext;
using PooPosting.Data.DbContext.Entities;
using PooPosting.Data.DbContext.Pagination;
using PooPosting.Data.Enums;
using PooPosting.Data.Exceptions;
using PooPosting.Service.Authorization;
using PooPosting.Service.Mappers;
using PooPosting.Service.Models.Dtos.Comment;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Service.Services
{
    public class CommentService
        (PictureDbContext dbContext,
            IAccountContextService accountContextService,
            IAuthorizationService authorizationService
            )
        : ICommentService
    {
        public async Task<PagedResult<CommentDto>> GetByPictureId(int picId, PaginationParameters paginationParameters)
        {
            var commentsQuery = dbContext.Comments
                .Where(c => c.PictureId == picId)
                .OrderByDescending(c => c.Id);

            var comments = await commentsQuery
                .ProjectToDto()
                .Skip(paginationParameters.PageSize * (paginationParameters.PageNumber - 1))
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            var totalComments = await commentsQuery.CountAsync();

            return new PagedResult<CommentDto>(
                comments,
                paginationParameters.PageNumber,
                paginationParameters.PageSize,
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
