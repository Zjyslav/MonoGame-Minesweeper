using Minesweeper.EventArgs;
using Minesweeper.State;
using System;

namespace Minesweeper.Services;
public class GameStateProvider
{
    public GameState ActiveGameState { get; private set; }
    public event EventHandler<RestartedEventArgs> GameRestarted;

    public GameStateProvider(int rows, int cols, int bombs)
    {
        ActiveGameState = new GameState(rows, cols, bombs);
    }

    public void RestartGame(int rows, int cols, int bombs)
    {
        ActiveGameState = new GameState(rows, cols, bombs);
        RestartedEventArgs e = new()
        {
            NewGameState = ActiveGameState
        };

        GameRestarted.Invoke(this, e);
    }

    public void RestartGame()
    {
        RestartGame(ActiveGameState.Rows, ActiveGameState.Cols, ActiveGameState.BombsTotal);
    }
}
