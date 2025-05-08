using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IMergeService
{
    public List<TextDiff> GetAddedFragments(string original, string modified);
}