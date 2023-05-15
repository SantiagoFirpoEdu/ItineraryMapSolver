using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Model;

public readonly struct MapNode : INode
{
    public static Result<MapNode, EMapCharacterConversionError> FromCharacter(char nodeCharacter, IntVector position)
    {
        if (char.IsDigit(nodeCharacter))
        {
            return int.TryParse(nodeCharacter.ToString(), out int harborId)
                ? Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(harborId, position))
                : Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter);
        }

        return nodeCharacter switch
        {
            '.' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(false, position)),
            '*' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(true, position)),
            var _ => Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter)
        };
    }

    public override string ToString()
    {
        return data.MapExpression(harborId => harborId.ToString(), isWall => isWall ? "*" : ".");
    }

    public bool TryGetAsHarbor(out int harborId)
    {
        return data.TryGetLeftValue(out harborId);
    }
    
    public bool TryGetAsRegularNode(out bool isWall)
    {
        return data.TryGetRightValue(out isWall);
    }

    private MapNode(bool isWall, IntVector position)
    {
        Position = position;
        data = Either<int, bool>.OfRightType(isWall);
    }

    private MapNode(int harborId, IntVector position)
    {
        Position = position;
        data = Either<int, bool>.OfLeftType(harborId);
    }

    private readonly Either<int /*harborId*/, bool /*isWall*/> data;
    public IntVector Position { get; }
}

public interface INode
{
    public IntVector Position { get; }
}

public enum EMapCharacterConversionError
{
    InvalidCharacter,
}