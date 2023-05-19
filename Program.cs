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

        var path = AStarSolver.SolvePath(grid, grid.Destinations[1], grid.Destinations[2]);

        if (path.WasSuccessful())
        {
            Console.WriteLine("Found a path: ");
            var okValueUnsafe = path.GetOkValueUnsafe();
            foreach (IntVector pathPosition in okValueUnsafe)
            {
                Console.WriteLine(pathPosition);
            }
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
