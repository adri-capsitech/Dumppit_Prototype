using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject tutorial_for_coins;
    public GameObject tutorial_for_swing;
    public GameObject tutorial_for_platform;
    int counter = 0;
    void Start()
    {
        tutorial_for_coins.SetActive(true);
        StartCoroutine(SwitchOffTutorials());
    }

    // Update is called once per frame
    void Update()
    {
        PowerUpsTutorials();
    }
    IEnumerator SwitchOffTutorials()
    {
        yield return new WaitForSeconds(2f);
        tutorial_for_coins.SetActive(false);
        tutorial_for_platform.SetActive(false);
        tutorial_for_swing.SetActive(false);
    }
    void PowerUpsTutorials()
    {
        int Coins =DataManager.Instance.GetCurrentCoins();
        if((Coins == 20) && (counter < 1))
        {
            tutorial_for_platform.SetActive(true);
            counter++;
            StartCoroutine(SwitchOffTutorials());
        }
        if((Coins >= 30) && (counter < 2))
        {
            tutorial_for_swing.SetActive(true);
            counter++;
            StartCoroutine(SwitchOffTutorials());
        }

    }

}
