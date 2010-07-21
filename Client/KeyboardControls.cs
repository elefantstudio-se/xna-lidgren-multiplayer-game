using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class KeyboardControls
    {
        public Keys Forward { get; set; }
        public Keys Backward { get; set; }
        public Keys RotateLeft { get; set; }
        public Keys RotateRight { get; set; }
        public Keys Shoot { get; set; }

        public KeyboardControls(Keys forward, Keys backward, Keys rotateLeft, Keys rotateRight, Keys shoot)
        {
            Forward = forward;
            Backward = backward;
            RotateLeft = rotateLeft;
            RotateRight = rotateRight;
            Shoot = shoot;
        }
    }
}