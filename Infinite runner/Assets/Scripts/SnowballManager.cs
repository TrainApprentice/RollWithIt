using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : MonoBehaviour
{
    public GameObject yeti;
    private Rigidbody rb;
    private float despawnTimer = 3f;
    private float currScale = 10f;
    private float currYPos = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        yeti = GameObject.Find("Sphere");
        transform.position = yeti.GetComponent<YetiAI>().snowballPos;
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.velocity = new Vector3(0f, 0f, 100f);
        print(rb.position);
        currYPos -= (Time.deltaTime * 5 / 6);
        currScale -= (Time.deltaTime * 5 / 3);
        transform.localScale = new Vector3(currScale, currScale, currScale);
        transform.position = new Vector3(transform.position.x, currYPos, transform.position.z);
        despawnTimer -= Time.deltaTime;

        if (despawnTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
