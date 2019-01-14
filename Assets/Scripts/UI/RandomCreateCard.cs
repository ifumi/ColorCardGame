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
        int random = Random.Range(0, 4);
        
        switch(random)
        {
            case 0:
                GetComponent<Image>().sprite = RED;
                break;
            case 1:
                GetComponent<Image>().sprite = GREEN;
                break;
            case 2:
                GetComponent<Image>().sprite = BLUE;
                break;
            case 3:
                GetComponent<Image>().sprite = YELLOW;
                break;
        }
    }
}
