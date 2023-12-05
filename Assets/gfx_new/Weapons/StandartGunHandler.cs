using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StandartGunHandler : MonoBehaviour
{

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private GameObject whiteFlash;
    [SerializeField] private AudioSource dinkSound;


    private Camera cam;
    private int tick;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {   
            GameObject flash = Instantiate(whiteFlash, new Vector2(gameObject.transform.position.x + gameObject.GetComponent<SpriteRenderer>().size.x/2, gameObject.transform.position.y), Quaternion.identity);
            flash.GetComponent<SpriteRenderer>().sortingOrder = 60;
            other.GetComponent<EnemyHealth>().health -= 30;
            Destroy(gameObject);

            AudioSource shoot = Instantiate(dinkSound);
            shoot.Play();
            Destroy(shoot.gameObject, shoot.clip.length);
        }
    }

    void DestroyBullet()
    {
        if (cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z)).x - 0.02f > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {   
        tick += 1;
        if (tick < 5)
        {
            transform.position = new Vector2(transform.position.x + xSpeed, transform.position.y + ySpeed);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + xSpeed, transform.position.y);
        }   
        DestroyBullet();
    }
}
