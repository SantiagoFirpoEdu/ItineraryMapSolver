using ItineraryMapSolver.MapLoading;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Pathfinding;

Console.WriteLine("Please insert the path to the map file: ");
string? mapFilePath = Console.ReadLine();

if (mapFilePath is not null)
{
    var loadMapResult = MapLoader.LoadGrid(mapFilePath);

    if (loadMapResult.TryGetOkValue(out MapGrid grid))
    {
        Console.WriteLine(grid.DebugPrint());

        var destinations = grid.Destinations.Keys;

        var pathResult = AStarSolver.SolvePath(grid, grid.Destinations[4], grid.Destinations[8]);

        if (pathResult.WasSuccessful())
        {
            Console.WriteLine("Found a path: ");
            List<IntVector> path = pathResult.GetOkValueUnsafe();

            foreach (IntVector pathPosition in path)
            {
                Console.WriteLine(pathPosition);
            }
            
            Console.WriteLine(grid.DebugPrintPath(path.ToHashSet()));
        }
        else
        {
            Console.WriteLine("No path exists");
        }
    }
    else
    {
        Console.Error.WriteLine($"Error while processing map: {loadMapResult.GetErrorValueUnsafe()}");
    }
}
