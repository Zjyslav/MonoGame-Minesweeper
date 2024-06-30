using Minesweeper.State;

namespace Minesweeper.EventArgs;
public class RestartedEventArgs
{
    public GameState NewGameState { get; set; }
}
