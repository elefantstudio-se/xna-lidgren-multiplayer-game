using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client
{
    class ObjectData
    {
        public TransferableObjectData RemoteData { get; set; }
        public TransferableObjectData LocalData { get; set; }
        public Texture2D Texture { get; set; }
        public Color[] TextureData { get; set; }
        public Vector2 Origin { get; set; }
        public Matrix Transform
        {
            get
            {
                return 
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    // Matrix.CreateScale(block.Scale) *  would go here
                    Matrix.CreateRotationZ(LocalData.Angle) *
                    Matrix.CreateTranslation(new Vector3(LocalData.Position, 0.0f));
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                return CollisionHelpers.CalculateBoundingRectangle(new Rectangle(0, 0, Texture.Width, Texture.Height), Transform);
            }
        }

        public ObjectData(TransferableObjectData remoteData, Texture2D texture, Vector2 centerOffset)
        {
            Texture = texture;
            RemoteData = remoteData;
            LocalData = new TransferableObjectData(remoteData.SessionID, remoteData.ID, remoteData.Index, remoteData.Position, remoteData.Angle);
            TextureData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(TextureData);
            Origin = new Vector2(Texture.Width/2, Texture.Height/2) + centerOffset;
        }
    }
}
