
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mooshika.Scripts
{
    internal class PlayerProjectile : Sprite
    {
        public int speed = 5;
        public int Damage = 10;
        public int Damage2 = 50;
        public string type = string.Empty;
        public bool hit = false;
        public Rectangle rec = new Rectangle();
        public PlayerProjectile(Texture2D texture, Vector2 pos, Vector2 scale, Color color, GameWindow gameWindow, int Direction, string type) : base(texture, pos, scale, color, gameWindow)
        {
            this.Direction = -Direction;
            if (Direction == -1)
            {
                Position.X += 32;
            }
            Position.Y -= 5;
            this.type = type;
        }
        public void Update(GameTime gameTime)
        {
            if(type == "normal")
            {
                rec = new Rectangle(12,12,9,9);
            }
            else if (type == "special")
            {
                rec = new Rectangle(0, 33, 32, 32);
            }
            Position.X += Direction * speed;
        }
        public virtual void Draw(SpriteBatch SpriteBatch, Vector2 campos)
        {
            //SpriteBatch.Draw(Texture, new Rectangle(Rectangle.X - (int)campos.X, Rectangle.Y - (int)campos.Y,Rectangle.Width,Rectangle.Height), Color);
            //SpriteBatch.Draw(Texture, new Rectangle(Rectangle.X - (int)campos.X, Rectangle.Y - (int)campos.Y,Rectangle.Width,Rectangle.Height), new Rectangle(rec.X - (int)campos.X, rec.Y - (int)campos.Y, rec.Width, rec.Height), Color);
            SpriteBatch.Draw(Texture, new Rectangle(Rectangle.X - (int)campos.X, Rectangle.Y - (int)campos.Y, rec.Width, rec.Height), new Rectangle(rec.X, rec.Y, rec.Width, rec.Height), Color.White);
        }
    }
}