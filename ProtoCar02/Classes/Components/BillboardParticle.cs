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

        public float speed = 0;
        public float rotation = 0;
        public float rotationSpeed = 0.1f;

        public Vector3 direction = new Vector3(0, 1, 0);
        public Vector3 position = new Vector3(0, 0, 0);

        public double lifeTime = 2.0;

        public BasicEffect bEffect;

        public bool rotate = false;


        public BillboardParticle()
        {
            plane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, 1.0f, 1.0f, 1, false);
        }

        public BillboardParticle(float speed, Vector3 position, Vector3 direction, float scale)
        {
            plane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, scale, scale, 1, false);

            bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);

            this.direction = direction;
            this.position = position;
            this.speed = speed;
        }

        public BillboardParticle(Texture2D texture, float speed, Vector3 position, Vector3 direction, float scale)
        {
            plane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, scale, scale, 1, false);

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

            if (rotate)
                rotation += rotationSpeed;
        }

        public void draw(Matrix view, Matrix projection, Vector3 cameraPos, Vector3 cameraDir)
        {
            Matrix world;

            if(rotate)
                world = Matrix.RotationZ(rotation) * Matrix.BillboardRH(this.position, cameraPos, Vector3.Up, cameraDir);

            else
                world = Matrix.BillboardRH(this.position, cameraPos, Vector3.Up, cameraDir);

            bEffect.Alpha       = (float)Math.Min(1.0f, lifeTime);
            bEffect.View        = view;
            bEffect.Projection  = projection;
            bEffect.World       = world;

            plane.Draw(bEffect);
        }
    }
}
