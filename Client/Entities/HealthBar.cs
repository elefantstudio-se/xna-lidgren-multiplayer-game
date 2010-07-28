using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client.Entities
{
    class HealthBar:SimpleDrawableGameObject, IDestroyable
    {
        public HealthBar(Game game, long sessionID, int id, string imageAssetPath, Vector2 position) : base(game, sessionID, id, imageAssetPath, position, 0, 0)
        {
            blankPixel = game.Content.Load<Texture2D>("blankpixel");
            innerBarPosition = Position;// new Vector2(Position.X + border, Position.Y + border);
            //innerBarPosition = Position - Origin;
        }

        public float Value { get; set; }

        private const int border = 5;
        private Texture2D blankPixel;
        private Vector2 innerBarPosition;

        //public HealthBar(Game game, long sessionID, int id, string imageAssetPath, Vector2 position) : base(game, sessionID, id, imageAssetPath, position, 0)
        //{
        //}

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            //SpriteBatch.Draw(blankPixel, Position, new Rectangle(border,border,10,50), Color.Black, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
            //SpriteBatch.Draw(blankPixel, new Vector2(Position.X + border, Position.Y + border), new Rectangle(border,border,10,10), Color.Red, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
        }
    }
}