using Minesweeper.State;

namespace Minesweeper.Services;
public class GameStateProvider
{
    public GameState ActiveGameState { get; private set; }

    public GameStateProvider(int rows, int cols, int bombs)
    {
        ActiveGameState = new GameState(rows, cols, bombs);
    }

    public void Restart(int rows, int cols, int bombs)
    {
        ActiveGameState = new GameState(rows, cols, bombs);
    }
}
