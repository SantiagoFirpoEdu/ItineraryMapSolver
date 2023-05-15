using ItineraryMapSolver.MapLoading;
using ItineraryMapSolver.Model;

Console.WriteLine("Please insert the path to the map file: ");
string? path = Console.ReadLine();

if (path is not null)
{
    var result = await MapLoader.LoadGrid(path);

    if (result.TryGetOkValue(out MapGrid grid))
    {
        Console.WriteLine(grid.DebugPrint());
    }
}
