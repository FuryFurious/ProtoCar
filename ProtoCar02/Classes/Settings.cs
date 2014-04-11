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
        //1336 x 768
        public static int windowWidth               = 800;
        public static int windowHeight              = 600;

        //how to controll:
        public static PlayerController controller1 = new PlayerArrow(); // new PlayerGamepad(SharpDX.XInput.UserIndex.One);
        public static PlayerController controller2  = new PlayerWASD();
         

        //BUG: causes mouse to flip randomly around (wrong mousePosition to reset?)
        //NOTE: put fullscreen only on, if the windowWidth and windowHeight support your screen -> otherwise: exception
        public static bool  enableFullscreen         = false;

        public static bool      enablePlayer2       = true;
        public static bool      enableNoclip        = false;

        //movementSpeed
        public static float     playerSpeed         = 0.25f;
        public static double    effectDuration      = 1.0f;
        public static float     playerSpeedUp       = 0.1f;   // 1f == full speed;
        public static float     playerBreakDown     = 0.9f;   // 1f == no break;

        //rotationSpeeds:
        public static float     mouseSpeed          = 0.75f;
        public static Vector3   gamePadSpeed        = new Vector3(0.03f, 0.03f, 0); //sometimes it feels better with different settings

        //between 0 and 1
        public static float     gamePadYawDeadZone  = 0.5f;

        //respawn settings for pointboxes:
        public static double    respawnInterval     = 5.0;
        public static float     respawnDistance     = 36.0f;







    }
}
