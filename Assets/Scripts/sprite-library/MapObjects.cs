using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class MapObjects : MonoBehaviour
{
    void Start()
    {
        int layer = (int)(transform.position.y * 10);
        layer *= 100;

        SpriteRenderer spriteRenderers = GetComponent<SpriteRenderer>();
        int baseLayer = spriteRenderers.sortingOrder % 100;
        spriteRenderers.sortingOrder = baseLayer + layer * -1;
    }
}
