using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mooshika.Scripts
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;

        Texture2D BackGround;
        Texture2D pixel;
        SpriteFont Font;

        Player Player;

        List<Wall> Walls = new List<Wall>();
        List<Platform> Platforms = new List<Platform>();
        List<MeleeEnemy> MeleeEnemies = new List<MeleeEnemy>();
        List<RangedEnemy> RangedEnemies = new List<RangedEnemy>();
        Texture2D RangedEnemyProjectile;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Fonts/Font");

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            BackGround = Content.Load<Texture2D>("Sprites/Sark");

            Player = new Player(Content.Load<Texture2D>("Sprites/stick"), new Vector2 (200, 200), new Vector2 (75, 75), Color.White, Window, pixel);

            Walls.Add(new Wall(pixel, new Vector2 (-50, Window.ClientBounds.Height - 20 - 300), new Vector2(100 ,300) ,Color.DarkGreen, Window));
            Walls.Add(new Wall(pixel, new Vector2 (Window.ClientBounds.Width - 70 -100, Window.ClientBounds.Height - 20 - 300), new Vector2(100 ,300) ,Color.DarkGreen, Window));
            Walls.Add(new Wall(pixel, new Vector2 (Window.ClientBounds.Width - 70 -70 -100, Window.ClientBounds.Height - 20 - 75 - 20-100), new Vector2(200 ,20) ,Color.DarkGreen, Window));


            Platforms.Add(new Platform(pixel, new Vector2 (Window.ClientBounds.Width - 70 -70 -100 - 400, Window.ClientBounds.Height - 20 - 75 - 20 -300), new Vector2(300 ,20) ,Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2 (Window.ClientBounds.Width - 70 -70 -100 - 150 - 400 -100, Window.ClientBounds.Height - 20 - 75 - 20-400), new Vector2(150 ,20) ,Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2 (Window.ClientBounds.Width - 70 -70 -100 - 150, Window.ClientBounds.Height - 20 - 75 - 20-100), new Vector2(100 ,20) ,Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2 (Window.ClientBounds.Width - 70 -70 -100 - 150 - 400, Window.ClientBounds.Height - 20 - 75 - 20-100), new Vector2(100 ,20) ,Color.Brown, Window));

            MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/meleeenemy"),new Vector2 (500, 0), new Vector2 (75, 75), Color.White, Window,1));
            MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/meleeenemy"),new Vector2 (800, 200), new Vector2 (75, 75), Color.White, Window,-1));

            RangedEnemyProjectile = Content.Load<Texture2D>("Sprites/rangeenemyprojectile");
            RangedEnemies.Add(new RangedEnemy(Content.Load<Texture2D>("Sprites/rangeenemy"),new Vector2 (900, 0), new Vector2 (75, 75), Color.White, Window,-1,RangedEnemyProjectile));

            // TODO: use this.Content to load your game content here
            
        }

        protected override void Update(GameTime gameTime)
        {
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Player.Update(gameTime, Walls, Platforms, MeleeEnemies,RangedEnemies);
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Update(gameTime, Player, Walls, Platforms);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Update(gameTime, Player, Walls, Platforms);
            }
            MeleeEnemies.RemoveAll ((Enemy) => Enemy.Health <= 0);
            RangedEnemies.RemoveAll ((Enemy) => Enemy.Health <= 0);
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteBatch.Draw(BackGround,new Rectangle (0,0,Window.ClientBounds.Width,Window.ClientBounds.Height),Color.White);
            //SpriteBatch.Draw(BackGround,Vector2.Zero,new Rectangle (0,0,Window.ClientBounds.Width,Window.ClientBounds.Height),Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            foreach (var Wall in Walls)
            {
                Wall.Draw(SpriteBatch);
            }
            foreach (var Platform in Platforms)
            {
                Platform.Draw(SpriteBatch);
            }
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Draw(SpriteBatch);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Draw(SpriteBatch);
            }

            Player.Draw(SpriteBatch);
            SpriteBatch.DrawString(Font,"Health : " + Player.Health, Vector2.Zero, Color.White);

            SpriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
