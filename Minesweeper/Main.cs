using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Components;
using Minesweeper.State;

namespace Minesweeper;
public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D _tileSpriteSheet;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        int rows = 16;
        int cols = 30;
        int bombs = 99;

        int margins = 50;

        GameState gameState = new(rows, cols, bombs);
        Services.AddService(gameState);
        Vector2 tileBoardDrawLocation = new(margins, margins);
        TileBoard tileBoard = new(this, tileBoardDrawLocation);
        Components.Add(tileBoard);

        _graphics.PreferredBackBufferHeight = margins * 2 + rows * 32;
        _graphics.PreferredBackBufferWidth = margins * 2 + cols * 32;
        _graphics.ApplyChanges();

        Window.AllowUserResizing = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _tileSpriteSheet = Content.Load<Texture2D>("minesweeper_spritesheet");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(243,243,243));

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}
