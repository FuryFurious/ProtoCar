using SharpDX;
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
        public static Random random = new Random();

        Player player1;
        Player player2;

        List<Item> items;

        List<APickUp> pickups = new List<APickUp>();

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

            player1 = new Player(Settings.controller1);
            player2 = new Player(Settings.controller2);


            items = new List<Item>();// { new Item(new Vector3(0, 0, 10)) };

            groundPlane = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, 360.0f, 360.0f, 1, false);
        }

        public EGameState update(GameTime gameTime)
        {
            respawnCount -= gameTime.ElapsedGameTime.TotalSeconds;

            if (respawnCount <= 0)
            {
                float x = ((float)random.NextDouble() - 0.5f) * 2.0f * Settings.respawnDistance;
                float z = ((float)random.NextDouble() - 0.5f) * 2.0f * Settings.respawnDistance;

                pickups.Add(new PointPickUp(new Vector3(x,1,z)));

                respawnCount = Settings.respawnInterval;
            }

            player1.update(gameTime);
            player2.update(gameTime);


            //updating the boundinbBoxes:
            //TODO: add them to player / objects
            b1.Minimum = player1.cam.position;
            b2.Minimum = player2.cam.position;
            b1.Maximum = player1.cam.position + new Vector3(1, 1, 1);
            b2.Maximum = player2.cam.position + new Vector3(1, 1, 1);


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


            foreach(APickUp pickup in pickups)
                pickup.draw(bEffect.View, bEffect.Projection);
           

            Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);



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

                foreach (APickUp pickup in pickups)
                    pickup.draw(bEffect.View, bEffect.Projection);

                Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);



               // foreach (Item item in items)
              //      item.draw(gameTime);

            }


            if (Settings.enablePlayer2)
            {
                //I had trouble drawing 3d stuff when 2d and 3d graphics are mixed, so draw 2d after all 3d stuff:
                Game1.gManager.GraphicsDevice.SetViewport(viewport0);

                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, Game1.alphaBlend, null, null, null, null);

                Game1.spriteBatch.Draw(Game1.hud, new Vector2(0, viewport1.Bounds.Bottom - 100), Color.White);
                Game1.spriteBatch.DrawString(Game1.font, "" + player1.points, new Vector2(5, viewport1.Bounds.Bottom - 60), Color.Black);

                Game1.spriteBatch.Draw(Game1.hud, new Vector2(0, viewport2.Bounds.Bottom - 200), Color.White);
                Game1.spriteBatch.DrawString(Game1.font, "" + player2.points, new Vector2(5, viewport2.Bounds.Bottom - 160), Color.Black);

                Game1.spriteBatch.DrawString(Game1.font, "" + 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds, Vector2.Zero, Color.Black);

                if (player1.drawEffectDuration >= 0)
                    Game1.spriteBatch.Draw(Game1.hitEffectTexture, new Rectangle((int)viewport1.X, (int)viewport1.Y, (int)viewport1.Width, (int)viewport1.Height), Color.White * Math.Max((float)player1.drawEffectDuration, 0));

                if(player2.drawEffectDuration >= 0)
                    Game1.spriteBatch.Draw(Game1.hitEffectTexture, new Rectangle((int)viewport2.X, (int)viewport2.Y, (int)viewport2.Width, (int)viewport2.Height), Color.White * Math.Max((float)player2.drawEffectDuration, 0));

                Game1.spriteBatch.End();
            }

      
        }


 
    }
}
