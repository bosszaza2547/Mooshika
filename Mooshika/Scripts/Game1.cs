using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        Texture2D Health;
        SpriteFont Font;

        Player Player;
        Vector2 campos;

        List<MeleeEnemy> MeleeEnemies = new List<MeleeEnemy>();
        List<RangedEnemy> RangedEnemies = new List<RangedEnemy>();
        Texture2D RangedEnemyProjectile;


        Dictionary<Vector2, int> TileMap;
        Dictionary<Vector2, int> CollisionMap;
        Dictionary<Vector2, int> EnemyMap;

        Texture2D Tile;
        Texture2D CollisionTile;

        
        int scalesize = 2;
        int tilesize = 40;

        List<Rectangle> Tiles = new List<Rectangle>();
        List<Rectangle> Platforms = new List<Rectangle>();
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Window.AllowUserResizing = false;

            TileMap = LoadMap("../../../Map Data/untitled_Tile Layer 1.csv");
            CollisionMap = LoadMap("../../../Map Data/untitled_collision.csv");
            EnemyMap = LoadMap("../../../Map Data/untitled_enemy.csv");
            tilesize = tilesize * scalesize;

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
            BackGround = Content.Load<Texture2D>("Sprites/bggg");
            Health = Content.Load<Texture2D>("Sprites/PlayerHealthBar");

            Player = new Player(Content.Load<Texture2D>("Sprites/Player_SpriteSheet"), new Vector2(200, 200), new Vector2(48*scalesize, 32*scalesize), Color.White, Window, pixel);

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
            RangedEnemyProjectile = Content.Load<Texture2D>("Sprites/rangeenemyprojectile");


            Tile = Content.Load<Texture2D>("TileMap/Tile");
            CollisionTile = Content.Load<Texture2D>("TileMap/tilecollision");
            int collisionrow = 2;
            foreach (var item in CollisionMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize, (int)item.Key.Y * tilesize, tilesize, tilesize);
                if (item.Value % collisionrow == 0)
                Tiles.Add(rectangle);
                if (item.Value % collisionrow == 1)
                Platforms.Add(rectangle);
            }
            foreach (var enemy in EnemyMap)
            {
                Rectangle rectangle = new Rectangle((int)enemy.Key.X * tilesize, (int)enemy.Key.Y * tilesize, tilesize, tilesize);
                if (enemy.Value % collisionrow == 0)
                    MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/enemy (2)"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(75, 75), Color.White, Window, 1));
                if (enemy.Value % collisionrow == 1)
                    RangedEnemies.Add(new RangedEnemy(Content.Load<Texture2D>("Sprites/rangeenemy"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(75, 75), Color.White, Window, -1, RangedEnemyProjectile));
            }
            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Player.Update(gameTime,campos,Tiles,Platforms, MeleeEnemies, RangedEnemies);
            if (Player.Position.X - Window.ClientBounds.Width / 2 > 0 && Player.Position.X - Window.ClientBounds.Width / 2 < 1120)
                campos.X = Player.Position.X - Window.ClientBounds.Width / 2 ;
            //if (Player.Position.Y+Player.hitbox.Height/2 - Window.ClientBounds.Height / 2 < -80 && Player.Position.Y + Player.hitbox.Height / 2 - Window.ClientBounds.Height / 2 > 800)
            if(Player.Position.Y - Window.ClientBounds.Height/2 > -60 && Player.Position.Y - Window.ClientBounds.Height /2 < 840)    
                campos.Y = Player.Position.Y - Window.ClientBounds.Height / 2;
            //Player.Update(gameTime, Walls, Platforms, MeleeEnemies, RangedEnemies);
            Debug.Write(Window.ClientBounds.Height / 2);
            Debug.WriteLine(Player.Position.Y - Window.ClientBounds.Height / 2);
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Update(gameTime, Player, Tiles, Platforms, campos);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Update(gameTime, Player, Tiles, Platforms, campos);
            }
            MeleeEnemies.RemoveAll((Enemy) => Enemy.Health <= 0);
            RangedEnemies.RemoveAll((Enemy) => Enemy.Health <= 0);

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Debug.WriteLine(campos);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            SpriteBatch.Draw(BackGround, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            //SpriteBatch.Draw(BackGround,Vector2.Zero,new Rectangle (0,0,Window.ClientBounds.Width,Window.ClientBounds.Height),Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            /*foreach (var Wall in Walls)
            {
                Wall.Draw(SpriteBatch);
            }
            foreach (var Platform in Platforms)
            {
                Platform.Draw(SpriteBatch);
            }*/
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Draw(SpriteBatch);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Draw(SpriteBatch);
            }



            Player.Draw(SpriteBatch);
            //SpriteBatch.DrawString(Font, "Health : " + Player.Health, Vector2.Zero, Color.White);

            
            int tilesize = 40*scalesize;
            int tilerow = 3;
            int tilepixel = 40;

            foreach (var item in TileMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);

                SpriteBatch.Draw(Tile, rectangle, sourcerectangle, Color.White);
            }

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
            SpriteBatch.Draw(pixel, new Vector2(132, 72), new Rectangle(0, 0, Player.Health * 300 / 60, 18), Color.DarkRed);
            SpriteBatch.Draw(Health, Vector2.Zero, new Rectangle(0,0,151,57), Color.White,0,Vector2.Zero,3f,SpriteEffects.None,1);
            

            SpriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();

            StreamReader reader = new(filepath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');

                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value > -1)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
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