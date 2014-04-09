﻿using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
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

        Player player1;
        Player player2;
        List<Item> items;
        //plane
        GeometricPrimitive primitive;

        int index = 0;

        BoundingBox b1 = new BoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        BoundingBox b2 = new BoundingBox(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1));

        string text = "";


        public Sandbox()
        {

        }

        public void init()
        {
            player1 = new Player(new PlayerWASD());
            player2 = new Player(new PlayerArrow());
            items = new List<Item>() { new Item(new Vector3(0, 0, 10)) };

            primitive = GeometricPrimitive.Plane.New(Game1.gManager.GraphicsDevice, 1000.0f, 1000.0f, 1, false);
        }

        void IGameState.loadContent(ContentManager Content)
        {

   
           
            
        }

        void IGameState.unloadContent()
        {
        
        }

        public EGameState update(GameTime gameTime)
        {

            if (Game1.keyboardState.IsKeyDown(Keys.D1))
                index = 0;

            else if (Game1.keyboardState.IsKeyDown(Keys.D2))
                index = 1;


            player1.update(gameTime, index == 0);
            player2.update(gameTime, index == 1);

            b1.Minimum = player1.cam.position;
            b2.Minimum = player2.cam.position;

            b1.Maximum = player1.cam.position + new Vector3(1, 1, 1);
            b2.Maximum = player2.cam.position + new Vector3(1, 1, 1);

            foreach (Item item in items)
                item.update(gameTime);

            if (b1.Intersects(ref b2))
                text = "Intersection";
            else
                text = "";

            Console.WriteLine("Help");


            return EGameState.Sandbox;
        }

        public void draw(GameTime gameTime)
        {
            Game1.gManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game1.gManager.GraphicsDevice.SetRasterizerState(Game1.gManager.GraphicsDevice.RasterizerStates.CullNone);
       //     Game1.gManager.GraphicsDevice.SetBlendState(null);

            //plane stuff
            BasicEffect bEffect = new BasicEffect(Game1.gManager.GraphicsDevice);

            bEffect.EnableDefaultLighting();
            bEffect.TextureEnabled = true;
            bEffect.Texture = Game1.stoneTexture;
            bEffect.SpecularPower = 10000.0f;

            //view for player1
            Game1.gManager.GraphicsDevice.SetViewport(new ViewportF(0, 0, Game1.width / 2, Game1.height));

            bEffect.View = player1.bEffect.View;
            bEffect.Projection = player1.bEffect.Projection;

            bEffect.World = Matrix.RotationX((float)Math.PI / 2);
            primitive.Draw(bEffect);

            bEffect.World = player2.world;
            player2.primitive.Draw(bEffect);
            Matrix skydomeMatrix = Matrix.RotationX((float)Math.PI) * Matrix.Scaling(10, 10, 10);

            Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);




            //view for player 2
            Game1.gManager.GraphicsDevice.SetViewport(new ViewportF(Game1.width / 2, 0, Game1.width / 2, Game1.height));

            bEffect.View = player2.bEffect.View;
            bEffect.Projection = player2.bEffect.Projection;

            bEffect.World = Matrix.RotationX((float)Math.PI / 2);
            primitive.Draw(bEffect);

            bEffect.World = player1.world;
            player1.primitive.Draw(bEffect);

            Game1.skydome.Draw(Game1.gManager.GraphicsDevice, skydomeMatrix, bEffect.View, bEffect.Projection);

            foreach (Item item in items)
                item.draw(gameTime);


            //I had trouble drawing 3d stuff when 2d and 3d graphics are mixed, so draw 2d after all 3d stuff:
            Game1.gManager.GraphicsDevice.SetViewport(new ViewportF(0, 0, Game1.width, Game1.height));

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, Game1.alphaBlend, null, null, null, null);
            Game1.spriteBatch.Draw(Game1.hud, new Vector2(0, 500), Color.White);
            Game1.spriteBatch.Draw(Game1.hud, new Vector2(Game1.width / 2, 500), Color.White);
            Game1.spriteBatch.DrawString(Game1.font, text, Vector2.Zero, Color.Black);
            Game1.spriteBatch.End();


      
        }


 
    }
}
