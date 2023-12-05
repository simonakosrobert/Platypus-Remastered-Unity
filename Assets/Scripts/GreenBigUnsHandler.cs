using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBigUnsMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private int tick;
    [SerializeField] private Camera cam;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private GameObject explosion; 

    [SerializeField] private PolygonCollider2D damage0Collider; 

    [SerializeField] private PolygonCollider2D damage1Collider; 
    [SerializeField] private PolygonCollider2D damage2Collider; 
    [SerializeField] private PolygonCollider2D damage3Collider; 
    [SerializeField] private GameObject debris;
    [SerializeField] private int debrisCount;

    private SpriteRenderer Renderer;

    void currentSprite()
    {
        if (GetComponent<EnemyHealth>().health <= 100) 
        {
            Renderer.sprite = sprites[5];
            damage2Collider.enabled = false;
            damage3Collider.enabled = true;
        }
        else if (GetComponent<EnemyHealth>().health <= 200)
        { 
            Renderer.sprite = sprites[4];
            damage1Collider.enabled = false;
            damage2Collider.enabled = true;
        }
        else if (GetComponent<EnemyHealth>().health <= 300)
        {
            Renderer.sprite = sprites[3];
            damage0Collider.enabled = false;
            damage1Collider.enabled = true;
        } 
        else if (GetComponent<EnemyHealth>().health <= 400)
        {
            Renderer.sprite = sprites[2];
        } 
        else if (GetComponent<EnemyHealth>().health <= 500)
        {
            Renderer.sprite = sprites[1];
        } 
    }

    void Movement()
    {
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0);
        if (tick > 350 && tick < 500)
        {
            speed += 0.0001f;
        }
    }

    void Despawner()
    {
        Vector3 leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min);

            if (leftX.x > 1.0f)
            {
                Destroy(gameObject);
            }
            else if (GetComponent<EnemyHealth>().health <= 0)
            {
                AudioSource explosionSoundClone = Instantiate(explosionSound);
                explosionSoundClone.pitch = 0.9f;
                explosionSoundClone.Play();
                Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

                GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
                exploding.GetComponent<SpriteRenderer>().sortingOrder = 20;
                Destroy(gameObject);

                for (int i = 0; i < debrisCount; i++)
                {
                    GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
                    clone.GetComponent<SpriteRenderer>().sortingOrder = 20;
                }
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        tick += 1;
        Movement();
        currentSprite();
        Despawner();
    }
}
