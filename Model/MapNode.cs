using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Model;

public struct MapNode : INode<MapNode>
{
    public static Result<MapNode, EMapCharacterConversionError> FromCharacter(char nodeCharacter, IntVector position, HashSet<int> neighbors, IntVector dimensions)
    {
        if (char.IsDigit(nodeCharacter))
        {
            return int.TryParse(nodeCharacter.ToString(), out int harborId)
                ? Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(harborId, position, neighbors, dimensions))
                : Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter);
        }

        return nodeCharacter switch
        {
            '.' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(false, position, neighbors, dimensions)),
            '*' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(true, position, neighbors, dimensions)),
            var _ => Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter)
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
    
    public bool TryGetAsRegularNode(out bool isWall)
    {
        return _data.TryGetRightValue(out isWall);
    }

    public MapNode(bool isWall, IntVector position, HashSet<int> neighbors, IntVector gridDimensions)
    {
        Position = position;
        Index = GridMath.PositionToIndex(position, gridDimensions);
        Neighbors = neighbors;
        _data = Either<int, bool>.OfRightType(isWall);
    }

    public MapNode(int harborId, IntVector position, HashSet<int> neighbors, IntVector gridDimensions)
    {
        Position = position;
        Index = GridMath.PositionToIndex(position, gridDimensions);
        Neighbors = neighbors;
        _data = Either<int, bool>.OfLeftType(harborId);
    }

    public IntVector? Position { get; set; }
    public HashSet<int>? Neighbors { get; set; }

    private readonly Either<int /*harborId*/, bool /*isWall*/> _data;
    public int Index { get; set; }
}

public enum EMapCharacterConversionError
{
    InvalidCharacter,
}