namespace EScinece.Domain.DTOs;

public record ArticleDto(Guid Id, string Title, string Description, Guid CreatorId, Guid TypeArticleId);