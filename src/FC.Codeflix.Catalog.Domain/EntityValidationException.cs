namespace FC.Codeflix.Catalog.Domain;

public class EntityValidationException : Exception
{
    public EntityValidationException(string? message) : base(message)
    {
        
    }

}