using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Wall : Tile
    {
        public Wall(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window) : base (texture, position, scale, color, window) 
        {

        }
    }
}