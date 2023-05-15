﻿using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.MapLoading;

public static class MapLoader
{
    public static async Task<Result<MapGrid, EGridLoadError>> LoadGrid(string gridFilePath)
    {
        try
        {
            string[] allLines = await File.ReadAllLinesAsync(gridFilePath);

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
            return Result<MapGrid, EGridLoadError>.Error(EGridLoadError.IOException);
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
            int y = i - 1;
            ref readonly string line = ref allLines[^i];

            for (int x = 0; x < line.Length; ++x)
            {
                char nodeCharacter = line[x];
                var nodeResult = MapNode.FromCharacter(nodeCharacter);

                if (!nodeResult.TryGetOkValue(out MapNode node))
                {
                    continue;
                }

                if (node.TryGetAsHarbor(out int harborId))
                {
                    grid.AddDestination(harborId, x, y);
                }
                grid.SetNode(node, x, y);
            }
        }
    }
}

public enum EGridLoadError
{
    NotEnoughLines,
    InvalidPath,
    IOException,
    UnauthorizedAccess
}