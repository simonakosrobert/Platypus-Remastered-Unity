using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditor;
using UnityEngine;

public class StandartGunHandler : MonoBehaviour
{

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private GameObject whiteFlash;
    [SerializeField] private AudioSource dinkSound;
    [SerializeField] private bool isTop;
    [SerializeField] private float offsetY;
    Vector3 origPos;
    private Camera cam;
    private float tick;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {   
            GameObject flash = Instantiate(whiteFlash, new Vector2(gameObject.transform.position.x + gameObject.GetComponent<SpriteRenderer>().size.x/2, gameObject.transform.position.y), Quaternion.identity);
            flash.transform.SetParent(GameObject.Find("Effects").transform);
            flash.GetComponent<SpriteRenderer>().sortingOrder = 60;
            other.GetComponent<EnemyHealth>().health -= 30;
            Destroy(gameObject);

            AudioSource shoot = Instantiate(dinkSound);
            shoot.transform.SetParent(GameObject.Find("Effects").transform);
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
        origPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   

        if (GameHandler.pause != true && GameHandler.StageIntro != true && Time.deltaTime < 0.08f) 
        {
            tick += Time.deltaTime;
        }

        if (isTop && origPos.y + offsetY > transform.position.y) transform.Translate(Vector2.up * Time.deltaTime * ySpeed);
        else if (!isTop && origPos.y - offsetY < transform.position.y) transform.Translate(Vector2.down * Time.deltaTime * ySpeed);

        if (isTop && origPos.y + offsetY < transform.position.y) transform.position = new Vector3(transform.position.x, origPos.y + offsetY, 0);
        else if (!isTop && origPos.y - offsetY > transform.position.y) transform.position = new Vector3(transform.position.x, origPos.y - offsetY, 0);

        transform.Translate(Vector2.right * Time.deltaTime * xSpeed);
        DestroyBullet();
    }
}
