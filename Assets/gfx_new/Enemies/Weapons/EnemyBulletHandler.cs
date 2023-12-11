using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 direction;
    public float speed;
    private Camera cam;

    private void Despawner()
    {
        float leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min).x;
        float rightX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max).x;
        float upY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max).y;
        float bottomY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min).y;
        
        if (leftX > 1.0f || rightX < 0.0f || upY < 0.0f || bottomY > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {   

        if (direction != null)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        Despawner();
        
    }
}
