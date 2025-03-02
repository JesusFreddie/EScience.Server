namespace EScinece.Domain.DTOs;

public record ArticleDto(Guid Id, string Title, string Description, bool IsPrivate, Guid AccountId, Guid? TypeArticleId);