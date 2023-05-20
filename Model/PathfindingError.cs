using ItineraryMapSolver.Pathfinding;

namespace ItineraryMapSolver.Model;

public readonly record struct PathfindingError(IntVector From, IntVector To, EPathfindingError Cause);