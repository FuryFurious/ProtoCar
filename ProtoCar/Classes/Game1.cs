using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class Game1 : Game
    {
        GraphicsDeviceManager gManager;
        GeometricPrimitive prime;
        Camera cam;

        BasicEffect bEffect;

        public static KeyboardManager keyboardManager;
        public static KeyboardState keyboardState;

        public static MouseManager mouseManager;
        public static MouseState mouseState;

        public static Texture2D stoneTexture;

        public Game1()
        {
            this.gManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            mouseManager = new MouseManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            cam = new Camera(GraphicsDevice, new SharpDX.Vector3(0,0,0));
            prime = GeometricPrimitive.Teapot.New(GraphicsDevice, 1, 8, false);
            stoneTexture = Content.Load<Texture2D>("Stone.png");

            bEffect = new BasicEffect(GraphicsDevice);

            bEffect.SpecularColor = new Vector3(0,0,0);
            bEffect.EnableDefaultLighting();
            bEffect.Texture = stoneTexture;
            bEffect.TextureEnabled = true;

        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = keyboardManager.GetState();
            mouseState = mouseManager.GetState();

            cam.update();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.W))
                cam.move(-Vector3.UnitZ * 0.1f);

            else if (keyboardState.IsKeyDown(Keys.S))
                cam.move(Vector3.UnitZ * 0.1f);

            if (keyboardState.IsKeyDown(Keys.A))
                cam.move(-Vector3.UnitX * 0.1f);

            else if (keyboardState.IsKeyDown(Keys.D))
                cam.move(Vector3.UnitX * 0.1f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            bEffect.World = Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds);
            bEffect.View = cam.view;
            bEffect.Projection = cam.projection;
            
            prime.Draw(bEffect);
            base.Draw(gameTime);
        }
    }
}
