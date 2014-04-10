using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    abstract class APickUp
    {
        protected BasicEffect effect;
        protected GeometricPrimitive model;
        public BoundingBox boundingBox;

        public APickUp(Vector3 position)
        {
            model = GeometricPrimitive.Cube.New(Game1.gManager.GraphicsDevice, 1.0f, false);
            this.boundingBox = new BoundingBox(position, position + new Vector3(1, 1, 1));
            this.effect = new BasicEffect(Game1.gManager.GraphicsDevice);
            this.effect.World = Matrix.Translation(position);
            this.effect.TextureEnabled = true;
            this.effect.Texture = Game1.blueTexture;   
        }

        public abstract void onHit(Player player);

        public void draw(Matrix view, Matrix projection)
        {
            this.effect.View = view;
            this.effect.Projection = projection;

            model.Draw(this.effect);
        }
       
    }
}
