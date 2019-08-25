using System;
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

        // Textures for the various pickups
        static public Texture2D PickupTexture;
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

       
        public void UpdateDuration(double secsElapsed)
        {
            RemainingDuration -= secsElapsed;
        }

        public virtual void Update(double secsElapsed)
        {
            Rotation = (Rotation + (float)secsElapsed * SpinSpeed) % 360;
            Position += Vector2.Multiply(Velocity, (float)(secsElapsed));
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

        public void Draw(SpriteBatch spriteBatch)
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

    class TempPickup : Pickup
    {
        private float SlowRate;
        private float Magnitude;

        public TempPickup(Vector2 position, Vector2 direction, float speed) : base(position, direction, speed)
        {
            ScoreChange = 1;
            Texture = Pickup.PickupTexture;
            Magnitude = speed;
            SlowRate = 10f;
        }

        public override void Update(double secsElapsed)
        {
            Rotation = (Rotation + (float)secsElapsed * SpinSpeed) % 360;

            if (Magnitude > 0f)
            {
                Velocity.Normalize();
                Magnitude -= (float)(SlowRate * secsElapsed);
                if (Magnitude < 0f)
                    Magnitude = 0f;

                Velocity = Vector2.Multiply(Velocity, Magnitude);
            }

            Position += Vector2.Multiply(Velocity, (float)(secsElapsed));
        }
    }

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
