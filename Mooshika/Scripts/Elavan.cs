using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Elavan : Enemy
    {
        int frame = 0;
        int maxframe = 8;
        float frametime = 0;
        int row = 0;
        int maxrow = 2;
        string state = "melee";
        string prestate ;
        float cooldown = 3f;
        public bool meleeattacking = false;
        bool rangeattacking = false;
        bool attacked = false;
        bool attacked2 = false;
        Texture2D spike;
        Rectangle spikerec = new Rectangle(0,0,64,96);
        public Rectangle spikebox = new Rectangle(0, 0, 44, 0);

        int spikeframe = 14;
        float spikeframetime = 0;
        int maxspikeframe = 11;
        Vector2 spikepos = new Vector2(-20,0);
        bool spikeactive = false;

        public Elavan(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D spike) : base (texture, position, scale, color, window) 
        {
            Position = new Vector2 (480-Scale.X, 290-40-Scale.Y);
            this.spike = spike;
            MaxHealth = 1000;
            Health = 1000;
        }
        public void Update(GameTime gameTime, Player Player)
        {
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            /*if(!spikeactive)
                spikepos = new Vector2(40 * 6, 40 * 6 - spikerec.Height);*/
            if(Health > 0) 
            {
                PlayerAttacked(Player);
                if (spikeframe == 0)
                {
                    spikebox.Height = 0;
                }
                else if (spikeframe == 1)
                {
                    spikebox.Height = 25;
                }
                else if (spikeframe == 3)
                {
                    spikebox.Height = 49;
                }
                else if (spikeframe == 5)
                {
                    spikebox.Height = 70;
                }
                else if (spikeframe == 6)
                {
                    spikebox.Height = 87;
                }

                spikebox = new Rectangle((int)spikepos.X, (int)spikepos.Y + spikerec.Height - spikebox.Height, spikebox.Width, spikebox.Height);
                if (cooldown < 0)
                {
                    if (Player.hitbox.Intersects(Rectangle))
                    {
                        state = "melee";
                    }
                    else if (!spikeactive) state = "range";
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
                    maxframe = 8;
                }
                else if (state == "melee")
                {
                    if (prestate != state)
                        frame = 0;
                    if (row != 1 && row != 2)
                        row = 1;
                    maxrow = 2;
                    maxframe = 10;
                }
                else if (state == "range")
                {
                    if (prestate != state)
                        frame = 0;
                    if (row != 3 && row != 4)
                        row = 3;
                    maxrow = 2;
                    maxframe = 10;
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
                if (spikeframetime < 0)
                {
                    if (spikeactive)
                        spikeframe++;
                    spikeframetime = 1f / 24f;
                }
                else
                {
                    spikeframetime -= Deltatime;
                }
                if (frame > 0 && row == 2)
                {
                    meleeattacking = true;
                    if (frame == 9)
                    {
                        attacked2 = true;
                    }
                }
                else
                {
                    if (attacked2)
                    {
                        cooldown = 3f;
                        attacked2 = false;
                    }
                    meleeattacking = false;
                }
                if (frame > 0 && row == 4)
                {
                    if (frame == 9)
                    {
                        attacked2 = true;
                        spikepos = new Vector2(40 * 6, 40 * 6 - spikerec.Height);
                    }
                }
                else
                {
                    if (attacked2)
                    {
                        cooldown = 3f;
                        attacked2 = false;
                    }
                }
                if (spikeframe > 12)
                {
                    spikepos = new Vector2(spikepos.X - 40, 40 * 6 - spikerec.Height);
                    spikeframe = 0;
                }
                if (spikepos.X < -40)
                {
                    spikeactive = false;
                }
                else { spikeactive = true; }
            }
            
            //Debug.WriteLine(Health);
            prestate = state;

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
            spriteBatch.Draw(spike, spikepos,new Rectangle(spikerec.Width*spikeframe, 0, spikerec.Width, spikerec.Height), Color.White);
            //spriteBatch.Draw(pixel, spikebox, Color.White);
            //spriteBatch.Draw(pixel, Rectangle, Color.White);
        }
    }
}