using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartMeneger : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public FloatValue heartConteniers;
    public FloatValue playerCurrentStateHealth;

    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for(int i = 0; i <heartConteniers.runtimeValue; i++)
        {
            if(i < hearts.Length)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeart;
            }
        }
    }

    public void UpdateHearts()
    {
        InitHearts();
        float tempHealth = playerCurrentStateHealth.runtimeValue / 2;
        for(int i = 0; i <heartConteniers.runtimeValue; i++)
        {
            if(i <= tempHealth - 1)
            {
                hearts[i].sprite = fullHeart;
            }
            else if(i >= tempHealth)
            {
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                hearts[i].sprite = halfHeart;
            }
        }
    }
}
