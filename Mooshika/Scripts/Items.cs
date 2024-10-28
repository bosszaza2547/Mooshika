using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Items : Sprite
    {
        public int type;
        public Items(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window, int type) : base(texture, position, scale, color, window)
        {
            this.type = type;
        }
        public void Draw(SpriteBatch SpriteBatch, Vector2 campos)
        {
            if (type == 1)
            {
                SpriteBatch.Draw(Texture, Position - campos, new Rectangle(0, 0, 18, 18), Color.White);
            }
            else if (type == 2)
            {
                SpriteBatch.Draw(Texture, Position - campos, new Rectangle(18, 0, 18, 18), Color.White);
            }
        }
    }
}
