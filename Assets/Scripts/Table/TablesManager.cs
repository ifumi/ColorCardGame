using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablesManager : MonoBehaviour
{
    
    public Table myTable, player2, player3, player4;

    // Start is called before the first frame update
    void Start()
    {
        SetAllInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTables(int count, string[] names, int myPlayerIndex)
    {
        SetAllInvisible();
        switch(count)
        {
            case 4:
                player4.GetComponent<Image>().enabled = true;
                player4.GetComponentInChildren<Text>().enabled = true;
                player4.GetComponentInChildren<Text>().text = names[(myPlayerIndex + 3) % count];
                goto case 3;
            case 3:
                player3.GetComponent<Image>().enabled = true;
                player3.GetComponentInChildren<Text>().enabled = true;
                player3.GetComponentInChildren<Text>().text = names[(myPlayerIndex + 2) % count];
                goto case 2;
            case 2:
                player2.GetComponent<Image>().enabled = true;
                player2.GetComponentInChildren<Text>().enabled = true;
                player2.GetComponentInChildren<Text>().text = names[(myPlayerIndex + 1) % count];
                goto case 1;
            case 1:
                myTable.GetComponent<Image>().enabled = true;
                myTable.GetComponentInChildren<Text>().enabled = true;
                myTable.GetComponentInChildren<Text>().text = names[myPlayerIndex];
                break;
        }
    }

    public void SetTableActive(string name)
    {
        SetAllInactive();
        if (myTable.GetComponentInChildren<Text>().text == name)
        {
            myTable.SetActive(true);
        }
        if (player2.GetComponentInChildren<Text>().text == name)
        {
            player2.SetActive(true);
        }
        if (player3.GetComponentInChildren<Text>().text == name)
        {
            player3.SetActive(true);
        }
        if (player4.GetComponentInChildren<Text>().text == name)
        {
            player4.SetActive(true);
        }
    }

    private void SetAllInactive()
    {
        myTable.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
    }

    private void SetAllInvisible()
    {
        myTable.GetComponent<Image>().enabled = false;
        player2.GetComponent<Image>().enabled = false;
        player3.GetComponent<Image>().enabled = false;
        player4.GetComponent<Image>().enabled = false;

        myTable.GetComponentInChildren<Text>().enabled = false;
        player2.GetComponentInChildren<Text>().enabled = false;
        player3.GetComponentInChildren<Text>().enabled = false;
        player4.GetComponentInChildren<Text>().enabled = false;
    }

    private void SetAllVisible()
    {
        myTable.GetComponent<Image>().enabled = true;
        player2.GetComponent<Image>().enabled = true;
        player3.GetComponent<Image>().enabled = true;
        player4.GetComponent<Image>().enabled = true;

        myTable.GetComponentInChildren<Text>().enabled = true;
        player2.GetComponentInChildren<Text>().enabled = true;
        player3.GetComponentInChildren<Text>().enabled = true;
        player4.GetComponentInChildren<Text>().enabled = true;
    }



}
