using System;
using Microsoft.Xna.Framework;

namespace NunchuckGame
{
    abstract class Pickup
    {
        protected double Duration;
        protected double RemainingDuration;
        protected int ScoreChange;
        protected bool AffectsPlayer;
        protected bool EndsGame;

        public virtual void Update(ref Player player)
        {

        }

        public void UpdateDuration(double secsElapsed)
        {
            RemainingDuration -= secsElapsed;
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
    }

    class SmallPointsPickup : Pickup
    {
        public SmallPointsPickup()
        {
            Duration = 0f;
            RemainingDuration = 0f;
            ScoreChange = 1;
            AffectsPlayer = false;
            EndsGame = false;
        }
    }

    class SpeedPickup : Pickup
    {
        public SpeedPickup()
        {
            Duration = 5f;
            RemainingDuration = Duration;
            ScoreChange = 10;
            AffectsPlayer = true;
            EndsGame = false;
        }

        public override void Update(ref Player player)
        {
            player.SetSpeed(1.5f);
        }
    }

    class FreezePickup : Pickup
    {
        public FreezePickup()
        {
            Duration = 2f;
            RemainingDuration = Duration;
            ScoreChange = 0;
            AffectsPlayer = true;
            EndsGame = false;
        }

        public override void Update(ref Player player)
        {
            player.SetSpeed(0f);
        }
    }

    class SlowPickup : Pickup
    {
        public SlowPickup()
        {
            Duration = 4f;
            RemainingDuration = Duration;
            ScoreChange = 0;
            AffectsPlayer = true;
            EndsGame = false;
        }

        public override void Update(ref Player player)
        {
            player.SetSpeed(0.5f);
        }
    }

    class LockTurningPickup : Pickup
    {
        private float Rotation;
        private Vector2 Direction;

        public LockTurningPickup(ref Player player)
        {
            Duration = 1.5f;
            RemainingDuration = Duration;
            ScoreChange = 0;
            AffectsPlayer = true;
            EndsGame = false;

            Rotation = player.Rotation;
            Direction = player.GetVelocity();
        }

        public override void Update(ref Player player)
        {
            player.SetDirection(Direction);
            player.Rotation = Rotation;
        }
    }

    class GameOverPickup : Pickup
    {
        public GameOverPickup()
        {
            Duration = 0f;
            RemainingDuration = Duration;
            ScoreChange = 0;
            AffectsPlayer = false;
            EndsGame = true;
        }
    }
}
