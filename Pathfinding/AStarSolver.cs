using System.Diagnostics;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Pathfinding;

public static class AStarSolver
{
    public static async Task<Result<List<IntVector>, PathfindingError>> SolvePathAsync(ReadonlyRef<MapGrid> grid, IntVector from,
        IntVector to)
    {
        Result<List<IntVector>, PathfindingError> SolvePathWithParameters() => SolvePath(grid.GetValueRef(), from, to);

        return await Task.Run(SolvePathWithParameters);
    }
    public static Result<List<IntVector>, PathfindingError> SolvePath(in MapGrid grid, IntVector from, IntVector to)
    {
        PathGrid pathGrid = new(grid.Width, grid.Height, grid);
        PriorityQueue<int, int> nodesToSearch = new();
        HashSet<int> searchedNodes = new();
        ref PathNode initialNode = ref pathGrid.GetNodeRef(pathGrid.PositionToIndex(from));

        if (!pathGrid.IsValidPosition(from))
        {
            return Result<List<IntVector>, PathfindingError>.Error(new PathfindingError(from, to, EPathfindingError.InvalidStartPosition));
        }
        
        if (!pathGrid.IsValidPosition(to))
        {
            return Result<List<IntVector>, PathfindingError>.Error(new PathfindingError(from, to,
                EPathfindingError.InvalidStartPosition));
        }

        initialNode.CostFromStart = 0;
        initialNode.ComputeTotalCost(to);

        int? endNodeIndex = pathGrid.GetNode(to).Index;

        nodesToSearch.Enqueue(initialNode.Index, initialNode.TotalCost);
        while (nodesToSearch.Count > 0)
        {
            var currentNodeIndex = DequeueLowestCost(nodesToSearch);

            if (currentNodeIndex.IsEmpty())
            {
                throw new InvalidOperationException("could not peek open list even though it is not empty");
            }

            int currentNodeIndexValue = currentNodeIndex.GetValue();

            if (currentNodeIndexValue == endNodeIndex)
            {
                break;
            }

            SearchNode(nodesToSearch, currentNodeIndexValue, ref searchedNodes, ref pathGrid, to, grid);
        }

        Debug.Assert(endNodeIndex != null, $"{nameof(endNodeIndex)} != null");
        ref readonly PathNode pathNode = ref pathGrid.GetNodeRef((int) endNodeIndex);

        return pathNode.CameFromNodeIndex.IsSet()
            //Found a path
            ? Result<List<IntVector>, PathfindingError>.Ok(TraceBackPath(pathGrid, pathNode) ?? throw new ApplicationException("Unable to trace path from end node"))

            //Did not find a path
            : Result<List<IntVector>, PathfindingError>.Error(new PathfindingError(from, to, EPathfindingError.NoAvailablePath));

    }

    private static void SearchNode(PriorityQueue<int, int> nodesToSearch, int currentNodeIndexValue,
        ref HashSet<int> searchedNodes, ref PathGrid pathGrid, IntVector endPosition, in MapGrid mapGrid)
    {
        searchedNodes.Add(currentNodeIndexValue);

        ref readonly PathNode currentNode = ref pathGrid.GetNodeRef(currentNodeIndexValue);

        var neighbors = pathGrid.GetNeighbors((IntVector)currentNode.Position);

        ProcessNeighbors(neighbors, ref pathGrid, currentNode, currentNodeIndexValue, ref searchedNodes,
            nodesToSearch, endPosition, mapGrid);
    }

    private static void ProcessNeighbors(in HashSet<int> neighbors, ref PathGrid pathGrid, in PathNode currentNode,
        int currentNodeIndexValue, ref HashSet<int> searchedNodes, PriorityQueue<int, int> nodesToSearch, IntVector endPosition, MapGrid mapGrid)
    {
        foreach (int neighborIndex in neighbors)
        {
            if (!mapGrid.GetNodeRef(neighborIndex).IsWalkable())
            {
                searchedNodes.Add(neighborIndex);
                continue;
            }

            ProcessNeighbor(ref pathGrid, neighborIndex, ref searchedNodes, currentNode, currentNodeIndexValue,
                nodesToSearch, endPosition);
        }
    }

    private static void ProcessNeighbor(ref PathGrid pathGrid, int neighborIndex, ref HashSet<int> searchedNodes,
        in PathNode currentNode, int currentNodeIndexValue, PriorityQueue<int, int> nodesToSearch, IntVector endPosition)
    {
        ref PathNode neighborNode = ref pathGrid.GetNodeRef(neighborIndex);
        if (searchedNodes.Contains(neighborIndex))
        {
            return;
        }

        int candidateCostFromStart = currentNode.CostFromStart + PathNode.UnitWalkingCost;

        if (candidateCostFromStart >= neighborNode.CostFromStart)
        {
            return;
        }

        neighborNode.CameFromNodeIndex = Option<int>.Some(currentNodeIndexValue);
        neighborNode.CostFromStart = candidateCostFromStart;
        neighborNode.ComputeTotalCost(endPosition);

        nodesToSearch.Enqueue(neighborIndex, neighborNode.TotalCost);
    }

    private static List<IntVector>? TraceBackPath(in PathGrid pathGrid, in PathNode endNode)
    {
        if (endNode.CameFromNodeIndex.IsEmpty())
        {
            return null;
        }

        var path = new List<IntVector>();

        ref readonly PathNode currentNode = ref endNode;
        if (currentNode.Position is not null)
        {
            path.Add((IntVector) currentNode.Position);
        }
        var currentNodeCameFromNodeIndex = currentNode.CameFromNodeIndex;
        do
        {
            int cameFromNodeIndex = currentNodeCameFromNodeIndex.GetValue();
            PathNode cameFromNode = pathGrid.GetNode(cameFromNodeIndex);
            if (cameFromNode.Position is null)
            {
                continue;
            }

            currentNode = ref pathGrid.GetNodeRefReadonly(cameFromNodeIndex);

            if (currentNode.Position is not null)
            {
                path.Add((IntVector) currentNode.Position);
            }

            currentNodeCameFromNodeIndex = currentNode.CameFromNodeIndex;
        }
        while (currentNodeCameFromNodeIndex.IsSet());

        path.Reverse();

        return path;
    }

    private static Option<int> DequeueLowestCost(PriorityQueue<int, int> nodesToSearch)
    {
        var currentLowest = Option<int>.None();
        if (nodesToSearch.TryDequeue(out int nodeIndex, out int _))
        {
            currentLowest = Option<int>.Some(nodeIndex);
        }

        return currentLowest;
    }
}