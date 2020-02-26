using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour
{
    public GameObject yeti;
    private Vector3 centerPoint = new Vector3(0, 0, 0);
    private Color matColor;
    //private float alphaValue = 1f;

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
        
        //if (alphaValue > 0f) alphaValue -= 0.036f;
        //else alphaValue = 0f;
        GetComponent<Renderer>().material.SetColor("_Color", matColor);
        transform.localScale += new Vector3(.4f, .4f, .4f);
        
        //print(alphaValue);
        if (transform.localScale.x >= 12f)
        {
            //alphaValue = 1;
            print("bleh");
            Destroy(gameObject);
            StopCoroutine("Fade");

        }
    }
    IEnumerator Fade()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.036f)
        {
            print(ft);
            Color c = matColor;
            c.a = ft;
            matColor = c;
            yield return new WaitForFixedUpdate();
        }
    }
}
