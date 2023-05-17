using System.Diagnostics;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Pathfinding;

public static class AStarSolver
{
    public static List<IntVector>? SolvePath(in MapGrid grid, in IntVector from, in IntVector to)
    {
        PathGrid pathGrid = new(grid.Width, grid.Height, to);
        HashSet<int> nodesToSearch = new();
        HashSet<int> searchedNodes = new();
        ref PathNode initialNode = ref pathGrid.GetNodeRef(pathGrid.ComputeIndex(from));

        initialNode.CostFromStart = 0;
        initialNode.ComputeTotalCost();

        int? endNodeIndex = pathGrid.GetNode(to).Index;

        Debug.Assert(initialNode.Index != null, "initialNode.Index != null");
        nodesToSearch.Add((int) initialNode.Index);
        while (nodesToSearch.Count > 0)
        {
            var currentNodeIndex = GetLowestTotalCost(nodesToSearch, pathGrid);

            if (currentNodeIndex.IsEmpty())
            {
                throw new InvalidOperationException("could not peek open list even though it is not empty");
            }

            int currentNodeIndexValue = currentNodeIndex.GetValue();

            if (currentNodeIndexValue == endNodeIndex)
            {
                break;
            }

            nodesToSearch.Remove(currentNodeIndexValue);

            searchedNodes.Add(currentNodeIndexValue);

            ref readonly PathNode currentNode = ref pathGrid.GetNodeRef(currentNodeIndexValue);

            var neighbors = currentNode.Neighbors;

            if (neighbors == null)
            {
                continue;
            }

            foreach (int neighborIndex in neighbors)
            {
                ProcessNeighbor(grid, ref pathGrid, neighborIndex, ref searchedNodes, currentNode, currentNodeIndexValue, ref nodesToSearch);
            }
        }

        Debug.Assert(endNodeIndex != null, nameof(endNodeIndex) + " != null");
        ref readonly PathNode pathNode = ref pathGrid.GetNodeRef((int) endNodeIndex);
        return pathNode.CameFromNodeIndex.IsSet()
            //Found a path
            ? TraceBackPath(pathGrid, pathNode)

            //Did not find a path
            : null;

    }

    private static void ProcessNeighbor(in MapGrid grid, ref PathGrid pathGrid, int neighborIndex, ref HashSet<int> searchedNodes,
        in PathNode currentNode, int currentNodeIndexValue, ref HashSet<int> nodesToSearch)
    {

        ref PathNode neighborNode = ref pathGrid.GetNodeRef(neighborIndex);
        if (searchedNodes.Contains(neighborIndex))
        {
            return;
        }

        if (!grid.GetNodeRef(neighborIndex).TryGetAsRegularNode(out bool isWall))
        {
            return;
        }

        if (isWall)
        {
            searchedNodes.Add(neighborIndex);
            return;
        }

        int candidateCostFromStart = currentNode.CostFromStart + PathNode.UnitWalkingCost;

        if (candidateCostFromStart >= neighborNode.CostFromStart)
        {
            return;
        }

        neighborNode.CameFromNodeIndex = Option<int>.Some(currentNodeIndexValue);
        neighborNode.CostFromStart = candidateCostFromStart;
        neighborNode.ComputeTotalCost();

        nodesToSearch.Add(neighborIndex);
    }

    private static List<IntVector>? TraceBackPath(in PathGrid pathGrid, in PathNode endNode)
    {
        if (endNode.CameFromNodeIndex.IsEmpty())
        {
            return null;
        }

        var path = new List<IntVector>();

        ref readonly PathNode currentNode = ref endNode;
        var currentNodeCameFromNodeIndex = currentNode.CameFromNodeIndex;
        while (currentNodeCameFromNodeIndex.IsSet())
        {
            int cameFromNodeIndex = currentNodeCameFromNodeIndex.GetValue();
            PathNode cameFromNode = pathGrid.GetNode(cameFromNodeIndex);
            if (cameFromNode.Position is null)
            {
                continue;
            }

            path.Add((IntVector) cameFromNode.Position);
            currentNode = ref pathGrid.GetNodeRefReadonly(cameFromNodeIndex);
        }

        path.Reverse();

        return path;
    }

    private static Option<int> GetLowestTotalCost(in HashSet<int> nodesToSearch, in PathGrid grid)
    {
        var currentLowest = Option<int>.None();
        foreach (int nodeIndex in nodesToSearch)
        {
            ref readonly PathNode node = ref grid.GetNodeRefReadonly(nodeIndex);

            if (currentLowest.IsEmpty() || node.TotalCost < currentLowest.GetValue())
            {
                currentLowest = Option<int>.Some(nodeIndex);
            }
        }

        return currentLowest;
    }
}