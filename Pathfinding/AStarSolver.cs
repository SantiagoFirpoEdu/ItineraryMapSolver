using ItineraryMapSolver.Model;
using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Pathfinding;

public static class AStarSolver
{
    public static List<IntVector> SolvePath(in MapGrid grid, in IntVector from, in IntVector to)
    {
        PathGrid pathGrid = new(grid.Width, grid.Height);
        Dictionary<IntVector, PathNode?> nodesToSearch = new();
        Dictionary<IntVector, PathNode> searchedNodes = new();
        PathNode? initialNode = pathGrid.GetNode(from.X, from.Y);

        nodesToSearch.Add(from, initialNode);
        while (nodesToSearch.Count > 0)
        {
            return null;
        }
        return null;
    }

    private static Option<int> getLowestTotalCost(ICollection<PathNode> pathNodes)
    {
        return pathNodes.Count == 0
            ? Option<int>.None()
            : Option<int>.Some(pathNodes.Min(LowestTotalCostSelector));
    }

    private static int LowestTotalCostSelector(PathNode node)
    {
        return node.TotalCost;
    }
}