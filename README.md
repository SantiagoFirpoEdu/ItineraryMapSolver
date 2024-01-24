# Itinerary Map Solver

This is is a simple pathfinding algorithm that can handle multiple destinations in a sequence. It uses the A* algorithm to find optimal paths. The implementation takes cache locality into consideration by using plain structs instead of heap-allocated objects. By allocating the grid data contiguously, cache misses are reduced.

Graded assignment for the Algorithms anda Data Structures II course, which is part of the Software Engineering course at PUC-RS.
