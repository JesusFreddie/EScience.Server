namespace EScience.Application.Requests;

public record struct CreateArticleRequest(string Title, string Description, Guid ArticleTypeId);