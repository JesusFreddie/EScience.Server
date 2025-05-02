namespace EScience.Application.Requests;

public record UpdateArticleRequest(string? Title, string? Description, bool? IsPrivate);