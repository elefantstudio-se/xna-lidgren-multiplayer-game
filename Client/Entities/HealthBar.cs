using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Players;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client.Entities
{
    class HealthBarDesign
    {
        public Color BorderColor { get; set; }
        public Color RemainingHealthColor { get; set; }

        public HealthBarDesign(Color border, Color inner)
        {
            BorderColor = border;
            RemainingHealthColor = inner;
        }
    }

    class HealthBar : SimpleDrawableGameObject, IRemotelyUpdateable, IUpdateSender
    {
        public float Value { get; set; }
        public float MaxValue { get; set; }
        public override int Width
        {
            get
            {
                return width;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
        }
        public HealthBarDesign Design { get; set; }

        private readonly int width, height;
        private const int border = 2;
        private int index;

        public HealthBar(Game game, long sessionID, int id, string imageAssetPath, int index, Vector2 position, int width, int height, float initialValue, float maxValue, HealthBarDesign design)
            : base(game, sessionID, id, imageAssetPath, position, 0, 0)
        {
            this.index = index;
            this.width = width;
            this.height = height;
            Value = initialValue;
            MaxValue = maxValue;
            Design = design;
        }

        public HealthBar(Game game, long sessionID, int id, string imageAssetPath, int index, Vector2 position, int width, int height, float initialValue, float maxValue) : this(game, sessionID, id, imageAssetPath, index, position, width, height, initialValue, maxValue, new HealthBarDesign(Color.Black, Color.Green)) { }

        public override void Draw(GameTime gameTime)
        {
            var innerBarPosition = new Vector2(Position.X + border, Position.Y + border);
            SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Width, Height), Design.BorderColor, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
            SpriteBatch.Draw(Texture, innerBarPosition, new Rectangle(border, border, (int)InnerBarWidth, (int)InnerBarHeight), Design.RemainingHealthColor, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
        }

        float InnerBarWidth
        {
            get
            {
                return (Value / MaxValue) * Width - border * 2;
            }
        }

        float InnerBarHeight
        {
            get
            {
                return Height - border * 2;
            }
        }

        public void Update(GameTime gameTime, ITransferable remoteData)
        {
            var data = (HealthTransferableData)remoteData;
            Value = data.Value;
        }

        public void SendUpdates(NetClient client)
        {
            var message = client.CreateMessage();
            message.Write(Helpers.TransferType.HealthUpdate);
            message.Write(new HealthTransferableData(client.UniqueIdentifier, ID, IsValid, index, Value));
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }
    }
}