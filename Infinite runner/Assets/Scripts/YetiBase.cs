using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiBase : MonoBehaviour
{
    public GameObject other;
    public Rigidbody otherRB;
    

    // Start is called before the first frame update
    void Start()
    {
        
        otherRB = other.GetComponent<Rigidbody>();
       
    }

    // Update is called once per physics calculation
    void FixedUpdate()
    {
        transform.position = new Vector3(other.transform.position.x, other.transform.position.y + (other.transform.localScale.y / 2f), other.transform.position.z);

        
    }
}
