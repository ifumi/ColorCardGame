using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    public Sprite ACTIVE, INACTIVE;

    public void SetActive(bool active)
    {
        if (active)
        {
            GetComponent<Image>().sprite = ACTIVE;
        } else
        {
            GetComponent<Image>().sprite = INACTIVE;
        }
    }

}
