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
    private AudioSource explosionSoundClone = null;
    [SerializeField] private GameObject explosion; 
    [SerializeField] private GameObject smokeRing; 

    [SerializeField] private PolygonCollider2D damage0Collider; 

    [SerializeField] private PolygonCollider2D damage1Collider; 
    [SerializeField] private PolygonCollider2D damage2Collider; 
    [SerializeField] private PolygonCollider2D damage3Collider; 
    [SerializeField] private GameObject debris;
    [SerializeField] private GameObject parachuteMan;
    private bool parachuteDeployed = false;
    [SerializeField] private int debrisCount;

    private SpriteRenderer Renderer;

    private Rigidbody2D rigidBody;

    private bool lastDamage = false;

    private GameObject smoke1;
    private int smoke1ScaleCounter;
    private bool smoke1Activated;
    private GameObject smoke2;
    private int smoke2ScaleCounter;
    private bool smoke2Activated;

    private GameObject damage3debris1;
    private GameObject damage3debris2;
    private bool damage3debris = false;
    private GameObject damage4debris1;
    private bool damage4debris = false;
    private GameObject damage5debris1;
    private GameObject damage5debris2;
    private bool damage5debris = false;

    void smokeHandler()
    {
        if (smoke1ScaleCounter < 30 && smoke1Activated)
            {
                smoke1.transform.localScale = new Vector3(smoke1.transform.localScale.x + 0.01f, smoke1.transform.localScale.y + 0.01f, smoke1.transform.localScale.y + 0.01f);
                smoke1.transform.position = new Vector3(smoke1.transform.position.x, smoke1.transform.position.y + 0.01f, 0);
                smoke1ScaleCounter += 1;
            }

        if (smoke2ScaleCounter < 20 && smoke2Activated)
            {
                smoke2.transform.localScale = new Vector3(smoke2.transform.localScale.x + 0.01f, smoke2.transform.localScale.y + 0.01f, smoke2.transform.localScale.y + 0.01f);
                smoke2.transform.position = new Vector3(smoke2.transform.position.x, smoke2.transform.position.y + 0.01f, 0);
                smoke2ScaleCounter += 1;
            }

        smoke1.transform.rotation = Quaternion.identity;
        smoke2.transform.rotation = Quaternion.identity;
        
    }

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

            if (!parachuteDeployed && smoke2ScaleCounter > 10)
            {
                GameObject paraClone = Instantiate(parachuteMan, new Vector3(transform.position.x  + GetComponent<Renderer>().bounds.extents.x, transform.position.y, 0), Quaternion.identity);
                //paraClone.transform.SetParent(GameObject.Find("Effects").transform);
                parachuteDeployed = true;
            } 

            smoke2.SetActive(true);
            smoke2.GetComponent<Renderer>().sortingOrder = 19;
            smoke2Activated = true;

            if (!damage5debris)
            {
                damage5debris1.SetActive(true);
                damage5debris2.SetActive(true);
                damage5debris = true;
            }

        }
        else if (GetComponent<EnemyHealth>().health <= 200)
        { 
            Renderer.sprite = sprites[4];
            damage1Collider.enabled = false;
            damage2Collider.enabled = true;
            smoke1.SetActive(true);
            smoke1.GetComponent<Renderer>().sortingOrder = 19;
            smoke1Activated = true;          
            if (!damage4debris)
            {
                damage4debris1.SetActive(true);
                damage4debris = true;
            } 
        }
        else if (GetComponent<EnemyHealth>().health <= 300)
        {
            Renderer.sprite = sprites[3];
            damage0Collider.enabled = false;
            damage1Collider.enabled = true;
            if (!damage3debris)
            {
                damage3debris1.SetActive(true);
                damage3debris2.SetActive(true);
                damage3debris = true;
            }
            
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

                explosionSoundClone = Instantiate(explosionSound, transform.position, Quaternion.identity);
                explosionSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
                float randomPitch = Random.Range(0.8f, 1.4f);
                explosionSoundClone.pitch = randomPitch;
                explosionSoundClone.Play();
                Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

                GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
                exploding.transform.SetParent(GameObject.Find("Effects").transform);
                exploding.GetComponent<SpriteRenderer>().sortingOrder = 20;
                Destroy(gameObject);

                SaveLoadData._PlayerData.totalXP += 10;
                InGameStats.totalPoints += 200;
                InGameStats.pointCounterTimer = 0;
                InGameStats.xpCounterTimer = 0;

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
        smoke1 = transform.GetChild(0).gameObject;
        smoke2 = transform.GetChild(1).gameObject;
        damage3debris1 = transform.GetChild(2).gameObject;
        damage3debris2 = transform.GetChild(3).gameObject;
        damage4debris1 = transform.GetChild(4).gameObject;
        damage5debris1 = transform.GetChild(5).gameObject;
        damage5debris2 = transform.GetChild(6).gameObject;

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
        smokeHandler();
    }
}
