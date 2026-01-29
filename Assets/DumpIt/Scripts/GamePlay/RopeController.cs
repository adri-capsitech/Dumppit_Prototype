using UnityEngine;

public class RopeController : MonoBehaviour
{
    public GameObject rope;

    public void DisableRope()
    {
        rope.SetActive(false);
    }

    public void EnableRope()
    {
        rope.SetActive(true);
    }
}
