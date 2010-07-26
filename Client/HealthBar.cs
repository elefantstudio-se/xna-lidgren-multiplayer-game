using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;

namespace Client
{
    class HealthBar:GameObject<HealthTransferableData>
    {
        public float Value { get; set; }

        private const int border = 5;
        private Texture2D blankPixel;
        private Vector2 innerBarPosition;

        public HealthBar(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float zOrder) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, 0, zOrder, 1, 0, CollisionCategory.None)
        {
            blankPixel = game.Content.Load<Texture2D>("blankpixel");
            innerBarPosition = Position;// new Vector2(Position.X + border, Position.Y + border);
            //innerBarPosition = Position - Origin;
            Scale = new Vector2(2,2);
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            spriteBatch.Draw(blankPixel, Position, new Rectangle(border,border,10,50), Color.Black, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
            spriteBatch.Draw(blankPixel, new Vector2(Position.X + border, Position.Y + border), new Rectangle(border,border,10,10), Color.Red, 0, Origin, 1, SpriteEffects.None, ZOrder + 0.1f);
        }
    }
}
