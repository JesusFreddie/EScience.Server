using System.Text;
using DiffMatchPatch;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using HtmlAgilityPack;

namespace EScinece.Infrastructure.Services;

public class MergeService : IMergeService
{
    
    private const string Separator = "␟";

    public List<TextDiff> GetAddedFragments(string original, string modified)
{
    var dmp = new diff_match_patch();
    dmp.Diff_Timeout = 0;
    dmp.Diff_EditCost = 4;
    
    var diffs = dmp.diff_main(original, modified);
    dmp.diff_cleanupSemanticLossless(diffs);
    
    var blocks = new List<TextDiff>();
    int oPos = 0, mPos = 0;

    foreach (var diff in diffs)
    {
        switch (diff.operation)
        {
            case Operation.INSERT:
                blocks.Add(new TextDiff {
                    Content = diff.text,
                    IsOriginal = false,
                    Start = mPos,
                    End = mPos + diff.text.Length
                });
                mPos += diff.text.Length;
                break;

            case Operation.DELETE:
                oPos += diff.text.Length;
                break;

            case Operation.EQUAL:
                // Всегда добавляем EQUAL блоки как оригинальные
                blocks.Add(new TextDiff {
                    Content = diff.text,
                    IsOriginal = true,
                    Start = oPos,
                    End = oPos + diff.text.Length
                });
                oPos += diff.text.Length;
                mPos += diff.text.Length;
                break;
        }
    }

    // Оптимизация: объединение смежных оригинальных блоков
    return MergeAdjacentBlocks(blocks);
}

private List<TextDiff> MergeAdjacentBlocks(List<TextDiff> blocks)
{
    var merged = new List<TextDiff>();
    TextDiff current = null;

    foreach (var block in blocks)
    {
        if (current == null)
        {
            current = block;
            continue;
        }

        if (current.IsOriginal == block.IsOriginal && 
            current.End == block.Start)
        {
            current.Content += block.Content;
            current.End = block.End;
        }
        else
        {
            merged.Add(current);
            current = block;
        }
    }

    if (current != null) merged.Add(current);
    return merged;
}
}