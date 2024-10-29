using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Water : Sprite
    {
        int speed = 5;
        int frame = 0;
        int maxframe = 4;
        float frametime = 0;
        float timer = 0;
        public Water(Texture2D texture,Vector2 position,Vector2 scale, Color color, GameWindow window,float timer) : base (texture, position, scale, color, window)
        {
            this.timer = timer;
        }
        public void Update(GameTime gameTime)
        {
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 0) 
            {
                timer -= Deltatime;
            }
            else
            Position.Y += speed;
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
            SpriteBatch.Draw(Texture, Position, new Rectangle(32 * frame, 0, 32, 32), Color);
        }
    }
}
