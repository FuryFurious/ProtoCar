using System;
using System.Collections.Generic;

namespace ProtoCar
{
    /// <summary>
    /// Simple ProtoCar02 application using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var program = new Game1())
                program.Run();

        }
    }
}