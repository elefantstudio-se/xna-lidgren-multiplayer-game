using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    class Player: GameObject
    {
        public short Index { get; set; }

        KeyboardControls Controls { get; set; }
        private int speed = 2;
        private int rotationSpeed = 5;
        public List<Projectile> Projectiles{ get; set;}
        private double lastShotFired = 0;
        private double fireInterval = 1000;
        public event EventHandler PlayerUpdated = delegate { };
        public event EventHandler<ProjectileFiredEventArgs> ProjectileFired = delegate { };
        public event EventHandler<BumpedAnotherPlayerEventArgs> BumpedAnotherPlayer = delegate { };


        public Player(Game game, long sessionId, short index, string imageAsset, Vector2 position, float angle, float zOrder, KeyboardControls controls) : this(game, sessionId, index, imageAsset, position, angle, zOrder, controls, new Vector2(0))
        {
        }
        public Player(Game game, long sessionId, short index, string imageAsset, Vector2 position, float angle, float zOrder, KeyboardControls controls, Vector2 boundsCenterOffset) : base(game, sessionId, imageAsset, position, angle, zOrder, boundsCenterOffset)
        {
            Index = index;
            Controls = controls;
            Projectiles = new List<Projectile>();
        }

        public void Move(int direction)
        {
            var tempNewPosition = Position;
            tempNewPosition += GetTranslatedTransform(direction, speed);
            /*if (tempNewPosition.X < 0)
            {
                tempNewPosition.X = ScreenBounds.Width + tempNewPosition.X;
            }
            if (tempNewPosition.Y < 0)
            {
                tempNewPosition.Y = ScreenBounds.Height + tempNewPosition.Y;
            }
            tempNewPosition.X = tempNewPosition.X % ScreenBounds.Width;
            tempNewPosition.Y = tempNewPosition.Y % ScreenBounds.Height;
            */
            var hitAfterMovement = PerformCollisionDetection(GetTransform(tempNewPosition));
            if (hitAfterMovement == null) //we didn't bump anyone
            {
                Position = tempNewPosition;
            } 
            //Position = tempNewPosition;

            /*Position += GetTranslatedTransform(direction, speed);
            if (Position.X < 0)
            {
                Position.X = ScreenBounds.Width + Position.X;
            }
            if (Position.Y < 0)
            {
                Position.Y = ScreenBounds.Height + Position.Y;
            }
            Position.X = Position.X % ScreenBounds.Width;
            Position.Y = Position.Y % ScreenBounds.Height;

            PerformCollisionDetection();*/
        }

        public void Rotate(float angle)
        {
            Angle += angle;
            var collidedPlayer = PerformCollisionDetection(GetTransform(Position));
            if (collidedPlayer != null)
            {
                Angle -= angle;
            }
        }

        ObjectData PerformCollisionDetection(Matrix transformation)
        {
            var boundRectangle = GetBoundingRectangle(transformation);
            ObjectData collidedPlayer = null;
            foreach (var player in SharedLists.Players.Values)
            {
                if (boundRectangle.Intersects(player.BoundingRectangle))
                {
                    if (CollisionHelpers.IntersectPixels(transformation,Width,Height,TextureData,player.Transform,player.Texture.Width,player.Texture.Height,player.TextureData))
                    {
                        collidedPlayer = player;
                        break;
                    }
                }
            }
            if (collidedPlayer == null)
            {
                return null;
            }
            //We have hit someone
            BumpedAnotherPlayer(this,new BumpedAnotherPlayerEventArgs(collidedPlayer));
            return collidedPlayer;
        }

        public override string ImagePath
        {
            get
            {
                return "Players/Avatars/";
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Projectiles.ForEach(p => p.Draw(gameTime));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bool playerUpdated = false;
            if (InputKeys.Contains(Controls.Forward))
            {
                Move(1);
                playerUpdated = true;
            }
            if (InputKeys.Contains(Controls.Backward))
            {
                Move(-1);
                playerUpdated = true;
            }
            if (InputKeys.Contains(Controls.RotateLeft))
            {
                Rotate(-MathHelper.ToRadians(rotationSpeed));
                playerUpdated = true;
            }
            if (InputKeys.Contains(Controls.RotateRight))
            {
                Rotate(MathHelper.ToRadians(rotationSpeed));
                playerUpdated = true;
            }
            if (InputKeys.Contains(Controls.Shoot))
            {
                Fire(gameTime);
            }
            Projectiles.ForEach(p => p.Update(gameTime));
            if (playerUpdated)
            {
                PlayerUpdated(this, EventArgs.Empty);
            }
        }

        bool Fire(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastShotFired < fireInterval)
            {
                return false;
            }
            lastShotFired = gameTime.TotalGameTime.TotalMilliseconds;
            var newProjectile = new Projectile(game, SessionID, SharedLists.ProjectileTextureNames[Index], Position, Angle, 0.5f, 10);
            newProjectile.Hit += (s, e) => { };
            Projectiles.Add(newProjectile);
            ProjectileFired(this, new ProjectileFiredEventArgs(newProjectile));
            return true;
        }
    }
}