using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBigUnsMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private float tick;
    private float accelerationTick;
    [SerializeField] private Camera cam;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private GameObject explosion; 
    [SerializeField] private GameObject smokeRing; 

    [SerializeField] private PolygonCollider2D damage0Collider; 

    [SerializeField] private PolygonCollider2D damage1Collider; 
    [SerializeField] private PolygonCollider2D damage2Collider; 
    [SerializeField] private PolygonCollider2D damage3Collider; 
    [SerializeField] private GameObject debris;
    [SerializeField] private int debrisCount;

    private SpriteRenderer Renderer;

    private Rigidbody2D rigidBody;

    private bool lastDamage = false;

    void currentSprite()
    {
        if (GetComponent<EnemyHealth>().health <= 100) 
        {   
            
            if (lastDamage == false)
            {
                rigidBody.AddTorque(-0.02f, ForceMode2D.Impulse);
            }

            if (speed >= 0.01f)
            {
                speed -= 0.0001f;
            }
            Renderer.sprite = sprites[5];
            damage2Collider.enabled = false;
            damage3Collider.enabled = true;
            rigidBody.gravityScale = 0.05f;
            lastDamage = true;
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
        if (GameHandler.pause != true && GameHandler.StageIntro != true)
        {
            //transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            if (accelerationTick > 9 && accelerationTick < 12 && GetComponent<EnemyHealth>().health > 100)
            {
                speed += 0.005f;
            }
        }
    }

    void Despawner()
    {
        Vector3 leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min);
        Vector3 rightY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max);

            if (leftX.x > 1.0f || rightY.y < 0.0f)
            {
                Destroy(gameObject);
            }
            else if (GetComponent<EnemyHealth>().health <= 0)
            {
                AudioSource explosionSoundClone = Instantiate(explosionSound);
                explosionSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
                explosionSoundClone.pitch = 0.9f;
                explosionSoundClone.Play();
                Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

                GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
                exploding.transform.SetParent(GameObject.Find("Effects").transform);
                exploding.GetComponent<SpriteRenderer>().sortingOrder = 20;
                Destroy(gameObject);

                SaveLoadData._PlayerData.totalXP += 10;

                for (int i = 0; i < debrisCount; i++)
                {
                    GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
                    clone.transform.SetParent(GameObject.Find("Effects").transform);
                    clone.GetComponent<SpriteRenderer>().sortingOrder = 20;
                }
            }
    }

    // Start is called before the first frame update
    void Start()
    {   
        rigidBody = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameHandler.pause != true && GameHandler.StageIntro != true)
        {
            tick += Time.deltaTime;
            accelerationTick += Time.deltaTime;
        }

        if (tick > 0.3f && GameHandler.pause != true && GameHandler.StageIntro != true && lastDamage != true)
        {
            GameObject smokering = Instantiate(smokeRing, new Vector2(transform.position.x - GetComponent<SpriteRenderer>().bounds.extents.x, transform.position.y), Quaternion.identity);
            smokering.transform.SetParent(GameObject.Find("Effects").transform);
            smokering.GetComponent<SpriteRenderer>().sortingOrder = 19;
            tick = 0;
        }
        
        Movement();
        currentSprite();
        Despawner();
    }
}
