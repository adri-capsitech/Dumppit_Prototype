using System.Collections.Generic;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    public bool hasLanded = false;

    void Update()
    {
        if (gameObject.transform.position.y < -5.5)
        {
            GameMechanics.Instance.isGameOver = true;
            GameMechanics.Instance.GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded)
        {
            return;
        }

        if (collision.collider.CompareTag("Platform") || collision.collider.CompareTag("Car"))
        {
            hasLanded = true;
         
            GameMechanics.Instance.SpawnCars();
            DataManager.Instance.UpdateScore();
            int score = DataManager.Instance.GetCurrentScore();
            Debug.Log("Score is - " + score);
        }
    }


}

