
using UnityEngine;
using UnityEngine.UI; 

public class StopButton : MonoBehaviour
{
    public void StopClicked()
    {
        FindObjectOfType<CustomNetworkManager>().StopHosting();
        FindObjectOfType<ConnectionDiscovery>().Stop();
    }
}
