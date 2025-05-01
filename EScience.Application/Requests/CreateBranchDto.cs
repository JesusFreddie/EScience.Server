namespace EScience.Application.Requests;

public record CreateBranchDto(Guid? ParentId, string Name);