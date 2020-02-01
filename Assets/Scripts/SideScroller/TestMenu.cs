using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMenu : MonoBehaviour
{
    bool isFading;
    GameObject fadeScreen;

    // Start is called before the first frame update
    void Start()
    {
        fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        fadeScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            fadeScreen.GetComponent<CanvasGroup>().alpha += 0.01f;
        }
    }

    public void ClickPlay()
    {
        StartCoroutine(FadeToWorld());
    }

    IEnumerator FadeToWorld()
    {
        fadeScreen.SetActive(true);
        isFading = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("World1");
    }
}
