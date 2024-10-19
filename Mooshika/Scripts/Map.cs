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
        MouseState mouseState;
        MouseState mouseState2;
        Texture2D Interface;
        Texture2D BackGround;
        Texture2D MapTex;
        Texture2D Ginari, PrayaKrut, Prayanak, Elavan;
        Rectangle GinariRectangle = new Rectangle (275,360,62*3,69*3), PrayaKrutRectangle = new Rectangle(300, 70, 76 * 3, 86 * 3), PrayanakRectangle = new Rectangle(540, 340, 128 * 3, 112 * 3), ElavanRectangle = new Rectangle(625, 50, 124 * 3, 101 * 3);
        Rectangle mouserectangle;
        Rectangle MenuRectangle = new Rectangle(50, 600, 39 * 5, 22 * 4), MenuRectangle2 = new Rectangle(154, 0, 33, 16);
        public String Scene = "Map";
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Interface");
            BackGround = Content.Load<Texture2D>("Sprites/Title Background");
            MapTex = Content.Load<Texture2D>("Sprites/Map");
            Ginari = Content.Load<Texture2D>("Sprites/Ginari");
            PrayaKrut = Content.Load<Texture2D>("Sprites/PrayaKrut");
            Prayanak = Content.Load<Texture2D>("Sprites/Prayanak");
            Elavan = Content.Load<Texture2D>("Sprites/Elavan");
            
        }
        public void Update(GameTime gameTime,Game Game)
        {
            mouseState = Mouse.GetState();
            mouserectangle = new Rectangle(mouseState.Position.X, mouseState.Position.Y, 1, 1);

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
                Scene = "Stage Test";
            }
            if (mouserectangle.Intersects(ElavanRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
            }
            if (mouserectangle.Intersects(MenuRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Title Screen";
            }
            mouseState2 = mouseState;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(MapTex, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
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
