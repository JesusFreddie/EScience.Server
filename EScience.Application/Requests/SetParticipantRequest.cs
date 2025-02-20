using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EScinece.Domain.Entities;

namespace EScience.Application.Requests;

public record SetParticipantRequest(Guid AccountId, ArticlePermissionLevel PermissionLevel);