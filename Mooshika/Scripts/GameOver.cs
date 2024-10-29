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
    internal class GameOver : GameScene
    {
        public MouseState mouseState, mouseState2;
        Texture2D Interface;
        Rectangle RetryRectangle = new Rectangle(100, 140, 33, 18), RetryRectangle2 = new Rectangle(6, 108, 33, 18);
        Rectangle ExitRectangle = new Rectangle(100, 220, 39, 22), ExitRectangle2 = new Rectangle(6, 57, 33, 22);
        Rectangle MenuRectangle = new Rectangle(100, 180, 39, 22), MenuRectangle2 = new Rectangle(6, 128, 33, 16);
        Rectangle mouserectangle;
        public bool gameover = false;
        public bool retry = false;
        public String Scene = "Title Screen";
        public String PreScene;
        public float volume = 1.0f;
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Menu_Interface");
        }
        public void Update(GameTime gameTime,Game Game, Vector2 Offset, float Scale)
        {
            retry = false;
            Scene = PreScene;
            mouserectangle = new Rectangle((int)((mouseState.Position.X - Offset.X) / Scale), (int)((mouseState.Position.Y - Offset.Y) / Scale), 1, 1);
            
            if (mouserectangle.Intersects(RetryRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                //Scene = "Title Screen";
                gameover = false;
                retry = true;
            }
                
            /*if (mouserectangle.Intersects(SettingRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
                //Game.Exit();
            }*/
            if (mouserectangle.Intersects(MenuRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Title Screen";
                gameover = false;
            }
            if (mouserectangle.Intersects(ExitRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Game.Exit();
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window,Texture2D pixel)
        {

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
        }
    }
}
