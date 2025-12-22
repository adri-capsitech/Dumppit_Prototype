using System.Collections.Generic;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    public bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Platform") || collision.collider.CompareTag("Car"))
        {
            GameMechanics.Instance.SpawnCars();
           // GameMechanics.Instance.OnCarLanded(gameObject);
        }
    }




}

