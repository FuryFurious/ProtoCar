using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class GameOver : IGameState
    {
        int points1;
        int points2;

        string gameOverString;

        public GameOver(int player1Point, int player2Points)
        {
            this.points1 = player1Point;
            this.points2 = player2Points;

            if (player1Point > player2Points)
                gameOverString = "Player1 has won with " + points1 + "!\nPlayer2 has " + points2 + " points!" ;

            else if(player2Points > player1Point)
                gameOverString = "Player2 has won with " + points2 + "!\nPlayer1 has " + points1 + " points!";

            else
                gameOverString = "Draw! Both player have " + points1 + " points!";
           
        }

        public void init()
        {
            
        }

        public EGameState update(GameTime gameTime)
        {
            if (Game1.keyboardState.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Enter))
                return EGameState.MainMenu;

            return EGameState.GameOver;
        }

        public void draw(GameTime gameTime)
        {
            Game1.gManager.GraphicsDevice.Clear(Color.Black);

            Game1.spriteBatch.Begin();

            Game1.spriteBatch.DrawString(Game1.font, "GameOver", new Vector2(0, 0), Color.White, 0, Vector2.Zero, 2.0f, SharpDX.Toolkit.Graphics.SpriteEffects.None, 0);
            Game1.spriteBatch.DrawString(Game1.font, gameOverString + "\n\nPress Enter...", new Vector2(0, 40), Color.White, 0, Vector2.Zero, 2.0f, SharpDX.Toolkit.Graphics.SpriteEffects.None, 0);


            Game1.spriteBatch.End();
        }
    }
}
