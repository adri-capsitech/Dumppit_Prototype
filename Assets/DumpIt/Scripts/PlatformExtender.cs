
// using UnityEngine;
 
// public class PlatformExtender : MonoBehaviour
// {
//     public static PlatformExtender Instance { get; private set; }
//     Vector3 scale;
//     Vector3 position;
//     [SerializeField] float extendAmount = 1f;
//     private bool canExtend = false;
//     private Vector3 originalScale;
//     private Vector3 originalPosition;
//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//        transform.localScale = originalScale;
//        transform.position = originalPosition;
//     }
//     public void ExtendRight()
//     {
//         Extend(Vector3.right);
//     }
 
//     public void ExtendLeft()
//     {
//         Extend(Vector3.left);
//     }
 
//     public void ExtendForward()
//     {
//         Extend(Vector3.forward);
//     }
 
//     public void ExtendBack()
//     {
//         Extend(Vector3.back);
//     }
 
//     void Extend(Vector3 direction)
//     {
//         Vector3 scale = transform.localScale;
//         Vector3 position = transform.position;
 
//         if (direction.x != 0)
//         {
//             scale.x += extendAmount;
//             position.x += direction.x * (extendAmount / 2f);
//         }
//         else if (direction.z != 0)
//         {
//             scale.z += extendAmount;
//             position.z += direction.z * (extendAmount / 2f);
//         }
 
//         transform.localScale = scale;
//         transform.position = position;
//     }
//     public void ResetPlatform()
//     {
//         transform.localScale = originalScale;
//         transform.position = originalPosition;
//         canExtend = false;

//         Debug.Log("Platform reset to original state");
//     }
// }
using UnityEngine;

public class PlatformExtender : MonoBehaviour
{
    public static PlatformExtender Instance { get; private set; }
    Vector3 originalScale;
    Vector3 originalPosition;
    [SerializeField] float extendAmount = 1f;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        originalScale = gameObject.transform.localScale;
        originalPosition = gameObject.transform.position;
        transform.localScale = originalScale;
        transform.position = originalPosition; 
    }
    public void ExtendRight()
    {
        Extend(Vector3.right);
    }

    public void ExtendLeft()
    {
        Extend(Vector3.left);
    }

    public void ExtendForward()
    {
        Extend(Vector3.forward);
    }

    public void ExtendBack()
    {
        Extend(Vector3.back);
    }

    void Extend(Vector3 direction)
    {
        Vector3 scale = transform.localScale;
        Vector3 position = transform.position;

        if (direction.x != 0)
        {
            scale.x += extendAmount;
            position.x += direction.x * (extendAmount / 2f);
        }
        else if (direction.z != 0)
        {
            scale.z += extendAmount;
            position.z += direction.z * (extendAmount / 2f);
        }

        transform.localScale = scale;
        transform.position = position;
    }
    public void ResetPlatform()
    {
        
       transform.localScale = originalScale;
       transform.position = originalPosition; 
    }
}