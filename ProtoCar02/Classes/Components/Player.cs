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
            boostEnergy += Settings.energyRegeneration;

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
                boostEnergy -= Settings.boostCost;
                speed = 5f;

                if (boostEnergy <= 0)
                    speed = 1.0f;
                //TODO: make more beautiful:

                for (int i = 0; i < Settings.numSmokeParticles; i++)
                {
                    float randX = (float)(Game1.random.NextDouble() - 0.5) * 2 * 20;
                    float randY = (float)(Game1.random.NextDouble() - 0.5) * 2 * 20;

                    Vector3 targetPos = this.position + new Vector3(randX, 0, randY);
                    Vector3 helpDir = targetPos - this.position;
                    helpDir.Normalize();
                    helpDir += Vector3.UnitY * (float)Game1.random.NextDouble();

                    BillboardParticle bPart = new BillboardParticle(Game1.flameTexture, 0.01f, position, helpDir, 0.75f);
                    bPart.rotate = true;
                    bPart.rotation = (float)(Game1.random.NextDouble() * Math.PI * 2);
                    bPart.rotationSpeed = (float)Game1.random.NextDouble() * 0.05f;

                    if (Game1.random.NextDouble() < 0.5)
                        bPart.rotationSpeed *= -1;

                    //   bPart.bEffect.DiffuseColor = new Vector4(0.25f, 0.25f, 0.25f, 0);

                    Sandbox.particles.Add(bPart);
                }

            
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


            Matrix rotationM = Matrix.RotationY(cam.rotation.Y);
            Vector3 rotation = Helper.Transform(-Vector3.UnitZ, ref rotationM);

            if (controler.shoot() && this.boostEnergy > 0)
            {
                this.boostEnergy -= Settings.shootCost;
                Sandbox.bullets.Add(new Bullet(this.position + rotation * 2, rotation));
                Game1.laserSound.Play();
            }

            bEffect.World = world;
            bEffect.View = cam.view;
            bEffect.Projection = cam.projection;
        }

        public void addPoints(int points)
        {
            this.points += points;

            for (int i = 0; i < Settings.numStarParticles; i++)
            {
                float randX = (float)(Game1.random.NextDouble() - 0.5) * 2 * 20;
                float randY = (float)(Game1.random.NextDouble() - 0.5) * 2 * 20;

                Vector3 targetPos = this.position + new Vector3(randX, 0, randY);
                Vector3 helpDir = targetPos - this.position;
                helpDir.Normalize();
                helpDir += Vector3.UnitY * (float)Game1.random.NextDouble();

                BillboardParticle tmpParticle = new BillboardParticle(Game1.starTexture, (float)Game1.random.NextDouble() / 15.0f, this.position, helpDir, 0.5f);
                tmpParticle.bEffect.DiffuseColor = new Vector4(1,1,0,0);
                tmpParticle.rotation = (float)(Game1.random.NextDouble() * Math.PI * 2.0);
                tmpParticle.rotate = true;
                tmpParticle.rotationSpeed = 0.1f;
                tmpParticle.lifeTime = 1.5;

                if (Game1.random.NextDouble() < 0.5)
                    tmpParticle.rotationSpeed *= -1;

                Sandbox.particles.Add(tmpParticle);
            }
        }



    }
}
