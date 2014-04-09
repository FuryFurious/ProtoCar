using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    /// <summary>
    /// The real game itself
    /// </summary>
    class InGame : IGameState
    {

        public InGame()
        {

        }

        public void init()
        {
          
        }

        public void loadContent(ContentManager content)
        {
            
        }

        public void unloadContent()
        {
            
        }

        public EGameState update(GameTime gameTime)
        {
            return EGameState.InGame;
        }

        public void draw(GameTime gameTime)
        {
            Game1.gManager.GraphicsDevice.Clear(SharpDX.Color.Coral);
        }
    }
}
