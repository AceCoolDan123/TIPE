using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFluid : MonoBehaviour
{

    public void DisplayTexture(Texture2D texture)
    {
        RawImage rawImage = gameObject.GetComponent<RawImage>();

        if (rawImage == null)
        {
            Debug.LogError("RawImage component not found on this GameObject.");
            return;
        }

        rawImage.texture = texture;
    }
}

