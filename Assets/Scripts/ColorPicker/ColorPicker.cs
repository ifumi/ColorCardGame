using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    private ColorCard.Color currentColor = ColorCard.Color.NONE;

    public void Show()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = true;
            child.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    public void Hide()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = false;
            child.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public ColorCard.Color GetPickedColor()
    {
        return currentColor;
    }

    public void Reset()
    {
        currentColor = ColorCard.Color.NONE;
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                FireRayCast(Input.GetTouch(i).position);
            }
        }

        // FOR TESTING
        if (Input.GetMouseButtonDown(0))
        {
            FireRayCast(Input.mousePosition);
        }

    }

    void FireRayCast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            switch (hit.transform.gameObject.name)
            {
                case "red_pick":
                    currentColor = ColorCard.Color.RED;
                    break;
                case "green_pick":
                    currentColor = ColorCard.Color.GREEN;
                    break;
                case "blue_pick":
                    currentColor = ColorCard.Color.BLUE;
                    break;
                case "yellow_pick":
                    currentColor = ColorCard.Color.YELLOW;
                    break;
                default:
                    break;
            }
            // Here you can check hitInfo to see which collider has been hit, and act appropriately.
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
    }
}
