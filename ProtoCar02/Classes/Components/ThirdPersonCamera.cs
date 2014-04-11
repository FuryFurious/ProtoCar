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
    class ThirdPersonCamera
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 direction;

        public Matrix view;
        public Matrix projection;
        public Matrix viewProjection;

        public ThirdPersonCamera(GraphicsDevice device, Vector3 position)
        {
            this.position = position;

            view = Matrix.LookAtRH(position, new Vector3(0,20,0), Vector3.Up);

            projection = Matrix.PerspectiveFovRH(
              0.6f,                                                             // Field of view
              (float)device.BackBuffer.Width / (Settings.enablePlayer2 ? (device.BackBuffer.Height/2) : device.BackBuffer.Height),        // Aspect ratio //only height/2 because our Viewport is just height / 2
              0.1f,                                                             // Near clipping plane
              500.0f);
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

        public void move(Vector3 deltaPos)
        {
            Matrix matrix;

            if(Settings.enableNoclip)
                matrix = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, 0);

            else
                matrix = Matrix.RotationY(rotation.Y);

            position += Helper.Transform(deltaPos, ref matrix);
        }


        

    }
}
