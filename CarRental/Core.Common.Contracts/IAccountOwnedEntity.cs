namespace Core.Common.Contracts
{
    /// <summary>
    /// Para especificar el Id de UserAccount
    /// </summary>
    public interface IAccountOwnedEntity
    {
        int OwnerAccountId { get; }
    }
}
