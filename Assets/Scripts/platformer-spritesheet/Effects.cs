using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects instance;
    Dictionary<string, GameObject> effectsDictionary;

    void Awake()
    {
        if (instance != null)
        {

            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);

        PopulateDictionary();
    }

    void PopulateDictionary()
    {
        effectsDictionary = new Dictionary<string, GameObject>();
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject item = transform.GetChild(i).gameObject;
            effectsDictionary.Add(item.name, item);
        }
    }

    public void Play(string effectName, Vector2 location, int direction)
    {
        Debug.Log(direction);
        Quaternion rotation = Quaternion.Euler(0, 0, -45);
        if (direction < 0)
        {
            rotation = Quaternion.Euler(0, 0, -135);
        }
        GameObject item = Instantiate(effectsDictionary[effectName], new Vector3(location.x, location.y, -1), rotation);
        item.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyEffect(item, item.GetComponent<ParticleSystem>().main.duration));
    }

    IEnumerator DestroyEffect(GameObject item, float duration)
    {
        yield return new WaitForSeconds(duration + 1f);
        Destroy(item);
    }

    void Update()
    {
        
    }
}
