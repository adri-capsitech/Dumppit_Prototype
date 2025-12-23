using System.Collections.Generic;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    public bool hasLanded = false;

    void Update()
    {
        if (gameObject.transform.position.y < -0.5)
        {
            GameMechanics.Instance.isGameOver = true;
            GameMechanics.Instance.GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (collision.collider.CompareTag("Platform") || collision.collider.CompareTag("Car"))
        {
            hasLanded = true;

            GameMechanics.Instance.SpawnCars();
            DataManager.Instance.UpdateScore();
            int score = DataManager.Instance.GetCurrentScore();
            Debug.Log("Score is - " + score);

            int coins = DataManager.Instance.GetCurrentCoins();
            if (coins >= 20)
            {
                Debug.Log(" YOu get to use platform extender");
            }
            else if (coins >= 30)
            {
                Debug.Log("You get to change the direction of swing");
            }

        }
    }


}

