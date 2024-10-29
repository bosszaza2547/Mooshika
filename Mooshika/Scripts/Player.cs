using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mooshika.Scripts
{
    internal class Player : Sprite
    {
        public int Health = 60;
        public int MaxHealth = 60;
        bool Invincible = false;
        public float InvincibleTime = 0;
        public float InvincibleTimer = 1;
        public Vector2 Knockback = new Vector2(3, -7);
        public float KnockbackTime = 0;
        public float KnockbackTimer = 0.25f;
        public Vector2 Velocity = Vector2.Zero;
        public int Speed = 75;
        public int MaxSpeed = 4;
        public int Gravity = 30;
        public SoundEffect hit;
        public int MaxGravity = 20;
        KeyboardState KeyboardState;
        KeyboardState PreviousKeyBoardState;
        public bool CanJump = true;
        public int JumpPower = 500;
        List<MeleeEnemy> MeleeEnemies = new List<MeleeEnemy>();
        List<Items> Items = new List<Items>();
        List<RangedEnemy> RangedEnemies = new List<RangedEnemy>();
        public List<PlayerProjectile> PlayerProjectile = new List<PlayerProjectile>();
        Vector2 PreviousPosition;
        public float CoyoteTime = 0;
        public float CoyoteTimer = 0.25f;
        float PreJumpTime = 0;
        float PreJumpTimer = 0.25f;
        public int JumpCount = 0;
        public int MaxJump = 2;
        bool jumped = false;
        public int FloorHeight = 20;
        bool dead = false;
        Texture2D pixel;
        Texture2D AttackSprite;
        public Rectangle AttackRectangle = new Rectangle(0, 0, 20, 32);
        public Rectangle AttackRectangle2 = new Rectangle(0, 0, 36, 32);
        public Rectangle AttackRectangle3 = new Rectangle(0, 0, 36, 32);
        Vector2 AttackPosition = Vector2.Zero;
        bool CanAttack = true;
        public bool Attacking = false;
        float AttackTimer = 0.32f;
        float AttackTimer2 = 0.48f;
        float AttackTimer3 = 0.6f;
        float AttackTimer4 = 0.5f;
        float AttackTimer5 = 1f;
        float AttackTimer6 = 1f;
        float AttackTime = 0;
        public int AttackCombo = 0;
        int AttackMaxCombo = 3;
        float AttackComboTimer = 1;
        public float AttackComboTime = 0;
        public bool attackactive = false;
        public int Damage = 10;
        public int frame = 0;
        public int maxframe = 4;
        public int row = 0;
        public float frametimer = 0;
        public float maxframetimer = 0.15f;
        public string state = "idle";
        string prestate = string.Empty;
        public Vector2 framesize = new Vector2(112, 54);
        public Rectangle hitbox;
        public int scale = 1;
        public int dashspeed = 8;
        public bool candash = true;
        public float dashtimer = 0;
        public float maxdashtimer = 0.25f;
        public float dashcooldown = 0;
        public float dashcooldowntime = 0.3f;
        public bool LockCamera = false;
        public Texture2D Projectile;
        public string SpecialType = string.Empty;
        public bool flash = false;
        public float flashvalue = 0;
        public Rectangle flashsize = Rectangle.Empty;
        public int potion1 = 0;
        public int healpower = 20;
        public int potion2 = 0;
        public int SpeedBoost = 2;
        public float Boosttime = 0;
        public string power = string.Empty;
        //List<Rectangle> recs = new List<Rectangle>();
        public Vector2 campos;
        public Player(Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window, Texture2D pixel, Texture2D projectile) : base(texture, position, scale, color, window)
        {
            this.pixel = pixel;
            hitbox = new Rectangle(Rectangle.X, Rectangle.Y, 32, 32);
            Direction = -1;
            Projectile = projectile;
        }
        public void Update(GameTime gameTime, Vector2 campos, List<Rectangle> tiles, List<Rectangle> platforms, List<MeleeEnemy> meleeEnemies, List<RangedEnemy> rangedEnemies, List<Items> items)
        {
            if (power == "Double Jump")
            {
                MaxJump = 2;
            }
            else MaxJump = 1;
            flash = false;
            this.campos = campos;
            attackactive = false;
            KeyboardState = Keyboard.GetState();
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Boosttime -= DeltaTime;
            MeleeEnemies = meleeEnemies;
            Items = items;
            RangedEnemies = rangedEnemies;
            if (InvincibleTime > 0)
            {
                InvincibleTime -= DeltaTime;
            }
            if (KnockbackTime > 0)
            {
                KnockbackTime -= DeltaTime;
            }
            if (CoyoteTime > 0)
            {
                CoyoteTime -= DeltaTime;
            }
            else if (JumpCount == MaxJump)
            {
                JumpCount--;
            }
            if (PreJumpTime > 0)
            {
                PreJumpTime -= DeltaTime;
            }
            Velocity.Y += Gravity * DeltaTime;

            if (KeyboardState.IsKeyDown(Keys.D1) && !PreviousKeyBoardState.IsKeyDown(Keys.D1))
            {
                if(Health < MaxHealth)
                {
                    if(potion1 > 0)
                    {
                        potion1--;
                        Health += healpower;
                    }
                }
            }
            if (KeyboardState.IsKeyDown(Keys.D2) && !PreviousKeyBoardState.IsKeyDown(Keys.D2))
            {
                if (potion2 > 0)
                {
                    potion2--;
                    Boosttime = 30;
                }
            }

            if (KnockbackTime <= 0)
            {
                if (KeyboardState.IsKeyDown(Keys.D) && state != "dashing" && state != "attacking" && state != "jumping")
                {
                    Velocity.X += Speed * DeltaTime;
                    Direction = -1;
                    if (state != "running" && !Attacking)
                    {
                        state = "running";
                    }
                }
                else if (KeyboardState.IsKeyDown(Keys.A) && state != "dashing" && state != "attacking" && state != "jumping")
                {
                    Velocity.X -= Speed * DeltaTime;
                    Direction = 1;
                    if (state != "running" && !Attacking && state != "dashing")
                    {
                        state = "running";
                    }
                }
                else
                {
                    if (state == "attacking")
                    {
                        Velocity.X *= 0.85f;
                    }
                    else if (state != "dashing")
                    {
                        Velocity.X *= 0.75f;
                    }
                    if (state != "idle" && !Attacking && state != "dashing" && state != "jumping")
                    {
                        frametimer = 0;
                        state = "idle";
                    }

                    /*if (CanJump)
                    {
                        if (state != "idle" && !Attacking && state != "dashing")
                        {
                            if (state == "attacking")
                            {
                            *//*if (Direction == 1)
                                Position.X += 32*scale;*//*
                            }

                         
                            state = "idle";
                            frame = 0;
                            frametimer = 0;
                        }
                        
                    }
                    else
                    {
                        state = "jumping";
                        frame = 0;
                        frametimer = 0;
                    }*/
                }
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift) && !PreviousKeyBoardState.IsKeyDown(Keys.LeftShift) && candash && power == "Dash")
            {
                if (state != "dashing" && !Attacking)
                {
                    Velocity.X = dashspeed * -Direction;
                    state = "dashing";
                }
            }
            /*if (KeyboardState.IsKeyDown(Keys.W))
           {
               Velocity.Y -= Speed * DeltaTime;
           }
           else if (KeyboardState.IsKeyDown(Keys.S))
           {
               Velocity.Y += Speed * DeltaTime;
           }
           else Velocity.Y *= 0.75f; */
            if (state != "dashing")
            {
                if(Boosttime > 0)
                    Velocity.X = Math.Max(-MaxSpeed-SpeedBoost, Math.Min(MaxSpeed + SpeedBoost, Velocity.X));
                else
                    Velocity.X = Math.Max(-MaxSpeed , Math.Min(MaxSpeed , Velocity.X));
            }
            /*Velocity.Y = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.Y)); */
            if ((KeyboardState.IsKeyDown(Keys.Space) && PreviousKeyBoardState.IsKeyDown(Keys.Space) != KeyboardState.IsKeyDown(Keys.Space)) || (KeyboardState.IsKeyDown(Keys.W) && PreviousKeyBoardState.IsKeyDown(Keys.W) != KeyboardState.IsKeyDown(Keys.W)) && KnockbackTime <= 0)
            {
                PreJumpTime = PreJumpTimer;
                jumped = false;
            }
            if (JumpCount > 0 && CanJump && PreJumpTime > 0 && !jumped && KnockbackTime <= 0)
            {
                Velocity.Y = -JumpPower * DeltaTime;
                JumpCount--;
                jumped = true;
            }
            Velocity.Y = Math.Min(Velocity.Y, MaxGravity);

            if (state == "dashing")
            {

                Velocity.Y = 0;
                dashtimer += DeltaTime;
                if (dashtimer > maxdashtimer)
                {
                    dashtimer = 0;
                    state = "running";
                    dashcooldown = 0;
                    candash = false;
                }
            }
            if (dashcooldown < dashcooldowntime)
            {
                dashcooldown += DeltaTime;
            }
            else
            {
                candash = true;
            }
            PreviousPosition = Position;
            Position += new Vector2((int)Velocity.X, (int)Velocity.Y);
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, hitbox.Width, hitbox.Height);
            flashsize = new Rectangle((int)campos.X, (int)campos.Y, 480, 270);
            if (KeyboardState.IsKeyDown(Keys.G) && !PreviousKeyBoardState.IsKeyDown(Keys.G))
            {
                Position.X++;
            }
            //Debug.WriteLine(Position);
            if (Health > 0)
            {
                TileCollision(tiles);
                PlatformCollision(platforms);
                MeleeEnemiesCollision();
                RangedEnemyProjectileCollision();
                ItemsCollision();
            }
            else
            {
                
                if(!dead)
                {
                    dead = true;
                    Velocity.Y = -10;
                }
                LockCamera = true;
            }
            if (Position.Y > 800)
                Health = 0;
            if (Health > MaxHealth) 
            {
                Health = MaxHealth;
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed && CanAttack && !Attacking)
            {
                Attacking = true;
                AttackTime = AttackTimer4;
                AttackCombo = 4;
                AttackComboTime = AttackComboTimer;
            }
            if (KeyboardState.IsKeyDown(Keys.E) && CanAttack && !Attacking && power == "Charge Attack")
            {
                Attacking = true;
                AttackTime = AttackTimer5;
                AttackCombo = 5;
                AttackComboTime = AttackComboTimer;
            }
            if (KeyboardState.IsKeyDown(Keys.E) && CanAttack && !Attacking && power == "Flash")
            {
                Attacking = true;
                AttackTime = AttackTimer6;
                AttackCombo = 6;
                AttackComboTime = AttackComboTimer;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && CanAttack && !Attacking)
            {
                if(hit != null)
                {
                    hit.Play();
                }
                Attacking = true;
                if (AttackCombo == 0)
                    AttackTime = AttackTimer;
                else if (AttackCombo == 1)
                    AttackTime = AttackTimer2;
                else AttackTime = AttackTimer3;
                AttackCombo += 1;
                AttackComboTime = AttackComboTimer;
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released && !Attacking && Mouse.GetState().RightButton == ButtonState.Released)
            {
                CanAttack = true;
            }
            if (AttackTime > 0)
            {
                CanAttack = false;
                AttackTime -= DeltaTime;
            }
            else
            {
                Attacking = false;
                if (AttackCombo >= AttackMaxCombo)
                    AttackCombo = 0;
            }

            if (AttackComboTime > 0)
                AttackComboTime -= DeltaTime;
            else
            {
                AttackCombo = 0;
                AttackComboTime = 0;
            }

            if (Direction != 1)
                /*AttackPosition.X = Position.X + hitbox.Width;*/
                AttackPosition.X = Position.X + hitbox.Width;


            else AttackPosition.X = Position.X - (AttackCombo == 1 ? AttackRectangle : (AttackCombo == 2 ? AttackRectangle2 : AttackRectangle3)).Width;
            AttackPosition.Y = Position.Y + hitbox.Height / 2 - (AttackCombo == 1 ? AttackRectangle : (AttackCombo == 2 ? AttackRectangle2 : AttackRectangle3)).Height / 2;
            if (AttackCombo == 1)
            {
                Damage = 10;
            }
            else if (AttackCombo == 2)
            {
                Damage = 20;
            }
            else if (AttackCombo == 3)
            {
                Damage = 30;
            }
            //Debug.WriteLine(AttackTime);
            /*AttackRectangle.X = (int)AttackPosition.X + ((Direction == 1) ? +30 : -60);
            AttackRectangle.Y = (int)AttackPosition.Y;
            AttackRectangle2.X = (int)AttackPosition.X + ((Direction == 1) ? +16 : -8);
            AttackRectangle2.Y = (int)AttackPosition.Y;
            AttackRectangle3.X = (int)AttackPosition.X + ((Direction == 1) ? +20 : -20);
            AttackRectangle3.Y = (int)AttackPosition.Y+2;*/
            AttackRectangle.X = (int)AttackPosition.X;
            AttackRectangle.Y = (int)AttackPosition.Y;
            AttackRectangle2.X = (int)AttackPosition.X;
            AttackRectangle2.Y = (int)AttackPosition.Y;
            AttackRectangle3.X = (int)AttackPosition.X;
            AttackRectangle3.Y = (int)AttackPosition.Y;
            PreviousKeyBoardState = KeyboardState;


            if (state == "idle")
            {
                framesize = new Vector2(112, 54);
                if (JumpCount > 0)
                {
                    maxframe = 4;
                    maxframetimer = 1f / maxframe;
                    if (Direction == 1)
                    {
                        row = 0;
                    }
                    else
                    {
                        row = 1;
                    }
                }
                else
                {
                    if (frame > 2)
                    {
                        frame = 2;
                    }
                    row = 8;

                    maxframe = 2;
                    maxframetimer = 1f / maxframe;
                }
            }
            if (state == "running")
            {
                framesize = new Vector2(112, 54);
                if (JumpCount > 0)
                {
                    maxframe = 6;
                    maxframetimer = 0.5f / maxframe;
                    if (Direction == 1)
                    {
                        row = 2;
                    }
                    else
                    {
                        row = 3;
                    }
                }
                else
                {
                    if (frame > 2)
                    {
                        frame = 2;
                    }
                    row = 8;

                    maxframe = 2;
                    maxframetimer = 1f / maxframe;
                }
            }
            if (Attacking == true)
            {
                if (state != "attacking")
                {
                    if (AttackCombo == 1)
                    {
                        row = 4;
                        maxframe = 4;
                        maxframetimer = AttackTimer / maxframe;
                    }
                    if (AttackCombo == 2)
                    {
                        row = 5;
                        maxframe = 6;
                        maxframetimer = AttackTimer2 / maxframe;
                    }
                    if (AttackCombo == 3)
                    {
                        row = 6;
                        maxframe = 9;
                        maxframetimer = AttackTimer3 / maxframe;
                    }
                    if (AttackCombo == 4)
                    {
                        row = 9;
                        maxframe = 4;
                        maxframetimer = AttackTimer4 / maxframe;
                    }
                    if (AttackCombo == 5)
                    {
                        row = 10;
                        maxframe = 12;
                        maxframetimer = AttackTimer5 / maxframe;
                    }
                    if (AttackCombo == 6)
                    {
                        row = 11;
                        maxframe = 10;
                        maxframetimer = AttackTimer6 / maxframe;
                    }
                    state = "attacking";
                    /*if (Direction == 1)
                        Position.X -= 32*scale;*/
                }
            }
            if (state == "attacking")
            {
                framesize = new Vector2(112, 54);
                if (AttackCombo == 1)
                {
                    row = 4;
                    maxframe = 4;
                    maxframetimer = AttackTimer / maxframe;
                }
                if (AttackCombo == 2)
                {
                    row = 5;
                    maxframe = 6;
                    maxframetimer = AttackTimer2 / maxframe;
                }
                if (AttackCombo == 3)
                {
                    row = 6;
                    maxframe = 9;
                    maxframetimer = AttackTimer3 / maxframe;
                }
                if (AttackCombo == 4)
                {
                    row = 9;
                    maxframe = 4;
                    maxframetimer = AttackTimer4 / maxframe;
                }
                if (AttackCombo == 5)
                {
                    row = 10;
                    maxframe = 12;
                    maxframetimer = AttackTimer5 / maxframe;
                }
                if (AttackCombo == 6)
                {
                    row = 11;
                    maxframe = 10;
                    maxframetimer = AttackTimer6 / maxframe;
                }
            }
            /*if ( maxframe == 12)
            */
            if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 3 && state == "attacking")
            {
                attackactive = true;
            }
            if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 1 && state == "attacking")
            {
                attackactive = true;
            }
            if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 2 && state == "attacking")
            {
                attackactive = true;
            }
            if (state != "attacking")
            {
                attackactive = false;
            }
            if (AttackTime < 0 && AttackCombo == 4)
            {
                PlayerProjectile.Add(new PlayerProjectile(Projectile, Position + new Vector2(0, hitbox.Height / 2), new Vector2(9, 9), Color.White, Window, Direction, "normal"));
            }
            if (AttackTime < 0 && AttackCombo == 5)
            {
                PlayerProjectile.Add(new PlayerProjectile(Projectile, Position + new Vector2(0, (hitbox.Height / 2) - 8), new Vector2(32, 32), Color.White, Window, Direction, "special"));
            }
            if (AttackTime < 0 && AttackCombo == 6)
            {
                flash = true;
            }
            if (AttackCombo > 3)
            {
                attackactive = false;
            }
            if (flashvalue <= 1 && AttackCombo == 6 && frame >= 5)
            {
                flashvalue += DeltaTime * 10;
            }
            else if (flashvalue > 1 && AttackCombo == 6 && frame >= 5)
            {
                flashvalue = 1;
            }
            else if (flashvalue >= 0) 
            { 
                flashvalue -= DeltaTime * 10; 
            }
            else
            {
                flashvalue = 0;
            }


            foreach (var Projectile in PlayerProjectile)
            {
                Projectile.Update(gameTime);
            }
            PlayerProjectile.RemoveAll((Projectile) => Projectile.Position.X - campos.X > Window.ClientBounds.Width || Projectile.Position.X - campos.X < 0 - Projectile.Rectangle.Width || Projectile.hit);
            if (state == "dashing")
            {
                framesize = new Vector2(112, 54);
                if (Direction == 1)
                {
                    row = 7;
                }
                else
                {
                    row = 7;
                }

                maxframe = 8;
                maxframetimer = maxdashtimer / maxframe;
            }
            if (state == "jumping")
            {
                framesize = new Vector2(112, 54);

                row = 8;

                maxframe = 2;
                maxframetimer = 0.15f;
            }
            //Scale = framesize *2;
            frametimer += DeltaTime;

            if (frametimer > maxframetimer)
            {

                frametimer = 0;
                
                frame++;
                if (frame >= maxframe)
                {
                    if (state != "attacking")
                    {
                        frame = 0;
                    }
                    else frame = maxframe-1;
                    
                }
            }
            if (prestate != state)
            {
                frame = 0;
                frametimer = 0;
            }
            prestate = state;
        }
        public void Update(GameTime gameTime, Vector2 campos, List<Rectangle> tiles, List<Rectangle> platforms, List<Rectangle> bossattacks)
        {
            {
                if (power == "Double Jump")
                {
                    MaxJump = 2;
                }
                else MaxJump = 1;
                flash = false;
                this.campos = campos;
                attackactive = false;
                KeyboardState = Keyboard.GetState();
                float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Boosttime -= DeltaTime;
                if (InvincibleTime > 0)
                {
                    InvincibleTime -= DeltaTime;
                }
                if (KnockbackTime > 0)
                {
                    KnockbackTime -= DeltaTime;
                }
                if (CoyoteTime > 0)
                {
                    CoyoteTime -= DeltaTime;
                }
                else if (JumpCount == MaxJump)
                {
                    JumpCount--;
                }
                if (PreJumpTime > 0)
                {
                    PreJumpTime -= DeltaTime;
                }
                Velocity.Y += Gravity * DeltaTime;

                if (KeyboardState.IsKeyDown(Keys.D1) && !PreviousKeyBoardState.IsKeyDown(Keys.D1))
                {
                    if (Health < MaxHealth)
                    {
                        if (potion1 > 0)
                        {
                            potion1--;
                            Health += healpower;
                        }
                    }
                }
                if (KeyboardState.IsKeyDown(Keys.D2) && !PreviousKeyBoardState.IsKeyDown(Keys.D2))
                {
                    if (potion2 > 0)
                    {
                        potion2--;
                        Boosttime = 30;
                    }
                }

                if (KnockbackTime <= 0)
                {
                    if (KeyboardState.IsKeyDown(Keys.D) && state != "dashing" && state != "attacking" && state != "jumping")
                    {
                        Velocity.X += Speed * DeltaTime;
                        Direction = -1;
                        if (state != "running" && !Attacking)
                        {
                            state = "running";
                        }
                    }
                    else if (KeyboardState.IsKeyDown(Keys.A) && state != "dashing" && state != "attacking" && state != "jumping")
                    {
                        Velocity.X -= Speed * DeltaTime;
                        Direction = 1;
                        if (state != "running" && !Attacking && state != "dashing")
                        {
                            state = "running";
                        }
                    }
                    else
                    {
                        if (state == "attacking")
                        {
                            Velocity.X *= 0.85f;
                        }
                        else if (state != "dashing")
                        {
                            Velocity.X *= 0.75f;
                        }
                        if (state != "idle" && !Attacking && state != "dashing" && state != "jumping")
                        {
                            frametimer = 0;
                            state = "idle";
                        }

                        /*if (CanJump)
                        {
                            if (state != "idle" && !Attacking && state != "dashing")
                            {
                                if (state == "attacking")
                                {
                                *//*if (Direction == 1)
                                    Position.X += 32*scale;*//*
                                }


                                state = "idle";
                                frame = 0;
                                frametimer = 0;
                            }

                        }
                        else
                        {
                            state = "jumping";
                            frame = 0;
                            frametimer = 0;
                        }*/
                    }
                }
                if (KeyboardState.IsKeyDown(Keys.LeftShift) && !PreviousKeyBoardState.IsKeyDown(Keys.LeftShift) && candash && power == "Dash")
                {
                    if (state != "dashing" && !Attacking)
                    {
                        Velocity.X = dashspeed * -Direction;
                        state = "dashing";
                    }
                }
                /*if (KeyboardState.IsKeyDown(Keys.W))
               {
                   Velocity.Y -= Speed * DeltaTime;
               }
               else if (KeyboardState.IsKeyDown(Keys.S))
               {
                   Velocity.Y += Speed * DeltaTime;
               }
               else Velocity.Y *= 0.75f; */
                if (state != "dashing")
                {
                    if (Boosttime > 0)
                        Velocity.X = Math.Max(-MaxSpeed - SpeedBoost, Math.Min(MaxSpeed + SpeedBoost, Velocity.X));
                    else
                        Velocity.X = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.X));
                }
                /*Velocity.Y = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.Y)); */
                if ((KeyboardState.IsKeyDown(Keys.Space) && PreviousKeyBoardState.IsKeyDown(Keys.Space) != KeyboardState.IsKeyDown(Keys.Space)) || (KeyboardState.IsKeyDown(Keys.W) && PreviousKeyBoardState.IsKeyDown(Keys.W) != KeyboardState.IsKeyDown(Keys.W)) && KnockbackTime <= 0)
                {
                    PreJumpTime = PreJumpTimer;
                    jumped = false;
                }
                if (JumpCount > 0 && CanJump && PreJumpTime > 0 && !jumped && KnockbackTime <= 0)
                {
                    Velocity.Y = -JumpPower * DeltaTime;
                    JumpCount--;
                    jumped = true;
                }
                Velocity.Y = Math.Min(Velocity.Y, MaxGravity);

                if (state == "dashing")
                {

                    Velocity.Y = 0;
                    dashtimer += DeltaTime;
                    if (dashtimer > maxdashtimer)
                    {
                        dashtimer = 0;
                        state = "running";
                        dashcooldown = 0;
                        candash = false;
                    }
                }
                if (dashcooldown < dashcooldowntime)
                {
                    dashcooldown += DeltaTime;
                }
                else
                {
                    candash = true;
                }
                PreviousPosition = Position;
                Position += new Vector2((int)Velocity.X, (int)Velocity.Y);
                hitbox = new Rectangle((int)Position.X, (int)Position.Y, hitbox.Width, hitbox.Height);
                flashsize = new Rectangle((int)campos.X, (int)campos.Y, 480, 270);
                if (KeyboardState.IsKeyDown(Keys.G) && !PreviousKeyBoardState.IsKeyDown(Keys.G)) 
                {
                    Position.X++;
                }
                //Debug.WriteLine(Position);
                if (Health > 0)
                {
                    TileCollision(tiles);
                    PlatformCollision(platforms);
                    bossattackscheck(bossattacks);
                    if (Position.X < 0)
                    {
                        Position.X = 0;
                        Velocity.X = 0;
                    }
                    else if (Position.X > 448)
                    {
                        Position.X = 448;
                        Velocity.X = 0;
                    }
                    
                }
                else
                {

                    if (!dead)
                    {
                        dead = true;
                        Velocity.Y = -10;
                    }
                    LockCamera = true;
                }
                if (Position.Y > 800)
                    Health = 0;
                
                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
                if (Mouse.GetState().RightButton == ButtonState.Pressed && CanAttack && !Attacking)
                {
                    Attacking = true;
                    AttackTime = AttackTimer4;
                    AttackCombo = 4;
                    AttackComboTime = AttackComboTimer;
                }
                if (KeyboardState.IsKeyDown(Keys.E) && CanAttack && !Attacking && power == "Charge Attack")
                {
                    Attacking = true;
                    AttackTime = AttackTimer5;
                    AttackCombo = 5;
                    AttackComboTime = AttackComboTimer;
                }
                if (KeyboardState.IsKeyDown(Keys.E) && CanAttack && !Attacking && power == "Flash")
                {
                    Attacking = true;
                    AttackTime = AttackTimer6;
                    AttackCombo = 6;
                    AttackComboTime = AttackComboTimer;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && CanAttack && !Attacking)
                {
                    Attacking = true;
                    if (AttackCombo == 0)
                        AttackTime = AttackTimer;
                    else if (AttackCombo == 1)
                        AttackTime = AttackTimer2;
                    else AttackTime = AttackTimer3;
                    AttackCombo += 1;
                    AttackComboTime = AttackComboTimer;
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && !Attacking && Mouse.GetState().RightButton == ButtonState.Released)
                {
                    CanAttack = true;
                }
                if (AttackTime > 0)
                {
                    CanAttack = false;
                    AttackTime -= DeltaTime;
                }
                else
                {
                    Attacking = false;
                    if (AttackCombo >= AttackMaxCombo)
                        AttackCombo = 0;
                }

                if (AttackComboTime > 0)
                    AttackComboTime -= DeltaTime;
                else
                {
                    AttackCombo = 0;
                    AttackComboTime = 0;
                }

                if (Direction != 1)
                    /*AttackPosition.X = Position.X + hitbox.Width;*/
                    AttackPosition.X = Position.X + hitbox.Width;


                else AttackPosition.X = Position.X - (AttackCombo == 1 ? AttackRectangle : (AttackCombo == 2 ? AttackRectangle2 : AttackRectangle3)).Width;
                AttackPosition.Y = Position.Y + hitbox.Height / 2 - (AttackCombo == 1 ? AttackRectangle : (AttackCombo == 2 ? AttackRectangle2 : AttackRectangle3)).Height / 2;
                if (AttackCombo == 1)
                {
                    Damage = 20;
                }
                else if (AttackCombo == 2)
                {
                    Damage = 30;
                }
                else if (AttackCombo == 3)
                {
                    Damage = 50;
                }
                //Debug.WriteLine(AttackTime);
                /*AttackRectangle.X = (int)AttackPosition.X + ((Direction == 1) ? +30 : -60);
                AttackRectangle.Y = (int)AttackPosition.Y;
                AttackRectangle2.X = (int)AttackPosition.X + ((Direction == 1) ? +16 : -8);
                AttackRectangle2.Y = (int)AttackPosition.Y;
                AttackRectangle3.X = (int)AttackPosition.X + ((Direction == 1) ? +20 : -20);
                AttackRectangle3.Y = (int)AttackPosition.Y+2;*/
                AttackRectangle.X = (int)AttackPosition.X;
                AttackRectangle.Y = (int)AttackPosition.Y;
                AttackRectangle2.X = (int)AttackPosition.X;
                AttackRectangle2.Y = (int)AttackPosition.Y;
                AttackRectangle3.X = (int)AttackPosition.X;
                AttackRectangle3.Y = (int)AttackPosition.Y;
                PreviousKeyBoardState = KeyboardState;


                if (state == "idle")
                {
                    framesize = new Vector2(112, 54);
                    if (JumpCount > 0)
                    {
                        maxframe = 4;
                        maxframetimer = 1f / maxframe;
                        if (Direction == 1)
                        {
                            row = 0;
                        }
                        else
                        {
                            row = 1;
                        }
                    }
                    else
                    {
                        if (frame > 2)
                        {
                            frame = 2;
                        }
                        row = 8;

                        maxframe = 2;
                        maxframetimer = 1f / maxframe;
                    }
                }
                if (state == "running")
                {
                    framesize = new Vector2(112, 54);
                    if (JumpCount > 0)
                    {
                        maxframe = 6;
                        maxframetimer = 0.5f / maxframe;
                        if (Direction == 1)
                        {
                            row = 2;
                        }
                        else
                        {
                            row = 3;
                        }
                    }
                    else
                    {
                        if (frame > 2)
                        {
                            frame = 2;
                        }
                        row = 8;

                        maxframe = 2;
                        maxframetimer = 1f / maxframe;
                    }
                }
                if (Attacking == true)
                {
                    if (state != "attacking")
                    {
                        if (AttackCombo == 1)
                        {
                            row = 4;
                            maxframe = 4;
                            maxframetimer = AttackTimer / maxframe;
                        }
                        if (AttackCombo == 2)
                        {
                            row = 5;
                            maxframe = 6;
                            maxframetimer = AttackTimer2 / maxframe;
                        }
                        if (AttackCombo == 3)
                        {
                            row = 6;
                            maxframe = 9;
                            maxframetimer = AttackTimer3 / maxframe;
                        }
                        if (AttackCombo == 4)
                        {
                            row = 9;
                            maxframe = 4;
                            maxframetimer = AttackTimer4 / maxframe;
                        }
                        if (AttackCombo == 5)
                        {
                            row = 10;
                            maxframe = 12;
                            maxframetimer = AttackTimer5 / maxframe;
                        }
                        if (AttackCombo == 6)
                        {
                            row = 11;
                            maxframe = 10;
                            maxframetimer = AttackTimer6 / maxframe;
                        }
                        state = "attacking";
                        /*if (Direction == 1)
                            Position.X -= 32*scale;*/
                    }
                }
                if (state == "attacking")
                {
                    framesize = new Vector2(112, 54);
                    if (AttackCombo == 1)
                    {
                        row = 4;
                        maxframe = 4;
                        maxframetimer = AttackTimer / maxframe;
                    }
                    if (AttackCombo == 2)
                    {
                        row = 5;
                        maxframe = 6;
                        maxframetimer = AttackTimer2 / maxframe;
                    }
                    if (AttackCombo == 3)
                    {
                        row = 6;
                        maxframe = 9;
                        maxframetimer = AttackTimer3 / maxframe;
                    }
                    if (AttackCombo == 4)
                    {
                        row = 9;
                        maxframe = 4;
                        maxframetimer = AttackTimer4 / maxframe;
                    }
                    if (AttackCombo == 5)
                    {
                        row = 10;
                        maxframe = 12;
                        maxframetimer = AttackTimer5 / maxframe;
                    }
                    if (AttackCombo == 6)
                    {
                        row = 11;
                        maxframe = 10;
                        maxframetimer = AttackTimer6 / maxframe;
                    }
                }
                /*if ( maxframe == 12)
                */
                if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 3 && state == "attacking")
                {
                    attackactive = true;
                }
                if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 1 && state == "attacking")
                {
                    attackactive = true;
                }
                if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 2 && state == "attacking")
                {
                    attackactive = true;
                }
                if (state != "attacking")
                {
                    attackactive = false;
                }
                if (AttackTime < 0 && AttackCombo == 4)
                {
                    PlayerProjectile.Add(new PlayerProjectile(Projectile, Position + new Vector2(0, hitbox.Height / 2), new Vector2(9, 9), Color.White, Window, Direction, "normal"));
                }
                if (AttackTime < 0 && AttackCombo == 5)
                {
                    PlayerProjectile.Add(new PlayerProjectile(Projectile, Position + new Vector2(0, (hitbox.Height / 2) - 8), new Vector2(32, 32), Color.White, Window, Direction, "special"));
                }
                if (AttackTime < 0 && AttackCombo == 6)
                {
                    flash = true;
                }
                if (AttackCombo > 3)
                {
                    attackactive = false;
                }
                if (flashvalue <= 1 && AttackCombo == 6 && frame >= 5)
                {
                    flashvalue += DeltaTime * 10;
                }
                else if (flashvalue > 1 && AttackCombo == 6 && frame >= 5)
                {
                    flashvalue = 1;
                }
                else if (flashvalue >= 0)
                {
                    flashvalue -= DeltaTime * 10;
                }
                else
                {
                    flashvalue = 0;
                }


                foreach (var Projectile in PlayerProjectile)
                {
                    Projectile.Update(gameTime);
                }
                PlayerProjectile.RemoveAll((Projectile) => Projectile.Position.X - campos.X > Window.ClientBounds.Width || Projectile.Position.X - campos.X < 0 - Projectile.Rectangle.Width || Projectile.hit);
                if (state == "dashing")
                {
                    framesize = new Vector2(112, 54);
                    if (Direction == 1)
                    {
                        row = 7;
                    }
                    else
                    {
                        row = 7;
                    }

                    maxframe = 8;
                    maxframetimer = maxdashtimer / maxframe;
                }
                if (state == "jumping")
                {
                    framesize = new Vector2(112, 54);

                    row = 8;

                    maxframe = 2;
                    maxframetimer = 0.15f;
                }
                //Scale = framesize *2;
                frametimer += DeltaTime;

                if (frametimer > maxframetimer)
                {

                    frametimer = 0;

                    frame++;
                    if (frame >= maxframe)
                    {
                        if (state != "attacking")
                        {
                            frame = 0;
                        }
                        else frame = maxframe - 1;

                    }
                }
                if (prestate != state)
                {
                    frame = 0;
                    frametimer = 0;
                }
                prestate = state;
            }
        }
        public void ItemsCollision()
        {
            foreach (var item in Items) 
            {
                if(hitbox.Intersects(item.Rectangle))
                {
                    if (item.type == 1)
                    {
                        potion1++;
                    }
                    else if(item.type == 2)
                    {
                        potion2++;
                    }
                }
            }
        }
        public void MeleeEnemiesCollision()
        {
            
            foreach (var meleeenemy in MeleeEnemies)
            {
                if (InvincibleTime <= 0 && state != "dashing"&&!meleeenemy.stunned)
                {
                    if (hitbox.Intersects(meleeenemy.Rectangle))
                    {
                        KnockbackTime = KnockbackTimer;
                        InvincibleTime = InvincibleTimer;
                        if (Position.X > meleeenemy.Position.X)
                        {
                            Velocity = Knockback;
                        }
                        else Velocity = new Vector2 (-Knockback.X, Knockback.Y);
                        Health -= meleeenemy.Damage;
                    }
                }
            }
        }
        public void bossattackscheck(List<Rectangle> bossattacks)
        {
            foreach (var bossattack in bossattacks)
            {
                if (InvincibleTime <= 0 && state != "dashing")
                {
                    if (hitbox.Intersects(bossattack))
                    {
                        KnockbackTime = KnockbackTimer;
                        InvincibleTime = InvincibleTimer;
                        if (Position.X > bossattack.X)
                        {
                            Velocity = Knockback;
                        }
                        else Velocity = new Vector2(-Knockback.X, Knockback.Y);
                        Health -= 10;
                    }
                }
            }
            //recs = bossattacks;
        }
        public void RangedEnemyProjectileCollision()
        {
            foreach (var rangeenemy in RangedEnemies)
            {
                foreach(var projectile in rangeenemy.RangedEnemyProjectiles)
                    if (InvincibleTime <= 0 && state != "dashing")
                    {
                        if (hitbox.Intersects(projectile.Rectangle))
                        {
                            KnockbackTime = KnockbackTimer;
                            InvincibleTime = InvincibleTimer;
                            if (Position.X > projectile.Position.X)
                            {
                                Velocity = Knockback;
                            }
                            else Velocity = new Vector2 (-Knockback.X, Knockback.Y);
                            Health -= projectile.Damage;
                        }
                    }
            }
        }
        public void TileCollision(List<Rectangle> tiles)
        {
            foreach (var Wall in tiles)
            {
                float moveright = Wall.X + Wall.Width;
                float moveleft = Wall.X - hitbox.Width;
                float moveup = Wall.Y - hitbox.Height;
                float movedown = Wall.Y + Wall.Height;

                Vector2 topleft = Position + new Vector2(0, 0); ;
                Vector2 topright = Position + new Vector2(hitbox.Width, 0);
                Vector2 botttomleft = Position + new Vector2(0, hitbox.Height);
                Vector2 botttomright = Position + new Vector2(hitbox.Width, hitbox.Height);

                Vector2 pos1 = Vector2.Zero;
                Vector2 pos2 = Vector2.Zero;
                Vector2 pos3 = Vector2.Zero;
                pos1 = new Vector2(Position.X + (hitbox.Width / 2), Position.Y + (hitbox.Height / 2));
                pos2 = new Vector2(Wall.X + (Wall.Width / 2), Wall.Y + (Wall.Height / 2));
                pos3 = Vector2.Normalize(pos1 - pos2);
                if (hitbox.Intersects(Wall))
                {
                    if (pos3.X <= 0 && pos3.Y <= 0)
                    {
                        if (Math.Abs(botttomright.X - Wall.X) < Math.Abs(botttomright.Y - Wall.Y))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                            
                        }
                        else if (Math.Abs(botttomright.X - Wall.X) > Math.Abs(botttomright.Y - Wall.Y))
                        {
                            
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            JumpCount = MaxJump;
                            CoyoteTime = CoyoteTimer;

                        }
                    }
                    else if (pos3.X >= 0 && pos3.Y <= 0)
                    {
                        
                        if (Math.Abs(botttomleft.X - (Wall.X + Wall.Width)) < Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            /*Debug.WriteLine(Position);
                            Debug.WriteLine(Wall.X + " " + Wall.Y);
                            Debug.WriteLine(pos1);
                            Debug.WriteLine(pos2);
                            Debug.WriteLine(pos3);
                            Debug.WriteLine(Math.Abs(botttomleft.X - Wall.X - Wall.Width));
                            Debug.WriteLine(Math.Abs(botttomleft.Y - Wall.Y));*/
                            
                            Position.X = moveright;
                            Velocity.X = 0;
                            /*Debug.WriteLine("this one" + Wall.X + " " + Wall.Y);
                            Debug.WriteLine(Position);*/
                            
                        }
                        else if (Math.Abs(botttomleft.X - (Wall.X + Wall.Width)) > Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            JumpCount = MaxJump;
                            CoyoteTime = CoyoteTimer;
                        }

                    }
                    else if (pos3.X <= 0 && pos3.Y >= 0)
                    {
                        if (Math.Abs(topright.X - Wall.X) < Math.Abs(topright.Y - (Wall.Y + Wall.Height)))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(topright.X - Wall.X) > Math.Abs(topright.Y - (Wall.Y + Wall.Height)))
                        {
                            Position.Y = movedown;
                            Velocity.Y = 0;
                        }
                    }
                    else if (pos3.X >= 0 && pos3.Y >= 0)
                    {
                        if (Math.Abs(topleft.X - (Wall.X + Wall.Width)) < Math.Abs(topleft.Y - (Wall.Y + Wall.Height)))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                            
                        }
                        else if (Math.Abs(topleft.X - (Wall.X + Wall.Width)) > Math.Abs(topleft.Y - (Wall.Y + Wall.Height)))
                        {
                            Position.Y = movedown;
                            Velocity.Y = 0;
                        }
                    }
                }
            }
        }
        /*public void WallCollision()
        {
            foreach (var Wall in Walls)
            {
                float moveright = Wall.Position.X + Wall.Rectangle.Width;
                float moveleft = Wall.Position.X - Rectangle.Width;
                float moveup = Wall.Position.Y - Rectangle.Height;
                float movedown = Wall.Position.Y + Wall.Rectangle.Height;

                Vector2 topleft = Position;
                Vector2 topright = Position + new Vector2(Rectangle.Width,0);
                Vector2 botttomleft = Position + new Vector2(0,Rectangle.Height);
                Vector2 botttomright = Position + new Vector2(Rectangle.Width,Rectangle.Height);

                Vector2 pos1 = Vector2.Zero;
                Vector2 pos2 = Vector2.Zero;
                Vector2 pos3 = Vector2.Zero;
                pos1 = new Vector2(Position.X+Rectangle.Width/2,Position.Y+Rectangle.Height/2);
                pos2 = new Vector2(Wall.Position.X+Wall.Rectangle.Width/2,Wall.Position.Y+Wall.Rectangle.Height/2);
                pos3 = Vector2.Normalize(pos1-pos2);
                if(Rectangle.Intersects(Wall.Rectangle))
                {
                    if (pos3.X < 0 && pos3.Y < 0)
                    {
                        if (Math.Abs(botttomright.X - Wall.Position.X) < Math.Abs(botttomright.Y - Wall.Position.Y))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(botttomright.X - Wall.Position.X) > Math.Abs(botttomright.Y - Wall.Position.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            JumpCount = MaxJump;
                            CoyoteTime = CoyoteTimer;
                        }
                    }
                    else if (pos3.X > 0 && pos3.Y < 0)
                    {
                        if (Math.Abs(botttomleft.X - Wall.Position.X - Wall.Rectangle.Width) < Math.Abs(botttomleft.Y - Wall.Position.Y))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(botttomleft.X - Wall.Position.X - Wall.Rectangle.Width) > Math.Abs(botttomleft.Y - Wall.Position.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            JumpCount = MaxJump;
                            CoyoteTime = CoyoteTimer;
                        }
                    }
                    else if (pos3.X < 0 && pos3.Y > 0)
                    {
                        if (Math.Abs(topright.X - Wall.Position.X) < Math.Abs(topright.Y - Wall.Position.Y - Wall.Rectangle.Height))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(topright.X - Wall.Position.X) > Math.Abs(topright.Y - Wall.Position.Y - Wall.Rectangle.Height))
                        {
                            Position.Y = movedown;
                            Velocity.Y = 0;
                        }
                    }
                    else if (pos3.X > 0 && pos3.Y > 0)
                    {
                        if (Math.Abs(topleft.X - Wall.Position.X - Wall.Rectangle.Width) < Math.Abs(topleft.Y - Wall.Position.Y - Wall.Rectangle.Height))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(topleft.X - Wall.Position.X - Wall.Rectangle.Width) > Math.Abs(topleft.Y - Wall.Position.Y - Wall.Rectangle.Height))
                        {
                            Position.Y = movedown;
                            Velocity.Y = 0;
                        }
                    }
                }
            }
        }*/
        bool FallThrough = false;
        public void PlatformCollision(List<Rectangle> platforms)
        {
            bool IntersectPlatform = false;
            foreach (var Platform in platforms)
            {
                if (hitbox.Intersects(Platform))
                {
                    IntersectPlatform = true;
                    if (KeyboardState.IsKeyDown(Keys.S))
                    {
                        FallThrough = true;
                    }
                    if (Velocity.Y == MaxGravity && !FallThrough)
                    {

                        Position.Y = Platform.Y - hitbox.Height;
                        Velocity.Y = 0;
                        JumpCount = MaxJump;
                        CoyoteTime = CoyoteTimer;
                        
                    }
                    else if (Velocity.Y > 0 && Position.Y - (hitbox.Height * 0.1) < Platform.Y && !FallThrough)
                    {
                        Position.Y = Platform.Y - hitbox.Height;
                        Velocity.Y = 0;
                        JumpCount = MaxJump;
                        CoyoteTime = CoyoteTimer;
                    }
                }
            }
            if (!IntersectPlatform)
            {
                FallThrough = false;
            }
        }
        /*public void platformCollision()
        {
            bool IntersectPlatform = false;
            foreach (var Platform in Platforms)
            {
                if(Rectangle.Intersects(Platform.Rectangle))
                {
                    IntersectPlatform = true;
                    if(KeyboardState.IsKeyDown(Keys.S))
                    {
                        FallThrough = true;
                    }
                    if(Velocity.Y == MaxGravity && !FallThrough)
                    {
                        
                        Position.Y = Platform.Position.Y - Rectangle.Height;
                        Velocity.Y = 0;
                        JumpCount = MaxJump;
                        CoyoteTime = CoyoteTimer;
                    }
                    else if(Velocity.Y > 0 && Position.Y + (Rectangle.Height*0.75) < Platform.Position.Y && !FallThrough)
                    {
                        
                        Position.Y = Platform.Position.Y - Rectangle.Height;
                        Velocity.Y = 0;
                        JumpCount = MaxJump;
                        CoyoteTime = CoyoteTimer;
                    }
                }
            }
            if (!IntersectPlatform)
            {
                FallThrough = false;
            }
        }*/
        public void FloorCollision()
        {
            if(Position.Y > Window.ClientBounds.Height - FloorHeight - Rectangle.Height)
            {
                Position.Y = Window.ClientBounds.Height - FloorHeight - Rectangle.Height;
                Velocity.Y = 0;
                JumpCount = MaxJump;
                CoyoteTime = CoyoteTimer;
            }
        }
        public override void Draw(SpriteBatch SpriteBatch)
        {
            foreach (var Projectile in PlayerProjectile)
            {
                Projectile.Draw(SpriteBatch, campos);
            }


            //SpriteBatch.Draw(Texture, Rectangle, Rectangle, Color, 0, Vector2.Zero, SpriteEffects.None, 0);
            //SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)Scale.X, (int)Scale.Y), Color, 0, Vector2.Zero, 1, (Direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //SpriteBatch.Draw(pixel, AttackCombo == 1 ? new Rectangle(AttackRectangle.X - (int)campos.X, AttackRectangle.Y - (int)campos.Y, AttackRectangle.Width, AttackRectangle.Height) : (AttackCombo == 2 ? new Rectangle(AttackRectangle2.X - (int)campos.X, AttackRectangle2.Y - (int)campos.Y, AttackRectangle2.Width, AttackRectangle2.Height) : new Rectangle(AttackRectangle3.X - (int)campos.X, AttackRectangle3.Y - (int)campos.Y, AttackRectangle3.Width, AttackRectangle3.Height)), new Rectangle(0,0,1,1), attackactive ? Color.Red : Color.Green, 0, Vector2.Zero, SpriteEffects.None, 0);
            SpriteBatch.Draw(Texture, new Rectangle((int)Position.X-8-32*scale - (int)campos.X, (int)Position.Y - (int)campos.Y-22, Rectangle.Width, Rectangle.Height), new Rectangle (frame* (int)framesize.X, row* (int)framesize.Y, (int)framesize.X, (int)framesize.Y), (InvincibleTime<=0)?Color : Color*0.75f, 0, Vector2.Zero, (state != "attacking" && state != "dashing" && JumpCount > 0) ? SpriteEffects.None : (Direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //SpriteBatch.Draw(pixel, Position-campos, hitbox, Color.White);
            //SpriteBatch.Draw(pixel, Position-campos, new Rectangle(0,0,2,2), Color.White);
            SpriteBatch.Draw(pixel, Vector2.Zero, flashsize, Color.White*flashvalue*0.50f);
            /*foreach (var hit in MeleeEnemies)
            {
                SpriteBatch.Draw(pixel, new Vector2(hit.cliffcheck.X - campos.X, hit.cliffcheck.Y - campos.Y), hit.cliffcheck, Color.White);
            }*/
            //SpriteBatch.Draw(pixel, Position-campos, new Rectangle(0,0,2,2), Color.White);
            /*foreach (var Projectile in recs)
            {
                SpriteBatch.Draw(pixel, Projectile , Projectile, Color.White);
            }*/
        }

    }
}
