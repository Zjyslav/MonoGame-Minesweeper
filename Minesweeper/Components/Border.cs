using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.EventArgs;
using Minesweeper.Services;
using Minesweeper.State;

namespace Minesweeper.Components;
public class Border : DrawableGameComponent
{
    private Texture2D _spriteSheet;
    private SpriteBatch _spriteBatch;
    private GameStateProvider _gameStateProvider;
    private GameState _gameState;
    private int _panelHeightInTiles = 3;

    private const int _tileWidth = 32;
    private const int _tileHeight = 32;
    private readonly Vector2 _drawLocation;

    public Border(Game game, Vector2 drawLocation) : base(game)
    {
        _drawLocation = drawLocation;
    }

    public override void Initialize()
    {
        base.Initialize();
        _gameStateProvider = Game.Services.GetService<GameStateProvider>();
        _gameStateProvider.GameRestarted += OnGameRestarted;
        _gameState = _gameStateProvider.ActiveGameState;
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _spriteBatch.Begin();

        float topRowY = 0;
        float lastColX = _tileWidth * (_gameState.Cols + 1);
        float lastRowY = _tileHeight * (_gameState.Rows + _panelHeightInTiles + 2);

        // Top row
        DrawTile(BorderTile.UpperLeftCorner, _drawLocation + new Vector2(0, topRowY));
        for (int col = 1; col <= _gameState.Cols; col++)
        {
            DrawTile(BorderTile.HorizontalTop, _drawLocation + new Vector2(_tileWidth * col, topRowY));
        }
        DrawTile(BorderTile.UpperRightCorner, _drawLocation + new Vector2(lastColX, topRowY));

        // Panel border and background
        for (int row = 1; row <= _panelHeightInTiles; row++)
        {
            DrawTile(BorderTile.VerticalLeft, _drawLocation + new Vector2(0, _tileHeight * row));
            for (int col = 1; col <= _gameState.Cols; col++)
            {
                DrawTile(BorderTile.Inside, _drawLocation + new Vector2(_tileWidth * col, _tileHeight * row));
            }
            DrawTile(BorderTile.VerticalRight, _drawLocation + new Vector2(lastColX, _tileHeight * row));
        }

        // Row between Panel and Board
        DrawTile(BorderTile.LeftT, _drawLocation + new Vector2(0, _tileHeight * (_panelHeightInTiles + 1)));
        for (int col = 1; col <= _gameState.Cols; col++)
        {
            DrawTile(BorderTile.HorizontalTop, _drawLocation + new Vector2(_tileWidth * col, _tileHeight * (_panelHeightInTiles + 1)));
        }
        DrawTile(BorderTile.RightT, _drawLocation + new Vector2(lastColX, _tileHeight * (_panelHeightInTiles + 1)));

        // Board border
        for (int row = 1; row <= _gameState.Rows; row++)
        {
            DrawTile(BorderTile.VerticalLeft, _drawLocation + new Vector2(0, _tileHeight * (row + _panelHeightInTiles + 1)));
            DrawTile(BorderTile.VerticalRight, _drawLocation + new Vector2(lastColX, _tileHeight * (row + _panelHeightInTiles + 1)));
        }

        // Bottom row
        DrawTile(BorderTile.BottomLeftCorner, _drawLocation + new Vector2(0, lastRowY));
        for (int col = 1; col <= _gameState.Cols; col++)
        {
            DrawTile(BorderTile.HorizontalBottom, _drawLocation + new Vector2(_tileWidth * col, lastRowY));
        }
        DrawTile(BorderTile.BottomRightCorner, _drawLocation + new Vector2(lastColX, lastRowY));


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
    private void OnGameRestarted(object sender, RestartedEventArgs e)
    {
        _gameState = e.NewGameState;
    }

    private void DrawTile(BorderTile tile, Vector2 position)
    {
        _spriteBatch.Draw(
                    _spriteSheet,
                    position,
                    GetSourceRectangle(tile),
                    Color.White
        );
    }

    private Rectangle GetSourceRectangle(BorderTile tile)
    {
        return tile switch
        {
            BorderTile.UpperLeftCorner => new(15 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.HorizontalTop => new(16 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.UpperRightCorner => new(17 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.VerticalRight => new(18 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.VerticalLeft => new(19 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.BottomLeftCorner => new(20 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.BottomRightCorner => new(21 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.LeftT => new(22 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.RightT => new(23 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.HorizontalBottom => new(24 * _tileWidth, 0, _tileWidth, _tileHeight),
            BorderTile.Inside => new(25 * _tileWidth, 0, _tileWidth, _tileHeight),
            _ => throw new System.NotImplementedException(),
        };
    }

    private enum BorderTile
    {
        UpperLeftCorner,
        HorizontalTop,
        UpperRightCorner,
        VerticalRight,
        VerticalLeft,
        BottomLeftCorner,
        BottomRightCorner,
        LeftT,
        RightT,
        HorizontalBottom,
        Inside,
    }
}
