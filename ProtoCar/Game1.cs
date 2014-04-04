using SharpDX.Toolkit;
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

        public Game1()
        {
            this.gManager = new GraphicsDeviceManager(this);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
