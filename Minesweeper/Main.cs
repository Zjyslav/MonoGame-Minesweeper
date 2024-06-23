using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        _graphics.PreferredBackBufferHeight = 500;
        _graphics.PreferredBackBufferWidth = 500;
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
        _spriteBatch.Begin();

        int startingX = 100;
        int startingY = 100;

        int rows = 8;
        int cols = 8;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                _spriteBatch.Draw(
                    _tileSpriteSheet,
                    new Vector2(col*32 + startingX, row*32 + startingY),
                    new Rectangle(0, 0, 32, 32),
                    Color.White
                );
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
