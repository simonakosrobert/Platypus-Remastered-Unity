using System.Collections.Generic;
using UnityEngine;

public class FishFighterHandler : MonoBehaviour
{   

    [SerializeField] private bool toLeft;
    [SerializeField] private Animator fishAnimator;
    [SerializeField] private RuntimeAnimatorController LeftToUp;
    [SerializeField] private RuntimeAnimatorController LeftToDown;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioSource shotSound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject debris;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Sprite[] bulletSprites;
    [SerializeField] private int debrisCount;
    private bool turning;
    private Vector2 shipDirection;
    private float tickDelta;
    private Camera cam;
    SpriteRenderer spriteRenderer;
    string currentSprite;
    private Sprite bulletToShoot;
    private Vector2 bulletDirection;
    private Vector3 bulletStartingPoint;
    PolygonCollider2D col;
    List<Vector2> physicsShape = new List<Vector2>();
    Vector3 pos;
    private bool alreadyShot;
    private float angle;
    private string spritePrefix = "fish fighter black_";

    private void GetCurrentSprite()
    {
        currentSprite = GetComponent<SpriteRenderer>().sprite.name;

        if (currentSprite == spritePrefix + "0" || currentSprite == spritePrefix + "8")
        {   

            bulletToShoot = bulletSprites[0];
            if (currentSprite == spritePrefix + "0")
            {
                angle = 270;
                bulletStartingPoint = new Vector3(transform.position.x - GetComponent<Renderer>().bounds.extents.x, transform.position.y, 0);
            }
            else
            {   
                angle = 90;
                bulletStartingPoint = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.extents.x, transform.position.y, 0);
            }
        }

