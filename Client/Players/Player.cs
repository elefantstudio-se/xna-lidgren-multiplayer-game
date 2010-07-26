using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Players
{
    abstract class Player:PhysicsGameObject<TransferableObjectData>
    {
        public short Index { get; set; }
        protected Player(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories, short index) : base(game, sessionID, id, imageAssetPath, position, physicsSimulator, speed, mass, collisionCategories)
        {
            Index = index;
        }
    }
}
