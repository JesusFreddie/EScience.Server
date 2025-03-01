using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.ErrorMessages;

public static class ArticleErrorMessage
{
    public const string TitleAndDescriptionCannotBeEmpty = "Title and Description cannot be empty!";
    public const string CreatorIsNull = "Creator cannot be null!";
    public const string ArticleNotFound = "Article not found!";
}