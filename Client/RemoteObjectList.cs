using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client
{
    class RemoteObjectList
    {
        public Dictionary<int, ObjectData> ObjectsData{ get; set;}

        private readonly Game game;
        private readonly SpriteBatch spriteBatch;
        private IUpdateable updater;


        public RemoteObjectList(Game game, IUpdateable updater)
        {
            this.game = game;
            this.updater = updater;
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            ObjectsData = new Dictionary<int, ObjectData>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in ObjectsData.Values)
            {
                obj.LocalData.Position = updater.UpdatePosition(obj.LocalData.Position, obj.RemoteData.Position);
                obj.LocalData.Angle = updater.UpdateAngle(obj.LocalData.Angle, obj.RemoteData.Angle);
            }
        }
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var obj in ObjectsData.Values)
            {
                spriteBatch.Draw(obj.Texture, obj.LocalData.Position, null, Color.White, obj.LocalData.Angle, obj.Origin, 1f, SpriteEffects.None, 0.5f);
            }
            spriteBatch.End();
        }
        
        public void UpdateData(TransferableObjectData data)
        {
            ObjectsData[data.ID].RemoteData = data;
        }

        public bool Exists(int id)
        {
            return ObjectsData.ContainsKey(id);
        }

        public void Add(TransferableObjectData data, Texture2D texture, Vector2 centerOffset)
        {
            ObjectsData.Add(data.ID, new ObjectData(data,texture,centerOffset));
        }
    }
}
