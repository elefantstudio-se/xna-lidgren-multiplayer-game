using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

//using SB = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Client
{
    class DrawableGameObject<T>:GO<T> where T:ITransferable
    {
        public virtual Vector2 Position { get; set; }
        public float ZOrder { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual float Angle { get; set; }
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch;

        private Vector2 position;

        public DrawableGameObject(Game game, long sessionID, int id, string imageAssetPath, Vector2 position) : base(game, sessionID, id)
        {
            SpriteBatch = (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch));
            Texture = Game.Content.Load<Texture2D>(imageAssetPath);
            this.position = position;
        }

        public override void Initialize()
        {
            base.Initialize();
            Position = position;
            Width = Texture.Width;
            Height = Texture.Height;
            Origin = new Vector2(Width/2, Height/2);
        }

        public override void Draw(GameTime gametime)
        {
            SpriteBatch.Draw(Texture,Position,null,Color.White,Angle,Origin,1,SpriteEffects.None,ZOrder);
        }

    }
}
