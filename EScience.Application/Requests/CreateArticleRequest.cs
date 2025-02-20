namespace EScience.Application.Requests;

public record CreateArticleRequest(string Title, string Description, Guid? ArticleTypeId);