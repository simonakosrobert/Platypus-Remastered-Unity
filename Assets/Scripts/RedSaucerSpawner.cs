using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSaucerSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    private float tick;
    [SerializeField] private int spawnCount;
    [SerializeField] private GameObject redSaucerDown;
    [SerializeField] private GameObject redSaucerUp;
    private int spawned;
    private bool upSpawned = false;
    private Camera cam;
    float randomY;
    void Start()
    {
        cam = Camera.main;
        randomY = Random.Range(0.1f, 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tick += Time.deltaTime;
        }

        if (tick > 0.2f && spawned < spawnCount)
        {
            tick = 0;
            upSpawned = false;
            spawned += 1;
            GameObject downClone = Instantiate(redSaucerDown, transform.position, Quaternion.identity);
            Vector3 cloneSizeDown = HillsMovement.objectSizeCalculator(cam, downClone);
            downClone.transform.position = cam.ViewportToWorldPoint(new Vector3(1f + cloneSizeDown.x, randomY, 0f));
            downClone.transform.SetParent(transform);
        }

        else if (tick > 0.10f && spawned < spawnCount && !upSpawned)
        {
            spawned += 1;
            GameObject downUp = Instantiate(redSaucerUp, transform.position, Quaternion.identity);
            Vector3 cloneSizeUp = HillsMovement.objectSizeCalculator(cam, downUp);
            downUp.transform.position = cam.ViewportToWorldPoint(new Vector3(1f + cloneSizeUp.x, randomY, 0f));
            downUp.transform.SetParent(transform);
            upSpawned = true;
        }

    }
}
