using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Mooshika.Scripts
{
    internal class Canva
    {
        GraphicsDevice device;
        RenderTarget2D RenderTarget;
        Rectangle destinationrec;
        float Scale;
        Vector2 offset = Vector2.Zero;
        public Canva(GraphicsDevice Device, int width, int height) 
        {
            device = Device;
            RenderTarget = new (device, width, height);
        }
        public void setscreensize()
        {
            var screensize = device.PresentationParameters.Bounds;

            float scaleX = (float)screensize.Width / RenderTarget.Width;
            float scaleY = (float)screensize.Height / RenderTarget.Height;
            float scale = Math.Min(scaleX, scaleY);
            Scale = scale;
            int newWidth = (int)(RenderTarget.Width * scale);
            int newHeight = (int)(RenderTarget.Height * scale);

            int posX = (screensize.Width - newWidth) / 2;
            int posY = (screensize.Height - newHeight) / 2;
            destinationrec = new Rectangle(posX, posY, newWidth, newHeight);
            if (scaleX > scaleY)
            {
                offset = new Vector2(posX, 0);
            }
            else if (scaleX < scaleY)
            {
                offset = new Vector2(0, posY);
            }
            else
            {
                offset = Vector2.Zero;
            }
            //Debug.WriteLine(offset);
        }
        public void setscreen()
        {
            device.SetRenderTarget(RenderTarget);
            device.Clear(Color.White);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            device.SetRenderTarget(null);
            device.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(RenderTarget,destinationrec,Color.White);
            spriteBatch.End();
        }
        public float getscale()
        {
            return Scale;
        }
        public Vector2 getoffset()
        {
            return offset;
        }
    }
}
