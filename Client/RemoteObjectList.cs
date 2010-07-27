using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class RemoteObjectList<T,V> where T:DrawableGameObject<V> where V:ITransferable
    {
        public Dictionary<int, T> ObjectsData{ get; set;}
        public Dictionary<int, ITransferable> RemoteData{ get; set;}

        public RemoteObjectList()
        {
            ObjectsData = new Dictionary<int, T>();
            RemoteData = new Dictionary<int, ITransferable>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in ObjectsData.Values)
            {
                obj.Update(gameTime, (V)RemoteData[obj.ID]);
            }

            foreach (var key in ObjectsData.Keys.ToArray())
            {
                var obj = ObjectsData[key];
                if (!obj.IsValid)
                {
                    //obj.Dispose();
                    ObjectsData.Remove(obj.ID);
                    RemoteData.Remove(obj.ID);
                }
            }
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
            if (!data.IsValid)
            {
                ObjectsData.Remove(data.ID);
                RemoteData.Remove(data.ID);
                return;
            }
            if (ObjectsData.ContainsKey(data.ID))
            {
                RemoteData[data.ID] = data;
            }
        }

        public bool Exists(int id)
        {
            return ObjectsData.ContainsKey(id);
        }

        public int Count()
        {
            return ObjectsData.Count;
        }

        public void Add(T entity, TransferableObjectData initialRemoteData)
        {
            ObjectsData.Add(entity.ID, entity);
            RemoteData.Add(entity.ID, initialRemoteData);
        }

        public void Remove(int id)
        {
            ObjectsData.Remove(id);
            RemoteData.Remove(id);
        }
    }
}
