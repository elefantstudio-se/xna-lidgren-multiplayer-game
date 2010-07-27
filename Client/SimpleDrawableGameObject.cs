using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class SimpleDrawableGameObject<T>:DrawableGameObject<T> where T:ITransferable
    {
        public sealed override Vector2 Position{ get; set;}
        public sealed override float Angle { get; set; }

        public SimpleDrawableGameObject(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, float speed) : base(game, sessionID, id, imageAssetPath)
        {
            Position = position;
            Angle = angle;
            Speed = speed;
        }
    }
}
