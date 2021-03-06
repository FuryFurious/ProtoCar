﻿using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    public enum EGameState {None, MainMenu, GameOver, Sandbox, GameStateCount};

    interface IGameState
    {
        void init();
        EGameState update(GameTime gameTime);
        void draw(GameTime gameTime);
    }
}
