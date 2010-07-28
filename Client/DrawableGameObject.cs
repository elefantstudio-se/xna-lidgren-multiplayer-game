using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client
{
    abstract class DrawableGameObject:GameObject
    {
        public abstract Vector2 Position{ get; set;}
        public abstract float Angle{ get; set;}
        public float Speed { get; set; }
        public float ZOrder { get; set; }
        public  int Width
        {
            get
            {
                return Texture.Width;
            }
        }
        public  int Height
        {
            get
            {
                return Texture.Height;
            }
        }

        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch;
        public Vector2 Velocity
        {
            get
            {
                return new Vector2((float)Math.Sin(Angle),-(float)Math.Cos(Angle)) * Speed;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }
        protected bool IsInScreen
        {
            get
            {
                return screenBounds.Intersects(Bounds);
            }
        }

        private Rectangle screenBounds;

        protected DrawableGameObject(Game game, long sessionID, int id, string imageAssetPath) : base(game, sessionID, id)
        {
            SpriteBatch = (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch));
            Texture = Game.Content.Load<Texture2D>(imageAssetPath);
            Origin = new Vector2(Width/2, Height/2);
            screenBounds = Game.GraphicsDevice.ScissorRectangle;
        }

        public virtual void Draw(GameTime gametime)
        {
            if (IsInScreen)
            {
                SpriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1, SpriteEffects.None, ZOrder);
            }
        }
    }
}
