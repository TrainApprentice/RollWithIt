using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour
{

    public GameObject yeti;
    private Vector3 centerPoint = new Vector3(0, 0, 0);
    
    
    // Start is called before the first frame update
    void Start()
        
    {
        yeti = GameObject.Find("Sphere (1)");
        centerPoint = yeti.GetComponent<YetiAI>().jumpPoint;
    }

    // Update is called once per frame
    void Update()
    {
        //print(transform.position);
        centerPoint = yeti.GetComponent<YetiAI>().jumpPoint;
        transform.position = centerPoint;
        if (yeti.GetComponent<YetiAI>().JumpLanded)
        {
            Destroy(gameObject);
        }
    }
}
