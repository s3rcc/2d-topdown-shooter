using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts
{
    public class Exp : MonoBehaviour
    {
        public int Amount { get; private set; } // Public property to store EXP amount

        public void Initialize(int amount) // New method to set the amount after instantiation
        {
            Amount = amount;
        }

        public static int GetExpDrop()
        {
            float randomValue = UnityEngine.Random.value;
            if (randomValue <= 0.1f)
                return 7; // 10% chance for 7 EXP
            else if (randomValue <= 0.4f)
                return 3; // 30% chance for 3 EXP
            else
                return 1; // 70% chance for 1 EXP
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.CollectExp(this.Amount);
                Destroy(gameObject);
            }   
        }
    }


}
