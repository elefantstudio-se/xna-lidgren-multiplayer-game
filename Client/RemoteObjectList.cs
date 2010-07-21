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

        private const short CorrectionThreshold = 3;
        private const float InterpolationConstant = 0.2f;

        public RemoteObjectList(Game game, string texturesPath, string[] textureNames)
        {
            this.game = game;
            this.texturesPath = texturesPath;
            this.textureNames = textureNames;
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            ObjectsData = new Dictionary<int, ObjectData>();
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var obj in ObjectsData.Values)
            {
                var positionDifference = obj.RemoteData.Position - obj.LocalData.Position;
                float newX = positionDifference.X * InterpolationConstant, newY = positionDifference.Y * InterpolationConstant;
                if (positionDifference.X > 0 && positionDifference.X < CorrectionThreshold)
                {
                    newX = positionDifference.X;
                }

                if (positionDifference.Y > 0 && positionDifference.Y < CorrectionThreshold)
                {
                    newY = positionDifference.Y;
                }

                obj.LocalData.Position += new Vector2(newX, newY);
                obj.LocalData.Angle = obj.RemoteData.Angle;
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
