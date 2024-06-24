using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.State;

namespace Minesweeper.Components;
public class Tile : DrawableGameComponent
{
    private readonly TileState _state;
    private TileBoard _board;
    private Texture2D _spriteSheet;
    private SpriteBatch _spriteBatch;

    public Tile(Game game, TileState state, TileBoard board) : base(game)
    {
        _state = state;
        _board = board;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
                    _spriteSheet,
                    new Vector2(_state.Col * 32 + _board.DrawLocation.X, _state.Row * 32 + _board.DrawLocation.Y),
                    GetSourceRectangle(),
                    Color.White
        );
        _spriteBatch.End();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new(Game.GraphicsDevice);
        _spriteSheet = Game.Content.Load<Texture2D>("minesweeper_spritesheet");
    }
    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    private Rectangle GetSourceRectangle()
    {
        if (_state.HasBomb)
        {
            return new(64, 64, 32, 32);
        }

        switch (_state.AdjacentBombs)
        {
            
            case 1:
                return new(64, 0, 32, 32);
            case 2:
                return new(96, 0, 32, 32);
            case 3:
                return new(0, 32, 32, 32);
            case 4:
                return new(32, 32, 32, 32);
            case 5:
                return new(64, 32, 32, 32);
            case 6:
                return new(96, 32, 32, 32);
            case 7:
                return new(0, 64, 32, 32);
            case 8:
                return new(32, 64, 32, 32);
            case 0:
            default:
                return new(32, 0, 32, 32);

        }
    }
}
