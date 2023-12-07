using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour
{   

    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject standartBulletTop;
    [SerializeField] private GameObject standartBulletBottom;
    [SerializeField] private GameObject whiteFlash;
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject bullets; 
    private int tick = 0;

    private bool shooting = false;

    // Start is called before the first frame update

    void addSprite()
    {
        GameObject bulletTop = Instantiate(standartBulletTop, new Vector2(ship.transform.position.x + ship.GetComponent<Renderer>().bounds.extents.x, ship.transform.position.y), Quaternion.identity);
        bulletTop.transform.SetParent(bullets.transform, false);
        Vector3 bulletSizeTop = bulletTop.GetComponent<Renderer>().bounds.extents;
        bulletTop.transform.position = new Vector2(ship.transform.position.x + ship.GetComponent<Renderer>().bounds.extents.x + bulletSizeTop.x, ship.transform.position.y - ship.GetComponent<Renderer>().bounds.extents.y/2);
        bulletTop.GetComponent<SpriteRenderer>().sortingOrder = 20;

        GameObject bulletBottom = Instantiate(standartBulletBottom, new Vector2(ship.transform.position.x + ship.GetComponent<Renderer>().bounds.extents.x, ship.transform.position.y), Quaternion.identity);
        bulletBottom.transform.SetParent(bullets.transform, false);
        Vector3 bulletSizeBottom = bulletBottom.GetComponent<Renderer>().bounds.extents;
        bulletBottom.transform.position = new Vector2(ship.transform.position.x + ship.GetComponent<Renderer>().bounds.extents.x + bulletSizeBottom.x, ship.transform.position.y - ship.GetComponent<Renderer>().bounds.extents.y/2);
        bulletBottom.GetComponent<SpriteRenderer>().sortingOrder = 20;

        GameObject flash = Instantiate(whiteFlash, new Vector2(ship.transform.position.x + ship.GetComponent<Renderer>().bounds.extents.x, ship.transform.position.y - ship.GetComponent<Renderer>().bounds.extents.y/2), Quaternion.identity);
        flash.transform.SetParent(bullets.transform, true);
        flash.GetComponent<SpriteRenderer>().sortingOrder = 60;
    
        AudioSource shoot = Instantiate(shootingSound);
        shoot.transform.SetParent(bullets.transform, false);
        shoot.Play();
        Destroy(shoot.gameObject, shoot.clip.length);
    }

    public void Shoot()
    {   
        if (!player.GetComponent<PlayerHandler>().isExploded && !GameHandler.pause)
        {
            shooting = true;
            addSprite();
        }     
    }

    public void StopShooting()
    {
        shooting = false;
        tick = 0;
    }

    public void pauseGame()
    {
        if (GameHandler.pause == false)
            {
                Time.timeScale = 0.0f;
                GameHandler.pause = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                GameHandler.pause = false;
            }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {   
            tick += 1;
            if (tick % 12 == 0)
            {
                addSprite();
            }
            
        }
    }
}
