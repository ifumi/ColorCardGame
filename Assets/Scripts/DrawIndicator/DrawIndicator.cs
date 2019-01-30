using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawIndicator : MonoBehaviour
{
    public void Show()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
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
                case "CardStack":
                    Player player = GameObject.Find("Player").GetComponent<Player>();
                    if (player.MustDrawBeforePlaying() && player.HasTurn())
                    {
                        player.DrawCards();
                        
                        Hide();
                    }                      
                    break;
                default:
                    break;
            }
            // Here you can check hitInfo to see which collider has been hit, and act appropriately.
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
    }
}
