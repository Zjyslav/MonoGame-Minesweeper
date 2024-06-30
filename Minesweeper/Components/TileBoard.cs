using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Services;
using Minesweeper.State;
using System.Collections.Generic;

namespace Minesweeper.Components;
public class TileBoard : DrawableGameComponent
{
    GameStateProvider _gameStateProvider;
    GameState _gameState;
    List<Tile> _tiles = new();

    public Vector2 DrawLocation { get; }

    public TileBoard(Game game, Vector2 drawLocation) : base(game)
    {
        DrawLocation = drawLocation;
    }

    public override void Initialize()
    {
        _gameStateProvider = Game.Services.GetService<GameStateProvider>();
        _gameStateProvider.GameRestarted += OnGameRestarted;
        _gameState = _gameStateProvider.ActiveGameState;

        GenerateTiles();

        base.Initialize();
    }

    private void OnGameRestarted(object sender, EventArgs.RestartedEventArgs e)
    {
        _gameState = e.NewGameState;
        GenerateTiles();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (var tile in _tiles)
        {
            tile.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var tile in _tiles)
        {
            tile.Draw(gameTime);
        }
        base.Draw(gameTime);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }
    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    private void GenerateTiles()
    {
        _tiles.Clear();

        foreach (var tileState in _gameState.TileStates)
        {
            Tile tile = new(Game, tileState, this);
            tile.Initialize();
            _tiles.Add(tile);
        }
    }
}
