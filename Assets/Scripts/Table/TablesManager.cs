using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablesManager : MonoBehaviour
{

    public Sprite ACTIVE, INACTIVE;

    // Start is called before the first frame update
    void Start()
    {
        SetAllInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTables(int count)
    {
        switch(count)
        {
            case 1:
                SetVisible("MyTable");
                break;
            case 2:
                SetVisible("MyTable");
                SetVisible("Player2");
                break;
            case 3:
                SetVisible("MyTable");
                SetVisible("Player2");
                SetVisible("Player3");
                break;
            case 4:
                SetVisible("MyTable");
                SetVisible("Player2");
                SetVisible("Player3");
                SetVisible("Player4");
                break;
        }

    }

    private void SetVisible(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == name)
                child.GetComponent<Image>().enabled = true;
        }
    }

    private void SetAllInvisible()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
    }

    private void SetAllVisible()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().enabled = true;
        }
    }


}
