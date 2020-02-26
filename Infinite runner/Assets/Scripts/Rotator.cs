using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    Vector3 rate = new Vector3(0f, 135f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rotates the power up
        transform.Rotate(rate * Time.deltaTime);
    }
}
