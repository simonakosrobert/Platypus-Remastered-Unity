using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisHandler : MonoBehaviour
{   

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minYSpeed;

    [SerializeField] private float maxYSpeed;
    [SerializeField] private float maxTorque;
    [SerializeField] private float forceAngleRange;

    [SerializeField] private bool randomizeStartingFrame;
    private float randomOffset;
    private Rigidbody2D rigidBody;

    Animator animator;
    AnimatorClipInfo[] animatorinfo;
    private Camera cam;



    // Start is called before the first frame update
    void Start()
    {   
        if (randomizeStartingFrame)
        {
            randomOffset = Random.Range(0.0f, 1.0f);
        }
        else
        {
            randomOffset = 0f;
        }
        animator = GetComponent<Animator>();
        animator.speed = Random.Range(0.5f, 1.5f);
        animatorinfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        GetComponent<Animator>().Play(animatorinfo[0].clip.name, 0, randomOffset);
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(-forceAngleRange, forceAngleRange), transform.eulerAngles.z);
        rigidBody.isKinematic = false;

        float speed = Random.Range(minSpeed, maxSpeed);
        float speedY = Random.Range(minYSpeed, maxYSpeed);

        Vector3 force = transform.forward;
        float forceX = Random.Range(-1, 1);

        force = new Vector3(force.x, 1, force.z);
        rigidBody.AddForce(force * speed);

        force = new Vector3(forceX, force.y, force.z);
        rigidBody.AddForce(force * speedY);

        rigidBody.AddTorque(Random.Range(1, maxTorque), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.WorldToViewportPoint(transform.position).y < -0.2f)
        {
            Destroy(gameObject);
        }
    }
}
