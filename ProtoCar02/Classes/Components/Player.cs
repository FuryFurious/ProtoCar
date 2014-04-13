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
        public Vector3 position;
        public Matrix world;
        public GeometricPrimitive primitive;
        public ACamera cam;
        public BasicEffect bEffect;

        public int points = 0;

        PlayerController controler;

        float speed = 1.0f;
        Vector3 direction = Vector3.Zero;

        public double boostEnergy = 0;
        public double drawEffectDuration = 0;

        public Player(PlayerController controler, Vector3 position)
        {
            this.position = position;
            this.primitive = GeometricPrimitive.Teapot.New(Game1.gManager.GraphicsDevice, 1.0f, 8, false);
            this.controler = controler;
            this.cam = new ThirdPersonCamera(Game1.gManager.GraphicsDevice, new Vector3(0,5,-5));

            this.bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);
            bEffect.SpecularColor = new Vector3(0, 0, 0);
            bEffect.EnableDefaultLighting();
            bEffect.Texture = Game1.stoneTexture;
            bEffect.TextureEnabled = true;
        }

        public void update(GameTime gameTime)
        {
            controler.update();

            drawEffectDuration -= gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 dir = controler.getMoveDirection();

            //only call cam.move if there is any actual movement:

            direction *= Settings.playerBreakDown;
          //  Console.WriteLine(direction.ToString());
            direction = direction + (dir * Settings.playerSpeedUp);
            if (direction.LengthSquared() > 1)
                direction.Normalize();

         //   else if (direction.LengthSquared() < 0.001)
         //       direction = Vector3.Zero;

            if (controler.speedPressed() && boostEnergy > 0)
            {
                boostEnergy -= gameTime.ElapsedGameTime.TotalSeconds;
                speed = 10f;

                //TODO: make more beautiful:
                Sandbox.particles.Add(new BillboardParticle(Game1.flameTexture, 0.01f, position, Vector3.Up));

                if (boostEnergy <= 0)
                    speed = 1.0f;
            }

            else if (!controler.speedPressed())
                speed = 1f;

            if(direction.LengthSquared() > 0)
                this.position = cam.moved(this.position, direction * speed * Settings.playerSpeed);


            Vector2 clampArea = cam.clampMinMax();
            controler.clamp(clampArea.X, clampArea.Y);

            cam.rotation = controler.rotate();

            if (controler.zoomIn())
                cam.zoomIn();

            else if (controler.zoomOut())
                cam.zoomOut();

            cam.updateMatrices(this.position);
        
            world = Matrix.RotationYawPitchRoll(cam.rotation.Y, cam.rotation.X, 0) * Matrix.Translation(this.position);

            bEffect.World = world;
            bEffect.View = cam.view;
            bEffect.Projection = cam.projection;
        }

        public void addPoints(int points)
        {
            this.points += points;
            this.drawEffectDuration = Settings.effectDuration;
        }



    }
}
