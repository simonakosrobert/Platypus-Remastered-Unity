using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteManHandler : MonoBehaviour
{   

    private Rigidbody2D rigidBody;
    private float tickDelta;
    Animator animator;
    AnimatorStateInfo animatorinfo;
    [SerializeField] GameObject parachute;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {   

        Vector3 rightY = cam.WorldToViewportPoint(GetComponent<Renderer>().bounds.max);

        if (rightY.y < -0.2f)
        {
            Destroy(gameObject);
        }

        tickDelta = 0;

        if (GameHandler.pause != true && GameHandler.StageIntro != true) 
        {
            tickDelta = Time.deltaTime;
        }

        animatorinfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animatorinfo.normalizedTime < 0.5f)
        {
            transform.Translate(Vector3.left * 0.3f * tickDelta);
        }


        else if (animatorinfo.normalizedTime < 0.6f && animatorinfo.normalizedTime > 0.55f)
        {
            Vector3 force = transform.forward;
            force = new Vector3(force.x, 5, force.z);
            rigidBody.AddForce(force * 15);
            transform.Translate(Vector3.left * 0.5f * tickDelta);
        }
        else if (animatorinfo.normalizedTime > 0.6f)
        {   
            rigidBody.gravityScale = 0.0f;
            transform.Translate(Vector3.left * 0.7f * tickDelta);
        }

        if (animatorinfo.normalizedTime > 0.50f && animatorinfo.normalizedTime < 0.55f)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<Renderer>().sortingOrder = 21;
        }
    }
}
