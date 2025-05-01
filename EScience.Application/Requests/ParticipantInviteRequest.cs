using EScinece.Domain.Entities;

namespace EScience.Application.Requests;

public record ParticipantInviteRequest(string Email, ArticlePermissionLevel PermissionLevel);