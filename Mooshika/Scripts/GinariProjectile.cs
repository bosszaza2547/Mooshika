
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class GinariProjectile : Sprite
    {
        public int speed = 4;
        public int Damage = 5;
        int frame = 0;
        int maxframe = 5;
        float frametime = 0;
        public bool LockOn = false;
        public GinariProjectile(Texture2D texture, Vector2 pos, Vector2 scale, Color color, GameWindow gameWindow, int Direction, bool lockOn) : base(texture, pos, scale, color, gameWindow)
        {
            this.Direction = -Direction;
            LockOn = lockOn;
        }
        public void Update(GameTime gameTime, Vector2 pos)
        {
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LockOn)
            {
                if (Position.Y > pos.Y)
                {
                    Position.Y -= speed;
                }
                else
                {
                    LockOn = false;
                }
            }
            Position.X += Direction * speed;
            if (frametime < 0)
            {
                frame++;
                if (frame >= maxframe)
                {
                    frame = 0;
                }
                frametime = 1f / 12f;
            }
            else
            {
                frametime -= Deltatime;
            }
        }
        public override void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(Texture, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height), new Rectangle(96 * frame, 0, 96, 24), Color, 0, Vector2.Zero, (Direction == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
}