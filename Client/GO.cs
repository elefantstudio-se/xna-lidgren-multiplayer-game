using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shared;

namespace Client
{
    class GO<T> where T:ITransferable
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

        public GO(Game game, long sessionID, int id)
        {
            Game = game;
            SessionID = sessionID;
            ID = id;
        }

        public virtual void Initialize()
        {
            
        }
        public virtual void Update(GameTime gameTime, T remoteData)
        {
            
        }

        public virtual void Draw(GameTime gametime)
        {
        }
    }
}
