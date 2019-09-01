using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NunchuckGame
{
    abstract class Pickup : Sprite
    {
        protected double Duration;
        protected double RemainingDuration;
        protected int ScoreChange;
        protected bool AffectsPlayer;
        protected bool EndsGame;
        protected float Rotation;
        protected float SpinSpeed;
        protected bool IsDead = false;

        // Textures for the various pickups
        static public Texture2D PickupTexture;
        static public Texture2D DeathTexture;
        static public float PickupScale;
        static public int PickupHeight()
        {
            return (int)(PickupScale * PickupTexture.Height);
        }

        static public int PickupWidth()
        {
            return (int)(PickupScale * PickupTexture.Width);
        }

        public Pickup(Vector2 position, Vector2 direction, float speed)
        {
            Duration = 0f;
            RemainingDuration = 0f;
            ScoreChange = 0;
            AffectsPlayer = false;
            EndsGame = false;
            Position = position;
            Random random = new Random();

            direction.Normalize();
            double angle = Math.Atan2(direction.Y, direction.X) + (random.Next(-40, 40) * Math.PI) / 180;
            direction.X = (float)Math.Cos(angle);
            direction.Y = (float)Math.Sin(angle);

            Velocity = Vector2.Multiply(direction, speed);
            Rotation = 0;

            //SpinSpeed = (float)random.Next(-1000, 1000) / 1000f;
            SpinSpeed = 0;
            Scale = Pickup.PickupScale;
        }

        public virtual void Kill()
        {
            IsDead = true;
            Velocity = Vector2.Zero;
        }
       
        public void UpdateDuration(double secsElapsed)
        {
            RemainingDuration -= secsElapsed;
        }

        public virtual void Update(GameTime gameTime)
        {
            double secsElapsed = gameTime.ElapsedGameTime.TotalSeconds;
            Rotation = (Rotation + (float)secsElapsed * SpinSpeed) % 360;
            Position += Vector2.Multiply(Velocity, (float)(secsElapsed));
        }

        public virtual void Update(double secsElapsed, Vector2 trackPos)
        {

        }

        public bool IsOutOfBounds(Rectangle bounds)
        {
            return (Position.X < -(Pickup.PickupWidth() << 2) || Position.X > bounds.Width + Pickup.PickupWidth() ||
                Position.Y < -(Pickup.PickupHeight() << 2) || Position.Y > bounds.Height + Pickup.PickupHeight());
        }

        public double GetRemainingDuration()
        {
            return RemainingDuration;
        }

        public double GetDuration()
        {
            return Duration;
        }

        public int GetScoreChange()
        {
            return ScoreChange;
        }

        public bool GetAffectsPlayer()
        {
            return AffectsPlayer;
        }

        public bool GetEndsGame()
        {
            return EndsGame;
        }

        public virtual bool NeedsDeletion()
        {
            return IsDead;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Vector2.Zero, Scale,
                SpriteEffects.None, 0f);
        }

        static public void Collide(ref Pickup pickup1, ref Pickup pickup2)
        {
            Vector2 tempVelocity1 = pickup1.Velocity;
            Vector2 tempVelocity2 = pickup2.Velocity;

            float magnitude = (pickup1.Velocity.Length() + pickup2.Velocity.Length()) / 2f;
            if (tempVelocity1 == Vector2.Zero)
            {
                tempVelocity2.Normalize();
                tempVelocity1 = tempVelocity2 = Vector2.Multiply(tempVelocity2, magnitude);
            }
            else if (tempVelocity2 == Vector2.Zero)
            {
                tempVelocity1.Normalize();
                tempVelocity2 = tempVelocity1 = Vector2.Multiply(tempVelocity1, magnitude);
            }

            tempVelocity1.Normalize();
            tempVelocity2.Normalize();

            pickup1.Velocity = Vector2.Multiply(tempVelocity2, magnitude);
            pickup2.Velocity = Vector2.Multiply(tempVelocity1, magnitude);
        }
    }

    class SmallPointsPickup : Pickup
    {
        public SmallPointsPickup(Vector2 position, Vector2 direction, float speed) : base(position, direction, speed)
        {
            ScoreChange = 1;
            Texture = Pickup.PickupTexture;
        }
    }

    class FollowPickup : Pickup
    {
        int AnimationDuration = 35;
        int DeathAnimDuration = 50;
        Animation EnemyAnim = new Animation();

        float Magnitude = 1f;
        public FollowPickup(Vector2 position, Vector2 direction, float speed) : base(position, direction, speed)
        {
            ScoreChange = 1;
            Texture = Pickup.PickupTexture;
            Magnitude = speed;
            EnemyAnim.Initialize(Texture, Position, 32, 32, 4, AnimationDuration, Color.White, Scale, true);
        }

        public override void Update(GameTime gameTime)
        {
            // Update position
            base.Update(gameTime);

            // Update the enemy animation
            if (!IsDead)
                EnemyAnim.Update(gameTime, Position, AnimationDuration, 0);
            else
                EnemyAnim.Update(gameTime, Position - new Vector2(Scale * 16, Scale * 16), DeathAnimDuration, 0);
        }

        public override void Update(double secsElapsed, Vector2 trackPos)
        {
            if (IsDead)
                return;

            // Make the enemy rotate towards the player. This is a sort of angle lerp that I created since
            // the Vector2 lerp wasn't working initially (something which I now think was due to a minor typo).
            Vector2 direction = trackPos - new Vector2(Position.X + this.Rectangle.Width / 2, Position.Y + this.Rectangle.Height / 2);
            direction.Normalize();
            double targetAngle = Math.Atan2(direction.Y, direction.X);
            double currentAngle = Math.Atan2(Velocity.Y, Velocity.X);

            // Find minimum distance the enemy has to rotate so the enemy rotates using whichever direction
            // would point them at the player faster.
            if (Math.Abs((targetAngle + 2f * Math.PI) - currentAngle) < Math.Abs(targetAngle - currentAngle))
                targetAngle += 2f * Math.PI;
            else if (Math.Abs(targetAngle - (currentAngle + 2f * Math.PI)) > Math.Abs(targetAngle - Math.PI))
                currentAngle += 2f * Math.PI;

            // Restrict the enemy to not rotate past the player.
            bool Above = targetAngle > currentAngle;
            currentAngle += (targetAngle - currentAngle) * 0.5f * secsElapsed;
            if (Above && currentAngle > targetAngle)
                currentAngle = targetAngle;
            else if (!Above && targetAngle < currentAngle)
                currentAngle = targetAngle;

            // Convert the angle back a vector.
            direction.X = (float)Math.Cos(currentAngle);
            direction.Y = (float)Math.Sin(currentAngle);

            Velocity = Vector2.Multiply(direction, Magnitude);
        }

        public override void Kill()
        {
            base.Kill();
            Texture = DeathTexture;
            EnemyAnim.Initialize(Texture, Position - new Vector2(Scale * 16, Scale * 16), 64, 64, 7, AnimationDuration, Color.White, Scale, false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float depth;
            if (IsDead)
                depth = 0.55f;
            else
                depth = 0.4f;

            EnemyAnim.Draw(spriteBatch, depth);
        }

        public override Rectangle Rectangle
        {
            get
            {
                if (IsDead)
                    return new Rectangle(-999999999, -99999999, 0, 0);
                else
                    return new Rectangle((int)(Position.X + (float)EnemyAnim.FrameWidth * Scale / 6f), 
                        (int)(Position.Y + (float)EnemyAnim.FrameHeight * Scale / 6f), 
                        (int)((float)EnemyAnim.FrameWidth * Scale / 1.5), 
                        (int)((float)EnemyAnim.FrameHeight * Scale / 1.5));
            }
        }

        public override bool NeedsDeletion()
        {
            return IsDead && !EnemyAnim.Active;
        }
    }

    class InitialTargetPickup : Pickup
    {
        float Magnitude = 1f;
        public InitialTargetPickup(Vector2 position, Vector2 direction, float speed) : base(position, direction, speed)
        {
            ScoreChange = 1;
            Texture = Pickup.PickupTexture;
            Magnitude = speed;
            direction.Normalize();
            Velocity = Vector2.Multiply(direction, speed);
        }
    }

    //class TempPickup : Pickup
    //{
    //    private float SlowRate;
    //    private float Magnitude;

    //    public TempPickup(Vector2 position, Vector2 direction, float speed) : base(position, direction, speed)
    //    {
    //        ScoreChange = 1;
    //        Texture = Pickup.PickupTexture;
    //        Magnitude = speed;
    //        SlowRate = 10f;
    //    }

    //    public override void Update(double secsElapsed)
    //    {
    //        Rotation = (Rotation + (float)secsElapsed * SpinSpeed) % 360;

    //        if (Magnitude > 0f)
    //        {
    //            Velocity.Normalize();
    //            Magnitude -= (float)(SlowRate * secsElapsed);
    //            if (Magnitude < 0f)
    //                Magnitude = 0f;

    //            Velocity = Vector2.Multiply(Velocity, Magnitude);
    //        }

    //        Position += Vector2.Multiply(Velocity, (float)(secsElapsed));
    //    }
    //}

    class SpeedPickup : Pickup
    {
        public SpeedPickup(Vector2 position, Vector2 rotation, float speed) : base(position, rotation, speed)
        {
            Duration = 5f;
            RemainingDuration = Duration;
            ScoreChange = 10;
            AffectsPlayer = true;
            Texture = Pickup.PickupTexture;
        }

      
    }

    class FreezePickup : Pickup
    {
        public FreezePickup(Vector2 position, Vector2 rotation, float speed) : base(position, rotation, speed)
        {
            Duration = 2f;
            RemainingDuration = Duration;
            AffectsPlayer = true;
            Texture = Pickup.PickupTexture;
        }

     
    }

    class SlowPickup : Pickup
    {
        public SlowPickup(Vector2 position, Vector2 rotation, float speed) : base(position, rotation, speed)
        {
            Duration = 4f;
            RemainingDuration = Duration;
            AffectsPlayer = true;
            Texture = Pickup.PickupTexture;
        }

       
    }

    //class LockTurningPickup : Pickup
    //{
    //    private float Rotation;
    //    private Vector2 Direction;

    //    public LockTurningPickup(ref Player player)
    //    {
    //        Duration = 1.5f;
    //        RemainingDuration = Duration;
    //        ScoreChange = 0;
    //        AffectsPlayer = true;
    //        EndsGame = false;

    //        Rotation = player.Rotation;
    //        Direction = player.GetVelocity();
    //    }

    //    public override void Update(ref Player player)
    //    {
    //        player.SetDirection(Direction);
    //        player.Rotation = Rotation;
    //    }
    //}

    class GameOverPickup : Pickup
    {
        public GameOverPickup(Vector2 position, Vector2 rotation, float speed) : base(position, rotation, speed)
        {
            EndsGame = true;
            Texture = Pickup.PickupTexture;
        }
    }
}
