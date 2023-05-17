using ItineraryMapSolver.Model;
using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.MapLoading;

public static class MapLoader
{
    public static Result<MapGrid, EGridLoadError> LoadGrid(string gridFilePath)
    {
        try
        {
            string[] allLines = File.ReadAllLines(gridFilePath);

            if (allLines.Length < 2)
            {
                return Result<MapGrid, EGridLoadError>.Error(EGridLoadError.NotEnoughLines);
            }

            int mapWidth = allLines[1].Length;
            MapGrid grid = new(mapWidth, allLines.Length - 1);
            PopulateGrid(grid, allLines);
            return Result<MapGrid, EGridLoadError>.Ok(grid);
        }
        catch (IOException)
        {
            return Result<MapGrid, EGridLoadError>.Error(EGridLoadError.IoException);
        }
        catch (ArgumentException)
        {
            return Result<MapGrid, EGridLoadError>.Error(EGridLoadError.InvalidPath);
        }
        catch (UnauthorizedAccessException)
        {
            return Result<MapGrid, EGridLoadError>.Error(EGridLoadError.UnauthorizedAccess);
        }
    }

    private static void PopulateGrid(MapGrid grid, in string[] allLines)
    {
        for (int i = 1; i < allLines.Length; ++i)
        {
            PopulateColumn(grid, allLines, i);
        }
    }

    private static void PopulateColumn(MapGrid grid, string[] allLines, int i)
    {
        int y = i - 1;
        ref readonly string line = ref allLines[^i];

        for (int x = 0; x < line.Length; ++x)
        {
            PopulateLine(grid, line, x, y);
        }
    }

    private static void PopulateLine(MapGrid grid, string line, int x, int y)
    {
        char nodeCharacter = line[x];
        IntVector nodePosition = new(x, y);
        var nodeResult = MapNode.FromCharacter(nodeCharacter, nodePosition, grid.GetNeighbors(nodePosition));

        if (!nodeResult.TryGetOkValue(out MapNode node))
        {
            return;
        }

        if (node.TryGetAsHarbor(out int harborId))
        {
            grid.AddDestination(harborId, x, y);
        }

        grid.SetNode(node, x, y);
    }
}

public enum EGridLoadError
{
    NotEnoughLines,
    InvalidPath,
    IoException,
    UnauthorizedAccess
}