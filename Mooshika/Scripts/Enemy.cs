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
        public List<Wall> Walls = new List<Wall>();
        public List<Platform> Platforms = new List<Platform>();
        public Enemy (Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window) : base (texture, position, scale, color, window) 
        {
        }
        public void GetList(List<Wall> walls,List<Platform> platforms)
        {
            Walls = walls;
            Platforms = platforms;
        }
    }
}