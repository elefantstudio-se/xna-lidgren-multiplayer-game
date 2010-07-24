using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
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
        private PhysicsSimulator physicsSimulator;
        private IUpdateable updater;


        public RemoteObjectList(Game game, PhysicsSimulator physicsSimulator, IUpdateable updater)
        {
            this.game = game;
            this.physicsSimulator = physicsSimulator;
            this.updater = updater;
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            ObjectsData = new Dictionary<int, ObjectData>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in ObjectsData.Values)
            {
                obj.Position = updater.UpdatePosition(obj.Position, obj.RemoteData.Position);
                obj.Angle = updater.UpdateAngle(obj.Angle, obj.RemoteData.Angle);
            }

            /*foreach (var key in ObjectsData.Keys.ToArray())
            {
                var obj = ObjectsData[key];
                if (!updater.IsStillValid(obj.BoundingRectangle))
                {
                    ObjectsData.Remove(key);
                }
            }*/
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var obj in ObjectsData.Values)
            {
                spriteBatch.Draw(obj.Texture, obj.Position, null, Color.White, obj.Angle, obj.Origin, 1f, SpriteEffects.None, 0.5f);
            }
        }
        
        public void UpdateData(TransferableObjectData data)
        {
            ObjectsData[data.ID].RemoteData = data;
        }

        public bool Exists(int id)
        {
            return ObjectsData.ContainsKey(id);
        }

        public int Count()
        {
            return ObjectsData.Count;
        }

        public void Add(TransferableObjectData data, Texture2D texture, Vector2 centerOffset, float mass)
        {
            ObjectsData.Add(data.ID, new ObjectData(data,physicsSimulator,texture,centerOffset,mass));
        }
    }
}
