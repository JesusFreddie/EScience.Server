namespace EScience.Application.Requests;

public record SaveArticleTextRequest(Guid BranchId, string Text);