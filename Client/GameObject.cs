using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shared;

namespace Client
{
    abstract class GameObject<T> where T:ITransferable
    {
        public Game Game { get; set; }
        public long SessionID { get; set; }
        public int ID { get; set; }
        public bool IsValid { get; set; }


        protected Keys[] InputKeys
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

        public virtual void Update(GameTime gameTime, T remoteData)
        {
            
        }
    }
}
