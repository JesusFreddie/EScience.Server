using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class AccountInfo : BaseEntity
{
    public string Field { get; set; }
    public string Value { get; set; }
    public Guid AccountId { get; set; }
    
    public AccountInfo() {}

    private AccountInfo(string field, string value, Guid accountId)
    {
        Field = field;
        Value = value;
        Id = Guid.NewGuid();
        AccountId = accountId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<AccountInfo, string> Create(string field, string value, Guid accountId)
    {
        if (string.IsNullOrWhiteSpace(field))
            return "field is empty";

        return new AccountInfo(field, value, accountId);
    }
}