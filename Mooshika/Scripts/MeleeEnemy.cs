using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;

namespace Mooshika.Scripts
{
    internal class MeleeEnemy : Enemy
    {
        public Vector2 Knockback = new Vector2(5,-10);
        public float KnockbackTime = 0;
        public float KnockbackTimer = 0.25f;
        public bool attacked = false;
        public int Gravity = 50;
        public int MaxGravity = 20;
        public int Speed = 5;
        public int FloorHeight = 20;
        public int Damage = 10;
        public float cannotattacktime = 0f;
        public float cannotattacktimer = 1f;
        bool CanMove = false;
        Vector2 campos;
        public MeleeEnemy (Texture2D texture, Vector2 position, Vector2 scale, Color color, GameWindow window, int direction) : base (texture, position, scale, color, window) 
        {
            Direction = direction;
            Health = 100;
            MaxHealth = 100;
        }
        public void Update(GameTime GameTime, Player Player, List<Rectangle> Walls, List<Rectangle> Platforms, Vector2 campos)
        {
            this.campos = campos;
            this.Tiles = Walls;
            this.Platforms = Platforms;
            float Distance = Vector2.Distance(Position,Player.Position);
            float DeltaTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            if (KnockbackTime <= 0 && CanMove)
            {
                Velocity.X = Speed * Direction;
            }
            else KnockbackTime -= DeltaTime;
            Velocity.Y += Gravity * DeltaTime;
            Math.Min(Velocity.Y,MaxGravity);
            Position += Velocity;
            TileCollision();
            platformCollision();
            PlayerAttacked(Player);
        }
        public void TileCollision()
        {
            foreach (var Wall in Tiles)
            {
                float moveright = Wall.X + Wall.Width;
                float moveleft = Wall.X - Rectangle.Width;
                float moveup = Wall.Y - Rectangle.Height;
                float movedown = Wall.Y + Wall.Height;

                Vector2 topleft = Position;
                Vector2 topright = Position + new Vector2(Rectangle.Width, 0);
                Vector2 botttomleft = Position + new Vector2(0, Rectangle.Height);
                Vector2 botttomright = Position + new Vector2(Rectangle.Width, Rectangle.Height);

                Vector2 pos1 = Vector2.Zero;
                Vector2 pos2 = Vector2.Zero;
                Vector2 pos3 = Vector2.Zero;
                pos1 = new Vector2(Position.X + Rectangle.Width / 2, Position.Y + Rectangle.Height / 2);
                pos2 = new Vector2(Wall.X + Wall.Width / 2, Wall.Y + Wall.Height / 2);
                pos3 = Vector2.Normalize(pos1 - pos2);
                if (Rectangle.Intersects(Wall))
                {
                    if (pos3.X < 0 && pos3.Y < 0)
                    {
                        if (Math.Abs(botttomright.X - Wall.X) < Math.Abs(botttomright.Y - Wall.Y))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                            Direction *= -1;
                        }
                        else if (Math.Abs(botttomright.X - Wall.X) > Math.Abs(botttomright.Y - Wall.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            CanMove = true;
                        }
                    }
                    else if (pos3.X > 0 && pos3.Y < 0)
                    {
                        if (Math.Abs(botttomleft.X - Wall.X - Wall.Width) < Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            Position.X = moveright;
                            Velocity.X = 0;
                            Direction *= -1;
                        }
                        else if (Math.Abs(botttomleft.X - Wall.X - Wall.Width) > Math.Abs(botttomleft.Y - Wall.Y))
                        {
                            Position.Y = moveup;
                            Velocity.Y = 0;
                            CanMove = true;
                        }
                    }
                    else if (pos3.X < 0 && pos3.Y > 0)
                    {
                        if (Math.Abs(topright.X - Wall.X) < Math.Abs(topright.Y - Wall.Y - Wall.Height))
                        {
                            Position.X = moveleft;
                            Velocity.X = 0;
                            Direction *= -1;
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
                            Direction *= -1;
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
        public void platformCollision()
        {
            foreach (var Platform in Platforms)
            {
                if (Rectangle.Intersects(Platform))
                {
                    if (Velocity.Y == MaxGravity)
                    {

                        Position.Y = Platform.Y - Rectangle.Height;
                        Velocity.Y = 0;
                        CanMove = true;
                    }
                    else if (Velocity.Y > 0 && Position.Y + (Rectangle.Height * 0.75) < Platform.Y)
                    {

                        Position.Y = Platform.Y - Rectangle.Height;
                        Velocity.Y = 0;
                        CanMove = true;
                    }
                }
            }
        }
        public override void Draw(SpriteBatch SpriteBatch)
        {
            //SpriteBatch.Draw(Texture, Rectangle, Rectangle, Color, 0, Vector2.Zero, SpriteEffects.None, 0);
            //SpriteBatch.Draw(Texture, new Rectangle((int)Position.X - (int)campos.X, (int)Position.Y - (int)campos.Y, Rectangle.Width, Rectangle.Height), new Rectangle(0,0,(int)Scale.X,(int)Scale.Y), Color, 0, Vector2.Zero, (Direction == 1)? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            SpriteBatch.Draw(Texture, new Rectangle((int)Position.X - (int)campos.X, (int)Position.Y - (int)campos.Y, Rectangle.Width, Rectangle.Height), new Rectangle(0, 0, 54, 59), Color, 0, Vector2.Zero, (Direction != 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

        }
        public void PlayerAttacked(Player Player)
        {
            if (Rectangle.Intersects(Player.AttackRectangle) && !attacked && Player.attackactive)
            {
                KnockbackTime = KnockbackTimer;
                cannotattacktime = cannotattacktimer;
                attacked = true;
                Health -= Player.Damage;
                CanMove = false;
                if (Position.X > Player.Position.X)
                {
                         Velocity = Knockback;
                }
                else Velocity = new Vector2 (-Knockback.X, Knockback.Y);
            }
            else if (Rectangle.Intersects(Player.AttackRectangle2) && !attacked && Player.attackactive)
            {
                KnockbackTime = KnockbackTimer;
                cannotattacktime = cannotattacktimer;
                attacked = true;
                Health -= Player.Damage;
                CanMove = false;
                if (Position.X > Player.Position.X)
                {
                         Velocity = Knockback;
                }
                else Velocity = new Vector2 (-Knockback.X, Knockback.Y);
            }
            else if (Rectangle.Intersects(Player.AttackRectangle3) && !attacked && Player.attackactive)
            {
                KnockbackTime = KnockbackTimer;
                cannotattacktime = cannotattacktimer;
                attacked = true;
                Health -= Player.Damage;
                CanMove = false;
                if (Position.X > Player.Position.X)
                {
                    Velocity = Knockback;
                }
                else Velocity = new Vector2(-Knockback.X, Knockback.Y);
            }
            if (attacked&&!Player.attackactive)
            {
                attacked = false;
            }
        }
    }
}