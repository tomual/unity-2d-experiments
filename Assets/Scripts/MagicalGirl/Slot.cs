using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSlot()
    {
        Debug.Log("Slot #" + index + " clicked");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount == 1)
            OnSingleClick();
        else if (clickCount == 2)
            OnDoubleClick();
        else if (clickCount > 2)
            OnMultiClick();
    }

    void OnSingleClick()
    {
        Debug.Log("Single Clicked");
    }

    void OnDoubleClick()
    {
        Debug.Log("Double Clicked");
    }

    void OnMultiClick()
    {
        Debug.Log("MultiClick Clicked");
    }
}
