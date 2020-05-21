using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    bool fadeIn = false;
    bool fadeOut = false;

    CanvasGroup fadeScreen;

    string portalDestination;

    Slider hp;
    Slider mp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        fadeScreen = GameObject.Find("FadeScreen").GetComponent<CanvasGroup>();
        StartCoroutine(DelayEnter());

        hp = GameObject.Find("SliderHP").GetComponent<Slider>();
        mp = GameObject.Find("SliderMP").GetComponent<Slider>();
    }

    void Update()
    {
        CheckFadeIn();
        CheckFadeOut();
    }

    void CheckFadeIn()
    {
        if (fadeIn)
        {
            fadeScreen.alpha += 0.05f;
            if (fadeScreen.alpha >= 1)
            {
                fadeScreen.alpha = 1;
                fadeIn = false;
            }
        }
    }

    void CheckFadeOut()
    {
        if (fadeOut)
        {
            fadeScreen.alpha -= 0.05f;
            if (fadeScreen.alpha <= 0)
            {
                fadeScreen.alpha = 0;
                fadeOut = false;
            }
        }
    }

    public void Portal(string destination)
    {
        portalDestination = destination;
        StartCoroutine(DelayPortal());
    }

    IEnumerator DelayEnter()
    {
        yield return new WaitForSeconds(0.2f);
        fadeOut = true;
    }

    IEnumerator DelayPortal()
    {
        fadeIn = true;
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(portalDestination);
        yield return new WaitForSeconds(0.2f);
        fadeOut = true;
    }

    public void InitHP(int maxHp)
    {
        hp.maxValue = maxHp;
    }

    public void InitMP(int maxMp)
    {
        mp.maxValue = maxMp;
    }

    public void UpdateHP(int value)
    {
        hp.value = value;
    }

    public void UpdateMP(int value)
    {
        mp.value = value;
    }
}
