using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindButton : MonoBehaviour
{
    public void findClicked()
    {
        FindObjectOfType<ConnectionDiscovery>().StartListening();
    }
}
