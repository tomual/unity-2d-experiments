using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestUIController : MonoBehaviour
{
    public static TestUIController instance;
    bool isFadingIn;
    bool isFadingOut;
    bool isTeleporting;
    GameObject fadeScreen;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (this != instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");

        StartCoroutine(FadeIn());
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            fadeScreen.GetComponent<CanvasGroup>().alpha -= 0.05f;
        }
        if (isFadingOut)
        {
            fadeScreen.GetComponent<CanvasGroup>().alpha += 0.05f;
        }
    }

    public void TriggerTeleport(string destination)
    {
        if (!isTeleporting)
        {
            StartCoroutine(ChangeScene(destination));
        }
    }

    IEnumerator FadeIn()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<CanvasGroup>().alpha = 1;
        isFadingIn = true;
        yield return new WaitForSeconds(0.75f);
        fadeScreen.SetActive(false);
        isFadingIn = false;
    }

    IEnumerator FadeOut()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<CanvasGroup>().alpha = 0;
        isFadingOut = true;
        yield return new WaitForSeconds(1f);
        fadeScreen.SetActive(false);
        isFadingOut = false;
    }
    
    IEnumerator ChangeScene(string destination)
    {
        isTeleporting = true;
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeIn());
        SceneManager.LoadScene(destination);
        isTeleporting = false;
    }

    public void GameOver()
    {
        if (!isTeleporting)
        {
            StartCoroutine(ChangeScene("World1"));
        }
    }
}
