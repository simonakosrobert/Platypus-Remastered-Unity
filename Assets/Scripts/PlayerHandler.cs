using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class PlayerHandler : MonoBehaviour
{

    
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float speed;
    [SerializeField] private GameObject ship;

    [SerializeField] private Sprite shipC;
    [SerializeField] private Sprite shipU1;
    [SerializeField] private Sprite shipU2;
    [SerializeField] private Sprite shipD1;
    [SerializeField] private Sprite shipD2;

    [SerializeField] private GameObject debris;
    [SerializeField] private int debrisCount;

    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;
    private SpriteRenderer shipRenderer;
    [SerializeField] public int animationSpeed;

    public bool isExploded = false;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private int tick = 0;

    Camera cam;
    Vector2 screenPosition;

    // Start is called before the first frame update
    void Start()
    {   
        SaveLoadData.Load();
        Application.targetFrameRate = 60;
        shipRenderer = ship.GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isInvincible && other.gameObject.CompareTag("Enemy"))
        {   
            shipRenderer.enabled = false;
            GameObject exploding = Instantiate(explosion, transform.position, Quaternion.identity);
            exploding.GetComponent<SpriteRenderer>().sortingOrder = 60;

            AudioSource explosionSoundClone = Instantiate(explosionSound);
            explosionSoundClone.Play();
            Destroy(explosionSoundClone.gameObject, explosionSoundClone.clip.length);

            other.GetComponent<EnemyHealth>().health -= 150;

            isExploded = true;
            isInvincible = true;

            for (int i = 0; i < debrisCount; i++)
            {
                GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
                clone.GetComponent<SpriteRenderer>().sortingOrder = 50;
            }
        }
    }

    void explosionHandling()
    {
        if (isExploded)
        {
            tick += 1;
        }

        if (tick >= 120 && isExploded)
        {   
            tick += 1;
            transform.position = new Vector3(2, 2, 0);
            shipRenderer.enabled = true;
            isExploded = false;
        }
        else if (tick >= 120 && tick % 7 == 0)
        {
            tick += 1;
            shipRenderer.enabled = !shipRenderer.enabled;
        }
        else if (tick >= 120 && tick < 360)
        {
            tick += 1;
        }

        if (tick == 360)
        {
            tick = 0;
            shipRenderer.enabled = true;
            isInvincible = false;
        }
    }

    void Shipmovement()
    {   
        if (GameHandler.pause == false && GameHandler.StageIntro == false)
        {
            if (moveActionToUse is not null && !isExploded)
            {

                Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();

                if (moveDirection.y > 0.0f && animationSpeed < 40)
                {
                    if (animationSpeed >= 0) animationSpeed += 4;
                    else if (animationSpeed < 0) animationSpeed += 8;
                    
                }
                else if (moveDirection.y < 0.0f && animationSpeed > -40)
                {
                    if (animationSpeed <= 0) animationSpeed -= 4;
                    else if (animationSpeed > 0) animationSpeed -= 8;
                }
                else if (moveDirection.y == 0.0f)
                {   
                    if (animationSpeed >= -40 && animationSpeed < 0)
                    {
                        animationSpeed += 4; 
                    }
                    if (animationSpeed <= 40 && animationSpeed > 0)
                    {
                        animationSpeed -= 4; 
                    }
                    
                }
                
                transform.Translate(moveDirection * speed * Time.deltaTime); 

                if (Input.GetKey(KeyCode.W))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position = new Vector3(transform.position.x  + 0.1f, transform.position.y, 0);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, 0);
                }
        }
               
        }

        screenPosition = cam.WorldToViewportPoint(transform.position);
        
        if (screenPosition.x < 0.03f)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(0.03f, screenPosition.y, 10));
            screenPosition = cam.WorldToViewportPoint(transform.position);
        }
        if (screenPosition.x > 0.97f)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(0.97f, screenPosition.y, 10));
            screenPosition = cam.WorldToViewportPoint(transform.position);
        }

        if (screenPosition.y < 0.05f)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(screenPosition.x, 0.05f, 10));
        }
        if (screenPosition.y > 0.95f)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(screenPosition.x, 0.95f, 10));
        }

    }

    void shipAnimation(float animationSpeed, SpriteRenderer shipRenderer)
    {
        if (!isExploded)
        {
            if (animationSpeed > 0 && animationSpeed < 20)
            {
                shipRenderer.sprite = shipU1;
            }
            else if (animationSpeed > 20)
            {
                shipRenderer.sprite = shipU2;
            }
            else if (animationSpeed < 0 && animationSpeed > -20)
            {
                shipRenderer.sprite = shipD1;
            }
            else if (animationSpeed < -20)
            {
                shipRenderer.sprite = shipD2;
            }
            else if (animationSpeed == 0)
            {
                shipRenderer.sprite = shipC;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
        Shipmovement();
        shipAnimation(animationSpeed, shipRenderer);
        explosionHandling();
    }
}
