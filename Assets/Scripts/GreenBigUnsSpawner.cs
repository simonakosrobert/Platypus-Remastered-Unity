using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBigUnsSpawner : MonoBehaviour
{   

    private int tick = 0;
    [SerializeField] private GameObject preFab;
    [SerializeField] Camera cam;
    [SerializeField] private int sortingOrder;

    void addSprite(GameObject _prefab)
    {
        float randomY = Random.Range(0.1f, 0.9f);

        GameObject clone = Instantiate(_prefab, new Vector3(0, 0, 0), Quaternion.identity);

        Vector3 cloneSize = HillsMovement.objectSizeCalculator(cam, clone);

        Vector3 BottomLeft = cam.ViewportToWorldPoint(new Vector3(0f - cloneSize.x, randomY, 0f));
        clone.transform.position = BottomLeft;
        clone.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;  
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tick += 1;

        if (tick % 300 == 0)
        {
            addSprite(preFab);
        }
    }
}
