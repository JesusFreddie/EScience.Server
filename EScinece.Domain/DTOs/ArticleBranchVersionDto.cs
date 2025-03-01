namespace EScinece.Domain.DTOs;

public record ArticleBranchVersionDto(
    Guid Id,
    string Text, 
    Guid CreatorId, 
    Guid ArticleBranchId, 
    DateTime CreatedAt, 
    DateTime UpdatedAt
    );