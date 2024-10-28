using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Mooshika.Scripts
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        Texture2D pixel;
        StageTest StageTest = new StageTest();
        BeforePrayanak BeforePrayanak = new BeforePrayanak();
        TestBoss TestBoss = new TestBoss();
        TitleScreen TitleScreen = new TitleScreen();
        Menu Menu = new Menu();
        Map Map = new Map();
        String Scene = "Title Screen";
        String preScene = String.Empty;
        String PreviousScene;
        Canva canva;
        int Width = 480, Height = 270;
        float Scale = 1.0f;
        Vector2 Offset = Vector2.Zero;
        bool isresizing = false;
        bool menu = false;
        bool premenu = false;
        KeyboardState KeyboardState, KeyboardState2;
        MouseState mouseState, mouseState2;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!isresizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
            {
                isresizing = true;
                setresolution(Window.ClientBounds.Width, Window.ClientBounds.Height);
                isresizing = false;
            }
        }

        public void setresolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
            canva.setscreensize();
            Scale = canva.getscale();
            Offset = canva.getoffset();
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            canva = new(Graphics.GraphicsDevice, Width, Height);
            setresolution(Width, Height);
            setresolution(960, 540);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            TitleScreen.LoadContent(Content);
            Menu.LoadContent(Content);
            /*Walls.Add(new Wall(pixel, new Vector2(-50, Window.ClientBounds.Height - 20 - 300), new Vector2(100, 300), Color.DarkGreen, Window));
            Walls.Add(new Wall(pixel, new Vector2(Window.ClientBounds.Width - 70 - 100, Window.ClientBounds.Height - 20 - 300), new Vector2(100, 300), Color.DarkGreen, Window));
            Walls.Add(new Wall(pixel, new Vector2(Window.ClientBounds.Width - 70 - 70 - 100, Window.ClientBounds.Height - 20 - 75 - 20 - 100), new Vector2(200, 20), Color.DarkGreen, Window));


            Platforms.Add(new Platform(pixel, new Vector2(Window.ClientBounds.Width - 70 - 70 - 100 - 400, Window.ClientBounds.Height - 20 - 75 - 20 - 300), new Vector2(300, 20), Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2(Window.ClientBounds.Width - 70 - 70 - 100 - 150 - 400 - 100, Window.ClientBounds.Height - 20 - 75 - 20 - 400), new Vector2(150, 20), Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2(Window.ClientBounds.Width - 70 - 70 - 100 - 150, Window.ClientBounds.Height - 20 - 75 - 20 - 100), new Vector2(100, 20), Color.Brown, Window));
            Platforms.Add(new Platform(pixel, new Vector2(Window.ClientBounds.Width - 70 - 70 - 100 - 150 - 400, Window.ClientBounds.Height - 20 - 75 - 20 - 100), new Vector2(100, 20), Color.Brown, Window));

            MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/meleeenemy"), new Vector2(500, 0), new Vector2(75, 75), Color.White, Window, 1));
            MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/meleeenemy"), new Vector2(800, 200), new Vector2(75, 75), Color.White, Window, -1));

            RangedEnemyProjectile = Content.Load<Texture2D>("Sprites/rangeenemyprojectile");
            RangedEnemies.Add(new RangedEnemy(Content.Load<Texture2D>("Sprites/rangeenemy"), new Vector2(900, 0), new Vector2(75, 75), Color.White, Window, -1, RangedEnemyProjectile));*/

            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Menu.KeyboardState = KeyboardState;
            Menu.KeyboardState2 = KeyboardState2;
            Menu.mouseState = mouseState;
            Menu.mouseState2 = mouseState2;
            TitleScreen.mouseState = mouseState;
            TitleScreen.mouseState2 = mouseState2;
            Map.mouseState = mouseState;
            Map.mouseState2 = mouseState2;
            StageTest.KeyboardState = KeyboardState;
            StageTest.KeyboardState2 = KeyboardState2;
            BeforePrayanak.KeyboardState = KeyboardState;
            BeforePrayanak.KeyboardState2 = KeyboardState2;
            TestBoss.KeyboardState = KeyboardState;
            TestBoss.KeyboardState2 = KeyboardState2;
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                setresolution(480, 270);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                setresolution(960, 270);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                setresolution(960, 540);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                setresolution(2000, 1000);
            }
            if (!menu)
            {
                switch (Scene)
                {
                    case "Title Screen":
                        {
                            if (PreviousScene != Scene)
                            {
                                PreviousScene = Scene;
                                TitleScreen.Scene = "Title Screen";
                            }
                            else
                            {
                                TitleScreen.menu = false;
                                TitleScreen.Update(gameTime, this, Offset, Scale);
                                Scene = TitleScreen.Scene;
                                menu = TitleScreen.menu;
                            }
                            break;
                        }
                    case "Map":
                        {
                            if (PreviousScene != Scene)
                            {
                                PreviousScene = Scene;
                                Map.Scene = "Map";
                                Map.LoadContent(Content);
                            }
                            else
                            {
                                Map.Update(gameTime, this, Offset, Scale);
                                Scene = Map.Scene;
                            }
                            break;
                        }
                    case "Stage Test":
                        {
                            if (PreviousScene != Scene || Menu.retry)
                            {
                                StageTest.LoadContent(Content, Window, pixel, GraphicsDevice);
                                PreviousScene = Scene;
                            }
                            else
                            {
                                StageTest.menu = false;
                                StageTest.Update(gameTime, Window);
                                Scene = StageTest.Scene;
                                menu = StageTest.menu;
                            }

                            //Scene = StageTest.Scene;
                            break;
                        }
                    case "Before Prayanak":
                        {
                            if (PreviousScene != Scene || Menu.retry)
                            {
                                BeforePrayanak.LoadContent(Content, Window, pixel, GraphicsDevice);
                                PreviousScene = Scene;
                            }
                            else
                            {
                                BeforePrayanak.menu = false;
                                BeforePrayanak.Update(gameTime, Window);
                                Scene = BeforePrayanak.Scene;
                                menu = BeforePrayanak.menu;
                            }

                            //Scene = StageTest.Scene;
                            break;
                        }
                    case "TestBoss":
                        {
                            if (PreviousScene != Scene || Menu.retry)
                            {
                                TestBoss.LoadContent(Content, Window, pixel, GraphicsDevice);
                                PreviousScene = Scene;
                            }
                            else
                            {
                                TestBoss.menu = false;
                                TestBoss.Update(gameTime, Window);
                                Scene = TestBoss.Scene;
                                menu = TestBoss.menu;
                            }

                            //Scene = StageTest.Scene;
                            break;
                        }
                }
                Menu.retry = false;
            }
            else
            {
                Menu.menu = menu;
                Menu.PreScene = Scene;
                Menu.Update(gameTime, this, Offset, Scale);
                Scene = Menu.Scene;
                menu = Menu.menu;
            }

            //Debug.WriteLine(Scene);
            //Debug.WriteLine(Scene);

            // TODO: Add your update logic here
            //intersections = getIntersectingTilesHorizontal(Player.Rectangle);

            /*foreach (var rectangle in intersections)
            {
                
                if (CollisionMap.TryGetValue(new Vector2(rectangle.X, rectangle.Y), out int value))
                {
                    Rectangle collision = new(
                    rectangle.X * tilesize,
                    rectangle.Y * tilesize,
                    tilesize,
                    tilesize
                );
                    if (Player.Velocity.X > 0.0f)
                    {
                        Player.Position.X = collision.Left - Player.Rectangle.Width;
                    }
                    else if (Player.Velocity.X < 0.0f)
                    {
                        Player.Position.X = collision.Right;
                    }
                }

                
            }

            //intersections = getIntersectingTilesVertical(Player.Rectangle);
            foreach (var rectangle in intersections)
            {
                
                if (CollisionMap.TryGetValue(new Vector2(rectangle.X, rectangle.Y), out int value))
                {
                    Rectangle collision = new(
                    rectangle.X * tilesize,
                    rectangle.Y * tilesize,
                    tilesize,
                    tilesize
                );
                    if (Player.Velocity.Y > 0.0f)
                    {
                        Player.Position.Y = collision.Top - Player.Rectangle.Height;
                    }
                    else if (Player.Velocity.Y < 0.0f)
                    {
                        Player.Position.Y = collision.Bottom;
                    }
                }
            }*/
            /*for (int i = 0; i < Tiles.Count; i++) 
            {
                Tiles[i] = ((int)campos.X, (int)campos.Y;
            }
            foreach (var item in Platforms)
            {
                
            }*/
            premenu = menu;
            KeyboardState2 = KeyboardState;
            mouseState2 = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Debug.WriteLine(campos);
            canva.setscreen();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (PreviousScene == Scene)
            {
                switch (Scene)
                {
                    case "Title Screen":
                        {
                            TitleScreen.Draw(SpriteBatch, Window);
                            break;
                        }
                    case "Map":
                        {
                            Map.Draw(SpriteBatch, Window);
                            break;
                        }
                    case "Stage Test":
                        {
                            StageTest.Draw(SpriteBatch, Window, pixel);
                            break;
                        }
                    case "Before Prayanak":
                        {
                            BeforePrayanak.Draw(SpriteBatch, Window, pixel);
                            break;
                        }
                    case "TestBoss":
                        {
                            TestBoss.Draw(SpriteBatch, Window, pixel);
                            break;
                        }
                }
            }
            if (menu)
            {
                SpriteBatch.Draw(pixel, Vector2.Zero, new Rectangle(0, 0, 480, 270), Color.Black * 0.75f);
                Menu.Draw(SpriteBatch, Window, pixel);
            }
                //SpriteBatch.Draw(BackGround,Vector2.Zero,new Rectangle (0,0,Window.ClientBounds.Width,Window.ClientBounds.Height),Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                /*foreach (var Wall in Walls)
                {
                    Wall.Draw(SpriteBatch);
                }
                foreach (var Platform in Platforms)
                {
                    Platform.Draw(SpriteBatch);
                }*/

                //SpriteBatch.DrawString(Font, "Health : " + Player.Health, Vector2.Zero, Color.White);




                /*tilerow = 2;
                foreach (var item in CollisionMap)
                {
                    Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                    int x = item.Value % tilerow; 
                    int y = item.Value / tilerow;

                    Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);

                    SpriteBatch.Draw(CollisionTile, rectangle, sourcerectangle, Color.White);
                }*/
                /*foreach (var rect in intersections)
                {

                    DrawRectHollow(
                        SpriteBatch,
                        new Rectangle(
                            rect.X * tilesize,
                            rect.Y * tilesize,
                            tilesize,
                            tilesize
                        ),
                        4
                    );

                }*/



                SpriteBatch.End();
            canva.Draw(SpriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        

        /*public List<Rectangle> getIntersectingTilesHorizontal (Rectangle target)
        {
            List<Rectangle> intersections = new();
            int widthInTiles = (target.Width - (target.Width % tilesize))/tilesize;
            int heightInTiles = (target.Height - (target.Height % tilesize)) / tilesize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Rectangle((target.X + x * tilesize) / tilesize, (target.Y + y * tilesize-1) / tilesize, tilesize, tilesize));
                }
            }
            return intersections;
        }
        public List<Rectangle> getIntersectingTilesVertical(Rectangle target)
        {
            
            List<Rectangle> intersections = new();
            int widthInTiles = (target.Width - (target.Width % tilesize)) / tilesize;
            int heightInTiles = (target.Height - (target.Height % tilesize)) / tilesize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    Debug.WriteLine(y);
                    intersections.Add(new Rectangle((target.X + x * tilesize-1) / tilesize, (target.Y + y * tilesize) / tilesize, tilesize, tilesize));
                }
            }
            return intersections;
        }

        public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness)
        {
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    rect.X,
                    rect.Bottom - thickness,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    rect.Right - thickness,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
        }*/

    }
}