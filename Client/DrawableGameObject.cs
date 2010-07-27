using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client
{
    abstract class DrawableGameObject<T>:GameObject<T> where T:ITransferable
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

        protected DrawableGameObject(Game game, long sessionID, int id, string imageAssetPath) : base(game, sessionID, id)
        {
            SpriteBatch = (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch));
            Texture = Game.Content.Load<Texture2D>(imageAssetPath);
            Origin = new Vector2(Width/2, Height/2);
        }

        public virtual void Draw(GameTime gametime)
        {
            SpriteBatch.Draw(Texture,Position,null,Color.White,Angle,Origin,1,SpriteEffects.None,ZOrder);
        }

        //public virtual void Dispose(){}
    }
}
