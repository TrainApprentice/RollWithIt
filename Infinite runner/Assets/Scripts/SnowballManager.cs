using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : MonoBehaviour
{
    public GameObject yeti;
    private Rigidbody rb;
    private float despawnTimer = 3f;
    private float currScale = 10f;
    private float currYPos = 18f;
    private Color matColor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        yeti = GameObject.Find("Sphere (1)");
        transform.position = yeti.GetComponent<YetiAI>().snowballPos;
        matColor = GetComponent<Renderer>().material.color;
        StartCoroutine("Fade");
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.velocity = new Vector3(0f, 0f, 100f);
        //print(rb.position);
        currYPos -= (Time.deltaTime * 5 / 6);
        currScale -= (Time.deltaTime * 5 / 3);
        transform.localScale = new Vector3(currScale, currScale, currScale);
        transform.position = new Vector3(transform.position.x, currYPos, transform.position.z);
        despawnTimer -= Time.deltaTime;
        GetComponent<Renderer>().material.SetColor("_Color", matColor);

        if (despawnTimer <= 0)
        {
            StopCoroutine("Fade");
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            
            other.gameObject.SetActive(false);
            
        }
        
    }
    IEnumerator Fade()
    {
        for (float ft = 2f; ft >= 0; ft -= Time.deltaTime)
        {
            if (ft <= .5)
            {
                Color c = matColor;
                c.a = ft * 2;
                matColor = c;
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}
