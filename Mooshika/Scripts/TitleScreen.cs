using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Mooshika.Scripts
{
    internal class TitleScreen : GameScene
    {
        MouseState mouseState;
        MouseState mouseState2;
        Texture2D Interface;
        Texture2D BackGround;
        Texture2D Title;
        Rectangle StartRectangle = new Rectangle(50,300,39*5,22*5), StartRectangle2 = new Rectangle(0, 0, 39, 22);
        Rectangle SettingRectangle = new Rectangle(50, 400, 39 * 5, 22 * 5), SettingRectangle2 = new Rectangle(39, 0, 49, 22);
        Rectangle ExitRectangle = new Rectangle(50, 500, 39 * 5, 22 * 5), ExitRectangle2 = new Rectangle(39+49, 0, 33, 22);
        Rectangle mouserectangle;
        public String Scene = "Title Screen";
        public void LoadContent(ContentManager Content)
        {
            Interface = Content.Load<Texture2D>("Sprites/Interface");
            BackGround = Content.Load<Texture2D>("Sprites/Title Background");
            Title = Content.Load<Texture2D>("Sprites/Title");
        }
        public void Update(GameTime gameTime,Game Game)
        {
            mouseState = Mouse.GetState();
            mouserectangle = new Rectangle(mouseState.Position.X, mouseState.Position.Y, 1, 1);
            
            if (mouserectangle.Intersects(StartRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Map";
                //Game.Exit();
            }
            /*if (mouserectangle.Intersects(SettingRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Scene = "Stage Test";
                //Game.Exit();
            }*/
            if (mouserectangle.Intersects(ExitRectangle) && mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton != mouseState2.LeftButton)
            {
                Game.Exit();
            }

            mouseState2 = mouseState;
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(Title, new Rectangle(50, 50, 150*5, 32*5), Color.White);
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