        else if (currentSprite == spritePrefix + "1" || currentSprite == spritePrefix + "9")
        {   

            bulletToShoot = bulletSprites[1];
            if (currentSprite == spritePrefix + "1")
            {
                angle = 292.5f;
                bulletStartingPoint = new Vector3(transform.position.x - GetComponent<Renderer>().bounds.extents.x, transform.position.y + GetComponent<Renderer>().bounds.extents.y/3, 0);
            }
            else
            {
                angle = 112.5f;
                bulletStartingPoint = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.extents.x, transform.position.y - GetComponent<Renderer>().bounds.extents.y/3, 0);
            }
        }

        else if (currentSprite == spritePrefix + "2" || currentSprite == spritePrefix + "10")
        {   

            bulletToShoot = bulletSprites[2];
            if (currentSprite == spritePrefix + "2")
            {
                angle = 315;
                bulletStartingPoint = new Vector3(transform.position.x - (GetComponent<Renderer>().bounds.extents.x - GetComponent<Renderer>().bounds.extents.x/3), transform.position.y + GetComponent<Renderer>().bounds.extents.y/1.5f, 0);
            }
            else
            {
                angle = 135;
                bulletStartingPoint = new Vector3(transform.position.x + (GetComponent<Renderer>().bounds.extents.x - GetComponent<Renderer>().bounds.extents.x/3), transform.position.y - GetComponent<Renderer>().bounds.extents.y/1.5f, 0);
            }
        }

        else if (currentSprite == spritePrefix + "3" || currentSprite == spritePrefix + "11")
        {   

            bulletToShoot = bulletSprites[3];
            if (currentSprite == spritePrefix + "3")
            {
                angle = 337.5f;
                bulletStartingPoint = new Vector3(transform.position.x - GetComponent<Renderer>().bounds.extents.x/3, transform.position.y + GetComponent<Renderer>().bounds.extents.y, 0);
            }
            else
            {
                angle = 157.5f;
                bulletStartingPoint = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.extents.x/3, transform.position.y - GetComponent<Renderer>().bounds.extents.y, 0);
            }
        }

        else if (currentSprite == spritePrefix + "4" || currentSprite == spritePrefix + "12")
        {   

            bulletToShoot = bulletSprites[4];
            if (currentSprite == spritePrefix + "4")
            {
                angle = 0;
                bulletStartingPoint = new Vector3(transform.position.x, transform.position.y + GetComponent<Renderer>().bounds.extents.y, 0);
            }
            else
            {
                angle = 180;
                bulletStartingPoint = new Vector3(transform.position.x, transform.position.y - GetComponent<Renderer>().bounds.extents.y, 0);
            }
        }

        else if (currentSprite == spritePrefix + "5" || currentSprite == spritePrefix + "13")
        {   

            bulletToShoot = bulletSprites[5];
            if (currentSprite == spritePrefix + "5")
            {
                angle = 22.5f;
                bulletStartingPoint = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.extents.x/3, transform.position.y + GetComponent<Renderer>().bounds.extents.y, 0);
            }
            else
            {
                angle = 202.5f;
                bulletStartingPoint = new Vector3(transform.position.x - GetComponent<Renderer>().bounds.extents.x/3, transform.position.y - GetComponent<Renderer>().bounds.extents.y, 0);
            }
        }

        else if (currentSprite == spritePrefix + "6" || currentSprite == spritePrefix + "14")
        {   

            bulletToShoot = bulletSprites[6];
            if (currentSprite == spritePrefix + "6")
            {
                angle = 45;
                bulletStartingPoint = new Vector3(transform.position.x + (GetComponent<Renderer>().bounds.extents.x - GetComponent<Renderer>().bounds.extents.x/3), transform.position.y + GetComponent<Renderer>().bounds.extents.y/1.5f, 0);
            }
            else
            {
                angle = 225;
                bulletStartingPoint = new Vector3(transform.position.x - (GetComponent<Renderer>().bounds.extents.x - GetComponent<Renderer>().bounds.extents.x/3), transform.position.y - GetComponent<Renderer>().bounds.extents.y/1.5f, 0);
            }
        }

        else if (currentSprite == spritePrefix + "7" || currentSprite == spritePrefix + "15")
        {   

            bulletToShoot = bulletSprites[7];
            if (currentSprite == spritePrefix + "7")
            {
                angle = 67.5f;
                bulletStartingPoint = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.extents.x, transform.position.y + GetComponent<Renderer>().bounds.extents.y/3, 0);
            }
            else
            {
                angle = 247.5f;
                bulletStartingPoint = new Vector3(transform.position.x - GetComponent<Renderer>().bounds.extents.x, transform.position.y - GetComponent<Renderer>().bounds.extents.y/3, 0);
            }        
        }

        bulletDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        shipDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

    }

    private void Shoot()
    {   

        Vector3 leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min);

        if (!alreadyShot && !GameHandler.pause && !GameHandler.StageIntro && leftX.x < 1.0f)
        {
            int dice = Random.Range(1, 201);
            if (dice <= 2)
            {   
                alreadyShot = true;

                GameObject shot = Instantiate(bullet, bulletStartingPoint, Quaternion.identity);
                shot.GetComponent<SpriteRenderer>().sortingOrder = 40;
                shot.GetComponent<SpriteRenderer>().sprite = bulletToShoot;
                shot.AddComponent<PolygonCollider2D>();
                shot.GetComponent<PolygonCollider2D>().isTrigger = true;
                shot.GetComponent<PolygonCollider2D>().excludeLayers = LayerMask.GetMask("Enemy");
                shot.GetComponent<PolygonCollider2D>().tag = "EnemyBullet";

                shot.AddComponent<EnemyBulletHandler>();
                shot.GetComponent<EnemyBulletHandler>().direction = bulletDirection;
                shot.GetComponent<EnemyBulletHandler>().speed = 10f;

                AudioSource shotSoundClone = Instantiate(shotSound);
                shotSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
                shotSoundClone.Play();
                Destroy(shotSoundClone.gameObject, shotSoundClone.clip.length);

            }
        }
    }


    void Despawner()
    {   

        float leftX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min).x;
        float rightX = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max).x;
        float upY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max).y;
        float bottomY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.min).y;
        
        if (((rightX < 0.0f || bottomY > 1.0f || upY < 0.0f) && toLeft) || ((leftX > 1.0f || bottomY > 1.0f || upY < 0.0f) && !toLeft))
        {
            Destroy(gameObject);
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
            InGameStats.totalPoints += 15;
            InGameStats.pointCounterTimer = 0;
            InGameStats.xpCounterTimer = 0;

            for (int i = 0; i < debrisCount; i++)
            {
                GameObject clone = Instantiate(debris, transform.position, Quaternion.identity);
                clone.transform.SetParent(GameObject.Find("Effects").transform);
                clone.GetComponent<SpriteRenderer>().sortingOrder = 21;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        pos = cam.WorldToViewportPoint(transform.position);
        if (pos.y < 0.5f)
        {
            fishAnimator.runtimeAnimatorController = LeftToUp;
        }
        else if (pos.y >= 0.5f)
        {
            fishAnimator.runtimeAnimatorController = LeftToDown;
        }
        
        fishAnimator.speed = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {   

        Despawner();
        GetCurrentSprite();
        Shoot();
        tickDelta = 0;

        pos = cam.WorldToViewportPoint(transform.position);

        if (!turning && GameHandler.pause != true && GameHandler.StageIntro != true && ((pos.x < 0.5 && toLeft) || (pos.x > 0.5 && !toLeft)))
        {
            int dice = Random.Range(0, 101);

            if (dice < 3)
            {
                turning = true;
                fishAnimator.enabled = true;
            }
        }

        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tickDelta = Time.deltaTime;
        }


        transform.Translate(shipDirection * speed * tickDelta);
        
    }

    void LateUpdate()
{
    spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
    col.SetPath(0, physicsShape);
}

}
