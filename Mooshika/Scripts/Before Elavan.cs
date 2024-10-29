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
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework.Audio;

namespace Mooshika.Scripts
{
    internal class BeforeElavan : GameScene
    {

        GraphicsDevice GraphicsDevice;
        
        const int MapWidth = 2400;
        const int MapHeight = 800;
        

        public String Scene = "Before Elavan";
        Texture2D BackGround;
        
        Texture2D Health;
        Texture2D ItemsIcons;
        SpriteFont Font;

        public Player Player;
        Vector2 campos;

        List<MeleeEnemy> MeleeEnemies = new List<MeleeEnemy>();
        List<RangedEnemy> RangedEnemies = new List<RangedEnemy>();
        Texture2D RangedEnemyProjectile;
        Texture2D PlayerProjectile;

        List<Items> Items = new List<Items>();
        Texture2D Item;

        Texture2D tutor,potion;
        Vector2 tutorpos;

        Dictionary<Vector2, int> TileMap;
        Dictionary<Vector2, int> TileMap2;
        Dictionary<Vector2, int> CollisionMap;
        Dictionary<Vector2, int> EnemyMap;
        Dictionary<Vector2, int> TileMapp;
        Dictionary<Vector2, int> ItemsMap;

        Texture2D Tile;
        Texture2D Tile2;
        Texture2D CollisionTile;
        Texture2D DecoTile;
        

        public KeyboardState KeyboardState, KeyboardState2;
        int scalesize = 1;
        int tilesize = 40;

        List<Rectangle> Tiles = new List<Rectangle>();
        List<Rectangle> Platforms = new List<Rectangle>();
        public bool menu = false;
        public void LoadContent(ContentManager Content, GameWindow Window, Texture2D pixel, GraphicsDevice graphicsDevice)
        {

            GraphicsDevice = graphicsDevice;
            

            Scene = "Before Elavan";
            scalesize = 1;
            tilesize = 40;
            TileMap = LoadMap("Map Data/Elavan_Stage_tile.csv");
            TileMapp = LoadMap("Map Data/Elavan_Stage_decoration.csv");
            CollisionMap = LoadMap("Map Data/Elavan_Stage_collsion.csv");
            ItemsMap = LoadMap("Map Data/Elavan_Stage_item.csv");
            EnemyMap = LoadMap("Map Data/Elavan_Stage_enemy.csv");
            /*TileMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Tile.csv");
            TileMapp = LoadMap("../../../Map Data/Platform_Stage_Prayanak_DecoTile.csv");
            TileMappp = LoadMap("../../../Map Data/Platform_Stage_Prayanak_DecoTile2.csv");
            CollisionMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Collision.csv");
            ItemsMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Items.csv");
            EnemyMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Enemy.csv");*/

            /*TileMap = LoadMap("Map Data/untitled_Tile Layer 1.csv");
            CollisionMap = LoadMap("Map Data/untitled_collision.csv");
            EnemyMap = LoadMap("Map Data/untitled_enemy.csv");*/
            tilesize = tilesize * scalesize;
            MeleeEnemies = new List<MeleeEnemy>();
            RangedEnemies = new List<RangedEnemy>();
            Items = new List<Items>();


            int row = 2;
            foreach (var item in CollisionMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize, (int)item.Key.Y * tilesize, tilesize, tilesize);
                if (item.Value % row == 0)
                    Tiles.Add(rectangle);
                if (item.Value % row == 1)
                    Platforms.Add(rectangle);
            }
            RangedEnemyProjectile = Content.Load<Texture2D>("Sprites/rangeenemyprojectile");
            row = 3;
            foreach (var enemy in EnemyMap)
            {
                Rectangle rectangle = new Rectangle((int)enemy.Key.X * tilesize, (int)enemy.Key.Y * tilesize, tilesize, tilesize);
                if (enemy.Value % row == 0)
                    MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/monster_walk_sheet"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(64, 59), Color.White, Window, 1, 1, 100, 1));
                if (enemy.Value % row == 1)
                    RangedEnemies.Add(new RangedEnemy(Content.Load<Texture2D>("Sprites/monster_walk_sheet"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(64, 63), Color.White, Window, -1, RangedEnemyProjectile, 1, 50));
                if (enemy.Value % row == 2)
                    MeleeEnemies.Add(new MeleeEnemy(Content.Load<Texture2D>("Sprites/monster_walk_sheet"), new Vector2(enemy.Key.X * tilesize, enemy.Key.Y * tilesize), new Vector2(64, 59), Color.White, Window, 1, 2, 60, 2));
            }
            row = 2;
            foreach (var item in ItemsMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize, (int)item.Key.Y * tilesize, tilesize, tilesize);
                if (item.Value % row == 0)
                    Items.Add(new Items(Content.Load<Texture2D>("Sprites/Items"), new Vector2(item.Key.X * tilesize, item.Key.Y * tilesize), new Vector2(18, 18), Color.White, Window, 1));
                if (item.Value % row == 1)
                    Items.Add(new Items(Content.Load<Texture2D>("Sprites/Items"), new Vector2(item.Key.X * tilesize, item.Key.Y * tilesize), new Vector2(18, 18), Color.White, Window, 2));
            }

