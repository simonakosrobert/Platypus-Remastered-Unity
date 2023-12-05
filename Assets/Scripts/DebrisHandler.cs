using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisHandler : MonoBehaviour
{   

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxTorque;
    [SerializeField] private float forceAngleRange;
    private Rigidbody2D rigidBody;
    AnimatorClipInfo[] animatorinfo;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {   
        float randomOffset = Random.Range(0.0f, 1.0f);
        animatorinfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        GetComponent<Animator>().Play(animatorinfo[0].clip.name, -1, randomOffset);
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(-forceAngleRange, forceAngleRange), transform.eulerAngles.z);
        rigidBody.isKinematic = false;

        float speed = Random.Range(-100, maxSpeed);
        float speedY = Random.Range(-400, 400);

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
