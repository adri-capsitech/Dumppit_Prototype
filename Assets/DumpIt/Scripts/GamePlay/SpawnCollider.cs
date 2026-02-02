// using UnityEngine;

// public class SpawnCollider : MonoBehaviour
// {
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Car"))
//         {
//             GameMechanics.Instance.SpawnCars();
//             GameMechanics.Instance.AdjustHeight();
//         }
//     }
// }


using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    private bool heightAdjusted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (heightAdjusted) return;

        if (other.CompareTag("Car"))
        {
            heightAdjusted = true;

            GameMechanics.Instance.AdjustHeight();
            GameMechanics.Instance.SpawnCars();
        }
    }

    // Reset when a new car is spawned / new level starts
    public void ResetTrigger()
    {
        heightAdjusted = false;
    }
}
