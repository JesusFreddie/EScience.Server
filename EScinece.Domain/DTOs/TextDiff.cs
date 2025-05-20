namespace EScinece.Domain.DTOs;

public class TextDiff
{
    public string Content { get; set; }
    public bool IsOriginal { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public List<TextDiff> Children { get; } = new List<TextDiff>();
}