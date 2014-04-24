using ProtoCar;
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
    class Bullet
    {
        public BoundingSphere boundingSphere;

        GeometricPrimitive sphere;
        BasicEffect effect;
        Vector3 position;
        Vector3 direction;

        const float speed = 0.75f;

        public double lifeTime = 10.0;


        public Bullet(Vector3 start, Vector3 direction)
        {
            this.boundingSphere = new BoundingSphere(start, 0.5f);

            this.position = start;
            this.direction = direction;

            this.effect = new BasicEffect(Game1.gManager.GraphicsDevice);
            this.effect.World = Matrix.Translation(position);

            this.effect.EnableDefaultLighting();

          //  this.effect.VertexColorEnabled = true;

            this.effect.DiffuseColor = new Vector4(0, 1.0f, 0, 0);

            this.sphere = GeometricPrimitive.Sphere.New(Game1.gManager.GraphicsDevice, 1.0f, 16, false);
            
        }

        public void update(GameTime gameTime)
        {
         //
            
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;

            position = position + direction * speed;

            this.boundingSphere.Center = position;
            
        }

        public void draw(Matrix projection, Matrix view)
        {
            this.effect.View = view;
            this.effect.Projection = projection;
            this.effect.World = Matrix.Translation(position);

            this.sphere.Draw(this.effect);

        }
    }
}
