using UnityEngine;

namespace SuperShooter
{
    public class AmmoBox : Consumable
    {

        [Header("Ammo Box")]
        public int amount = 25;


        public override void Consume(PlayerCharacter player)
        {
            base.Consume(player);
            player.AddAmmo(amount);
        }

    }
}
