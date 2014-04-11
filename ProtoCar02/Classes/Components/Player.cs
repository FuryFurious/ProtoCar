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

        public int points = 0;

        PlayerController controler;

        Vector3 direction;


        public Player(PlayerController controler)
        {
            this.primitive = GeometricPrimitive.Teapot.New(Game1.gManager.GraphicsDevice, 1.0f, 8, false);
            this.controler = controler;
            this.cam = new Camera(Game1.gManager.GraphicsDevice, new Vector3(0, 1.0f, 0));

            this.bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);
            bEffect.SpecularColor = new Vector3(0, 0, 0);
            bEffect.EnableDefaultLighting();
            bEffect.Texture = Game1.stoneTexture;
            bEffect.TextureEnabled = true;
        }

        public void update(GameTime gameTime)
        {
            controler.update();

            Vector3 dir = controler.getMoveDirection();
            dir.Normalize();

            //only call cam.move if there is any actual movement:

            direction *= Settings.playerBreakDown;
            Console.WriteLine(direction.ToString());
            direction = direction + (dir * Settings.playerSpeedUp);
            if (direction.LengthSquared() > 1)
                direction.Normalize();
            if (direction.LengthSquared() < 0.1 * 0.1)
                direction = Vector3.Zero;

            if(direction.LengthSquared() > 0)
                cam.move(direction * Settings.playerSpeed);

            cam.rotation = controler.rotate();

            cam.updateMatrices();
        
            world = Matrix.RotationYawPitchRoll(cam.rotation.Y, cam.rotation.X, 0) * Matrix.Translation(cam.position);

            bEffect.World = world;
            bEffect.View = cam.view;
            bEffect.Projection = cam.projection;
        }

    }
}
