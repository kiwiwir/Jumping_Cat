using TMPro;
using UnityEngine;
using TMPro;

public class StarManager : MonoBehaviour
{
    public int totalStars;
    public TMP_Text starText;

    void Start()
    {
        starText.text = "Stars: " + totalStars;
    }

    public void ChangeStarCount(int amount)
    {
        totalStars += amount;
        starText.text = "Stars: " + totalStars;
    }
}
