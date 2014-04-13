using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class FirstPersonCamera : ACamera
    {
        public Vector3 direction;


        public FirstPersonCamera(GraphicsDevice device)
        {
            projection = Matrix.PerspectiveFovRH(
              0.6f,                                                             // Field of view
              (float)device.BackBuffer.Width / (Settings.enablePlayer2 ? (device.BackBuffer.Height/2) : device.BackBuffer.Height),        // Aspect ratio //only height/2 because our Viewport is just height / 2
              0.1f,                                                             // Near clipping plane
              500.0f);
        }



        public override void updateMatrices(Vector3 position)
        {
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, 0);
            direction = Helper.Transform(-Vector3.UnitZ, ref rotationMatrix);
            direction.Normalize();

            Vector3 lookAt = position + direction;

            view = Matrix.LookAtRH(position, lookAt, Vector3.Up);
        }

        public override Vector3 moved(Vector3 position, Vector3 deltaPos)
        {
            Matrix matrix;

            if (Settings.enableNoclip)
                matrix = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, 0);

            else
                matrix = Matrix.RotationY(rotation.Y);

            position += Helper.Transform(deltaPos, ref matrix);

            return position;
        }


        public override Vector2 clampMinMax()
        {
            return new Vector2(-1.5f, 1.5f);
        }



        public override void zoomIn()
        {
         
        }

        public override void zoomOut()
        {
            
        }
    }
}
