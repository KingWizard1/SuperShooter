using UnityEngine;

namespace SuperShooter
{
    public class HealthBox : Consumable
    {

        [Header("Health Box")]
        public int amount = 10;


        public override void Consume(PlayerCharacter player)
        {
            base.Consume(player);
            player.AddHealth(amount);
        }

    }
}
