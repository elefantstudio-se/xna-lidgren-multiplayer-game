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
        private readonly string texturesPath;
        private string[] textureNames;
        private IUpdateable updater;


        public RemoteObjectList(Game game, string texturesPath, string[] textureNames, IUpdateable updater)
        {
            this.game = game;
            this.texturesPath = texturesPath;
            this.textureNames = textureNames;
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
                spriteBatch.Draw(obj.Texture, obj.LocalData.Position, null, Color.White, obj.LocalData.Angle, obj.RemoteData.BoundsCenterOffset, 1f, SpriteEffects.None, 0.5f);
            }
            spriteBatch.End();
        }
        
        public void Update(TransferableObjectData data)
        {
            if (!ObjectsData.ContainsKey(data.ID))
            {
                ObjectsData[data.ID] = new ObjectData(data, game.Content.Load<Texture2D>(texturesPath + textureNames[data.Index]));
            }
            else {
                ObjectsData[data.ID].RemoteData = data;
            }
        }
    }
}
