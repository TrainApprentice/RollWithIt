using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int platMax = 20;
    public int obstacleMax = 20;
    public GameObject patch;
    public GameObject playerRef;
    List<GameObject> goList;
    List<GameObject> obstacleList;
    private GameObject randSpawn;

    // All obstacle prefabs
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    public GameObject spruce;
    public GameObject fir;
    public GameObject log1;
    public GameObject log2;
    public GameObject log3;
    public GameObject acorn;
    public GameObject pinecone;

    // Start is called before the first frame update
    void Start()
    {
        goList = new List<GameObject>();
        obstacleList = new List<GameObject>();
        Vector3 pos = playerRef.transform.position;
        GameObject spawn = Instantiate(patch, pos, Quaternion.identity);
        goList.Add(spawn);
        

        for(int i = 0; i < platMax; i++)
        {
            Vector3 mainPos = goList[i].transform.Find("Connector").position;
            GameObject spawnMany = Instantiate(patch, mainPos, Quaternion.identity);
            goList.Add(spawnMany);
            CreateObstacles(obstacleMax, spawnMany);
            //print("GG" + i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = playerRef.transform.position;
        //print(playerPos.z + "          " + goList[0].transform.Find("Connector").position.z);
        if (playerPos.z >= goList[0].transform.Find("Connector").position.z)
        {
            Remove(goList[0]);
            //spawn new guy
            Vector3 mainPos = goList[platMax-1].transform.Find("Connector").position;
            GameObject spawnMany = Instantiate(patch, mainPos, Quaternion.identity);
            goList.Add(spawnMany);
        }
        for (int i = 0; i < obstacleList.Count; i++)
        {
            if (playerPos.z >= obstacleList[i].transform.position.z + 20)
            {
                RemoveObstacles(obstacleList[i]);
            }    
        }
        //CreateObstacles(goList.Count * obstacleMax);
    }

   private void Remove(GameObject patch)
    {
        goList.Remove(patch);
        Destroy(patch);
    }
    private void CreateObstacles(int obstacleMax, GameObject patch)
    {
        
        for (int i = 0; i < obstacleMax; i++)
        {
            int obstacleType = Random.Range(0, 9);
            float powerupChance = Random.Range(0f, 1f);
            if (powerupChance <= 0.02f)
            {
                float powerupChoice = Random.Range(0f, 1f);
                if (powerupChoice <= .49) randSpawn = pinecone;
                else randSpawn = acorn;
            }
            else
            {
                switch (obstacleType)
                {
                    case 0:
                        randSpawn = log1;
                        break;
                    case 1:
                        randSpawn = rock1;
                        break;
                    case 2:
                        randSpawn = rock2;
                        break;
                    case 3:
                        randSpawn = rock3;
                        break;
                    case 4:
                        randSpawn = rock4;
                        break;
                    case 5:
                        randSpawn = spruce;
                        break;
                    case 6:
                        randSpawn = fir;
                        break;
                    case 7:
                        randSpawn = log2;
                        break;
                    case 8:
                        randSpawn = log3;
                        break;
                    default:
                        randSpawn = log1;
                        break;
                }
            }
            //int obstacleType = 6;
            
            Vector3 randSpawnPoint = new Vector3(Random.Range(-100f, 100f), 13f, Random.Range(patch.transform.position.z - 200f, patch.transform.position.z + 200f));
            GameObject randObstacle = Instantiate(randSpawn, randSpawnPoint, Quaternion.identity);
            obstacleList.Add(randObstacle);
            //print(randSpawn.transform.position + " " + obstacleType.ToString());
            //print("location = " + randSpawnPoint);
        }
    }
    private void RemoveObstacles(GameObject randSpawn)
    {
        obstacleList.Remove(randSpawn);
        Destroy(randSpawn);
    }
}
