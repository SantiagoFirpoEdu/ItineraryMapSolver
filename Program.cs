using System.Text;
using ItineraryMapSolver.MapLoading;
using ItineraryMapSolver.Model;
using ItineraryMapSolver.Pathfinding;

namespace ItineraryMapSolver;

public static class Program
{
    public static void Main()
    {

        Console.WriteLine("Please insert the path to the map file: ");
        string? mapFilePath = Console.ReadLine();

        if (mapFilePath is null)
        {
            return;
        }

        var loadMapResult = MapLoader.LoadGrid(mapFilePath);

        if (loadMapResult.TryGetOkValue(out MapGrid grid))
        {
            Console.WriteLine(grid.DebugPrint());

            var allDestinations = grid.Destinations.ToList();
            allDestinations.Sort(SortByHarborId);

            List<IntVector> completeItinerary = new();

            for (int index = 0; index < allDestinations.Count;)
            {
                SolveHarborInItinerary(allDestinations, ref index, grid, completeItinerary);
            }
        
            Console.WriteLine("Complete itinerary path: ");
            Console.WriteLine(grid.DebugPrintPath(completeItinerary.ToHashSet()));

            StringBuilder itinerary = new();
            foreach (var destination in allDestinations)
            {
                itinerary.Append($"{destination.Key}");
                itinerary.Append(" -> ");
            }
            if (allDestinations.Count > 0)
            {
                itinerary.Append(itinerary[0]);
            }

            Console.WriteLine($"Total walking cost of itinerary ({itinerary}) is {completeItinerary.Count - 1}");
        }
        else
        {
            Console.Error.WriteLine($"Error while processing map: {loadMapResult.GetErrorValueUnsafe()}");
        }
    }

    private static void SolveHarborInItinerary(List<KeyValuePair<int, IntVector>> allDestinations, ref int index, in MapGrid grid, in List<IntVector> completeItinerary)
    {
        (int _, IntVector fromPosition) = allDestinations[index];
        int nextIndex = index < allDestinations.Count - 1 ? index + 1 : 0;
        (int _, IntVector toPosition) = allDestinations[nextIndex];

        var pathfindingResult = AStarSolver.SolvePath(grid, fromPosition, toPosition);

        if (pathfindingResult.WasSuccessful())
        {
            var pathfindingSolution = pathfindingResult.GetOkValueUnsafe();
            var fromHarborId = grid.GetHarborId(pathfindingSolution[0]);
            var toHarborId = grid.GetHarborId(pathfindingSolution[^1]);

            // Console.WriteLine($"Found a path between harbor {fromHarborId.GetValue()} at position {pathfindingSolution[0]} and harbor {toHarborId.GetValue()} at position {pathfindingSolution[^1]}: ");

            // Console.WriteLine(grid.DebugPrintPath(pathfindingSolution.ToHashSet()));

            completeItinerary.AddRange(pathfindingSolution);
            ++index;
        }
        else
        {
            (IntVector from, IntVector to, EPathfindingError _) = pathfindingResult.GetErrorValueUnsafe();
            int toHarborId = grid.GetHarborId(to).GetValue();
            // Console.WriteLine($"No path exists between harbor {grid.GetHarborId(from).GetValue()} at {from} and harbor {toHarborId} at {to}. Ignoring..");
            allDestinations.Remove(new KeyValuePair<int, IntVector>(toHarborId, to));
        }
    }

    private static int SortByHarborId(KeyValuePair<int, IntVector> x1, KeyValuePair<int, IntVector> x2)
    {
        return x1.Key - x2.Key;
    }
}