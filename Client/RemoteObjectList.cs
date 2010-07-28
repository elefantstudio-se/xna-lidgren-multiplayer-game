using System;
using System.Collections.Generic;
using System.Linq;
using Client.Players;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class UpdateableObject
    {
        public IRemotelyUpdateable Entity { get; set; }
        public ITransferable RemoteData { get; set; }

        public UpdateableObject(IRemotelyUpdateable entity, ITransferable remoteData)
        {
            Entity = entity;
            RemoteData = remoteData;
        }
    }

    class RemoteObjectList
    {
        public Dictionary<int, UpdateableObject> Entities;

        public RemoteObjectList()
        {
            Entities = new Dictionary<int, UpdateableObject>();
        }

        public void Add<T>(T entity, ITransferable remoteData) where T:DrawableGameObject, IRemotelyUpdateable
        {
            Entities.Add(entity.ID,new UpdateableObject(entity,remoteData));
        }


        public void Update(GameTime gameTime)
        {
            foreach (var data in Entities.Values)
            {
                var entity = data.Entity;
                entity.Update(gameTime,data.RemoteData);
            }
            foreach (var id in Entities.Keys.ToArray())
            {
                var entity = (DrawableGameObject)Entities[id].Entity;
                if (!entity.IsValid)
                {
                    Remove(id);
                    entity.Destroy();
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var data in Entities.Values)
            {
                ((DrawableGameObject)data.Entity).Draw(gameTime);
            }
        }
        
        public void UpdateData(ITransferable data)
        {
            if (!data.IsValid)
            {
                Entities.Remove(data.ID);
                return;
            }
            if (Entities.ContainsKey(data.ID))
            {
                Entities[data.ID].RemoteData = data;
            }
        }

        public bool Exists(int id)
        {
            return Entities.ContainsKey(id);
        }

        public void Remove(int id)
        {
            Entities.Remove(id);
        }
    }
}
