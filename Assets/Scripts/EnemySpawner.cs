using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBigUnsSpawner : MonoBehaviour
{   

    private float tick = 0;
    [SerializeField] private GameObject preFab;
    [SerializeField] Camera cam;
    [SerializeField] private int sortingOrder;
    [SerializeField] private int spawnRate;
    [SerializeField] private bool fromLeft;
    [SerializeField] private bool isRandomX;

    Vector3 StartingPoint;
    private float randomX;

    void addSprite(GameObject _prefab)
    {
        float randomY = Random.Range(0.1f, 0.9f);

        randomX = 0;
        if (isRandomX) randomX = Random.Range(0.2f, 0.5f);

        GameObject clone = Instantiate(_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        clone.transform.SetParent(this.transform);

        Vector3 cloneSize = HillsMovement.objectSizeCalculator(cam, clone);

        if (fromLeft)
        {
            StartingPoint = cam.ViewportToWorldPoint(new Vector3(0f - cloneSize.x - randomX, randomY, 0f));
        }
        else
        {
            StartingPoint = cam.ViewportToWorldPoint(new Vector3(1f + cloneSize.x + randomX, randomY, 0f));
        }
        
        clone.transform.position = StartingPoint;
        clone.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;  
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

        if (GameHandler.StageIntro != true){
            tick += Time.deltaTime;
        }
        


        if (tick > spawnRate && GameHandler.pause != true && GameHandler.StageIntro != true)
        {
            addSprite(preFab);
            tick = 0;
        }
    }
}
