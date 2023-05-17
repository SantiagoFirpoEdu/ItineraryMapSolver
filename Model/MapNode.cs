using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Model;

public struct MapNode : INode<MapNode>
{
    public static Result<MapNode, EMapCharacterConversionError> FromCharacter(char nodeCharacter, IntVector position, Dictionary<IntVector, MapNode> neighbors)
    {
        if (char.IsDigit(nodeCharacter))
        {
            return int.TryParse(nodeCharacter.ToString(), out int harborId)
                ? Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(harborId, position, neighbors))
                : Result<MapNode, EMapCharacterConversionError>.Error(EMapCharacterConversionError.InvalidCharacter);
        }

        return nodeCharacter switch
        {
            '.' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(false, position, neighbors)),
            '*' => Result<MapNode, EMapCharacterConversionError>.Ok(new MapNode(true, position, neighbors)),
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

    public MapNode(bool isWall, IntVector position, Dictionary<IntVector, MapNode> neighbors)
    {
        Position = position;
        Neighbors = neighbors;
        _data = Either<int, bool>.OfRightType(isWall);
    }

    public MapNode(int harborId, IntVector position, Dictionary<IntVector, MapNode> neighbors)
    {
        Position = position;
        Neighbors = neighbors;
        _data = Either<int, bool>.OfLeftType(harborId);
    }

    public IntVector? Position { get; set; }
    public Dictionary<IntVector, MapNode>? Neighbors { get; set; }

    private readonly Either<int /*harborId*/, bool /*isWall*/> _data;
}

public enum EMapCharacterConversionError
{
    InvalidCharacter,
}