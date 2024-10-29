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
    internal class PrayanakBoss : GameScene
    {

        GraphicsDevice GraphicsDevice;
        
        const int MapWidth = 480;
        const int MapHeight = 280;
        

        public String Scene = "PrayanakBoss";
        Texture2D BackGround;
        
        Texture2D Health;
        Texture2D ItemsIcons;
        SpriteFont Font;

        public Player Player;
        Vector2 campos;
        public MouseState mouseState, mouseState2;
        SoundEffect hit;

        Texture2D PlayerProjectile;
        Texture2D Prayanaktex;
        Texture2D water;
        Prayanak Prayanak;

        List<Items> Items = new List<Items>();
        Texture2D Item;

        Dictionary<Vector2, int> TileMap;
        /*Dictionary<Vector2, int> TileMapp;
        Dictionary<Vector2, int> TileMappp;*/
        Dictionary<Vector2, int> CollisionMap;
        /*Dictionary<Vector2, int> EnemyMap;
        Dictionary<Vector2, int> ItemsMap;*/

        Texture2D Tile;
        /*Texture2D DecoTile;
        Texture2D DecoTile2;*/
        Texture2D CollisionTile;

        public KeyboardState KeyboardState, KeyboardState2;
        int scalesize = 1;
        int tilesize = 40;

        List<Rectangle> Tiles = new List<Rectangle>();
        List<Rectangle> Platforms = new List<Rectangle>();
        public bool menu = false;
        public void LoadContent(ContentManager Content, GameWindow Window, Texture2D pixel, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            

            Scene = "PrayanakBoss";
            scalesize = 1;
            tilesize = 40;
            TileMap = LoadMap("Map Data/BossPrayanak_Tile.csv");
            CollisionMap = LoadMap("Map Data/BossPrayanak_Coliision.csv");

            /*TileMap = LoadMap("../../../Map Data/TestBoss_Tile.csv");
            CollisionMap = LoadMap("../../../Map Data/TestBoss_Coliision.csv");*/
            hit = Content.Load<SoundEffect>("Sounds/Normal_Atk");


            /*TileMapp = LoadMap("../../../Map Data/Platform_Stage_Prayanak_DecoTile.csv");
            TileMappp = LoadMap("../../../Map Data/Platform_Stage_Prayanak_DecoTile2.csv");*/

            /*ItemsMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Items.csv");
            EnemyMap = LoadMap("../../../Map Data/Platform_Stage_Prayanak_Enemy.csv");*/
            /*TileMap = LoadMap("Map Data/untitled_Tile Layer 1.csv");
            CollisionMap = LoadMap("Map Data/untitled_collision.csv");
            EnemyMap = LoadMap("Map Data/untitled_enemy.csv");*/
            tilesize = tilesize * scalesize;


            int row = 2;
            foreach (var item in CollisionMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize, (int)item.Key.Y * tilesize, tilesize, tilesize);
                if (item.Value % row == 0)
                    Tiles.Add(rectangle);
                if (item.Value % row == 1)
                    Platforms.Add(rectangle);
            }
            
            

            Font = Content.Load<SpriteFont>("Fonts/Font");
            BackGround = Content.Load<Texture2D>("Sprites/Background_Cave");
            Health = Content.Load<Texture2D>("Sprites/Skill4");
            ItemsIcons = Content.Load<Texture2D>("Sprites/ItemsSlot");
            Prayanaktex = Content.Load<Texture2D>("Sprites/Prayanak_Sheet");
            water = Content.Load<Texture2D>("Sprites/Boss_2_ATK_EF");
            Prayanak = new Prayanak(Prayanaktex, new Vector2(0 * 40, 0 * 40), new Vector2(300, 175), Color.White, Window, water);

            PlayerProjectile = Content.Load<Texture2D>("Sprites/player_S_Atk_EF");
            Player = new Player(Content.Load<Texture2D>("Sprites/Player_SpriteSheet"), new Vector2(1*40, 5*40), new Vector2(112 * scalesize, 54 * scalesize), Color.White, Window, pixel, PlayerProjectile);

            Player.power = "Flash";

            campos.X = Player.Position.X - 480 / 2;

            Tile = Content.Load<Texture2D>("TileMap/stone_platoformmmmmm");
            /*DecoTile = Content.Load<Texture2D>("TileMap/AssetsForestttt");
            DecoTile2 = Content.Load<Texture2D>("TileMap/stone_platoformmmmmm");*/
            CollisionTile = Content.Load<Texture2D>("TileMap/tilecollision");
            Player.LockCamera = true;
            campos.X = 0;
        }
        public void Update(GameTime gameTime , GameWindow Window)
        {
            //Debug.WriteLine(Player.LockCamera);
            List<Rectangle> bossattacks = new List<Rectangle>();
            if (Prayanak.meleeattacking == true)
            bossattacks.Add(Prayanak.Rectangle);
            foreach(var water in Prayanak.waters)
            {
                bossattacks.Add(water.Rectangle);
            }
            //bossattacks.Add(Prayanak.spikebox);
            if (mouseState.LeftButton == ButtonState.Released && mouseState2.LeftButton != ButtonState.Released)
                hit.Play();
            if (KeyboardState.IsKeyDown(Keys.Escape) && !KeyboardState2.IsKeyDown(Keys.Escape))
            {
                menu = true;
            }
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Player.Update(gameTime, campos, Tiles, Platforms,bossattacks);
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
            Prayanak.Update(gameTime, Player);
            //Debug.WriteLine(campos);

            //if (Player.Position.Y+Player.hitbox.Height/2 - Window.ClientBounds.Height / 2 < -80 && Player.Position.Y + Player.hitbox.Height / 2 - Window.ClientBounds.Height / 2 > 800)

            //Player.Update(gameTime, Walls, Platforms, MeleeEnemies, RangedEnemies);
            /*Debug.Write(270 / 2);
            Debug.WriteLine(Player.Position.Y - 270 / 2);*/
            Items.RemoveAll((item) => item.Rectangle.Intersects(Player.hitbox));
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                Scene = "Title Screen";
            }
            if (KeyboardState.IsKeyDown(Keys.L) && !KeyboardState2.IsKeyDown(Keys.L))
            {
                Player.LockCamera = !Player.LockCamera;
            }
            if (Prayanak.Health <= 0 && Prayanak.Position.Y > 1000)
            {
                Scene = "Map";
            }
        }
        public void Draw(SpriteBatch spriteBatch,GameWindow Window, Texture2D pixel)
        {

            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 480, 270), Color.White);
            
            int tilesize = 40 * scalesize;
            int tilerow = 8;
            int tilepixel = 40;

            foreach (var item in TileMap)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;
                //y -= 1;
                x -= 1;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);
                spriteBatch.Draw(Tile, rectangle, sourcerectangle, Color.White);
            }
            /*tilerow = 18;
            foreach (var item in TileMapp)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);
                spriteBatch.Draw(DecoTile, rectangle, sourcerectangle, Color.White);
            }
            tilerow = 9;
            foreach (var item in TileMappp)
            {
                Rectangle rectangle = new Rectangle((int)item.Key.X * tilesize - (int)campos.X, (int)item.Key.Y * tilesize - (int)campos.Y, tilesize, tilesize);

                int x = item.Value % tilerow;
                int y = item.Value / tilerow;

                Rectangle sourcerectangle = new Rectangle(x * tilepixel, y * tilepixel, tilepixel, tilepixel);
                spriteBatch.Draw(DecoTile2, rectangle, sourcerectangle, Color.White);
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
            }*/


            Prayanak.Draw(spriteBatch,pixel);


            spriteBatch.Draw(pixel, new Vector2(48, 25), new Rectangle(0, 0, (int)(Player.Health * 1.59f), 4), Color.DarkRed);
            //Debug.WriteLine(Mouse.GetState().Position);
            spriteBatch.Draw(Health, Vector2.Zero, new Rectangle(16, 8, 151, 57), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            spriteBatch.Draw(ItemsIcons, new Vector2(56,38),Color.White);
            spriteBatch.DrawString(Font, Player.potion1.ToString(), new Vector2(56, 38), Color.White,0,Vector2.Zero,0.25f,SpriteEffects.None,1);
            spriteBatch.DrawString(Font, Player.potion2.ToString(), new Vector2(73, 38), Color.White, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 1);

            Player.Draw(spriteBatch);
        }
    }
}
