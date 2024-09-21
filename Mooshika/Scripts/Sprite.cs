using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Sprite
    {
        public GameWindow Window;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color;
        public int Direction = 1;
        public Rectangle Rectangle 
        { 
            get 
            { 
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Scale.X, (int)Scale.Y); 
            } 
        }
        public Sprite(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window)
        {
            this.Texture = texture;
            this.Position = position;
            this.Scale = scale;
            this.Color = color;
            this.Window =  window;
        }
        public virtual void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
