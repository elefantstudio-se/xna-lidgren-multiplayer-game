using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    class Projectile: GameObject
    {
        public int Speed { get; set; }
        public bool IsActive { get; set; }
        public event EventHandler<ProjectileHitAnotherPlayerEventArgs> Hit = delegate { };

        public Projectile(Game game, long sessionId, string imageAsset, Vector2 position, float angle, float zOrder, int speed, Vector2 boundsCenterOffset) : base(game, sessionId, imageAsset, position, angle, zOrder, boundsCenterOffset)
        {
            Speed = speed;
            IsActive = true;
        }

        public override string ImagePath
        {
            get { return "Players/Projectiles/"; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!ScreenBounds.Contains(RelativeBounds))
            {
                IsActive = false;
            }
            Position += GetTranslatedTransform(1, Speed);
            var transformation = GetTransform();
            var boundRectangle = GetBoundingRectangle(transformation);
            foreach (var player in SharedLists.Players.Values)
            {
                if (boundRectangle.Intersects(player.BoundingRectangle)) //per pixel collision is expensive, therefore check if the rectangles intersect before doing the collision detection
                {
                    if (CollisionHelpers.IntersectPixels(transformation,Width,Height,TextureData,player.Transform,player.Texture.Width,player.Texture.Height,player.TextureData))
                    {
                        Hit(this, new ProjectileHitAnotherPlayerEventArgs(player));
                        break;
                    }
                }
            }
        }
    }
}
