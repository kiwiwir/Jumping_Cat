using UnityEngine;

public class Star : MonoBehaviour
{
    public StarManager starManager;
    public int value = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        starManager.ChangeStarCount(value);
        Destroy(gameObject);
    }
}
