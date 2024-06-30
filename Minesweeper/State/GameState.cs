using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.State;
public class GameState
{
    public List<TileState> TileStates { get; } = new List<TileState>();
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public int BombsRemaining { get; private set; }
    public int BombsTotal { get; init; }
    public event EventHandler GameLost;
    
    public GameState(int rows, int cols, int bombs)
    {
        GameLost += OnGameLost;

        if (bombs > rows * cols)
        {
            throw new ArgumentException("You cannot have more bombs than tiles.", nameof(bombs));
        }

        BombsRemaining = bombs;
        BombsTotal = bombs;

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
            tile.RMBClicked += OnTileRMBClicked;
            tile.LMBClicked += OnTileLMBClicked;
            tile.BothMBClicked += OnTileBothMBClicked;
            tile.Exploded += OnTileExploded;
        }
    }

    private void OnTileRMBClicked(object sender, System.EventArgs e)
    {
        TileState? tile = sender as TileState;
        if (tile is null)
        {
            throw new InvalidOperationException($"{nameof(OnTileRMBClicked)} invoked by object of type {sender.GetType()}, not of type {typeof(TileState)}.");
        }

        switch (tile.Status)
        {
            case TileStatus.Hidden:
            case TileStatus.MouseDown:
                HandleHiddenTileRMBClick(tile);
                break;
            case TileStatus.Revealed:
                HandleRevealedTileRMBClick(tile);
                break;
            case TileStatus.Flagged:
                HandleFlaggedTileRMBClick(tile);
                break;
        }
    }
    private void OnTileLMBClicked(object sender, System.EventArgs e)
    {
        TileState? tile = sender as TileState;
        if (tile is null)
        {
            throw new InvalidOperationException($"{nameof(OnTileRMBClicked)} invoked by object of type {sender.GetType()}, not of type {typeof(TileState)}.");
        }

        switch (tile.Status)
        {
            case TileStatus.Hidden:
            case TileStatus.MouseDown:
                HandleHiddenTileLMBClick(tile);
                break;
            case TileStatus.Revealed:
                HandleRevealedTileLMBClick(tile);
                break;
            case TileStatus.Flagged:
                HandleFlaggedTileLMBClick(tile);
                break;
        }
    }

    private void OnTileBothMBClicked(object sender, System.EventArgs e)
    {
        TileState? tile = sender as TileState;
        if (tile is null)
        {
            throw new InvalidOperationException($"{nameof(OnTileBothMBClicked)} invoked by object of type {sender.GetType()}, not of type {typeof(TileState)}.");
        }

        switch (tile.Status)
        {
            case TileStatus.Hidden:
            case TileStatus.MouseDown:
                HandleHiddenTileBothMBClick(tile);
                break;
            case TileStatus.Revealed:
                HandleRevealedTileBothMBClick(tile);
                break;
            case TileStatus.Flagged:
                HandleFlaggedTileBothMBClick(tile);
                break;
        }
    }

    private void HandleHiddenTileRMBClick(TileState tile)
    {
        tile.ToggleFlag();
        BombsRemaining--;
    }
    private void HandleHiddenTileLMBClick(TileState tile)
    {
        tile.Reveal();
    }
    private void HandleHiddenTileBothMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }

    private void HandleRevealedTileLMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }
    private void HandleRevealedTileRMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }
    private void HandleRevealedTileBothMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }

    private void HandleFlaggedTileLMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }
    private void HandleFlaggedTileRMBClick(TileState tile)
    {
        tile.ToggleFlag();
        BombsRemaining++;
    }
    private void HandleFlaggedTileBothMBClick(TileState tile)
    {
        throw new NotImplementedException();
    }

    private void OnTileExploded(object sender, System.EventArgs e)
    {
        LoseGame();
    }

    private void LoseGame()
    {
        GameLost?.Invoke(this, System.EventArgs.Empty);
    }

    private void OnGameLost(object sender, System.EventArgs e)
    {
        Status = GameStatus.Lost;
        foreach (TileState tile in TileStates)
        {
            tile.RevealOnGameLost();
        }
    }
}

public enum GameStatus
{
    NotStarted,
    Started,
    Won,
    Lost,
}