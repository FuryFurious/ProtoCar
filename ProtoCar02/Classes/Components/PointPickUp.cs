using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    class PointPickUp : APickUp
    {
        public PointPickUp(Vector3 position)
            : base(position)
        {

        }

        public override void onHit(Player player)
        {
            player.addPoints(1);
            Game1.soundHit.Play();
        }
    }
}
