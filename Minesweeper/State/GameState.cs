using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Minesweeper.State;
public class GameState
{
    public List<TileState> TileStates { get; } = new List<TileState>();
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public int BombsRemaining { get; private set; }
    public int BombsTotal { get; init; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public TimeSpan GameTime => _stopwatch.Elapsed;
    public event EventHandler GameStarted;
    public event EventHandler GameLost;
    public event EventHandler GameWon;

    private Stopwatch _stopwatch = new();
    public GameState(int rows, int cols, int bombs)
    {
        GameStarted += OnGameStarted;
        GameLost += OnGameLost;
        GameWon += OnGameWon;

        if (bombs > rows * cols)
        {
            throw new ArgumentException("You cannot have more bombs than tiles.", nameof(bombs));
        }

        Rows = rows;
        Cols = cols;
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
        TileState tile = sender as TileState;
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
        TileState tile = sender as TileState;
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
        TileState tile = sender as TileState;
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
        if (Status == GameStatus.NotStarted)
        {
            if (tile.HasBomb)
            {
                MoveBombToRandomEmptyTile(tile);
            }
            StartGame();
        }
        tile.Reveal();
        if (CheckWinCondition())
        {
            WinGame();
        }
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
        var bombs = tile.AdjacentBombs;
        var flags = tile.AdjacentTiles.Count(t => t.Status == TileStatus.Flagged);

        if (bombs != flags)
        {
            return;
        }

        foreach (var adjacent in tile.AdjacentTiles)
        {
            if (adjacent.Status == TileStatus.Hidden)
            {
                adjacent.Reveal();
            }
        }
        if (CheckWinCondition())
        {
            WinGame();
        }
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

    private void StartGame()
    {
        GameStarted?.Invoke(this, System.EventArgs.Empty);
    }

    private void LoseGame()
    {
        GameLost?.Invoke(this, System.EventArgs.Empty);
    }
    private void WinGame()
    {
        GameWon?.Invoke(this, System.EventArgs.Empty);
    }

    private void OnGameStarted(object sender, System.EventArgs e)
    {
        Status = GameStatus.Started;
        _stopwatch.Start();
    }

    private void OnGameLost(object sender, System.EventArgs e)
    {
        Status = GameStatus.Lost;
        _stopwatch.Stop();
        foreach (TileState tile in TileStates)
        {
            tile.RevealOnGameLost();
        }
    }
    private void OnGameWon(object sender, System.EventArgs e)
    {
        Status = GameStatus.Won;
        _stopwatch.Stop();
        // TODO highscores
    }

    private void MoveBombToRandomEmptyTile(TileState tileWithBomb)
    {
        TileState emptyTile = TileStates
            .Where(t => t.HasBomb == false)
            .OrderBy(t => Random.Shared.Next())
            .First();

        tileWithBomb.HasBomb = false;
        emptyTile.HasBomb = true;
    }
    private bool CheckWinCondition()
    {
        if (Status == GameStatus.Lost)
        {
            return false;
        }

        return TileStates.All(tile =>
            tile.Status == TileStatus.Revealed ||
            tile.HasBomb);
    }
}

public enum GameStatus
{
    NotStarted,
    Started,
    Won,
    Lost,
}