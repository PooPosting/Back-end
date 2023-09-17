using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly PictureDbContext _dbContext;
        private readonly IAccountContextService _accountContextService;
        private readonly IAuthorizationService _authorizationService;

        public CommentService(
            IMapper mapper,
            PictureDbContext dbContext,
            IAccountContextService accountContextService,
            IAuthorizationService authorizationService
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accountContextService = accountContextService;
            _authorizationService = authorizationService;
        }

        public async Task<PagedResult<CommentDto>> GetByPictureId(int picId, Query query)
        {
            var commentsQuery = _dbContext.Comments
                .Where(c => c.PictureId == picId)
                .OrderByDescending(c => c.Id)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider);

            var comments = await commentsQuery
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
            var picture = await _dbContext.Pictures.FirstOrDefaultAsync(p => p.Id == picId);
            if (picture == null) throw new NotFoundException();

            var comment = new Comment()
            {
                AccountId = _accountContextService.GetAccountId(),
                PictureId = picId,
                Text = text,
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            var commentDto = _dbContext.Comments
                .Include(c => c.Account)
                .Where(c => c.Id == comment.Id)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return await commentDto; 
        }

        public async Task<CommentDto> Update(int commId, string text)
        {
            await AuthorizeCommentOperation(commId, ResourceOperation.Update, "You can't modify a comment you didn't add");

            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);
            if (comment == null) throw new NotFoundException();

            comment.Text = text;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<bool> Delete(int commId)
        {
            await AuthorizeCommentOperation(commId, ResourceOperation.Delete, "You have no rights to delete this comment");

            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);
            if (comment == null) throw new NotFoundException();

            comment.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task AuthorizeCommentOperation(int commId, ResourceOperation operation, string message)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commId);

            if (comment == null) throw new NotFoundException();
            var user = _accountContextService.User;
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation));
            if (!authorizationResult.Succeeded) throw new ForbidException(message);
        }
    }
}
