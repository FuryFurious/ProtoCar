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
    class Player
    {
        public Matrix world;
        public GeometricPrimitive primitive;
        public Camera cam;
        public BasicEffect bEffect;

        PlayerControler controler;

        float speed = 0.1f;

        public Player(PlayerControler controler)
        {
            this.primitive = GeometricPrimitive.Teapot.New(Game1.gManager.GraphicsDevice, 1.0f, 8, false);
            this.controler = controler;
            this.cam = new Camera(Game1.gManager.GraphicsDevice, new Vector3(0, 0, 0));

            this.bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);
            bEffect.SpecularColor = new Vector3(0, 0, 0);
            bEffect.EnableDefaultLighting();
            bEffect.Texture = Game1.stoneTexture;
            bEffect.TextureEnabled = true;
            
        }

        public void update(GameTime gameTime, bool useMouse)
        {
            if(controler.moveLeft())
                cam.move(-Vector3.UnitX * speed);

            else if(controler.moveRight())
                cam.move(Vector3.UnitX * speed);

            if (controler.moveUp())
                cam.move(-Vector3.UnitZ * speed);

            else if (controler.moveDown())
                cam.move(Vector3.UnitZ * speed);


            if (useMouse)
                cam.update();
            else
                cam.updateMatrices();
        

        
          //  cam.updateMatrices();
            world = Matrix.RotationYawPitchRoll(cam.rotation.Y, cam.rotation.X, 0) * Matrix.Translation(cam.position);

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
