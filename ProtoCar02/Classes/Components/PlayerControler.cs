using SharpDX;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{

    public interface PlayerController
    {
        bool zoomIn();
        bool zoomOut();
        bool speedPressed();

        bool shoot();

        void clamp(float xMin, float xMax);
        void update();

        Vector3 getMoveDirection();
        Vector3 rotate();
    }

    //Merge PlayerWASD / PlayerArrow (too much copied code, too lazy to do this now)
    public class PlayerWASD : PlayerController
    {
        Vector3 rotation = new Vector3(0, 0, 0);

        private float oldMouseX;
        private float oldMouseY;

        private int oldMouseWheel;

        public Vector3 getMoveDirection()
        {
            Vector3 direction = new Vector3(0,0,0);

            if (Game1.keyboardState.IsKeyDown(Keys.W))
                direction -= Vector3.UnitZ;

            else if (Game1.keyboardState.IsKeyDown(Keys.S))
                direction += Vector3.UnitZ;

            if (Game1.keyboardState.IsKeyDown(Keys.A))
                direction -= Vector3.UnitX;

            else if (Game1.keyboardState.IsKeyDown(Keys.D))
                direction += Vector3.UnitX;

            return direction;
        }

        public bool speedPressed()
        {
            return Game1.keyboardState.IsKeyDown(Keys.LeftShift);
        }

        public Vector3 rotate()
        {

            Vector2 mousePos = new Vector2(Game1.mouseState.X, Game1.mouseState.Y);

            float dx = mousePos.X - oldMouseX;
            rotation.Y -= Settings.mouseSpeed * dx;

            float dy = mousePos.Y - oldMouseY;
            rotation.X -= Settings.mouseSpeed * dy;

            if (Game1.active)
                resetMouse();

            return rotation;
        }

        private void resetMouse()
        {
            Game1.mouseManager.SetPosition(new Vector2(0.5f, 0.5f));
           
            oldMouseX = 0.5f;
            oldMouseY = 0.5f;
        }

        public void update()
        {
         
        }


        public void clamp(float xMin, float xMax)
        {
            rotation.X = MathUtil.Clamp(rotation.X, xMin, xMax);
        }

        public bool zoomIn()
        {
            return Game1.mouseState.WheelDelta - Game1.oldMouseState.WheelDelta > 0;
        }

        public bool zoomOut()
        {
            return Game1.mouseState.WheelDelta - Game1.oldMouseState.WheelDelta < 0;
        }


        public bool shoot()
        {
            return Game1.keyboardState.IsKeyPressed(Keys.Space);
        }
    }

    public class PlayerArrow : PlayerController
    {

        Vector3 rotation = new Vector3(0, 0, 0);

        private float oldMouseX;
        private float oldMouseY;

        public Vector3 getMoveDirection()
        {

            Vector3 direction = new Vector3(0, 0, 0);

            if (Game1.keyboardState.IsKeyDown(Keys.Up))
                direction -= Vector3.UnitZ;

            else if (Game1.keyboardState.IsKeyDown(Keys.Down))
                direction += Vector3.UnitZ;

            if (Game1.keyboardState.IsKeyDown(Keys.Left))
                direction -= Vector3.UnitX;

            else if (Game1.keyboardState.IsKeyDown(Keys.Right))
                direction += Vector3.UnitX;

            return direction;
        }

        public bool speedPressed()
        {
            return Game1.keyboardState.IsKeyDown(Keys.RightShift);
        }

        public Vector3 rotate()
        {
            Vector2 mousePos = new Vector2(Game1.mouseState.X, Game1.mouseState.Y);

            float dx = mousePos.X - oldMouseX;
            rotation.Y -= Settings.mouseSpeed * dx;

            float dy = mousePos.Y - oldMouseY;
            rotation.X -= Settings.mouseSpeed * dy;

            if (Game1.active)
                resetMouse();

            return rotation;
        }

        private void resetMouse()
        {
            Game1.mouseManager.SetPosition(new Vector2(0.5f, 0.5f));

            oldMouseX = 0.5f;
            oldMouseY = 0.5f;
        }

        public void update()
        {

        }

        public void clamp(float xMin, float xMax)
        {
            rotation.X = MathUtil.Clamp(rotation.X, xMin, xMax);
        }

        public bool zoomIn()
        {
           return Game1.mouseState.WheelDelta - Game1.oldMouseState.WheelDelta > 0;
        }

        public bool zoomOut()
        {
           return Game1.mouseState.WheelDelta - Game1.oldMouseState.WheelDelta < 0;
        }


        public bool shoot()
        {
            return Game1.keyboardState.IsKeyPressed(Keys.Space);
        }
    }

    public class PlayerGamepad : PlayerController
    {
        CustomGamepad gamepad;

        Vector3 rotation = new Vector3(0, 0, 0);

        public PlayerGamepad(SharpDX.XInput.UserIndex index)
        {
            gamepad = new CustomGamepad(index);
        }

        public Vector3 getMoveDirection()
        {
            Vector2 direction = gamepad.leftPad();


            direction.X = (1 - Math.Abs(direction.Y) * Settings.gamePadYawDeadZone) * direction.X;

            return new Vector3(direction.X, 0, -direction.Y);
        }

        public Vector3 rotate()
        {

            Vector2 rotation = gamepad.rightPad();

            //y-deadzone when using x axis
            rotation.Y = (1 - (Math.Abs(rotation.X) * Settings.gamePadYawDeadZone)) * rotation.Y;


            this.rotation += new Vector3(rotation.Y, -rotation.X, 0) * Settings.gamePadSpeed;

            return this.rotation;
        }

        public bool speedPressed()
        {
            return gamepad.isPressed(SharpDX.XInput.GamepadButtonFlags.A);
        }


        public void update()
        {
            gamepad.update();
        }

        public void clamp(float xMin, float xMax)
        {
            rotation.X = MathUtil.Clamp(rotation.X, xMin, xMax);
        }

        public bool zoomIn()
        {
            return gamepad.isPressed(SharpDX.XInput.GamepadButtonFlags.DPadUp);
        }

        public bool zoomOut()
        {
            return gamepad.isPressed(SharpDX.XInput.GamepadButtonFlags.DPadDown);
        }


        public bool shoot()
        {
            throw new NotImplementedException();
        }
    }




}
