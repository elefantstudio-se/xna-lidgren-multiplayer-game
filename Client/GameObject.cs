using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    abstract class GameObject
    {
        public Game Game { get; set; }
        public long SessionID { get; set; }
        public int ID { get; set; }
        public bool IsValid { get; set; }


        protected static Keys[] InputKeys
        {
            get
            {
                return Keyboard.GetState().GetPressedKeys();
            }
        }

        protected GameObject(Game game, long sessionID, int id)
        {
            Game = game;
            SessionID = sessionID;
            ID = id;
            IsValid = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Destroy()
        {
            
        }
    }
}
