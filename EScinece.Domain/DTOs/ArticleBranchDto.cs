namespace EScinece.Domain.DTOs;

public record ArticleBranchDto(
    Guid Id,
    string Name,
    Guid ArticleId,
    Guid CreatorId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );