using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GameMechanics.Instance.SpawnCars();
            GameMechanics.Instance.AdjustHeight();
        }
    }
}
