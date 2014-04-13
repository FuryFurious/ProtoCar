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
    abstract class ACamera
    {
        public Vector3 rotation;

        public Matrix view;
        public Matrix projection;

        public abstract void updateMatrices(Vector3 position);

        /// <summary>
        /// Returns the transformed position of the given position in deltaPos direction.
        /// </summary>
        /// <param name="position">Current position.</param>
        /// <param name="deltaPos">Direction in AXISALLIGNED direction.</param>
        /// <returns>New position.</returns>
        public abstract Vector3 moved(Vector3 position, Vector3 deltaPos);

        public abstract Vector2 clampMinMax();

        public abstract void zoomIn();

        public abstract void zoomOut();

        public abstract Vector3 getPosition();

        public abstract Vector3 getDirection();
    }
}
