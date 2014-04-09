using SharpDX;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class Camera
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 direction;

        public Matrix view;
        public Matrix projection;
        public Matrix viewProjection;

        private float rotationSpeed = 0.75f;

        private float oldMouseX;
        private float oldMouseY;

        public Camera(GraphicsDevice device, Vector3 position)
        {
            this.position = position;

            view = Matrix.LookAtRH(position, new Vector3(0,20,0), Vector3.Up);

            projection = Matrix.PerspectiveFovRH(
              0.6f,                                                             // Field of view
              (float)device.BackBuffer.Width / device.BackBuffer.Height,        // Aspect ratio
              0.5f,                                                             // Near clipping plane
              500.0f); 
        }


        public void update()
        {

            rotation.X = MathUtil.Clamp(rotation.X, -1.5f, 1.5f);

            Vector2 mousePos = new Vector2(Game1.mouseState.X, Game1.mouseState.Y);

            float dx = mousePos.X - oldMouseX;
            rotation.Y -= rotationSpeed * dx;

            float dy = mousePos.Y - oldMouseY;
            rotation.X -= rotationSpeed * dy;

            if(Game1.active)
                resetMouse();

            updateMatrices();
        }

        public void updateMatrices()
        {
 
            Matrix rotationMatrix = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y);

            direction = Helper.Transform(-Vector3.UnitZ, ref rotationMatrix);
            direction.Normalize();

            Vector3 lookAt = position + direction;

            view = Matrix.LookAtRH(position, lookAt, Vector3.Up);

            viewProjection = view * projection;
        }

        private void resetMouse()
        {
            Game1.mouseManager.SetPosition(new Vector2(0.5f));

            oldMouseX = 0.5f;
            oldMouseY = 0.5f;
        }

        public void move(Vector3 deltaPos)
        {
            Matrix matrix = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, 0);
            position += Helper.Transform(deltaPos, ref matrix);
        }


        

    }
}
