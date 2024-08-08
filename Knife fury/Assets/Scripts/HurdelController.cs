using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurdelController : MonoBehaviour
{
    public GameObject Hurdle;
    private void Start()
    {
        Instantiate(Hurdle, new Vector3(0f, -0.67f, 0f), Quaternion.identity);

    }
 
    
}
