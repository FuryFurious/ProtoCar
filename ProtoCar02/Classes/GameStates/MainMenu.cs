using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class MainMenu : IGameState
    {
        int currentIndex = 0;
        int count = 2;

        string mainMenuString = "Mainmenu";
        string startString  = "Start";
        string endString    = "End";

        Vector2 mainMenuStringLen;
        Vector2 startStringLen;
        Vector2 endStringLen;

        public MainMenu()
        {
            startStringLen = Game1.font.MeasureString(startString);
            endStringLen = Game1.font.MeasureString(endString);
            mainMenuStringLen = Game1.font.MeasureString(mainMenuString);
        }

        public void init()
        {
           
        }

        public EGameState update(GameTime gameTime)
        {
            if (Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Down) || Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.S))
                currentIndex = (currentIndex + 1) % count;

            else if (Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Up) || Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.W))
                currentIndex = (currentIndex + count - 1) % count;

            if (Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Enter))
            {
                switch (currentIndex)
                {
                    case 0:
                        return EGameState.Sandbox;

                    case 1:
                        return EGameState.None;
                }
            }


            else if (Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Escape))
                return EGameState.None;


            return EGameState.MainMenu;
        }

        public void draw(GameTime gameTime)
        {
            Game1.gManager.GraphicsDevice.Clear(Color.Black);

            Game1.spriteBatch.Begin();
            Game1.spriteBatch.DrawString(Game1.font, mainMenuString, new Vector2((Settings.windowWidth - mainMenuStringLen.X) / 2, Settings.windowHeight / 4 - mainMenuStringLen.Y / 2), Color.White);
            Game1.spriteBatch.DrawString(Game1.font, startString, new Vector2((Settings.windowWidth - startStringLen.X) / 2, Settings.windowHeight / 4 - startStringLen.Y / 2 + startStringLen.Y), currentIndex == 0 ? Color.Red : Color.White);
            Game1.spriteBatch.DrawString(Game1.font, endString, new Vector2((Settings.windowWidth - endStringLen.X) / 2, Settings.windowHeight / 4 - endStringLen.Y / 2 + endStringLen.Y * 2), currentIndex == 1 ? Color.Red : Color.White);
            Game1.spriteBatch.End();
        }
    }
}
