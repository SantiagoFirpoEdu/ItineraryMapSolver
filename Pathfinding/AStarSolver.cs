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
        PathGrid pathGrid = new(grid.Width, grid.Height, to);
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
        initialNode.ComputeTotalCost();

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

            SearchNode(grid, ref nodesToSearch, currentNodeIndexValue, ref searchedNodes, ref pathGrid);
        }

        Debug.Assert(endNodeIndex != null, nameof(endNodeIndex) + " != null");
        ref readonly PathNode pathNode = ref pathGrid.GetNodeRef((int) endNodeIndex);

        return pathNode.CameFromNodeIndex.IsSet()
            //Found a path
            ? Result<List<IntVector>, PathfindingError>.Ok(TraceBackPath(pathGrid, pathNode) ?? throw new ApplicationException("Unable to trace path from end node"))

            //Did not find a path
            : Result<List<IntVector>, PathfindingError>.Error(new PathfindingError(from, to, EPathfindingError.NoAvailablePath));

    }

    private static void SearchNode(in MapGrid grid, ref PriorityQueue<int, int> nodesToSearch, int currentNodeIndexValue,
        ref HashSet<int> searchedNodes, ref PathGrid pathGrid)
    {
        searchedNodes.Add(currentNodeIndexValue);

        ref readonly PathNode currentNode = ref pathGrid.GetNodeRef(currentNodeIndexValue);

        var neighbors = currentNode.Neighbors;

        if (neighbors == null)
        {
            return;
        }

        ProcessNeighbors(grid, neighbors, ref pathGrid, currentNode, currentNodeIndexValue, ref searchedNodes,
            ref nodesToSearch);
    }

    private static void ProcessNeighbors(in MapGrid grid, in HashSet<int> neighbors, ref PathGrid pathGrid, in PathNode currentNode,
        int currentNodeIndexValue, ref HashSet<int> searchedNodes, ref PriorityQueue<int, int> nodesToSearch)
    {
        foreach (int neighborIndex in neighbors)
        {
            ProcessNeighbor(grid, ref pathGrid, neighborIndex, ref searchedNodes, currentNode, currentNodeIndexValue,
                ref nodesToSearch);
        }
    }

    private static void ProcessNeighbor(in MapGrid grid, ref PathGrid pathGrid, int neighborIndex, ref HashSet<int> searchedNodes,
        in PathNode currentNode, int currentNodeIndexValue, ref PriorityQueue<int, int> nodesToSearch)
    {
        ref PathNode neighborNode = ref pathGrid.GetNodeRef(neighborIndex);
        if (searchedNodes.Contains(neighborIndex))
        {
            return;
        }

        if (grid.GetNodeRef(neighborIndex).TryGetAsRegularNode(out bool isWall))
        {
            if (isWall)
            {
                searchedNodes.Add(neighborIndex);
                return;
            }
        }

        int candidateCostFromStart = currentNode.CostFromStart + PathNode.UnitWalkingCost;

        if (candidateCostFromStart >= neighborNode.CostFromStart)
        {
            return;
        }

        neighborNode.CameFromNodeIndex = Option<int>.Some(currentNodeIndexValue);
        neighborNode.CostFromStart = candidateCostFromStart;
        neighborNode.ComputeTotalCost();
        
        //TODO (Santiago Firpo) replace hashset with binary heap and remove/reinsert changed nodes.

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