using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.State;
using System;

namespace Minesweeper.Components;
public class Tile : DrawableGameComponent
{
    private readonly TileState _state;
    private TileBoard _board;
    private Texture2D _spriteSheet;
    private SpriteBatch _spriteBatch;
    private Vector2 _tilePosition;
    private bool _leftMouseButtonDown = false;
    private bool _rightMouseButtonDown = false;

    private const int _tileWidth = 32;
    private const int _tileHeight = 32;

    public Tile(Game game, TileState state, TileBoard board) : base(game)
    {
        _state = state;
        _board = board;
    }

    public override void Initialize()
    {
        base.Initialize();

        _tilePosition = new Vector2(_state.Col * 32 + _board.DrawLocation.X, _state.Row * 32 + _board.DrawLocation.Y);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        HandleMouseButtons();
    }

    private void HandleMouseButtons()
    {
        var mouseState = Mouse.GetState();

        HandleLeftMouseButton(mouseState);
        HandleRightMouseButton(mouseState);
    }

    private void HandleLeftMouseButton(MouseState mouseState)
    {
        if (_leftMouseButtonDown == false)
        {
            if (mouseState.LeftButton == ButtonState.Pressed &&
                IsMouseOver(mouseState.Position) &&
                _state.Status == TileStatus.Hidden)
            {
                _leftMouseButtonDown = true;
            }
        }
        else
        {
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _state.LMBClick();
                _leftMouseButtonDown = false;
            }
            if (mouseState.LeftButton == ButtonState.Pressed &&
                IsMouseOver(mouseState.Position) == false)
            {
                _leftMouseButtonDown = false;
            }
        }
    }

    private void HandleRightMouseButton(MouseState mouseState)
    {
        if (_rightMouseButtonDown == false)
        {
            if (mouseState.RightButton == ButtonState.Pressed &&
                IsMouseOver(mouseState.Position) &&
                (_state.Status == TileStatus.Hidden || _state.Status == TileStatus.Flagged))
            {
                _rightMouseButtonDown = true;
            }
        }
        else
        {
            if (mouseState.RightButton == ButtonState.Released)
            {
                _state.RMBClick();
                _rightMouseButtonDown = false;
            }
            if (mouseState.RightButton == ButtonState.Pressed &&
                IsMouseOver(mouseState.Position) == false)
            {
                _rightMouseButtonDown = false;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
                    _spriteSheet,
                    _tilePosition,
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
        if (_leftMouseButtonDown)
        {
            return new(1 * _tileWidth, 0, _tileWidth, _tileHeight);
        }

        if (_state.Status == TileStatus.Hidden)
        {
            return new(0, 0, _tileWidth, _tileHeight);
        }

        if (_state.Status == TileStatus.Exploded)
        {
            return new(3 * _tileWidth, 0, _tileWidth, _tileHeight);
        }

        if (_state.Status == TileStatus.WronglyFlagged)
        {
            return new(4 * _tileWidth, 0, _tileWidth, _tileHeight);
        }

        if (_state.Status == TileStatus.Flagged)
        {
            return new(5 * _tileWidth, 0, _tileWidth, _tileHeight);
        }

        if (_state.HasBomb)
        {
            return new(2 * _tileWidth, 0, _tileWidth, _tileHeight);
        }

        switch (_state.AdjacentBombs)
        {
            
            case 1:
                return new(7 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 2:
                return new(8 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 3:
                return new(9 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 4:
                return new(10 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 5:
                return new(11 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 6:
                return new(12 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 7:
                return new(13 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 8:
                return new(14 * _tileWidth, 0, _tileWidth, _tileHeight);
            case 0:
            default:
                return new(1 * _tileWidth, 0, _tileWidth, _tileHeight);

        }
    }

    private bool IsMouseOver(Point mousePosition)
    {
        return mousePosition.X >= _tilePosition.X &&
               mousePosition.X < _tilePosition.X + _tileWidth &&
               mousePosition.Y >= _tilePosition.Y &&
               mousePosition.Y < _tilePosition.Y + _tileHeight;
    }
}
