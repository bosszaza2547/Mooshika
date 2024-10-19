using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mooshika.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Mooshika.Scripts
{
    internal class StageTest : GameScene
    {
        public String Scene = "Stage Test";
        Texture2D BackGround;
        
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

        KeyboardState KeyboardState, KeyboardState2;
        int scalesize = 2;
        int tilesize = 40;

        List<Rectangle> Tiles = new List<Rectangle>();
        List<Rectangle> Platforms = new List<Rectangle>();
        public void LoadContent(ContentManager Content, GameWindow Window, Texture2D pixel)
        {
            Scene = "Stage Test";
            scalesize = 2;
            tilesize = 40;
            TileMap = LoadMap("../../../Map Data/untitled_Tile Layer 1.csv");
            CollisionMap = LoadMap("../../../Map Data/untitled_collision.csv");
            EnemyMap = LoadMap("../../../Map Data/untitled_enemy.csv");
            /*TileMap = LoadMap("Map Data/untitled_Tile Layer 1.csv");
            CollisionMap = LoadMap("Map Data/untitled_collision.csv");
            EnemyMap = LoadMap("Map Data/untitled_enemy.csv");*/
            tilesize = tilesize * scalesize;
            MeleeEnemies = new List<MeleeEnemy>();
            RangedEnemies = new List<RangedEnemy>();


            int collisionrow = 2;
            foreach (var item in CollisionMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize, (int)item.Key.Y * tilesize, tilesize, tilesize);
                if (item.Value % collisionrow == 0)
                    Tiles.Add(rectangle);
                if (item.Value % collisionrow == 1)
                    Platforms.Add(rectangle);
            }
            RangedEnemyProjectile = Content.Load<Texture2D>("Sprites/rangeenemyprojectile");
            foreach (var enemy in EnemyMap)
            {
                Rectangle rectangle = new Rectangle((int)enemy.Key.X * tilesize, (int)enemy.Key.Y * tilesize, tilesize, tilesize);
                if (enemy.Value % collisionrow == 0)
                    MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/enemy (2)"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(75, 75), Color.White, Window, 1));
                if (enemy.Value % collisionrow == 1)
                    RangedEnemies.Add(new RangedEnemy(Content.Load<Texture2D>("Sprites/rangeenemy"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(75, 75), Color.White, Window, -1, RangedEnemyProjectile));
            }

            Font = Content.Load<SpriteFont>("Fonts/Font");
            BackGround = Content.Load<Texture2D>("Sprites/bggg");
            Health = Content.Load<Texture2D>("Sprites/PlayerHealthBar");

            Player = new Player(Content.Load<Texture2D>("Sprites/Player_SpriteSheet"), new Vector2(200, 200), new Vector2(48 * scalesize, 32 * scalesize), Color.White, Window, pixel);


            campos.X = Player.Position.X - Window.ClientBounds.Width / 2;

            Tile = Content.Load<Texture2D>("TileMap/Tile");
            CollisionTile = Content.Load<Texture2D>("TileMap/tilecollision");
        }
        public void Update(GameTime gameTime , GameWindow Window)
        {
            KeyboardState = Keyboard.GetState();
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Player.Update(gameTime, campos, Tiles, Platforms, MeleeEnemies, RangedEnemies);
            if (!Player.LockCamera)
            {
                campos.X = Player.Position.X - Window.ClientBounds.Width / 2;
            }
            
            //if (Player.Position.Y+Player.hitbox.Height/2 - Window.ClientBounds.Height / 2 < -80 && Player.Position.Y + Player.hitbox.Height / 2 - Window.ClientBounds.Height / 2 > 800)
            if (Player.Position.Y - Window.ClientBounds.Height / 2 > -60 && Player.Position.Y - Window.ClientBounds.Height / 2 < 840)
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
            if (KeyboardState.IsKeyDown(Keys.N))
            {
                Scene = "Title Screen";
            }
            if (KeyboardState.IsKeyDown(Keys.L) && !KeyboardState2.IsKeyDown(Keys.L))
            {
                Player.LockCamera = !Player.LockCamera;
            }
            KeyboardState2 = KeyboardState;
        }
        public void Draw(SpriteBatch spriteBatch,GameWindow Window, Texture2D pixel)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(pixel, new Vector2(132, 72), new Rectangle(0, 0, Player.Health * 300 / 60, 18), Color.DarkRed);
            spriteBatch.Draw(Health, Vector2.Zero, new Rectangle(0, 0, 151, 57), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 1);

            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Draw(spriteBatch);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Draw(spriteBatch);
            }



            Player.Draw(spriteBatch);

            int tilesize = 40 * scalesize;
            int tilerow = 3;
            int tilepixel = 40;

            foreach (var item in TileMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);

                spriteBatch.Draw(Tile, rectangle, sourcerectangle, Color.White);
            }
        }
    }
}
