using EScinece.Domain.Entities;

namespace EScinece.Domain.DTOs;

public record ArticleParticipantDto(Guid Id, Guid AccountId, bool IsAccepted, Guid ArticleId, ArticlePermissionLevel PermissionLevel);