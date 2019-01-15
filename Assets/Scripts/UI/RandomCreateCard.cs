using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCreateCard : MonoBehaviour
{
    public Sprite RED, GREEN, BLUE, YELLOW;

    // Start is called before the first frame update
    void Start()
    {
        Text myText = gameObject.GetComponentInChildren<Text>();
        int random = Random.Range(0, 4);
        
        switch(random)
        {
            case 0:
                GetComponent<Image>().sprite = RED;
                if (myText != null) myText.color = new Color(241f / 255.0f, 56f / 255.0f, 56f / 255.0f);
                break;
            case 1:
                GetComponent<Image>().sprite = GREEN;
                if (myText != null) myText.color = new Color(101f / 255.0f, 199f / 255.0f, 61f / 255.0f);
                break;
            case 2:
                GetComponent<Image>().sprite = BLUE;
                if (myText != null) myText.color = new Color(63f / 255.0f, 116f / 255.0f, 196f / 255.0f);
                break;
            case 3:
                GetComponent<Image>().sprite = YELLOW;
                if (myText != null) myText.color = new Color(254f / 255.0f, 231f / 255.0f, 26f / 255.0f);
                break;
        }

    }
}
