using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{

    /// <summary>
    /// Stores tweakable variables
    /// </summary>
    public static class Settings
    {
        public static int windowWidth               = 800;
        public static int windowHeight              = 600;

        //BUG: causes mouse to flip randomly around (wrong mousePosition to reset?)
        public static bool enableFullscreen         = false;

        public static bool enablePlayer2            = false;
        public static bool enableNoclip             = false;

        //movementSpeed
        public static float     playerSpeed         = 0.25f;

        //rotationSpeeds:
        public static float     mouseSpeed          = 0.75f;
        public static Vector3   gamePadSpeed        = new Vector3(0.03f, 0.03f, 0);

        public static float     gamePadYawDeadZone  = 0.0f;

        //respawn settings for pointboxes:
        public static double    respawnInterval     = 5.0;
        public static float     respawnDistance     = 36.0f;








    }
}
