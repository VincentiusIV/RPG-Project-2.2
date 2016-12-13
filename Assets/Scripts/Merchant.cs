using UnityEngine;
using System.Collections;

public class Merchant : MonoBehaviour
{
    Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
	
}
