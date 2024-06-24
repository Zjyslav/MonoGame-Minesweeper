using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.State;
public class GameState
{
    public List<TileState> TileStates { get; } = new List<TileState>();
    public GameState(int rows, int cols, int bombs)
    {
        if (bombs > rows * cols)
        {
            throw new ArgumentException("You cannot have more bombs than tiles.", nameof(bombs));
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                TileStates.Add(new TileState(r, c));
            }
        }
        var tilesWithBombs = TileStates
            .OrderBy(t => Random.Shared.Next())
            .Take(bombs);

        foreach (var tile in tilesWithBombs)
        {
            tile.HasBomb = true;
        }

        foreach (var tile in TileStates)
        {
            tile.LinkWithAdjacent(TileStates);
        }
    }
}
