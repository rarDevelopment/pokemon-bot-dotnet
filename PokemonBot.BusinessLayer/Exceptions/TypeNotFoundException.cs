namespace PokemonBot.BusinessLayer.Exceptions;

public class TypeNotFoundException(string typeName) : Exception($"Type not found with name {typeName}")
{
    public string TypeName { get; } = typeName;
}