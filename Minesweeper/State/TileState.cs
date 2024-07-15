using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.State;
public class TileState
{
    public int Row { get; }
    public int Col { get; }
    public bool HasBomb { get; set; } = false;
    public TileStatus Status { get; set; } = TileStatus.Hidden;
    public bool RevealingAdjacent { get; set; } = false;
    public List<TileState> AdjacentTiles { get; private set; } = new();
    public bool ToBeRevealed => AdjacentTiles.Any(t => t.RevealingAdjacent);
    public int AdjacentBombs => AdjacentTiles.Where(t => t.HasBomb).Count();

    public event EventHandler RMBClicked;
    public event EventHandler LMBClicked;
    public event EventHandler BothMBClicked;
    public event EventHandler Exploded;

    public TileState(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void LinkWithAdjacent(IEnumerable<TileState> tiles)
    {
        AdjacentTiles = tiles
            .Where(t => t.Row >= Row - 1 && t.Row <= Row + 1)
            .Where(t => t.Col >= Col - 1 && t.Col <= Col + 1)
            .Where(t => t != this)
            .ToList();
    }

    public void LMBClick()
    {
        LMBClicked?.Invoke(this, System.EventArgs.Empty);
    }
    public void RMBClick()
    {
        RMBClicked?.Invoke(this, System.EventArgs.Empty);
    }
    public void BothMBClick()
    {
        BothMBClicked?.Invoke(this, System.EventArgs.Empty);
    }

    public void Reveal()
    {
        if (Status != TileStatus.Hidden &&
            Status != TileStatus.Flagged)
        {
            return;
        }

        if (HasBomb)
        {
            Explode();
            return;
        }

        Status = TileStatus.Revealed;

        if (AdjacentBombs == 0)
        {
            RevealAdjacent();
        }
    }

    public void RevealOnGameLost()
    {
        if (Status == TileStatus.Flagged)
        {
            Status = HasBomb ? TileStatus.Flagged : TileStatus.WronglyFlagged;
            return;
        }
        Status = TileStatus.Revealed;
    }

    public void ToggleFlag()
    {
        Status = Status switch
        {
            TileStatus.Hidden => TileStatus.Flagged,
            TileStatus.Flagged => TileStatus.Hidden,
            _ => Status,
        };
    }

    private void Explode()
    {
        Exploded?.Invoke(this, System.EventArgs.Empty);
        Status = TileStatus.Exploded;
    }

    private void RevealAdjacent()
    {
        foreach (TileState tile in AdjacentTiles)
        {
            tile.Reveal();
        }
    }
}

public enum TileStatus
{
    Hidden,
    Revealed,
    MouseDown,
    Flagged,
    Exploded,
    WronglyFlagged,
}