namespace Core.Common.Contracts
{
    /// <summary>
    /// Permite a la Data Access Layer saber qué propiedad de la Entidad sirve como identificador en la BD
    /// Es una alternativa a usar "[Key]" en Entity Framework
    /// </summary>
    public interface IIdentifiableEntity
    {
        int EntityId { get; set; }
    }
}
