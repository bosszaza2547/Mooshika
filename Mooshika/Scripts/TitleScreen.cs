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
    internal class TitleScreen : GameScene
    {
        public MouseState mouseState, mouseState2;
        Texture2D Interface;
        Texture2D BackGround;
        Texture2D Title;
        Rectangle StartRectangle = new Rectangle(50,150,39,22), StartRectangle2 = new Rectangle(6, 7, 39, 22);
        Rectangle SettingRectangle = new Rectangle(50, 180, 39, 22), SettingRectangle2 = new Rectangle(6, 30, 49, 22);
        Rectangle ExitRectangle = new Rectangle(50, 210, 39, 22), ExitRectangle2 = new Rectangle(6, 57, 33, 22);
        Rectangle mouserectangle;
        public String Scene = "Title Screen";
        public bool menu = false;
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Menu_Interface");
            BackGround = Content.Load<Texture2D>("Sprites/Menu_Background");
            Title = Content.Load<Texture2D>("Sprites/Title");
        }
        public void Update(GameTime gameTime,Game Game, Vector2 Offset, float Scale)
        {
            mouserectangle = new Rectangle((int)((mouseState.Position.X - Offset.X) / Scale), (int)((mouseState.Position.Y - Offset.Y) / Scale), 1, 1);
            
            if (mouserectangle.Intersects(StartRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Map";
                //Game.Exit();
            }
            if (mouserectangle.Intersects(SettingRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                menu = true;
                
                //Game.Exit();
            }
            if (mouserectangle.Intersects(ExitRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Game.Exit();
            }

        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(BackGround, Vector2.Zero/*new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height)*/, Color.White);
            spriteBatch.Draw(Title, new Rectangle(50, 50, 150, 32), Color.White);
            if (mouserectangle.Intersects(StartRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Interface, StartRectangle, StartRectangle2, Color.Gray);
            }
            else if (mouserectangle.Intersects(StartRectangle))
            {
                spriteBatch.Draw(Interface, StartRectangle, StartRectangle2, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Interface, StartRectangle, StartRectangle2, Color.White);
            }
            if (mouserectangle.Intersects(SettingRectangle) && mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Draw(Interface, SettingRectangle, SettingRectangle2, Color.Gray);
            }
            else if (mouserectangle.Intersects(SettingRectangle))
            {
                spriteBatch.Draw(Interface, SettingRectangle, SettingRectangle2, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(Interface, SettingRectangle, SettingRectangle2, Color.White);
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
        }
    }
}
