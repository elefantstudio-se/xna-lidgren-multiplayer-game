using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public abstract class GameObject
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public Vector2 Position;
        public float Angle { get; set; }
        public float ZOrder { get; set; }
        public Color[] TextureData { get; set; }
        public int Width
        {
            get
            {
                return Texture.Width;
            }
        }
        public int Height
        {
            get
            {
                return Texture.Height;
            }
        }
        public Vector2 BoundsCenter { get; private set; }

        protected SpriteBatch SpriteB { get; set; }
        protected Texture2D Texture { get; set; }
        protected readonly Game game;
        protected Rectangle RelativeBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        protected bool IsInScreen
        {
            get
            {
                //return ScreenBounds.Intersects(RelativeBounds);
                return RelativeBounds.X >= 0 && (RelativeBounds.X + Texture.Width) <= ScreenBounds.Width
                       && RelativeBounds.Y >= 0 && (RelativeBounds.Y + Texture.Height) <= ScreenBounds.Height;
            }
        }

        private readonly string imageAssetName;
        protected Rectangle ScreenBounds { get; private set;}

        protected GameObject(Game game, long sessionId, int id, string imageAsset, Vector2 position, float angle, float zOrder, Vector2 boundsCenterOffset)
        {
            SessionID = sessionId;
            ID = id;
            Position = position;
            Angle = angle;
            ZOrder = zOrder;
            this.game = game;
            imageAssetName = imageAsset;
            SpriteB = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            Texture = game.Content.Load<Texture2D>(ImagePath + imageAssetName);
            ScreenBounds = new Rectangle(0, 0, game.GraphicsDevice.PresentationParameters.BackBufferWidth, game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            TextureData = new Color[Texture.Width*Texture.Height];
            Texture.GetData(TextureData);
            BoundsCenter = new Vector2(Width/2, Height/2) + boundsCenterOffset;
            
        }
        protected GameObject(Game game, long sessionId, string imageAsset, Vector2 position, float angle, float zOrder, Vector2 boundsCenterOffset):this(game, sessionId, Shared.Helpers.GetNewID(), imageAsset,position,angle,zOrder,boundsCenterOffset) { }

        public virtual void Initialize() { } 

        public virtual void Draw(GameTime gameTime)
        {
            SpriteB.Begin();
            SpriteB.Draw(Texture, Position, null, Color.White, Angle, BoundsCenter, 1f, SpriteEffects.None, ZOrder);
            SpriteB.End();
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        protected Vector2 GetTranslatedTransform(int dir, float speed) // dir: 1 for forward, -1 for backwards
        {
            Matrix rotation = Matrix.CreateRotationZ(Angle);
            return new Vector2(dir * (rotation.M12 * speed), -dir * (rotation.M11 * speed));
        }

        public abstract string ImagePath { get; }

        protected Keys[] InputKeys
        {
            get
            {
                return Keyboard.GetState().GetPressedKeys();
            }
        }

        public Matrix GetTransform()
        {
            return GetTransform(Position);
        }
        public Matrix GetTransform(Vector2 position)
        {
                return 
                    Matrix.CreateTranslation(new Vector3(-BoundsCenter, 0.0f)) *
                    //// Matrix.CreateScale(block.Scale) *  would go here
                    Matrix.CreateRotationZ(Angle) *
                    Matrix.CreateTranslation(new Vector3(position, 0.0f));
        }

        public Rectangle GetBoundingRectangle()
        {
            return GetBoundingRectangle(Position);
        }
        public Rectangle GetBoundingRectangle(Vector2 position)
        {
            return GetBoundingRectangle(GetTransform(position));
        }

        public Rectangle GetBoundingRectangle(Matrix transform)
        {
            return CollisionHelpers.CalculateBoundingRectangle(new Rectangle(0, 0, Texture.Width, Texture.Height), transform);
        }

    }
}