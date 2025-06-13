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

    public async Task<Result<ArticleVersion, string>> Copy(Guid versionId, Guid? creatorId = null, Guid? articleBranchId = null)
    {
        try
        {
            var original = await GetById(versionId);
            if (original is null)
                return "Version not found";

            var version = ArticleVersion.Create(
                articleBranchId: articleBranchId ?? original.ArticleBranchId,
                creatorId: creatorId ?? original.CreatorId,
                text: original.Text
                );
            
            if (!version.onSuccess)
                return version.Error;
            
            await articleVersionRepository.Create(version.Value);
            
            return version.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("An error occured while copying the article version");
        }
    }

    public async Task<Result<ArticleVersion, string>> CopyByBranch(Guid branchId, Guid? creatorId = null, Guid? articleBranchId = null)
    {
        try
        {
            var original = await GetLast(branchId);
            if (original is null)
                return "Branch not found";

            var version = ArticleVersion.Create(
                articleBranchId: articleBranchId ?? branchId,
                creatorId: creatorId ?? original.CreatorId,
                text: original.Text
            );
            
            if (!version.onSuccess)
                return version.Error;

            await articleVersionRepository.Create(version.Value);
            
            return version.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("An error occured while copying the article version");
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

    public async Task<ArticleVersion?> GetLast(Guid branchId)
    {
        try
        {
            return await articleVersionRepository.GetLatestVersion(branchId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<ArticleVersion?> GetFirst(Guid branchId)
    {
        try
        {
            return await articleVersionRepository.GetFirstVersion(branchId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> Save(Guid branchId, string text, Guid? creatorId = null)
    {
        try
        {
            var lastVersion = await articleVersionRepository.GetLastVersionInfoAsync(branchId);
            if (lastVersion is null)
                return false;

            DateTime dbTimeUtc = lastVersion.CreatedAt.Kind == DateTimeKind.Unspecified 
                ? DateTime.SpecifyKind(lastVersion.CreatedAt, DateTimeKind.Utc) 
                : lastVersion.CreatedAt.ToUniversalTime();
    
            TimeSpan difference = DateTime.UtcNow - dbTimeUtc;
            
            var lastVersionId = lastVersion.Id;
            
            if (difference.TotalMinutes > 10 || creatorId != lastVersion.CreatorId)
            {
                Console.WriteLine(difference.TotalMinutes);
                Console.WriteLine(creatorId);
                Console.WriteLine(lastVersion.CreatorId);
                var newVersionResult = await Copy(lastVersion.Id, creatorId);

                if (!newVersionResult.onSuccess)
                    return false;
                
                lastVersionId = newVersionResult.Value.Id;
            }
            
            return await articleVersionRepository.SaveText(lastVersionId, text);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<VersionInfo>> GetVersionInfo(Guid branchId)
    {
        try
        {
            return await articleVersionRepository.GetAllVersionInfo(branchId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("An error occured while getting version info");
        }
    }


    private string ParseHtml(string html)
    {
        return html;
    }
}