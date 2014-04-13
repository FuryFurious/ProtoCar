using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class BillboardParticle
    {
        GeometricPrimitive plane;

        float speed;

        Vector3 direction;
        Vector3 position;

        public double lifeTime = 2.0;

        BasicEffect bEffect;

        public BillboardParticle(Texture2D texture, float speed, Vector3 position, Vector3 direction)
        {
            plane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, 1.0f, 1.0f, 1, false);

            bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);
            bEffect.TextureEnabled = true;
            bEffect.Texture = texture;

            this.direction = direction;
            this.position = position;
            this.speed = speed;
        }

        public void update(GameTime gameTime)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;


            position = position + direction * speed;
        }

        public void draw(Matrix view, Matrix projection, Vector3 cameraPos, Vector3 cameraDir)
        {

            bEffect.Alpha       = (float)Math.Min(1.0f, lifeTime);
            bEffect.View        = view;
            bEffect.Projection  = projection;
            bEffect.World       = Matrix.BillboardRH(this.position, cameraPos, Vector3.Up, cameraDir);

            plane.Draw(bEffect);
        }
    }
}
