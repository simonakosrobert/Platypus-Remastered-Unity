using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerHandler : MonoBehaviour
{
    [SerializeField] private float amp;
    [SerializeField] private float freq;
    [SerializeField] private Sprite up1;
    [SerializeField] private Sprite up2;
    [SerializeField] private Sprite center;
    [SerializeField] private Sprite down1;
    [SerializeField] private Sprite down2;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject debris;
    [SerializeField] private int debrisCount;
    private Camera cam;
    private Vector3 initPos;
    private SpriteRenderer saucerRenderer;
    private float tick;
    private float tickDelta;

    [SerializeField] private bool goUpFirst;

    private bool down;
    private bool up;

    private float lastY;
    void Start()
    {   
        cam = Camera.main;
        saucerRenderer = GetComponent<SpriteRenderer>();
        initPos = transform.position;
    }

    void Despawner()
    {
        Vector3 rightX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max);

            if (rightX.x < 0.0f)
            {
                Destroy(gameObject);
            }
            else if (GetComponent<EnemyHealth>().health <= 0)
            {
                AudioSource explosionSoundClone = Instantiate(explosionSound);
                explosionSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
                float randomPitch = Random.Range(0.7f, 1.4f);
                explosionSoundClone.pitch = randomPitch;
                explosionSoundClone.Play();
                Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

                GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
                exploding.transform.SetParent(GameObject.Find("Effects").transform);
                exploding.GetComponent<SpriteRenderer>().sortingOrder = 21;
                Destroy(gameObject);

                SaveLoadData._PlayerData.totalXP += 2;

                for (int i = 0; i < debrisCount; i++)
                {
                    GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
                    clone.transform.SetParent(GameObject.Find("Effects").transform);
                    clone.GetComponent<SpriteRenderer>().sortingOrder = 21;
                }
            }
    }

    // Update is called once per frame
    void Update()
    {   
        
        tickDelta = 0;

        Despawner();

        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tick += Time.deltaTime;
            tickDelta = Time.deltaTime;
        }

        if (GetComponent<EnemyHealth>().health <= 0) Destroy(gameObject);

        if (goUpFirst)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(tick * freq) * amp + initPos.y, 0);
        }
        else if (!goUpFirst)
        {
            transform.position = new Vector3(transform.position.x, -Mathf.Sin(tick * freq) * amp + initPos.y, 0);
        }
        
        transform.Translate(Vector3.left * speed * tickDelta);

        if (lastY < transform.position.y)
        {
            up = true;
            down = false;
        }
        if (lastY > transform.position.y)
        {
            up = false;
            down = true;
        }


        lastY = transform.position.y;

        // 3/3 UP
        if (transform.position.y > initPos.y + amp/1.5 && up)
        {
            saucerRenderer.sprite = center;
        }
        // 2/3 UP
        else if (transform.position.y > initPos.y + amp/3 && up)
        {
            saucerRenderer.sprite = up1;
        }

        // -3/3 UP
        else if (transform.position.y < initPos.y - amp/1.5 && up)
        {
            saucerRenderer.sprite = up1;
        }
        // -2/3 UP
        else if (transform.position.y < initPos.y - amp/3 && up)
        {
            saucerRenderer.sprite = up2;
        }

        // 3/3 DOWN
        else if (transform.position.y > initPos.y + amp/1.5 && down)
        {
            saucerRenderer.sprite = down1;
        }
        // 2/3 DOWN
        else if (transform.position.y > initPos.y + amp/3 && down)
        {
            saucerRenderer.sprite = down2;
        }

        // -2/3 UP
        else if (transform.position.y < initPos.y - amp/3 && down)
        {
            saucerRenderer.sprite = down1;
        }
        // -3/3 UP
        else if (transform.position.y < initPos.y - amp/1.5 && down)
        {
            saucerRenderer.sprite = center;
        }
        
        
        
    }
}
