using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class FishFighterHandler : MonoBehaviour
{   

    [SerializeField] private bool toLeft;
    private bool toUp;
    [SerializeField] private Animator fishAnimator;
    [SerializeField] private AnimatorController LeftToUp;
    [SerializeField] private AnimatorController LeftToDown;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject debris;
    [SerializeField] private int debrisCount;
    private float Xspeed;
    private float Yspeed;
    private bool turning;
    private float tickDelta;
    private Camera cam;
    SpriteRenderer spriteRenderer;
    PolygonCollider2D col;
    List<Vector2> physicsShape = new List<Vector2>();
    Vector3 pos;

    void Despawner()
    {   
        if(toLeft)
        {   
            Vector3 rightX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max);
            if (rightX.x < 0.0f)
            {
                Destroy(gameObject);
            }
        }

        else
        {
            Vector3 leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max);
            if (leftX.x > 1.0f)
            {
                Destroy(gameObject);
            }
        }

        if (GetComponent<EnemyHealth>().health <= 0)
        {
            AudioSource explosionSoundClone = Instantiate(explosionSound);
            explosionSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
            float randomPitch = Random.Range(0.7f, 1.4f);
            explosionSoundClone.pitch = randomPitch;
            explosionSoundClone.Play();
            Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

            GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
            exploding.transform.SetParent(GameObject.Find("Effects").transform);
            exploding.GetComponent<SpriteRenderer>().sortingOrder = 22;
            Destroy(gameObject);

            SaveLoadData._PlayerData.totalXP += 3;

            // for (int i = 0; i < debrisCount; i++)
            // {
            //     GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
            //     clone.transform.SetParent(GameObject.Find("Effects").transform);
            //     clone.GetComponent<SpriteRenderer>().sortingOrder = 21;
            // }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Xspeed = speed;
        Yspeed = 0;
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        pos = cam.WorldToViewportPoint(transform.position);
        if (pos.y < 0.5f)
        {
            fishAnimator.runtimeAnimatorController = LeftToUp;
            toUp = true;
        }
        else if (pos.y >= 0.5f)
        {
            fishAnimator.runtimeAnimatorController = LeftToDown;
        }
        
        fishAnimator.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {   

        Despawner();

        tickDelta = 0;

        pos = cam.WorldToViewportPoint(transform.position);

        if (!turning && GameHandler.pause != true && GameHandler.StageIntro != true && ((pos.x < 0.5 && toLeft) || (pos.x > 0.5 && !toLeft)))
        {
            int dice = Random.Range(0, 101);

            if (dice < 10)
            {
                turning = true;
                fishAnimator.enabled = true;
            }
        }

        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tickDelta = Time.deltaTime;
        }

        if (!turning)
        {
            transform.Translate(Vector3.left * Xspeed * tickDelta);
        }
        else if(turning)
        {   
            
            if (Xspeed > 0.0f)
            {
                Xspeed -= speed/25;
                Yspeed += speed/25;
            }

            transform.Translate(Vector3.left * Xspeed * tickDelta);

            if (toUp)
            {
                transform.Translate(Vector3.up * Yspeed * tickDelta);
            }
            else
            {
                transform.Translate(Vector3.down * Yspeed * tickDelta);
            }
            
        }
        
    }

    void LateUpdate()
{
    spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
    col.SetPath(0, physicsShape);
}

}
