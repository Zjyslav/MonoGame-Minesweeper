using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.State;
using System.Collections.Generic;

namespace Minesweeper.Components;
public class TileBoard : DrawableGameComponent
{
    GameState _gameState;
    List<Tile> _tiles = new();

    public Vector2 DrawLocation { get; }

    public TileBoard(Game game, Vector2 drawLocation) : base(game)
    {
        DrawLocation = drawLocation;
    }

    public override void Initialize()
    {
        _gameState = Game.Services.GetService<GameState>();
        foreach (var tileState in _gameState.TileStates)
        {
            Tile tile = new(Game, tileState, this);
            tile.Initialize();
            _tiles.Add(tile);
        }

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
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
}
