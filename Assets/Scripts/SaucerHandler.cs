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
    private Vector3 initPos;
    private SpriteRenderer saucerRenderer;
    private float tick;
    private float tickDelta;

    private bool down;
    private bool up;

    private float lastY;
    void Start()
    {
        saucerRenderer = GetComponent<SpriteRenderer>();
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tick += Time.deltaTime;
            tickDelta = Time.deltaTime;
        }

        if (GetComponent<EnemyHealth>().health <= 0) Destroy(gameObject);

        transform.position = new Vector3(transform.position.x, Mathf.Sin(tick * freq) * amp + initPos.y, 0);
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
