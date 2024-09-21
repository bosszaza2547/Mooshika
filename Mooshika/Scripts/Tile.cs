using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Tile : Sprite
    {
        public Tile(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window) : base (texture, position, scale, color, window) 
        {

        }
    }
}