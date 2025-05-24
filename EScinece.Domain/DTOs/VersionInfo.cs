namespace EScinece.Domain.DTOs;

public record VersionInfo(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, Guid ArticleBranchId);