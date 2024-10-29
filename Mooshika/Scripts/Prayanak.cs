using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Prayanak : Enemy
    {
        Vector2 velocity = Vector2.Zero;
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
        bool dead = false;
        public bool stun = false;
        float stuntime = 0f;
        Texture2D water;
        public List<Water> waters = new List<Water>();
        bool canbestun = false;

        public Prayanak(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D water) : base (texture, position, scale, color, window) 
        {
            Position = new Vector2 (480-Scale.X, 290-40-Scale.Y);
            this.water = water;
            MaxHealth = 1000;
            Health = 1000;
        }
        public void Update(GameTime gameTime, Player Player)
        {
            meleeattacking = false;
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            /*if(!spikeactive)
                spikepos = new Vector2(40 * 6, 40 * 6 - spikerec.Height);*/
            if (Health > 0)
            {
                PlayerAttacked(Player);
                if(stuntime<=0)
                {
                    if (cooldown < 0)
                    {
                        state = "range";
                    }
                    else
                    {
                        if (Player.hitbox.Intersects(Rectangle))
                        {
                            state = "melee";
                            cooldown = 3f;
                        }
                        else
                        state = "idle";
                        cooldown -= Deltatime;
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

                    if (state == "idle")
                    {
                        if (prestate != state)
                            frame = 0;
                        row = 0;
                        maxrow = 1;
                        maxframe = 6;
                    }
                    else if (state == "melee")
                    {
                        if (prestate != state)
                            frame = 0;
                        if (row != 1 && row != 2)
                            row = 1;
                        maxrow = 2;
                        maxframe = 9;
                    }
                    else if (state == "range")
                    {
                        if (prestate != state)
                            frame = 0;
                        if (row != 3 && row != 4)
                            row = 3;
                        maxrow = 2;
                        maxframe = 9;
                    }

                    if ((frame == 3 || frame == 4) && row == 1 && state == "melee" && row != 0)
                    {
                        meleeattacking = true;
                    }
                    else if ((frame == 2 || frame == 3 || frame == 7 || frame == 8) && row == 2 && state == "melee" && row != 0)
                    {
                        meleeattacking = true;
                        if (frame == 8)
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
                        if (frame == 8)
                        {
                            attacked2 = true;
                            for (int i = 0; i < 12; i++)
                            {
                                waters.Add(new Water(water, new Vector2(i * 40, 0), new Vector2(32, 32), Color.White, Window, 1 + (i / 4f)));
                            }
                            canbestun = true;
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
                }
                else
                {
                    stuntime -= Deltatime;
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
            foreach (var water in waters)
            {
                water.Update(gameTime);
            }
            waters.RemoveAll(water => water.Position.Y > 270 || (water.Rectangle.Intersects(Player.flashsize) && Player.flash));
            if (Player.flash)
            {
                if (canbestun)
                {
                    stuntime = 5f;
                    canbestun = false;
                }
            }

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
            foreach (var water in waters)
            {
                water.Draw(spriteBatch);
            }
            //spriteBatch.Draw(pixel, spikebox, Color.White);
            //spriteBatch.Draw(pixel, Rectangle, Color.White);
        }
    }
}