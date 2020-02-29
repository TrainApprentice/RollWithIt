using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour
{
    public GameObject yeti;
    private Vector3 centerPoint = new Vector3(0, 0, 0);
    private Color matColor;
    

    // Start is called before the first frame update
    void Start()
    {
        yeti = GameObject.Find("Sphere");
        centerPoint = yeti.GetComponent<YetiAI>().transform.position;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = centerPoint;
        matColor = GetComponent<Renderer>().material.color;
        StartCoroutine("Fade");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        GetComponent<Renderer>().material.SetColor("_Color", matColor);
        transform.localScale += new Vector3(.6f, .6f, .6f);
        
        
        if (transform.localScale.x >= 18f)
        {

            Destroy(gameObject);
            StopCoroutine("Fade");

        }

    }
    IEnumerator Fade()
    {
        for (float ft = 1f; ft >= 0; ft -= Time.deltaTime)
        {
           
            Color c = matColor;
            c.a = ft;
            matColor = c;
            yield return new WaitForFixedUpdate();
        }
    }
}
