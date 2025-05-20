using System.Text.RegularExpressions;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;

namespace EScinece.Infrastructure.Services;

public class MergeService : IMergeService
{
    public List<TextDiff> GetAddedFragments(string originalHtml, string newHtml)
    {
        var originalTree = ParseHtmlToTree(originalHtml);
        var newTree = ParseHtmlToTree(newHtml);
        return CompareTrees(originalTree, newTree);
    }

    private TextDiff ParseHtmlToTree(string html)
    {
        var root = new TextDiff();
        var stack = new Stack<TextDiff>();
        stack.Push(root);
        int pos = 0;

        while (pos < html.Length)
        {
            var tagMatch = Regex.Match(html.Substring(pos), @"<(/?)([^\s>]+)([^>]*)>");
            if (tagMatch.Success && tagMatch.Index == 0)
            {
                // Handle text node before tag
                if (pos > stack.Peek().End)
                {
                    var textContent = html.Substring(stack.Peek().End, pos - stack.Peek().End);
                    if (!string.IsNullOrWhiteSpace(textContent))
                    {
                        stack.Peek().Children.Add(new TextDiff {
                            Content = textContent,
                            Start = pos - textContent.Length,
                            End = pos
                        });
                    }
                }

                var isClosing = tagMatch.Groups[1].Value == "/";
                var fullTag = html.Substring(pos, tagMatch.Length);
                var node = new TextDiff {
                    Content = fullTag,
                    Start = pos,
                    End = pos + tagMatch.Length
                };

                if (!isClosing)
                {
                    stack.Peek().Children.Add(node);
                    stack.Push(node);
                }
                else
                {
                    while (stack.Count > 1 && stack.Peek().Content != $"<{tagMatch.Groups[2].Value}>")
                        stack.Pop();

                    if (stack.Count > 1)
                    {
                        var openedNode = stack.Pop();
                        openedNode.Content = html.Substring(openedNode.Start, pos + tagMatch.Length - openedNode.Start);
                        openedNode.End = pos + tagMatch.Length;
                    }
                }

                pos += tagMatch.Length;
            }
            else
            {
                pos++;
            }
        }

        return root.Children.Count > 0 ? root.Children[0] : root;
    }

    private List<TextDiff> CompareTrees(TextDiff original, TextDiff modified)
    {
        var result = new List<TextDiff>();
        CompareNodes(original, modified, result);
        return result;
    }

    private void CompareNodes(TextDiff orig, TextDiff mod, List<TextDiff> result)
    {
        if (orig == null && mod == null) return;

        var newNode = new TextDiff();
        bool isModified = orig?.Content != mod?.Content;

        if (mod != null)
        {
            newNode.Content = mod.Content;
            newNode.Start = mod.Start;
            newNode.End = mod.End;
            newNode.IsOriginal = !isModified;

            // Compare children recursively
            for (int i = 0; i < mod.Children.Count; i++)
            {
                var origChild = orig?.Children.Count > i ? orig.Children[i] : null;
                var modChild = mod.Children[i];
                CompareNodes(origChild, modChild, newNode.Children);
            }

            result.Add(newNode);
        }
    }
}