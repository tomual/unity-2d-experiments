using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : EventTrigger
{

    private bool dragging;
    float offsetX;
    float offsetY;

    public void Update()
    {
        if (dragging)
        {
            transform.parent.transform.position = new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        Vector3 offset = Input.mousePosition - transform.parent.transform.position;
        offsetX = offset.x;
        offsetY = offset.y;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}