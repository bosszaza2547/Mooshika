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
        public int Health = 100;
        public int MaxHealth = 100;
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
        List<Wall> Walls = new List<Wall>();
        List<Platform> Platforms = new List<Platform>();
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
        public Rectangle AttackRectangle = new Rectangle(0, 0, 100, 75);
        public Rectangle AttackRectangle2 = new Rectangle(0, 0, 150, 125);
        Vector2 AttackPosition = Vector2.Zero;
        bool CanAttack = true;
        public bool Attacking = false;
        float AttackTimer = 0.25f;
        float AttackTimer2 = 0.35f;
        float AttackTime = 0;
        int AttackCombo = 0;
        int AttackMaxCombo = 3;
        float AttackComboTimer = 1;
        public float AttackComboTime = 0;
        public int Damage = 10;
        public Player (Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window,Texture2D pixel) : base (texture, position, scale, color, window) 
        {
            this.pixel = pixel;
        }
        public void Update(GameTime gameTime, List<Wall> walls,List<Platform> platforms, List<MeleeEnemy> meleeEnemies, List<RangedEnemy> rangedEnemies)
        {
            KeyboardState = Keyboard.GetState ();
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
                JumpCount --;
            }
            if (PreJumpTime > 0)
            {
                PreJumpTime -= DeltaTime;
            }
            Velocity.Y += Gravity * DeltaTime;
            if(KnockbackTime<=0)
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
            
            /* if (KeyboardState.IsKeyDown(Keys.W))
            {
                Velocity.Y -= Speed * DeltaTime;
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                Velocity.Y += Speed * DeltaTime;
            }
            else Velocity.Y *= 0.75f; */
            Velocity.X = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.X));
            /* Velocity.Y = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.Y)); */
            if (KeyboardState.IsKeyDown(Keys.Space) && PreviousKeyBoardState.IsKeyDown(Keys.Space) != KeyboardState.IsKeyDown(Keys.Space) &&KnockbackTime<=0)
            {
                PreJumpTime = PreJumpTimer;
                jumped = false;
            }
            if (JumpCount > 0 && CanJump && PreJumpTime > 0 && !jumped &&KnockbackTime<=0)
            {
                Velocity.Y = -JumpPower * DeltaTime;
                JumpCount --;
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
            if(AttackCombo == 1)
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
            Debug.WriteLine(Rectangle.Y-Rectangle.Height);
        }
        public void MeleeEnemiesCollision()
        {
            
            foreach (var meleeenemy in MeleeEnemies)
            {
                if (InvincibleTime <= 0)
                {
                    if (Rectangle.Intersects(meleeenemy.Rectangle))
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
                    if (InvincibleTime <= 0)
                    {
                        if (Rectangle.Intersects(projectile.Rectangle))
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
        public void WallCollision()
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
        }
        bool FallThrough = false;
        public void platformCollision()
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
        }
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
            //SpriteBatch.Draw(Texture, Position, new Rectangle(0,0,(int)Scale.X,(int)Scale.Y), Color, 0, Vector2.Zero, 1, (Direction == 1)? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //SpriteBatch.Draw(pixel, AttackPosition, AttackCombo == AttackMaxCombo ? AttackRectangle2 : AttackRectangle, Attacking ? Color.Red : Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            SpriteBatch.Draw(Texture,Rectangle,Color.White);
        
        }
        
    }
}
