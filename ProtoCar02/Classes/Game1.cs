﻿using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Audio;
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
        //TODO: dont make everything public static... but for prototyping its the easiest way
        public static GraphicsDeviceManager gManager;
        public static AudioManager aManager;

        public static SpriteBatch spriteBatch;

        public static KeyboardManager keyboardManager;
        public static KeyboardState keyboardState;

        public static MouseManager mouseManager;
        public static MouseState mouseState;
        public static MouseState oldMouseState;

        public static BlendState alphaBlend;
        public static BlendState opaqueBlend;

        public static bool active = false;

        public static Texture2D stoneTexture;
        public static Texture2D hud;
        public static Texture2D stoneTextureBig;
        public static Texture2D blueTexture;
        public static Texture2D hitEffectTexture;
        public static Texture2D pixelTexture;
        public static Texture2D flameTexture;
        public static Texture2D starTexture;
        
        public static SpriteFont font;

        public static Model skydome;

        public static SoundEffect soundHit;
        public static SoundEffect laserSound;
        public static SoundEffect explosionSound;

        public static Random random = new Random();

        //assign this variable, to start with a different gameState
        EGameState currentState = EGameState.MainMenu;
        EGameState prevState;

        IGameState gameState;

        public Game1()
        {
            gManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            mouseManager = new MouseManager(this);
            aManager = new AudioManager(this);

            Content.RootDirectory = "Content";

            gManager.PreferredBackBufferWidth = Settings.windowWidth; 
            gManager.PreferredBackBufferHeight = Settings.windowHeight;

        }

        private void gainedFocus(object sender, EventArgs e)
        {
            active = true;
        }

        private void lostFocus(object sender, EventArgs e)
        {
            active = false;
        }

        protected override void Initialize()
        {
            Window.Title = "ProtoCar";

            //to keep track if the window has focus or not (e.g. for not reseting the mouse pos)
            Window.Deactivated += lostFocus;
            Window.Activated += gainedFocus;
           

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("Arial16");
            stoneTexture = Content.Load<Texture2D>("redThing");
            hud = Content.Load<Texture2D>("hud");
            skydome = Content.Load<Model>("skydome");
            stoneTextureBig = Content.Load<Texture2D>("StoneFloorBig");
            blueTexture = Content.Load<Texture2D>("blueTexture");
            hitEffectTexture = Content.Load<Texture2D>("hitEffect");
            soundHit = Content.Load<SoundEffect>("pickup.wav");
            pixelTexture = Content.Load<Texture2D>("pixel");
            flameTexture = Content.Load<Texture2D>("flameParticle");

            explosionSound = Content.Load<SoundEffect>("gotHit.wav");
            laserSound = Content.Load<SoundEffect>("laserShot.wav");

            starTexture = Content.Load<Texture2D>("star");

            alphaBlend = BlendState.New(GraphicsDevice, 
                                                SharpDX.Direct3D11.BlendOption.SourceAlpha,         //sourceBlend
                                                SharpDX.Direct3D11.BlendOption.InverseSourceAlpha,         //destinationBlend
                                                SharpDX.Direct3D11.BlendOperation.Add,              //blendoperation
                                                SharpDX.Direct3D11.BlendOption.SourceAlpha,    //source alphaBlend
                                                SharpDX.Direct3D11.BlendOption.InverseSourceAlpha,         //destination alpha blend
                                                SharpDX.Direct3D11.BlendOperation.Add,              //alphablend operation
                                                SharpDX.Direct3D11.ColorWriteMaskFlags.All,  //which píxel affected
                                                -1);

            var blendStateDesc = SharpDX.Direct3D11.BlendStateDescription.Default();
            opaqueBlend = BlendState.New(GraphicsDevice, "Opaque", blendStateDesc);

            handleNewGameState();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = keyboardManager.GetState();

            oldMouseState = mouseState;
            mouseState = mouseManager.GetState();

            currentState = gameState.update(gameTime);

            if (currentState != prevState)
                handleNewGameState();

            if(keyboardState.IsKeyPressed(Keys.F1))
            {
                Settings.enableFullscreen = !Settings.enableFullscreen;
                gManager.IsFullScreen = Settings.enableFullscreen;
                gManager.ApplyChanges();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            gameState.draw(gameTime);

            base.Draw(gameTime);
        }

        private void handleNewGameState()
        {
            //gameState.unloadContent();

            switch (currentState)
            {

                case EGameState.Sandbox:
                    gameState = new Sandbox();
                    break;

                case EGameState.MainMenu:
                    gameState = new MainMenu();
                    break;

                case EGameState.GameOver:

                    int p1 = 0;
                    int p2 = 0;

                    if (gameState != null)
                    {
                        p1 = (gameState as Sandbox).player1.points;
                        p2 = (gameState as Sandbox).player2.points;
                    }
              
                    gameState = new GameOver(p1, p2);
                    break;

                default:
                    Exit();
                    break;
            }
            
            gameState.init();
          //  gameState.loadContent(Content);

            prevState = currentState;
        }



    }
}
