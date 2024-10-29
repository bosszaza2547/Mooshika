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
    internal class Menu : GameScene
    {
        public MouseState mouseState, mouseState2;
        public KeyboardState KeyboardState, KeyboardState2;
        Texture2D Interface;
        Rectangle ResumeRectangle = new Rectangle(100, 100, 44, 22), ResumeRectangle2 = new Rectangle(6, 81, 44, 22);
        Rectangle RetryRectangle = new Rectangle(100, 140, 33, 18), RetryRectangle2 = new Rectangle(6, 108, 33, 18);
        Rectangle ExitRectangle = new Rectangle(100, 220, 39, 22), ExitRectangle2 = new Rectangle(6, 57, 33, 22);
        Rectangle MenuRectangle = new Rectangle(100, 180, 39, 22), MenuRectangle2 = new Rectangle(6, 128, 33, 16);
        Rectangle InterfaceRectangle = new Rectangle(200, 20, 90 * 2, 94 * 2), InterfaceRectangle2 = new Rectangle(63, 47, 90, 94);
        Rectangle Slider = new Rectangle(0, 0, 73, 12), Slider2 = new Rectangle(80, 24, 73, 12);
        Rectangle volumerec = new Rectangle(0, 0, 56, 4);
        Rectangle mouserectangle;
        public bool menu = false;
        public bool retry = false;
        public String Scene = "Title Screen";
        public String PreScene;
        public float volume = 1.0f;
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Menu_Interface");
            Slider = new Rectangle(InterfaceRectangle.X+50, InterfaceRectangle.Y+100, 73, 12);
            volumerec = new Rectangle(Slider.X+17, Slider.Y+4, 56, 4);
        }
        public void Update(GameTime gameTime,Game Game, Vector2 Offset, float Scale)
        {
            retry = false;
            Scene = PreScene;
            if (KeyboardState.IsKeyDown(Keys.Escape) && !KeyboardState2.IsKeyDown(Keys.Escape))
            {
                //Debug.WriteLine("real");
                menu = false;
            }
            mouserectangle = new Rectangle((int)((mouseState.Position.X - Offset.X) / Scale), (int)((mouseState.Position.Y - Offset.Y) / Scale), 1, 1);
            
            if (Scene != "Title Screen")
            {
                if (mouserectangle.Intersects(ResumeRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
                {
                    menu = false;
                }

                    if (mouserectangle.Intersects(RetryRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
                {
                    menu = false;
                    retry = true;
                }
            }
                
            /*if (mouserectangle.Intersects(SettingRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
                //Game.Exit();
            }*/
            if (mouserectangle.Intersects(MenuRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Title Screen";
                menu = false;
            }
            if (mouserectangle.Intersects(ExitRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Game.Exit();
            }
            if (mouserectangle.Intersects(Slider) && mouseState.LeftButton == ButtonState.Pressed)
            {
                volumerec.Width = (int)(mouserectangle.X  - volumerec.X);
                if (volumerec.Width < 0)
                    volumerec.Width = 0;
                /*if (volumerec.Width < 0) 
                    volumerec.Width = 0;
                if (volumerec.Width > 56) 
                    volumerec.Width = 56;*/
            }
            volume = volumerec.Width / 56f;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window,Texture2D pixel)
        {
            spriteBatch.Draw(Interface, InterfaceRectangle, InterfaceRectangle2, Color.White);
            if (Scene != "Title Screen")
            {
                if (mouserectangle.Intersects(ResumeRectangle) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(Interface, ResumeRectangle, ResumeRectangle2, Color.Gray);
                }
                else if (mouserectangle.Intersects(ResumeRectangle))
                {
                    spriteBatch.Draw(Interface, ResumeRectangle, ResumeRectangle2, Color.LightGray);
                }
                else
                {
                    spriteBatch.Draw(Interface, ResumeRectangle, ResumeRectangle2, Color.White);
                }

                if (mouserectangle.Intersects(RetryRectangle) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(Interface, RetryRectangle, RetryRectangle2, Color.Gray);
                }
                else if (mouserectangle.Intersects(RetryRectangle))
                {
                    spriteBatch.Draw(Interface, RetryRectangle, RetryRectangle2, Color.LightGray);
                }
                else
                {
                    spriteBatch.Draw(Interface, RetryRectangle, RetryRectangle2, Color.White);
                }
            }   
            if (mouserectangle.Intersects(ExitRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Interface, ExitRectangle, ExitRectangle2, Color.Gray);
            }
            else if (mouserectangle.Intersects(ExitRectangle))
            {
                spriteBatch.Draw(Interface, ExitRectangle, ExitRectangle2, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Interface, ExitRectangle, ExitRectangle2, Color.White);
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
            spriteBatch.Draw(Interface, Slider, Slider2, Color.White);
            spriteBatch.Draw(pixel, volumerec, Color.Green);
        }
    }
}
