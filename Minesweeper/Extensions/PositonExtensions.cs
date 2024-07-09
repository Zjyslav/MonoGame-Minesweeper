using Microsoft.Xna.Framework;

namespace Minesweeper.Extensions;
public static class PositonExtensions
{
    public static bool IsInBounds(this Point position, Rectangle bounds)
    {
        return position.X >= bounds.X &&
               position.X < bounds.X + bounds.Width &&
               position.Y >= bounds.Y &&
               position.Y < bounds.Y + bounds.Height;
    }
}
