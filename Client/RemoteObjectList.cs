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
    class RemoteObjectList<T> where T:RemoteObject
    {
        public Dictionary<int, T> ObjectsData{ get; set;}

        public RemoteObjectList()
        {
            ObjectsData = new Dictionary<int, T>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in ObjectsData.Values)
            {
                obj.Update(gameTime);
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
                obj.Draw(gameTime);
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

        public void Add(T entity)
        {
            ObjectsData.Add(entity.ID, entity);
        }
    }
}
