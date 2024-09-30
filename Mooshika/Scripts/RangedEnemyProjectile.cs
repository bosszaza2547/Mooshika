
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class RangedEnemyProjectile : Sprite
    {
        public int speed = 10;
        public float lifespan = 3;
        public int Damage = 10;
        public RangedEnemyProjectile(Texture2D texture, Vector2 pos, Vector2 scale, Color color, GameWindow gameWindow, int Direction) : base (texture, pos, scale, color, gameWindow)
        {
            this.Direction = Direction;
        }
        public void Update(GameTime gameTime)
        {
            Position.X += Direction * speed;
            lifespan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public virtual void Draw(SpriteBatch SpriteBatch, Vector2 campos)
        {
            SpriteBatch.Draw(Texture, new Rectangle(Rectangle.X - (int)campos.X, Rectangle.Y - (int)campos.Y,Rectangle.Width,Rectangle.Height), Color);
        }
    }
}