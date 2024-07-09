using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Extensions;
using Minesweeper.Services;
using Minesweeper.State;

namespace Minesweeper.Components;
public class Panel : DrawableGameComponent
{
    private GameStateProvider _gameStateProvider;
    private GameState _gameState;
    private SpriteBatch _spriteBatch;
    private Texture2D _spriteSheet;
    private SpriteFont _spriteFont;

    private const int _tileWidth = 32;
    private const int _tileHeight = 32;
    private bool _mouseDown = false;
    public Panel(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        _gameStateProvider = Game.Services.GetService<GameStateProvider>();
        _gameStateProvider.GameRestarted += OnGameRestarted;
        _gameState = _gameStateProvider.ActiveGameState;
    }

    private void OnGameRestarted(object sender, EventArgs.RestartedEventArgs e)
    {
        _gameState = e.NewGameState;
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin();
        Vector2 facePosition = GetFacePosition();
        Vector2 bombsScreenPosition = GetBombsScreenPosition();
        Vector2 timeScreenPosition = GetTimeScreenPosition();

        DrawBlackScreen(bombsScreenPosition);
        DrawBlackScreen(timeScreenPosition);

        DrawTile(GetFaceTile(), facePosition);

        _spriteBatch.DrawString(_spriteFont, GetBombText(), bombsScreenPosition + new Vector2(0.35f * _tileWidth, 0), Color.Red);
        _spriteBatch.DrawString(_spriteFont, GetTimeText(), timeScreenPosition + new Vector2(0.35f * _tileWidth, 0), Color.Red);

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        HandleLeftMouseButton(mouseState);
    }

    

    private bool IsMouseOver(MouseState mouseState)
    {
        var facePosition = GetFacePosition();
        var faceBounds = new Rectangle((int)facePosition.X,
                                       (int)facePosition.Y,
                                       _tileWidth,
                                       _tileHeight);
        bool mouseOver = mouseState.Position.IsInBounds(faceBounds);
        return mouseOver;
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new(Game.GraphicsDevice);
        _spriteSheet = Game.Content.Load<Texture2D>("minesweeper_spritesheet");
        _spriteFont = Game.Content.Load<SpriteFont>("PanelFont");
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    private void HandleLeftMouseButton(MouseState mouseState)
    {
        bool mouseOver = IsMouseOver(mouseState);
        if (mouseOver == false)
        {
            _mouseDown = false;
            return;
        }

        if (_mouseDown == false && mouseState.LeftButton == ButtonState.Pressed)
        {
            _mouseDown = true;
            return;
        }

        if (_mouseDown && mouseState.LeftButton == ButtonState.Released)
        {
            _gameStateProvider.RestartGame();
        }
    }

    private void DrawBlackScreen(Vector2 startingPosition)
    {
        DrawTile(PanelTile.UpperLeftCorner, new Vector2(0 * _tileWidth, 0 * _tileHeight) + startingPosition);
        DrawTile(PanelTile.HorizontalTop, new Vector2(1 * _tileWidth, 0 * _tileHeight) + startingPosition);
        DrawTile(PanelTile.UpperRightCorner, new Vector2(2 * _tileWidth, 0 * _tileHeight) + startingPosition);
        DrawTile(PanelTile.BottomLeftCorner, new Vector2(0 * _tileWidth, 1 * _tileHeight) + startingPosition);
        DrawTile(PanelTile.HorizontalBottom, new Vector2(1 * _tileWidth, 1 * _tileHeight) + startingPosition);
        DrawTile(PanelTile.BottomRightCorner, new Vector2(2 * _tileWidth, 1 * _tileHeight) + startingPosition);
    }

    private Vector2 GetTimeScreenPosition()
    {
        return new((_gameState.Cols - 2.25f) * _tileWidth, 1.5f * _tileHeight);
    }

    private static Vector2 GetBombsScreenPosition()
    {
        return new(1.25f * _tileWidth, 1.5f * _tileHeight);
    }

    private Vector2 GetFacePosition()
    {
        return new(((_gameState.Cols + 1) * _tileWidth) / 2, 2 * _tileHeight);
    }

    private void DrawTile(PanelTile tile, Vector2 position)
    {
        _spriteBatch.Draw(
                    _spriteSheet,
                    position,
                    GetSourceRectangle(tile),
                    Color.White
        );
    }

    private Rectangle GetSourceRectangle(PanelTile tile)
    {
        return tile switch
        {
            PanelTile.UpperLeftCorner => new(26 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.HorizontalTop => new(27 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.UpperRightCorner => new(28 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.RightHoriziontal => new(29 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.LeftHorizontal => new(30 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.BottomLeftCorner => new(31 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.HorizontalBottom => new(32 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.BottomRightCorner => new(33 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.Inside => new(34 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.FaceNeutral => new(35 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.FaceWin => new(36 * _tileWidth, 0, _tileWidth, _tileHeight),
            PanelTile.FaceLoss => new(37 * _tileWidth, 0, _tileWidth, _tileHeight),
            _ => throw new System.NotImplementedException(),
        };
    }

    private string GetBombText()
    {
        string format = (_gameState.BombsRemaining > 99 || _gameState.BombsRemaining < 0) ? "00" : "000";
        string output = _gameState.BombsRemaining.ToString(format);

        if (output.Length > 3)
        {
            return "ERR";
        }
        return output;
    }

    private string GetTimeText()
    {
        if (_gameState.Status == GameStatus.NotStarted)
        {
            return "---";
        }

        double seconds = _gameState.GameTime.TotalSeconds;
        double minutes = _gameState.GameTime.TotalMinutes;
        double hours = _gameState.GameTime.TotalHours;
        double days = _gameState.GameTime.TotalDays;

        if (seconds <= 999)
        {
            return seconds.ToString("000");
        }
        else if (minutes <= 99)
        {
            return $"{minutes.ToString("00")}M";
        }
        else if (hours <= 99)
        {
            return $"{hours.ToString("00")}H";
        }
        else if (days <= 99)
        {
            return $"{days.ToString("00")}D";
        }
        return "ERR";
    }

    private PanelTile GetFaceTile()
    {
        return _gameState.Status switch
        {
            GameStatus.Won => PanelTile.FaceWin,
            GameStatus.Lost => PanelTile.FaceLoss,
            _ => PanelTile.FaceNeutral,
        };
    }

    private enum PanelTile
    {
        UpperLeftCorner,
        HorizontalTop,
        UpperRightCorner,
        RightHoriziontal,
        LeftHorizontal,
        BottomLeftCorner,
        HorizontalBottom,
        BottomRightCorner,
        Inside,
        FaceNeutral,
        FaceWin,
        FaceLoss,
    }
}
