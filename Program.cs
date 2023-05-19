using ItineraryMapSolver.MapLoading;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Pathfinding;

Console.WriteLine("Please insert the path to the map file: ");
string? mapFilePath = Console.ReadLine();

if (mapFilePath is not null)
{
    var result = MapLoader.LoadGrid(mapFilePath);

    if (result.TryGetOkValue(out MapGrid grid))
    {
        Console.WriteLine(grid.DebugPrint());

        var path = AStarSolver.SolvePath(grid, grid.Destinations[6], grid.Destinations[8]);

        if (path != null)
        {
            Console.WriteLine("Found a path: ");
            foreach (IntVector pathPosition in path)
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
