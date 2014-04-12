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
    class Item
    {
        public Matrix world;
        public GeometricPrimitive primitive;
        public BasicEffect bEffect;
        public ACamera cam;

        public Item(Vector3 position)
        {
            this.primitive = GeometricPrimitive.Teapot.New(Game1.gManager.GraphicsDevice, 1.0f, 8, false);
            this.cam = new FirstPersonCamera(Game1.gManager.GraphicsDevice);
            this.world = Matrix.Translation(position);

            this.bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);
            bEffect.SpecularColor = new Vector3(0, 0, 0);
            bEffect.EnableDefaultLighting();
            bEffect.Texture = Game1.stoneTexture;
            bEffect.TextureEnabled = true;
            
        }

        public void update(GameTime gameTime)
        {
            bEffect.World = world;
            bEffect.View = cam.view;
            bEffect.Projection = cam.projection;
        }

        public void draw(GameTime gameTime)
        {
            primitive.Draw(bEffect);
        }
    }
}
