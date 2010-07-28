using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    class LocalObjectList
    {
        private readonly Dictionary<int,DrawableGameObject> entities;

        public LocalObjectList()
        {
            entities = new Dictionary<int, DrawableGameObject>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                entity.Draw(gameTime);
            }
        }

        public void Add(params DrawableGameObject[] newEntities)
        {
            foreach (var entity in newEntities)
            {
                entities.Add(entity.ID,entity);
            }
        }
    }
}
