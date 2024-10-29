using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mooshika.Scripts
{
    internal class Ginari : Enemy
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
        bool attacked3 = true;
        bool dead = false;
        public List<GinariProjectile> GinariProjectiles = new List<GinariProjectile>();
        public Texture2D Projectile;
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
        int type;

        public Ginari(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D projectile,int health,int type,int direction) : base (texture, position, scale, color, window) 
        {
            Position = position;/*new Vector2 (480-Scale.X, 290-40-Scale.Y);*/
            MaxHealth = health;
            Health = health;
            Projectile = projectile;
            this.type = type;
            Direction = direction;
            
        }
        public void Update(GameTime gameTime, Player Player)
        {
            float Deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            /*if(!spikeactive)
                spikepos = new Vector2(40 * 6, 40 * 6 - spikerec.Height);*/
            
            if (Health > 0)
            {
                PlayerAttacked(Player);

                if (cooldown < 0)
                {
                    if (type == 1)
                    { if (Health >= MaxHealth / 2)
                        {
                            state = "range1";
                        }
                        else if (!flameactive) state = "range2"; 
                    }
                    else
                    {
                        state = "range1";
                    }
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
                    if(type == 1)
                    row = 0;
                    else if (type ==2)
                        row = 5;
                    else if (type == 3)
                        row = 8;
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
                    if(type == 1) 
                    {
                        if (row != 1 && row != 2)
                            row = 1;
                    }
                        
                    else if (type == 2)
                    {
                        if (row != 6 && row != 7)
                            row = 6;
                    }
                    else if (type == 3)
                    {
                        if (row != 9 && row != 10)
                            row = 9;
                    }
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
                if (frame > 0 && (row == 2 || row == 7 || row == 10))
                {
                    if (frame == 8 && ! attacked2)
                    {
                        attacked2 = true;
                        Shoot();
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
                if (frame > 0 && row == 4)
                {
                    if (frame == 8 && !attacked3)
                    {
                        attacked3 = true;
                        Shoot();
                    }
                }
                else
                {
                    if (attacked3)
                    {
                        cooldown = 1f;
                        attacked3 = false;
                        row = 1;
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
            foreach (var Projectile in GinariProjectiles)
            {
                Projectile.Update(gameTime,Player.Position);
            }
            GinariProjectiles.RemoveAll((Projectile) => Projectile.Position.X < 0 - Projectile.Rectangle.Width|| Projectile.Position.X > 480);
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
            
            
            spriteBatch.Draw(Texture, Position, new Rectangle(frame * (int)Scale.X, row * (int)Scale.Y, (int)Scale.X, (int)Scale.Y), Color.White, 0, Vector2.Zero, 1, (Direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            
            //spriteBatch.Draw(spike, new Vector2(40*10,40*8), new Rectangle(spikeframe * spikerec.Width, spikerec.Height, spikerec.Width, spikerec.Height), Color.White);
            foreach (var Projectile in GinariProjectiles)
            {
                Projectile.Draw(spriteBatch);
            }
            //spriteBatch.Draw(pixel, spikebox, Color.White);
            //spriteBatch.Draw(pixel, Rectangle, Color.White);
        }
        public void Shoot()
        {
            if (type == 1)
            {
                if (Direction == 1)
                {
                    GinariProjectiles.Add(new GinariProjectile(Projectile, Position + new Vector2(0, Rectangle.Height / 3 + 5), new Vector2(96, 24), Color.White, Window, Direction, true));
                }
                else if (Direction == -1)
                {
                    GinariProjectiles.Add(new GinariProjectile(Projectile, Position + new Vector2(Rectangle.Width - 39, Rectangle.Height / 3 + 5), new Vector2(96, 24), Color.White, Window, Direction, true));

                }
            }
            else
            {
                if (Direction == 1)
                {
                    GinariProjectiles.Add(new GinariProjectile(Projectile, Position + new Vector2(0, Rectangle.Height / 3 + 5), new Vector2(96, 24), Color.White, Window, Direction, false));
                }
                else if (Direction == -1)
                {
                    GinariProjectiles.Add(new GinariProjectile(Projectile, Position + new Vector2(Rectangle.Width - 39, Rectangle.Height / 3 + 5), new Vector2(96, 24), Color.White, Window, Direction, false));

                }
            }
        }
    }
}