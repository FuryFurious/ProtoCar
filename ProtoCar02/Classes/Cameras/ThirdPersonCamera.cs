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
    class ThirdPersonCamera : ACamera
    {
        public Vector3 offset;

        public ThirdPersonCamera(GraphicsDevice device, Vector3 offset)
        {
            this.offset = offset;
            projection = Matrix.PerspectiveFovRH(
              0.6f,                                                             // Field of view
              (float)device.BackBuffer.Width / (Settings.enablePlayer2 ? (device.BackBuffer.Height/2) : device.BackBuffer.Height),        // Aspect ratio //only height/2 because our Viewport is just height / 2
              0.1f,                                                             // Near clipping plane
              500.0f);
        }

        public override void updateMatrices(Vector3 position)
        {
            if (rotation.X > -0.15f)
                rotation.X = -0.15f;

            Matrix rotationM = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, 0);
            Vector3 newOffset = Helper.Transform(offset, ref rotationM);
            Vector3 ownPos = position - newOffset;

            this.view = Matrix.LookAtRH(ownPos, position, Vector3.Up);
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
            return new Vector2(-1.5f, -0.75f);
        }

        public override void zoomIn()
        {
            if (offset.Y <= Settings.maxZoomIn)
                return;

            offset -= new Vector3(0,Settings.zoomSpeed, -Settings.zoomSpeed);
        }

        public override void zoomOut()
        {
            if (offset.Y >= Settings.maxZoomOut)
                return;

            offset += new Vector3(0, Settings.zoomSpeed, -Settings.zoomSpeed);
        }
    }
}
