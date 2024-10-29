using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Krut : Enemy
    {
        Vector2 velocity = Vector2.Zero;
        int frame = 0;
        int maxframe = 8;
        float frametime = 0;
        int row = 0;
        int maxrow = 2;
        string state = "range1";
        string prestate ;
        float cooldown = 3f;
        bool rangeattacking = false;
        bool attacked = false;
        bool attacked2 = false;
        bool dead = false;
        Texture2D spike;
        Texture2D flame;
        Rectangle flamerec = new Rectangle(0,0,32,128);
        public Rectangle flamebox = new Rectangle(0, 0, 32, 0);
        Rectangle spikerec = new Rectangle(0, 0, 64, 41);
        public Rectangle spikebox = new Rectangle(0, 0, 64, 41);

        int flameframe = 6;
        float flameframetime = 0;
        int maxflameframe = 5;
        Vector2 flamepos = new Vector2(-20,0);
        bool flameactive = false;
        int spikeframe = 6;
        float spikeframetime = 0;
        int maxspikeframe = 5;
        Vector2 spikepos = new Vector2(-20, 0);
        public bool spikeactive = false;

        public Krut(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D flame, Texture2D spike) : base (texture, position, scale, color, window) 
        {
            Position = new Vector2 (480-Scale.X, 290-40-Scale.Y);
            this.flame = flame;
            this.spike = spike;
            MaxHealth = 1000;
            Health = 1000;
        }
        public void Update(GameTime gameTime, Player Player)
        {
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            /*if(!spikeactive)
                spikepos = new Vector2(40 * 6, 40 * 6 - spikerec.Height);*/
            if (Health > 0)
            {
                PlayerAttacked(Player);
                if (flameframe == 0)
                {
                    flamebox.Height = 58;
                }
                else if (flameframe == 1)
                {
                    flamebox.Height = 85;
                }
                else if (flameframe == 2)
                {
                    flamebox.Height = 120;
                }
                else if (flameframe == 3)
                {
                    flamebox.Height = 128;
                }
                else if (flameframe == 4)
                {
                    flamebox.Height = 120;
                }

                flamebox = new Rectangle((int)flamepos.X, (int)flamepos.Y + flamerec.Height - flamebox.Height, flamebox.Width, flamebox.Height);
                spikebox = new Rectangle((int)spikepos.X, (int)spikepos.Y + spikerec.Height - spikebox.Height, spikebox.Width, spikebox.Height);
                if (cooldown < 0)
                {
                    if (Health >= MaxHealth/2)
                    {
                        state = "range1";
                    }
                    else if (!flameactive) state = "range2";
                }
                else
                {
                    state = "idle";
                    cooldown -= Deltatime;
                }
                if (state == "idle")
                {
                    if (prestate != state)
                        frame = 0;
                    row = 0;
                    maxrow = 1;
                    maxframe = 6;
                }
                else if (state == "range2")
                {
                    if (prestate != state)
                        frame = 0;
                    if (row != 3 && row != 4)
                        row = 3;
                    maxrow = 2;
                    maxframe = 9;
                }
                else if (state == "range1")
                {
                    if (prestate != state)
                        frame = 0;
                    if (row != 1 && row != 2)
                        row = 1;
                    maxrow = 2;
                    maxframe = 9;
                }
                if (frametime < 0)
                {
                    frame++;
                    if (frame >= maxframe)
                    {
                        frame = 0;
                        row++;
                    }
                    frametime = 1f / 12f;
                }
                else
                {
                    frametime -= Deltatime;
                }
                if (frame > 0 && row == 2)
                {
                    if (frame == 8)
                    {
                        attacked2 = true;
                        spikepos = new Vector2(Player.Position.X-spikebox.Width/2/2, 40 * 6 - spikerec.Height);
                        spikeframe = 0;
                        spikeactive = true;
                    }
                }
                if (frame > 0 && row == 4)
                {
                    if (frame == 8)
                    {
                        attacked2 = true;
                        flamepos = new Vector2(40 * 11, 40 * 6 - flamerec.Height);
                    }
                }
                else
                {
                    if (attacked2)
                    {
                        cooldown = 2f;
                        attacked2 = false;
                    }
                }
            }
            else
            {
                if (!dead)
                {
                    dead = true;
                    velocity.Y = -10;
                }
                velocity.Y += 40 * Deltatime;
                Position += velocity;
            }
            //Debug.WriteLine(Health);
            prestate = state;
            if (flameframetime < 0)
            {
                if (flameactive)
                    flameframe++;
                flameframetime = 1f / 24f;
            }
            else
            {
                flameframetime -= Deltatime;
            }
            if (spikeframetime < 0 && spikeactive)
            {
                spikeframe++;
                spikeframetime = 1f / 12f;
            }
            else
            {
                spikeframetime -= Deltatime;
            }

            if (flameframe > 5)
            {
                flamepos = new Vector2(flamepos.X - 40, 40 * 6 - flamerec.Height);
                flameframe = 0;
            }
            if (spikeframe > 5)
            {
                spikeactive = false;
                spikeframe = 6;
            }
            if (flamepos.X < -40)
            {
                flameactive = false;
            }
            else { flameactive = true; }

        }
        public void PlayerAttacked(Player Player)
        {
            foreach (var projectile in Player.PlayerProjectile)
            {
                if (Rectangle.Intersects(projectile.Rectangle))
                {
                    attacked = true;
                    Health -= (projectile.type == "normal") ? projectile.Damage : projectile.Damage2;
                    projectile.hit = true;
                }
            }
            if (Rectangle.Intersects(Player.AttackRectangle) && !attacked && Player.attackactive && Player.AttackCombo == 1)
            {
                attacked = true;
                Health -= Player.Damage;

            }
            else if (Rectangle.Intersects(Player.AttackRectangle2) && !attacked && Player.attackactive && Player.AttackCombo == 2)
            {
                attacked = true;
                Health -= Player.Damage;
            }
            else if (Rectangle.Intersects(Player.AttackRectangle3) && !attacked && Player.attackactive && Player.AttackCombo == 3)
            {
                attacked = true;
                Health -= Player.Damage;
            }
            if (attacked && !Player.attackactive)
            {
                attacked = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch,Texture2D pixel)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(frame* (int)Scale.X, row * (int)Scale.Y, (int)Scale.X, (int)Scale.Y), Color.White);
            //spriteBatch.Draw(spike, new Vector2(40*10,40*8), new Rectangle(spikeframe * spikerec.Width, spikerec.Height, spikerec.Width, spikerec.Height), Color.White);
            spriteBatch.Draw(flame, flamepos, new Rectangle(flamerec.Width* flameframe, 0, flamerec.Width, flamerec.Height), Color.White);
            spriteBatch.Draw(spike, spikepos, new Rectangle(spikerec.Width * spikeframe, 23, spikerec.Width, spikerec.Height), Color.White);
            //spriteBatch.Draw(pixel, spikebox, Color.White);
            //spriteBatch.Draw(pixel, Rectangle, Color.White);
        }
    }
}