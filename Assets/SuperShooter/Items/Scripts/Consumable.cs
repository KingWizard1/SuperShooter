using System.Collections;
using UnityEngine;

namespace SuperShooter
{
    public interface IConsumable
    {

        void Consume(PlayerCharacter target);

    }

    public enum ConsumableConsumedAction
    {
        DestroyObject = 0,
        RespawnAfterTime = 1,
    }

    [RequireComponent(typeof(Collider))]
    public class Consumable : MonoBehaviour, IConsumable
    {
        [Header("Consumable")]
        //public bool isConsumed = false;

        public float respawnTime = 5;

        public ConsumableConsumedAction actionWhenConsumed = ConsumableConsumedAction.RespawnAfterTime;

        private Collider col;
        private Renderer rend;

        private void Awake()
        {
            col = GetComponent<Collider>();
            rend = GetComponent<Renderer>();
        }

        public virtual void Consume(PlayerCharacter target)
        {
            switch (actionWhenConsumed)
            {
                default:
                case ConsumableConsumedAction.DestroyObject:
                    Destroy(gameObject);
                    break;
                case ConsumableConsumedAction.RespawnAfterTime:
                    StartCoroutine(respawn());
                    break;
            }
        }

        private IEnumerator respawn()
        {
            showhide(false);
            yield return new WaitForSeconds(respawnTime);
            showhide(true);
        }
        
        private void showhide(bool show)
        {
            col.enabled = show;
            rend.enabled = show;
        }

        private void OnTriggerEnter(Collider other)
        {

            var pc = other.gameObject.GetComponentInParent<PlayerCharacter>() ??
                     other.gameObject.GetComponentInChildren<PlayerCharacter>();

            if (pc == null)
                return;

            if (pc.isDead)
                return;

            // Give ourselves willingly
            Consume(pc);

        }

    }
}
