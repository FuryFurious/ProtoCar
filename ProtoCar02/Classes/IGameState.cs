using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    public enum EGameState {None, Sandbox, InGame };

    interface IGameState
    {
        void init();
        void loadContent(SharpDX.Toolkit.Content.ContentManager content);
        void unloadContent();
        EGameState update(GameTime gameTime);
        void draw(GameTime gameTime);
    }
}
