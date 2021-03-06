﻿using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    /// <summary>
    /// GameState for testing stuff
    /// </summary>
    class Sandbox : IGameState
    {
        DateTime startTime;
        public Player player1;
        public Player player2;

        List<Item> items;

        List<APickUp> pickups = new List<APickUp>();
        public static List<Bullet> bullets = new List<Bullet>();

        //plane
        GeometricPrimitive groundPlane;

        //for 3d boundingBox intersections testing:
        BoundingBox b1 = new BoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        BoundingBox b2 = new BoundingBox(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1));

        BasicEffect bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);

        //fullsize viewport:
        ViewportF viewport0 = new ViewportF(0, 0, Settings.windowWidth, Settings.windowHeight);

        //upper-half viewport
        ViewportF viewport1 = new ViewportF(0, 0, Settings.windowWidth, Settings.windowHeight / 2);

        //lower-half viewport
        ViewportF viewport2 = new ViewportF(0, Settings.windowHeight / 2, Settings.windowWidth, Settings.windowHeight / 2);


        Matrix skydomeMatrix = Matrix.RotationX((float)Math.PI) * Matrix.Scaling(10, 10, 10);
      

        public static List<BillboardParticle> particles = new List<BillboardParticle>();

        double respawnCount;

        public Sandbox()
        {
           
        }

        public void init()
        {

            bEffect.EnableDefaultLighting();
            bEffect.TextureEnabled = true;
            bEffect.Texture = Game1.stoneTextureBig;
            //makes the specularPower more dull:
            bEffect.SpecularPower = 100.0f;

            player1 = new Player(Settings.controller1, new Vector3(0,1,0));
            player2 = new Player(Settings.controller2, new Vector3(0, 1, 0));


            items = new List<Item>();// { new Item(new Vector3(0, 0, 10)) };
            startTime = DateTime.Now.AddMinutes(Settings.roundDuration);

            groundPlane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, 360.0f, 360.0f, 1, false);
        }

        public EGameState update(GameTime gameTime)
        {
            if (Game1.keyboardState.IsKeyPressed(Keys.Escape))
                return EGameState.GameOver;

            respawnCount -= gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update(gameTime);

                if (particles[i].lifeTime <= 0)
                {
                    particles.Remove(particles[i]);
                    i--;
                }
            }

            if (startTime < DateTime.Now)
                return EGameState.GameOver;

            if (respawnCount <= 0)
            {
                float x = ((float)Game1.random.NextDouble() - 0.5f) * 2.0f * Settings.respawnDistance; //random x coord
                float z = ((float)Game1.random.NextDouble() - 0.5f) * 2.0f * Settings.respawnDistance; //random z coord

                pickups.Add(new PointPickUp(new Vector3(x, 1, z)));

                respawnCount = Settings.respawnInterval;
            }

            player1.update(gameTime);
            player2.update(gameTime);


            //updating the boundinbBoxes:
            //TODO: add them to player / objects
            b1.Minimum = player1.position;
            b2.Minimum = player2.position;
            b1.Maximum = player1.position + new Vector3(1, 1, 1);
            b2.Maximum = player2.position + new Vector3(1, 1, 1);


            for (int i = 0; i < pickups.Count; i++)
            {
                if (pickups[i].boundingBox.Intersects(b1))
                {
                    pickups[i].onHit(player1);
                    pickups.Remove(pickups[i]);
                }


                else if (pickups[i].boundingBox.Intersects(b2))
                {
                    pickups[i].onHit(player2);
                    pickups.Remove(pickups[i]);
                }
            }


            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].update(gameTime);


       

             //   Console.Clear();
            //    Console.WriteLine(bullets[i].boundingSphere.Center);

                if (bullets[i].boundingSphere.Intersects(b1))
                {
                    player1.drawEffectDuration = Settings.effectDuration;
                    Game1.explosionSound.Play();
                    player1.points = Math.Max(player1.points - 1, 0);
                    bullets.Remove(bullets[i]);
                    continue;
                }

                else if (bullets[i].boundingSphere.Intersects(b2))
                {

                    player2.drawEffectDuration = Settings.effectDuration;
                    Game1.explosionSound.Play();
                    player2.points = Math.Max(player2.points - 1, 0);
                    bullets.Remove(bullets[i]);
                    continue;
                }

                else if (bullets[i].lifeTime <= 0)
                {
                    bullets.Remove(bullets[i]);
                    continue;
                }

        
         

            }

            //  foreach (Item item in items)
            //       item.update(gameTime);

            return EGameState.Sandbox;
        }

        public void draw(GameTime gameTime)
        {
            Game1.gManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game1.gManager.GraphicsDevice.SetRasterizerState(Game1.gManager.GraphicsDevice.RasterizerStates.CullNone);

            //TODO organize the draw calls for cleaner code:

            //view for player1
            if (Settings.enablePlayer2)
                Game1.gManager.GraphicsDevice.SetViewport(viewport1);

            bEffect.View = player1.bEffect.View;
            bEffect.Projection = player1.bEffect.Projection;

            bEffect.Texture = Game1.stoneTextureBig;
            bEffect.World = Matrix.RotationX((float)Math.PI / 2);
            groundPlane.Draw(bEffect);

            bEffect.Texture = Game1.stoneTexture;
            bEffect.World = player2.world;
            player2.primitive.Draw(bEffect);

            bEffect.World = player1.world;
            player1.primitive.Draw(bEffect);

            foreach(APickUp pickup in pickups)
                pickup.draw(bEffect.View, bEffect.Projection);

            foreach (Bullet b in bullets)
                b.draw(bEffect.Projection, bEffect.View);
            
            Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);

            Game1.gManager.GraphicsDevice.SetDepthStencilState(Game1.gManager.GraphicsDevice.DepthStencilStates.DepthRead);
            Game1.gManager.GraphicsDevice.SetBlendState(Game1.alphaBlend);

            foreach (BillboardParticle p in particles)
                p.draw(bEffect.View, bEffect.Projection, player1.cam.getPosition(), player1.cam.getDirection());

            Game1.gManager.GraphicsDevice.SetBlendState(null);
            Game1.gManager.GraphicsDevice.SetDepthStencilState(Game1.gManager.GraphicsDevice.DepthStencilStates.Default);
        
 
            
            if (Settings.enablePlayer2)
            {
                //view for player 2
                Game1.gManager.GraphicsDevice.SetViewport(viewport2);

                bEffect.View = player2.bEffect.View;
                bEffect.Projection = player2.bEffect.Projection;

                bEffect.Texture = Game1.stoneTextureBig;
                bEffect.World = Matrix.RotationX((float)Math.PI / 2);
                groundPlane.Draw(bEffect);

                bEffect.Texture = Game1.stoneTexture;
                bEffect.World = player1.world;
                player1.primitive.Draw(bEffect);

                bEffect.World = player2.world;
                player2.primitive.Draw(bEffect);

                foreach (APickUp pickup in pickups)
                    pickup.draw(bEffect.View, bEffect.Projection);

                foreach (Bullet b in bullets)
                    b.draw(bEffect.Projection, bEffect.View);


                Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);

                Game1.gManager.GraphicsDevice.SetDepthStencilState(Game1.gManager.GraphicsDevice.DepthStencilStates.DepthRead);
                Game1.gManager.GraphicsDevice.SetBlendState(Game1.alphaBlend);

                foreach (BillboardParticle p in particles)
                    p.draw(bEffect.View, bEffect.Projection, player2.cam.getPosition(), player2.cam.getDirection());

                Game1.gManager.GraphicsDevice.SetBlendState(null);
                Game1.gManager.GraphicsDevice.SetDepthStencilState(Game1.gManager.GraphicsDevice.DepthStencilStates.Default);


               // foreach (Item item in items)
              //      item.draw(gameTime);

            }


            if (Settings.enablePlayer2)
            {
                //I had trouble drawing 3d stuff when 2d and 3d graphics are mixed, so draw 2d after all 3d stuff:
                Game1.gManager.GraphicsDevice.SetViewport(viewport0);

                Vector2 len1 = Game1.font.MeasureString(player1.points + "");
                Vector2 len2 = Game1.font.MeasureString(player2.points + "");

                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, Game1.alphaBlend, null, null, null, null);


                Game1.spriteBatch.Draw(Game1.pixelTexture, new RectangleF(100.0f, viewport1.Bounds.Bottom - 60.0f, (float)player1.boostEnergy * 50.0f, 20.0f), Color.Blue);
                Game1.spriteBatch.Draw(Game1.pixelTexture, new RectangleF(100.0f, viewport2.Bounds.Bottom - 60.0f, (float)player2.boostEnergy * 50.0f, 20.0f), Color.Blue);

                Game1.spriteBatch.Draw(Game1.hud, new Vector2(0, viewport1.Bounds.Bottom - 100), Color.White);
                Game1.spriteBatch.DrawString(Game1.font, "" + player1.points, new Vector2((100 - len1.X) / 2, viewport1.Bounds.Bottom - 60), Color.Black);

                Game1.spriteBatch.Draw(Game1.hud, new Vector2(0, viewport2.Bounds.Bottom - 100), Color.White);
                Game1.spriteBatch.DrawString(Game1.font, "" + player2.points, new Vector2((100 - len2.X) / 2, viewport2.Bounds.Bottom - 60), Color.Black);

                Game1.spriteBatch.DrawString(Game1.font, "" + 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds, Vector2.Zero, Color.Black);
                string time = (startTime - DateTime.Now).Minutes + ":" + (((startTime - DateTime.Now).Seconds < 10) ? ("0" + (startTime - DateTime.Now).Seconds) : ("" + (startTime - DateTime.Now).Seconds));
                Game1.spriteBatch.DrawString(Game1.font, time, new Vector2((viewport1.Width - Game1.font.MeasureString(time).X) / 2, viewport1.Y + 10), Color.Black);
                Game1.spriteBatch.DrawString(Game1.font, time, new Vector2((viewport2.Width - Game1.font.MeasureString(time).X) / 2, viewport2.Y + 10), Color.Black);


                if (player1.drawEffectDuration >= 0)
                    Game1.spriteBatch.Draw(Game1.hitEffectTexture, new Rectangle((int)viewport1.X, (int)viewport1.Y, (int)viewport1.Width, (int)viewport1.Height), Color.Red * Math.Max((float)player1.drawEffectDuration, 0));

                if(player2.drawEffectDuration >= 0)
                    Game1.spriteBatch.Draw(Game1.hitEffectTexture, new Rectangle((int)viewport2.X, (int)viewport2.Y, (int)viewport2.Width, (int)viewport2.Height), Color.Red * Math.Max((float)player2.drawEffectDuration, 0));

                Game1.spriteBatch.End();
            }

      
        }


 
    }
}
