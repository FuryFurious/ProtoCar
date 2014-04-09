using SharpDX;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class PlayerWASD : PlayerControler
    {

        public bool moveLeft()
        {
            return Game1.keyboardState.IsKeyDown(Keys.A);
        }

        public bool moveRight()
        {
            return Game1.keyboardState.IsKeyDown(Keys.D);
        }

        public bool moveUp()
        {
            return Game1.keyboardState.IsKeyDown(Keys.W);
        }

        public bool moveDown()
        {
            return Game1.keyboardState.IsKeyDown(Keys.S);
        }
    }

    class PlayerArrow : PlayerControler
    {

        public bool moveLeft()
        {
            return Game1.keyboardState.IsKeyDown(Keys.Left);
        }

        public bool moveRight()
        {
            return Game1.keyboardState.IsKeyDown(Keys.Right);
        }

        public bool moveUp()
        {
           return Game1.keyboardState.IsKeyDown(Keys.Up);
        }

        public bool moveDown()
        {
            return Game1.keyboardState.IsKeyDown(Keys.Down);
        }

    }


    interface PlayerControler
    {
        bool moveLeft();
        bool moveRight();
        bool moveUp();
        bool moveDown();
    }
}
