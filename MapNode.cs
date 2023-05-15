using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver;

public readonly struct MapNode
{
    public static Result<MapNode, EMapCharacterConversionError> FromCharacter(char nodeCharacter)
    {
        if (char.IsDigit(nodeCharacter))
        {
            return int.TryParse(nodeCharacter.ToString(), out int harborId)
                ? Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(harborId))
                : Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter);
        }

        return nodeCharacter switch
        {
            '.' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(false)),
            '*' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(true)),
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

    private MapNode(bool isWall)
    {
        data = Either<int, bool>.OfRightType(isWall);
    }

    private MapNode(int harborId)
    {
        data = Either<int, bool>.OfLeftType(harborId);
    }

    private readonly Either<int /*harborId*/, bool /*isWall*/> data;
}

public enum EMapCharacterConversionError
{
    InvalidCharacter,
}