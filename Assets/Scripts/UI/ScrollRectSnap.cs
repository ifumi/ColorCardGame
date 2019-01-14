using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    private readonly float LERP_SPEED = 20f;

    public RectTransform panel; // To hold the ScrollPanel
    public Image[] images;
    public RectTransform center; // Center to compare the distance for each button

    private float[] distance;   // All buttons distance to the center
    private bool dragging = false; // Will be true, while we drag the panel
    private int imageDistance;
    private int minImageNum; // Hold the number of the image with smallest distance to the center


    // Start is called before the first frame update
    void Start()
    {
        int imageLength = images.Length;
        distance = new float[imageLength];

        imageDistance = (int) Mathf.Abs(images[1].GetComponent<RectTransform>().anchoredPosition.x - images[0].GetComponent<RectTransform>().anchoredPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < images.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - images[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance);

        for (int a = 0; a < images.Length; a++)
        {
            if (minDistance == distance[a])
            {
                minImageNum = a;
            }
        }

        if (!dragging)
        {
            LerpToImage(minImageNum * -imageDistance);
        }

    }

    private void LerpToImage(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * LERP_SPEED);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public int GetSelectedImage()
    {
        return minImageNum;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }
}
