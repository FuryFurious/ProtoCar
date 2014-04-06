﻿using SharpDX;
using SharpDX.Toolkit;
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
    class Game1 : Game
    {
        public static GraphicsDeviceManager gManager;
        public static SpriteBatch spriteBatch;

        public static KeyboardManager keyboardManager;
        public static KeyboardState keyboardState;

        public static MouseManager mouseManager;
        public static MouseState mouseState;

        public static Texture2D stoneTexture;
        public static Texture2D hud;

        public static int width = 800;
        public static int height = 600;

        Player player1;
        Player player2;
        List<Item> items;

        GeometricPrimitive primitive;

        int index = 0;

        public Game1()
        {
            gManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            mouseManager = new MouseManager(this);

            Content.RootDirectory = "Content";

            gManager.PreferredBackBufferWidth = width;
            gManager.PreferredBackBufferHeight = height;

        }

        protected override void Initialize()
        {
            Window.Title = "ProtoCar";
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player1 = new Player(new PlayerWASD());
            player2 = new Player(new PlayerArrow());
            items = new List<Item>() { new Item(new Vector3(0, 0, 10)) };

            primitive = GeometricPrimitive.Plane.New(GraphicsDevice, 16.0f, 16.0f, 1, false);
    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
      

            stoneTexture = Content.Load<Texture2D>("Stone.png");
            hud = Content.Load<Texture2D>("hud.png");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = keyboardManager.GetState();
            mouseState = mouseManager.GetState();


            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.D1))
                index = 0;

            else if (keyboardState.IsKeyDown(Keys.D2))
                index = 1;


            player1.update(gameTime, index == 0);
            player2.update(gameTime, index == 1);
            foreach (Item item in items)
                item.update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullNone);

            //plane stuff
            BasicEffect bEffect = new BasicEffect(GraphicsDevice);

            bEffect.EnableDefaultLighting();
            bEffect.TextureEnabled = true;
            bEffect.Texture = stoneTexture;

            //view for player1
            GraphicsDevice.SetViewport(new ViewportF(0, 0, width/2, height));

            bEffect.View = player1.bEffect.View;
            bEffect.Projection = player1.bEffect.Projection;

            bEffect.World = Matrix.RotationX((float)Math.PI / 2);
            primitive.Draw(bEffect);

            bEffect.World = player2.world;
            player2.primitive.Draw(bEffect);

            spriteBatch.Begin();
            spriteBatch.Draw(hud, new Vector2(0, 500), Color.White);
            spriteBatch.End();
 

            //view for player 2
            GraphicsDevice.SetViewport(new ViewportF(width/2, 0, width/2, height));

            bEffect.View = player2.bEffect.View;
            bEffect.Projection = player2.bEffect.Projection;

            bEffect.World = Matrix.RotationX((float)Math.PI / 2);
            primitive.Draw(bEffect); 

            bEffect.World = player1.world;
            player1.primitive.Draw(bEffect);

            foreach (Item item in items)
                item.draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(hud, new Vector2(0, 500), Color.White);
            spriteBatch.End();


      

            base.Draw(gameTime);
        }
    }
}
