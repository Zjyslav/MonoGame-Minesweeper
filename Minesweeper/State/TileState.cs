using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.State;
public class TileState
{
    public int Row { get; }
    public int Col { get; }
    public bool HasBomb { get; set; } = false;

    public int AdjacentBombs => _adjacent.Where(t => t.HasBomb).Count();

    private List<TileState> _adjacent = new();

    public TileState(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void LinkWithAdjacent(IEnumerable<TileState> tiles)
    {
        _adjacent = tiles
            .Where(t => t.Row >= Row - 1 && t.Row <= Row + 1)
            .Where(t => t.Col >= Col - 1 && t.Col <= Col + 1)
            .Where(t => t != this)
            .ToList();
    }
}
