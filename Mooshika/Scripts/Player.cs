using Microsoft.Xna.Framework;
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
        public float InvincibleTimer = 2;
        public Vector2 Knockback = new Vector2(5,-10);
        public float KnockbackTime = 0;
        public float KnockbackTimer = 0.25f;
        public Vector2 Velocity = Vector2.Zero;
        public int Speed = 150;
        public int MaxSpeed = 10;
        public int Gravity = 50;
        public int MaxGravity = 20;
        KeyboardState KeyboardState;
        KeyboardState PreviousKeyBoardState;
        public bool CanJump = true;
        public int JumpPower = 1200;
        List<MeleeEnemy> MeleeEnemies = new List<MeleeEnemy>();
        List<RangedEnemy> RangedEnemies = new List<RangedEnemy>();
        Vector2 PreviousPosition;
        public float CoyoteTime = 0;
        public float CoyoteTimer = 0.25f;
        float PreJumpTime = 0;
        float PreJumpTimer = 0.25f;
        public int JumpCount = 0;
        public int MaxJump = 2;
        bool jumped = false;
        public int FloorHeight = 20;

        Texture2D pixel;
        Texture2D AttackSprite;
        public Rectangle AttackRectangle = new Rectangle(0, 0, 100, 48);
        public Rectangle AttackRectangle2 = new Rectangle(0, 0, 70, 48);
        public Rectangle AttackRectangle3 = new Rectangle(0, 0, 80, 60);
        Vector2 AttackPosition = Vector2.Zero;
        bool CanAttack = true;
        public bool Attacking = false;
        float AttackTimer = 0.32f;
        float AttackTimer2 = 0.48f;
        float AttackTimer3 = 0.72f;
        float AttackTime = 0;
        int AttackCombo = 0;
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
        public Vector2 framesize = new Vector2 (48,32);
        public Rectangle hitbox;
        public int scale = 2;
        public int dashspeed = 15;
        public bool candash = true;
        public float dashtimer = 0;
        public float maxdashtimer = 0.2f;
        public float dashcooldown = 0;
        public float dashcooldowntime = 0.3f;
        public bool LockCamera = false;

        public Vector2 campos;
        public Player (Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D pixel) : base (texture, position, scale, color, window) 
        {
            this.pixel = pixel;
            hitbox = Rectangle;
            Direction = -1;
        }
        /*public void Update(GameTime gameTime, List<Wall> walls, List<Platform> platforms, List<MeleeEnemy> meleeEnemies, List<RangedEnemy> rangedEnemies)
        {
            KeyboardState = Keyboard.GetState();
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Walls = walls;
            Platforms = platforms;
            MeleeEnemies = meleeEnemies;
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
            if (KnockbackTime <= 0)
            {
                if (KeyboardState.IsKeyDown(Keys.D))
                {
                    Velocity.X += Speed * DeltaTime;
                    Direction = -1;
                }
                else if (KeyboardState.IsKeyDown(Keys.A))
                {
                    Velocity.X -= Speed * DeltaTime;
                    Direction = 1;
                }
                else Velocity.X *= 0.75f;
            }

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                Velocity.Y -= Speed * DeltaTime;
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                Velocity.Y += Speed * DeltaTime;
            }
            else Velocity.Y *= 0.75f;
            Velocity.X = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.X));
            Velocity.Y = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.Y));
            if (KeyboardState.IsKeyDown(Keys.Space) && PreviousKeyBoardState.IsKeyDown(Keys.Space) != KeyboardState.IsKeyDown(Keys.Space) && KnockbackTime <= 0)
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

            PreviousPosition = Position;
            Position += new Vector2((int)Velocity.X, (int)Velocity.Y);
            WallCollision();
            platformCollision();
            FloorCollision();
            MeleeEnemiesCollision();
            RangedEnemyProjectileCollision();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && CanAttack && !Attacking)
            {
                Attacking = true;
                if (AttackCombo != AttackMaxCombo)
                    AttackTime = AttackTimer;
                else AttackTime = AttackTimer2;
                AttackCombo += 1;
                AttackComboTime = AttackComboTimer;
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released && !Attacking)
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
                if (AttackCombo == AttackMaxCombo)
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
                AttackPosition.X = Position.X + Rectangle.Width;
            else AttackPosition.X = Position.X - (AttackCombo != AttackMaxCombo ? AttackRectangle : AttackRectangle2).Width;
            AttackPosition.Y = Position.Y + Rectangle.Height / 2 - (AttackCombo != AttackMaxCombo ? AttackRectangle : AttackRectangle2).Height / 2;
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
            AttackRectangle.X = (int)AttackPosition.X;
            AttackRectangle.Y = (int)AttackPosition.Y;
            AttackRectangle2.X = (int)AttackPosition.X;
            AttackRectangle2.Y = (int)AttackPosition.Y;
            PreviousKeyBoardState = KeyboardState;
        }*/
        public void Update(GameTime gameTime,Vector2 campos , List <Rectangle> tiles, List<Rectangle> platforms, List<MeleeEnemy> meleeEnemies, List<RangedEnemy> rangedEnemies)
        {
            this.campos = campos;
            attackactive = false;
            KeyboardState = Keyboard.GetState();
            float DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MeleeEnemies = meleeEnemies;
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
            
            if (KnockbackTime <= 0)
            {
                if (KeyboardState.IsKeyDown(Keys.D) && state != "dashing" && state != "attacking")
                {
                    Velocity.X += Speed * DeltaTime;
                    Direction = -1;
                    if (state != "running" && !Attacking ) 
                    {
                        state = "running";
                        frame = 0;
                        frametimer = 0;
                    }
                }
                else if (KeyboardState.IsKeyDown(Keys.A) && state != "dashing" && state != "attacking")
                {
                    Velocity.X -= Speed * DeltaTime;
                    Direction = 1;
                    if (state != "running" && !Attacking && state != "dashing")
                    {
                        state = "running";
                        frame = 0;
                        frametimer = 0;
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
                    if (state != "idle" && !Attacking && state != "dashing")
                    {
                        if (state == "attacking") 
                        { 
                            /*if (Direction == 1)
                                Position.X += 32*scale;*/
                        }
                        
                        state = "idle";
                        frame = 0;
                        frametimer = 0;
                    }
                }
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift) && !PreviousKeyBoardState.IsKeyDown(Keys.LeftShift) && candash)
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
                Velocity.X = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.X));
            }
             /*Velocity.Y = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.Y)); */
            if (KeyboardState.IsKeyDown(Keys.Space) && PreviousKeyBoardState.IsKeyDown(Keys.Space) != KeyboardState.IsKeyDown(Keys.Space) && KnockbackTime <= 0)
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
                if (Direction == 1)
                {
                    row = 2;
                }
                else
                {
                    row = 3;
                }
                frame = 3;
                maxframe = 1;
                Velocity.Y = 0;
                dashtimer += DeltaTime;
                if(dashtimer > maxdashtimer)
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
            hitbox = new Rectangle((int)Position.X, (int)Position.Y,hitbox.Width,hitbox.Height);
            TileCollision(tiles);
            PlatformCollision(platforms);
            MeleeEnemiesCollision();
            RangedEnemyProjectileCollision();

            


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
            else if (Mouse.GetState().LeftButton == ButtonState.Released && !Attacking)
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
                if (AttackCombo == AttackMaxCombo)
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
                AttackPosition.X = Position.X + hitbox.Width;
            else AttackPosition.X = Position.X - (AttackCombo == 0 ? AttackRectangle : (AttackCombo == 1 ? AttackRectangle2 : AttackRectangle3)).Width;
            AttackPosition.Y = Position.Y + hitbox.Height / 2 - (AttackCombo == 0 ? AttackRectangle : (AttackCombo == 1 ? AttackRectangle2 : AttackRectangle3)).Height / 2;
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
            AttackRectangle.X = (int)AttackPosition.X + ((Direction == 1) ? +30 : -60);
            AttackRectangle.Y = (int)AttackPosition.Y;
            AttackRectangle2.X = (int)AttackPosition.X + ((Direction == 1) ? +16 : -8);
            AttackRectangle2.Y = (int)AttackPosition.Y;
            AttackRectangle3.X = (int)AttackPosition.X + ((Direction == 1) ? +20 : -20);
            AttackRectangle3.Y = (int)AttackPosition.Y+2;
            PreviousKeyBoardState = KeyboardState;
            frametimer += DeltaTime;
            if (frametimer > maxframetimer)
            {
                frametimer = 0;
                frame++;
                if (frame >= maxframe)
                {
                    frame = 0;
                }
            }
            if (state == "idle")
            {
                framesize = new Vector2(48, 32);
                maxframe = 4;
                maxframetimer = 0.15f;
                if (Direction == 1)
                {
                    row = 0;
                }
                else
                {
                    row = 1;
                }
            }
            if (state == "running")
            {
                framesize = new Vector2(48, 32);
                maxframe = 6;
                maxframetimer = 0.15f;
                if (Direction == 1)
                {
                    row = 2;
                }
                else
                {
                    row = 3;
                }
            }
            if(Attacking == true)
            {
                if(state != "attacking")
                {
                    frame = 0;
                    frametimer = 0;
                    state = "attacking";
                    /*if (Direction == 1)
                        Position.X -= 32*scale;*/
                }
            }
            if (state == "attacking")
            {
                framesize = new Vector2(80, 32);
                if (AttackCombo == 1)
                {
                    row = 4;
                    maxframe = 4;
                    maxframetimer = 0.08f;
                }
                if (AttackCombo == 2) 
                {
                    row = 5;
                    maxframe = 6;
                    maxframetimer = 0.08f;
                }
                if (AttackCombo == 3)
                {
                    row = 6;
                    maxframe = 9;
                    maxframetimer = 0.08f;
                }
                if (frame >= maxframe / 3 && frame < maxframe && AttackCombo == 3)
                    attackactive = true;
                else if (frame >=  maxframe/2 && frame <maxframe)
                    attackactive = true;
            }
            Scale = framesize *2;
        }
        public void MeleeEnemiesCollision()
        {
            
            foreach (var meleeenemy in MeleeEnemies)
            {
                if (InvincibleTime <= 0 && state != "dashing")
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

                Vector2 topleft = Position;
                Vector2 topright = Position + new Vector2(hitbox.Width, 0);
                Vector2 botttomleft = Position + new Vector2(0, hitbox.Height);
                Vector2 botttomright = Position + new Vector2(hitbox.Width, hitbox.Height);

                Vector2 pos1 = Vector2.Zero;
                Vector2 pos2 = Vector2.Zero;
                Vector2 pos3 = Vector2.Zero;
                pos1 = new Vector2(Position.X + hitbox.Width / 2, Position.Y + hitbox.Height / 2);
                pos2 = new Vector2(Wall.X + Wall.Width / 2, Wall.Y + Wall.Height / 2);
                pos3 = Vector2.Normalize(pos1 - pos2);
                if (hitbox.Intersects(Wall))
                {
                    if (pos3.X < 0 && pos3.Y < 0)
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
                    else if (pos3.X > 0 && pos3.Y < 0)
                    {
                        if (Math.Abs(botttomleft.X - Wall.X - Wall.Width) < Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(botttomleft.X - Wall.X - Wall.Width) > Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            JumpCount = MaxJump;
                            CoyoteTime = CoyoteTimer;
                        }
                    }
                    else if (pos3.X < 0 && pos3.Y > 0)
                    {
                        if (Math.Abs(topright.X - Wall.X) < Math.Abs(topright.Y - Wall.Y - Wall.Height))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                        }
                        else if (Math.Abs(topright.X - Wall.X) > Math.Abs(topright.Y - Wall.Y - Wall.Height))
                        {
                            Position.Y = movedown;
                            Velocity.Y = 0;
                        }
                    }
                    else if (pos3.X > 0 && pos3.Y > 0)
                    {
                        if (Math.Abs(topleft.X - Wall.X - Wall.Width) < Math.Abs(topleft.Y - Wall.Y - Wall.Height))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                            
                        }
                        else if (Math.Abs(topleft.X - Wall.X - Wall.Width) > Math.Abs(topleft.Y - Wall.Y - Wall.Height))
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
                    else if (Velocity.Y > 0 && Position.Y + (hitbox.Height * 0.75) < Platform.Y && !FallThrough)
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

            //SpriteBatch.Draw(Texture, Rectangle, Rectangle, Color, 0, Vector2.Zero, SpriteEffects.None, 0);
            //SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)Scale.X, (int)Scale.Y), Color, 0, Vector2.Zero, 1, (Direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //SpriteBatch.Draw(pixel, AttackCombo == 1 ? new Rectangle(AttackRectangle.X - (int)campos.X, AttackRectangle.Y - (int)campos.Y, AttackRectangle.Width, AttackRectangle.Height) : (AttackCombo == 2 ? new Rectangle(AttackRectangle2.X - (int)campos.X, AttackRectangle2.Y - (int)campos.Y, AttackRectangle2.Width, AttackRectangle2.Height) : new Rectangle(AttackRectangle3.X - (int)campos.X, AttackRectangle3.Y - (int)campos.Y, AttackRectangle3.Width, AttackRectangle3.Height)), AttackCombo == 1 ? new Rectangle(AttackRectangle.X - (int)campos.X, AttackRectangle.Y - (int)campos.Y, AttackRectangle.Width, AttackRectangle.Height) : (AttackCombo == 2 ? new Rectangle(AttackRectangle2.X - (int)campos.X, AttackRectangle2.Y - (int)campos.Y, AttackRectangle2.Width, AttackRectangle2.Height) : new Rectangle(AttackRectangle3.X - (int)campos.X, AttackRectangle3.Y - (int)campos.Y, AttackRectangle3.Width, AttackRectangle3.Height)), attackactive ? Color.Red : Color.Green, 0, Vector2.Zero, SpriteEffects.None, 0);
            SpriteBatch.Draw(Texture, (state == "attacking" && Direction == 1)? new Rectangle((int)Position.X-32*scale - (int)campos.X, (int)Position.Y - (int)campos.Y, Rectangle.Width, Rectangle.Height) : new Rectangle((int)Position.X - (int)campos.X, (int)Position.Y - (int)campos.Y, Rectangle.Width,Rectangle.Height), new Rectangle (frame* (int)framesize.X, row* (int)framesize.Y, (int)framesize.X, (int)framesize.Y), Color, 0, Vector2.Zero, (state != "attacking") ? SpriteEffects.None : (Direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

    }
}
