using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Model;

public struct MapNode : INode<MapNode>
{
    public static Result<MapNode, EMapCharacterConversionError> FromCharacter(char nodeCharacter, IntVector position, IntVector dimensions)
    {
        if (char.IsDigit(nodeCharacter))
        {
            return int.TryParse(nodeCharacter.ToString(), out int harborId)
                ? Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(harborId, position, dimensions))
                : Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter);
        }

        return nodeCharacter switch
        {
            '.' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(false, position, dimensions)),
            '*' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(true, position, dimensions)),
            _ => Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter)
        };
    }

    public override string ToString()
    {
        return _data.MapExpression(harborId => harborId.ToString(), isWall => isWall ? "*" : ".");
    }

    public bool TryGetAsHarbor(out int harborId)
    {
        return _data.TryGetLeftValue(out harborId);
    }
    
    public readonly bool TryGetAsRegularNode(out bool isWall)
    {
        return _data.TryGetRightValue(out isWall);
    }

    public MapNode(bool isWall, IntVector position, IntVector gridDimensions)
    {
        Position = position;
        Index = GridMath.PositionToIndex(position, gridDimensions);
        _data = Either<int, bool>.OfRightType(isWall);
    }

    public MapNode(int harborId, IntVector position, IntVector gridDimensions)
    {
        Position = position;
        Index = GridMath.PositionToIndex(position, gridDimensions);
        _data = Either<int, bool>.OfLeftType(harborId);
    }

    public IntVector? Position { get; set; }
    public HashSet<int>? Neighbors { get; set; }

    private readonly Either<int /*harborId*/, bool /*isWall*/> _data;
    public int Index { get; set; }
    public readonly bool IsWalkable()
    {
        if (TryGetAsRegularNode(out bool isWall))
        {
            return !isWall;
        }

        return true;
    }
}

public enum EMapCharacterConversionError
{
    InvalidCharacter,
}