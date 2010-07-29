using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Entities;
using Client.Players;
using Microsoft.Xna.Framework;

namespace Client.Factories
{
    class HealthBarFactory
    {
        private Game game;
        private int width, height;
        private float initialValue, maxValue;
        private string texture = "blankpixel";

        public HealthBarFactory(Game game, int width, int height, float initialValue, int maxValue)
        {
            this.game = game;
            this.width = width;
            this.height = height;
            this.initialValue = initialValue;
            this.maxValue = maxValue;
        }
        public HealthBar NewHealthBar(long sessionID, int id, int index, Vector2 position)
        {
            return new HealthBar(game, sessionID, id, texture, index, position, width, height, initialValue, maxValue);
        }
    }
}
