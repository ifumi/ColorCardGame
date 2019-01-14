using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIndicator : MonoBehaviour
{
    public Sprite RED, GREEN, BLUE, YELLOW, WILD;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetColor(ColorCard.Color color)
    {        
        switch (color)
        {
            case ColorCard.Color.RED:
                spriteRenderer.sprite = RED;
                break;
            case ColorCard.Color.BLUE:
                spriteRenderer.sprite = BLUE;
                break;
            case ColorCard.Color.GREEN:
                spriteRenderer.sprite = GREEN;
                break;
            case ColorCard.Color.YELLOW:
                spriteRenderer.sprite = YELLOW;
                break;
            case ColorCard.Color.NONE:
                spriteRenderer.sprite = WILD;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
