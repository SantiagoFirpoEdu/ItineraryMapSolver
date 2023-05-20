using ItineraryMapSolver.MapLoading;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Monads;
using ItineraryMapSolver.Pathfinding;

int SortByHarborId(KeyValuePair<int, IntVector> x1, KeyValuePair<int, IntVector> x2)
{
     return x1.Key - x2.Key;
}

Console.WriteLine("Please insert the path to the map file: ");
string? mapFilePath = Console.ReadLine();

if (mapFilePath is not null)
{
    var loadMapResult = MapLoader.LoadGrid(mapFilePath);

    if (loadMapResult.TryGetOkValue(out MapGrid grid))
    {
        Console.WriteLine(grid.DebugPrint());

        bool needsToRetry = false;
        var allDestinations = grid.Destinations.ToList();
        allDestinations.Sort(SortByHarborId);
        do
        {
            int pathsCount = allDestinations.Count;
            var tasks = new List<Task<Result<List<IntVector>, PathfindingError>>>(pathsCount);
            for (int index = 0; index < pathsCount; index++)
            {
                (int _, IntVector fromPosition) = allDestinations[index];
                int nextIndex = index < pathsCount - 1 ? index + 1 : 0;
                (int _, IntVector toPosition) = allDestinations[nextIndex];

                var gridRef = new ReadonlyRef<MapGrid>(grid);

                var pathfindingTask = AStarSolver.SolvePathAsync(gridRef, fromPosition, toPosition);
                tasks.Add(pathfindingTask);

            }

            var pathfindingResults = await Task.WhenAll(tasks);

            foreach (var pathfindingResult in pathfindingResults)
            {
                if (pathfindingResult.WasSuccessful())
                {
                    var pathResult = pathfindingResult.GetOkValueUnsafe();
                    var fromHarborId = grid.GetHarborId(pathResult[0]);
                    var toHarborId = grid.GetHarborId(pathResult[^1]);
                    
                    Console.WriteLine($"Found a path between harbor {fromHarborId.GetValue()} at position {pathResult[0]} and harbor {toHarborId.GetValue()} at position {pathResult[^1]}: ");

                    Console.WriteLine(grid.DebugPrintPath(pathResult.ToHashSet()));
                }
                else
                {
                    (IntVector from, IntVector to, EPathfindingError _) = pathfindingResult.GetErrorValueUnsafe();
                    int toHarborId = grid.GetHarborId(to).GetValue();
                    Console.WriteLine($"No path exists between harbor {grid.GetHarborId(from).GetValue()} at {from} and harbor {toHarborId} at {to}");
                    allDestinations.Remove(new KeyValuePair<int, IntVector>(toHarborId, to));
                    needsToRetry = true;
                }
            }
        }
        while (needsToRetry);
    }
    else
    {
        Console.Error.WriteLine($"Error while processing map: {loadMapResult.GetErrorValueUnsafe()}");
    }
}
