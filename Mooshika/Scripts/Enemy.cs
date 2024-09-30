using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Enemy : Sprite
    {
        public int Health ;
        public int MaxHealth ;
        public Vector2 Velocity;
        public List<Rectangle> Tiles = new List<Rectangle>();
        public List<Rectangle> Platforms = new List<Rectangle>();
        public Enemy (Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window) : base (texture, position, scale, color, window) 
        {
        }
        public void GetList(List<Rectangle> walls,List<Rectangle> platforms)
        {
            Tiles = walls;
            Platforms = platforms;
        }
    }
}