            Font = Content.Load<SpriteFont>("Fonts/Font");
            BackGround = Content.Load<Texture2D>("Sprites/bggg");
            Health = Content.Load<Texture2D>("Sprites/PlayerHealthBar");
            ItemsIcons = Content.Load<Texture2D>("Sprites/ItemsSlot");

            

            PlayerProjectile = Content.Load<Texture2D>("Sprites/player_S_Atk_EF");
            Player = new Player(Content.Load<Texture2D>("Sprites/Player_SpriteSheet"), new Vector2(1*40, 3*40), new Vector2(112 * scalesize, 54 * scalesize), Color.White, Window, pixel, PlayerProjectile);

            Player.power = "Double Jump";
            campos.X = Player.Position.X - 480 / 2;

            Tile = Content.Load<Texture2D>("TileMap/Tiles");
            Tile2 = Content.Load<Texture2D>("TileMap/clouddddd");
            DecoTile = Content.Load<Texture2D>("TileMap/AssetsForestttt");
            CollisionTile = Content.Load<Texture2D>("TileMap/tilecollision");

            tutor = Content.Load<Texture2D>("Sprites/2xJump");
            potion = Content.Load<Texture2D>("Sprites/12Items");
            tutorpos = Player.Position + new Vector2(10,0);
        }
        public void Update(GameTime gameTime , GameWindow Window)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape) && !KeyboardState2.IsKeyDown(Keys.Escape))
            {
                menu = true;
            }
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Player.Update(gameTime, campos, Tiles, Platforms, MeleeEnemies, RangedEnemies,Items);
            if (!Player.LockCamera)
            {
                if (Player.Position.X > 240 && Player.Position.X < 2160)
                    campos.X = Player.Position.X - 480 / 2;
                else if (Player.Position.X < 240)
                    campos.X = 0;
                else if (Player.Position.X > 2160)
                    campos.X = 1920;
                if (Player.Position.Y > 135 && Player.Position.Y < 665)
                    campos.Y = Player.Position.Y - 270 / 2;
                else if (Player.Position.Y < 135)
                    campos.Y = 0;
                else if (Player.Position.Y > 665)
                    campos.Y = 530;
            }
            if(Player.Position.X > 2400)
            {
                Scene = ("ElavanBoss");
            }
            //Debug.WriteLine(campos);

            //if (Player.Position.Y+Player.hitbox.Height/2 - Window.ClientBounds.Height / 2 < -80 && Player.Position.Y + Player.hitbox.Height / 2 - Window.ClientBounds.Height / 2 > 800)

            //Player.Update(gameTime, Walls, Platforms, MeleeEnemies, RangedEnemies);
            /*Debug.Write(270 / 2);
            Debug.WriteLine(Player.Position.Y - 270 / 2);*/
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Update(gameTime, Player, Tiles, Platforms, campos);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Update(gameTime, Player, Tiles, Platforms, campos);
            }
            MeleeEnemies.RemoveAll((Enemy) => Enemy.Health <= 0 && Enemy.Position.Y > 270 + campos.Y);
            RangedEnemies.RemoveAll((Enemy) => Enemy.Health <= 0 && Enemy.Position.Y > 270 + campos.Y);
            Items.RemoveAll((item) => item.Rectangle.Intersects(Player.hitbox));
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                Scene = "Title Screen";
            }
            if (KeyboardState.IsKeyDown(Keys.L) && !KeyboardState2.IsKeyDown(Keys.L))
            {
                Player.LockCamera = !Player.LockCamera;
            }
        }
        public void Draw(SpriteBatch spriteBatch,GameWindow Window, Texture2D pixel)
        {

            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 480, 270), Color.White);
            
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
            tilerow = 18;
            foreach (var item in TileMapp)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);
                spriteBatch.Draw(DecoTile, rectangle, sourcerectangle, Color.White);
            }
            foreach (var MeleeEnemy in MeleeEnemies)
            {
                MeleeEnemy.Draw(spriteBatch);
            }
            foreach (var RangedEnemy in RangedEnemies)
            {
                RangedEnemy.Draw(spriteBatch);
            }
            foreach (var item in Items)
            {
                item.Draw(spriteBatch, campos);
            }





            spriteBatch.Draw(pixel, new Vector2(48, 25), new Rectangle(0, 0, (int)(Player.Health * 1.59f), 4), Color.DarkRed);
            //Debug.WriteLine(Mouse.GetState().Position);
            spriteBatch.Draw(Health, Vector2.Zero, new Rectangle(0, 0, 151, 57), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            spriteBatch.Draw(ItemsIcons, new Vector2(56,38),Color.White);
            spriteBatch.DrawString(Font, Player.potion1.ToString(), new Vector2(56, 38), Color.White,0,Vector2.Zero,0.25f,SpriteEffects.None,1);
            spriteBatch.DrawString(Font, Player.potion2.ToString(), new Vector2(73, 38), Color.White, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 1);
            
            spriteBatch.Draw (tutor,tutorpos -campos,Color.White);
            spriteBatch.Draw(potion, tutorpos - campos + new Vector2 (0 , 20), Color.White);
            Player.Draw(spriteBatch);
        }
    }
}
