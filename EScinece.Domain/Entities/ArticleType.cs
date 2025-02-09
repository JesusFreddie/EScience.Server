using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleType : BaseEntity
{
    public ArticleType() {}
    public string Name { get; set; }
}