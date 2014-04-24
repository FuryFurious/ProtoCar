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
        //1366 x 768
        public static int windowWidth               = 1366;
        public static int windowHeight              = 768;

        //how to controll:
        public static PlayerController controller1  = new PlayerArrow();//new PlayerGamepad(SharpDX.XInput.UserIndex.One);
        public static PlayerController controller2  = new PlayerWASD();//new PlayerWASD();//new PlayerGamepad(SharpDX.XInput.UserIndex.Two);
         
        //NOTE: put fullscreen only on, if the windowWidth and windowHeight support your screen -> otherwise: exception
        //activate fullscreen with F1 ingame
        public static bool      enableFullscreen    = false;

        public static bool      enablePlayer2       = true;
        public static bool      enableNoclip        = false;

        //movementSpeed
        public static float     playerSpeed         = 0.25f;
        public static float     playerSpeedUp       = 0.1f;   // 1.0f == full speed;
        public static float     playerBreakDown     = 0.9f;   // 1.0f == no break;

        //rotationSpeeds:
        public static float     mouseSpeed          = 0.75f;
        public static Vector3   gamePadSpeed        = new Vector3(0.1f, 0.1f, 0); //sometimes it feels better with different x and y, z is not used

        //Camera Zoom
        public static float     zoomSpeed           = 0.1f;
        public static float     maxZoomIn           = 1.5f;
        public static float     maxZoomOut          = 10.0f;

        //between 0 and 1
        public static float     gamePadYawDeadZone  = 0.5f;

        //respawn settings for pointboxes:
        public static double    respawnInterval     = 5.0;
        public static float     respawnDistance     = 36.0f;

        //energy (in seconds) when on item pickup
        public static double    energyRegeneration  = 0.0015; //per tick
        public static double    receivedEnergy      = 0.5; //picking up item
        public static double    shootCost           = 0.2; //per click
        public static double    boostCost           = 0.01; //per tick


        public static double    effectDuration      = 1.0f; //duration of being hit
        public static int       numStarParticles    = 50;
        public static int       numSmokeParticles   = 3;

        public static double    roundDuration       = 2.0;







    }
}
