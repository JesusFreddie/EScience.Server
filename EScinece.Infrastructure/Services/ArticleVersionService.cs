using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleVersionService(
    IArticleVersionRepository articleVersionRepository,
    ILogger<ArticleVersionService> logger
    ) : IArticleVersionService
{
    public async Task<Result<ArticleVersion, string>> Create(string text, Guid creatorId, Guid articleBranchId)
    {
        try
        {
            var versionResult = ArticleVersion.Create(
                text, creatorId, articleBranchId);

            await articleVersionRepository.Create(versionResult.Value);
            return versionResult.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("An error occured while creating the article branch version");
        }
    }

    public async Task<ArticleVersion?> GetById(Guid id)
    {
        try
        {
            return await articleVersionRepository.GetById(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public Task<ArticleVersion?> GetLast(Guid branchId)
    {
        try
        {
            return articleVersionRepository.GetLatestVersion(branchId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> Save(Guid branchId, string text)
    {
        try
        {
            return await articleVersionRepository.SaveText(branchId, text);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private string ParseHtml(string html)
    {
        return html;
    }
}