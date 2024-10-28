using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Mooshika.Scripts
{
    internal class Map : GameScene
    {
        public MouseState mouseState, mouseState2;
        Texture2D Interface;
        Texture2D BackGround;
        Texture2D MapTex;
        Texture2D Ginari, PrayaKrut, Prayanak, Elavan;
        Rectangle GinariRectangle = new Rectangle (115,152,62,69), PrayaKrutRectangle = new Rectangle(116, 30, 76, 86), PrayanakRectangle = new Rectangle(250, 125, 128, 112), ElavanRectangle = new Rectangle(250, 35, 124, 101);
        Rectangle mouserectangle;
        Rectangle MenuRectangle = new Rectangle(10, 240, 39 , 22 ), MenuRectangle2 = new Rectangle(6, 128, 33, 16);
        public String Scene = "Map";
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Menu_Interface");
            BackGround = Content.Load<Texture2D>("Sprites/Menu_Background");
            MapTex = Content.Load<Texture2D>("Sprites/Map");
            Ginari = Content.Load<Texture2D>("Sprites/Ginari");
            PrayaKrut = Content.Load<Texture2D>("Sprites/PrayaKrut");
            Prayanak = Content.Load<Texture2D>("Sprites/Prayanak");
            Elavan = Content.Load<Texture2D>("Sprites/Elavan");
            
        }
        public void Update(GameTime gameTime,Game Game, Vector2 Offset, float Scale)
        {
            mouserectangle = new Rectangle((int)((mouseState.Position.X - Offset.X) / Scale), (int)((mouseState.Position.Y - Offset.Y) / Scale), 1, 1);

            if (mouserectangle.Intersects(GinariRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
            }
            if (mouserectangle.Intersects(PrayaKrutRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
            }
            if (mouserectangle.Intersects(PrayanakRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton && !mouserectangle.Intersects(ElavanRectangle))
            {
                Scene = "Before Prayanak";
            }
            if (mouserectangle.Intersects(ElavanRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "TestBoss";
            }
            if (mouserectangle.Intersects(MenuRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Title Screen";
            }
            //Debug.WriteLine(mouseState.Position);
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(BackGround, Vector2.Zero, Color.White);
            spriteBatch.Draw(MapTex, Vector2.Zero, Color.White);
            if (mouserectangle.Intersects(GinariRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Ginari, GinariRectangle, Color.Gray);
            }
            else if (mouserectangle.Intersects(GinariRectangle))
            {
                spriteBatch.Draw(Ginari, GinariRectangle, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Ginari, GinariRectangle, Color.White);
            }
            if (mouserectangle.Intersects(PrayaKrutRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(PrayaKrut, PrayaKrutRectangle, Color.Gray);
            }
            else if (mouserectangle.Intersects(PrayaKrutRectangle))
            {
                spriteBatch.Draw(PrayaKrut, PrayaKrutRectangle, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(PrayaKrut, PrayaKrutRectangle, Color.White);
            }
            if (mouserectangle.Intersects(PrayanakRectangle) && mouseState.LeftButton == ButtonState.Pressed && !mouserectangle.Intersects(ElavanRectangle))
            {
                spriteBatch.Draw(Prayanak, PrayanakRectangle, Color.Gray);
            }
            else if (mouserectangle.Intersects(PrayanakRectangle) && !mouserectangle.Intersects(ElavanRectangle))
            {
                spriteBatch.Draw(Prayanak, PrayanakRectangle, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Prayanak, PrayanakRectangle, Color.White);
            }
            if (mouserectangle.Intersects(ElavanRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Elavan, ElavanRectangle, Color.Gray);
            }
            else if (mouserectangle.Intersects(ElavanRectangle))
            {
                spriteBatch.Draw(Elavan, ElavanRectangle, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Elavan, ElavanRectangle, Color.White);
            }
            if (mouserectangle.Intersects(MenuRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Interface, MenuRectangle, MenuRectangle2, Color.Gray);
            }
            else if (mouserectangle.Intersects(MenuRectangle))
            {
                spriteBatch.Draw(Interface, MenuRectangle, MenuRectangle2, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Interface, MenuRectangle, MenuRectangle2, Color.White);
            }
        }
    }
}
