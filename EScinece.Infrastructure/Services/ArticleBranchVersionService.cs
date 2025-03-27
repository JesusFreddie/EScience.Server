using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleBranchVersionService(
    IArticleBranchVersionRepository articleBranchVersionRepository,
    ILogger<ArticleBranchVersionService> logger
    ) : IArticleBranchVersionService
{
    public async Task<Result<ArticleBranchVersionDto, string>> Create(string text, Guid creatorId, Guid articleBranchId)
    {
        try
        {
            var versionResult = ArticleBranchVersion.Create(
                text, creatorId, articleBranchId);

            await articleBranchVersionRepository.Create(versionResult.Value);
            var version = versionResult.Value;
            
            return new ArticleBranchVersionDto(
                Id: version.Id,
                Text: ParseHtml(version.Text),
                CreatorId: version.ArticleParticipantId,
                ArticleBranchId: version.ArticleBranchId,
                CreatedAt: version.CreatedAt,
                UpdatedAt: version.UpdatedAt
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("An error occured while creating the article branch version");
        }
    }

    public async Task<ArticleBranchVersionDto?> GetById(Guid id)
    {
        var version = await articleBranchVersionRepository.GetById(id);
        if (version is null)
            return null;

        return new ArticleBranchVersionDto(
            Id: version.Id,
            Text: ParseHtml(version.Text),
            CreatorId: version.ArticleParticipantId,
            ArticleBranchId: version.ArticleBranchId,
            CreatedAt: version.CreatedAt,
            UpdatedAt: version.UpdatedAt
            );
    }

    private string ParseHtml(string html)
    {
        return html;
    }
}