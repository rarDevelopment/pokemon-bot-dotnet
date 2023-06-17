namespace PokemonBot.BusinessLayer.Exceptions;

public class TypeNotFoundException : Exception
{
    public TypeNotFoundException(string typeName) : base($"Type not found with name {typeName}") { TypeName = typeName; }

    public string TypeName { get; set; }
}