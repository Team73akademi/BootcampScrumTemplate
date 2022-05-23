using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private Animator animator;
    private Rigidbody characterRigidbody;
    private float roadWidth;
    private float PlatformHalfWidth = 10f;
    private float PlatformLineWidth = 1f;
    private float PlayerWidth = 4f;
    private float ExtraRoadWidthReduction = 2f;
    public bool heldDown = false;
    private Vector2 touchedPosition = Vector2.zero;
    private float forwardSpeed = 10f;
    private Vector3 PlayerStartingPosition = new Vector3(0, 42.28f, 91.28f);  //TO DO: must be determined
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
        roadWidth = PlatformHalfWidth - PlatformLineWidth - PlayerWidth - ExtraRoadWidthReduction;
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            if (!heldDown)
            {
                heldDown = true;
                touchedPosition = Input.mousePosition;
                StartCoroutine(EvaluateUserInput());
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            heldDown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SecondStageFinish"))
        {
            Stop();

        }
    }

    private IEnumerator EvaluateUserInput()
    {
        while (true)
        {
            if (!SecondStageManager.instance.DidSecondStageEnd)
            {
                transform.position += new Vector3((Input.mousePosition.x - touchedPosition.x) / 300f * roadWidth, 0, 0);

                float positionX = Mathf.Clamp(transform.position.x, -7.5f, 7.5f);

                transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
                touchedPosition = Input.mousePosition;
            }
            if (!heldDown)
            {
                yield break;
            }
            yield return new WaitForFixedUpdate();


        }
    }

    public void Stop()
    {
        characterRigidbody.velocity = Vector3.zero;
    }
    public void ResetPlayer()
    {
        transform.position = PlayerStartingPosition;

    }
    public void Move()
    {
        animator.SetBool("IsRunning", true);
        characterRigidbody.velocity = Vector3.forward * forwardSpeed;

    }

}
