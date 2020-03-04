using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public GameObject yeti;
    private Vector3 centerPoint = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        yeti = GameObject.Find("Sphere (1)");
    }

    // Update is called once per frame
    void Update()
    {
        centerPoint = yeti.GetComponent<YetiAI>().lanePoint;
        transform.position = centerPoint;
        
        if (!yeti.GetComponent<YetiAI>().laneSet)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Shrink()
    {
        for (float ft = 3f; ft >= 0; ft -= Time.deltaTime)
        {
            //print(ft);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
