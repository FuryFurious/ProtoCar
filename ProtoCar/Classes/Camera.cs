using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class Camera
    {
        Vector3 position;

        Vector3 rotation;
        Vector3 direction;

        Matrix view;
        Matrix projection;
        Matrix viewProjection;

        public Camera(GraphicsDevice device)
        {
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            direction = new Vector3(0, 0, -1);

            view = Matrix.LookAtRH(position, direction, Vector3.Up);

            projection = Matrix.PerspectiveFovRH(
              0.6f,                                                           // Field of view
              (float)device.BackBuffer.Width / device.BackBuffer.Height,  // Aspect ratio
              0.5f,                                                           // Near clipping plane
              500.0f); 
        }

        public void updateMatrices()
        {
            Matrix rotationMatrix = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y);

            direction = Helper.Transform(-Vector3.UnitZ, ref rotationMatrix);

            Vector3 lookAt = position + direction;

            view = Matrix.LookAtRH(position, lookAt, Vector3.Up);

            viewProjection = view * projection;
        }


        

    }
}
