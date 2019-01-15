using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingServerButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!FindObjectOfType<CustomNetworkManager>().isHosting) {
            gameObject.SetActive(false);
        }
    }
}
