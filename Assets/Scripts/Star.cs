using UnityEngine;

public class Star : MonoBehaviour
{
    public StarManager starManager;
    public int value = 1;
    public Animator anim;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        starManager.ChangeStarCount(value);
        anim.SetBool("IsHidden", true);
        //Destroy(gameObject);
    }
    public void DestroyStar()
    {
        Destroy(gameObject);
    }
}
