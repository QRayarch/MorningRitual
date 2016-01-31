using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController1))]
public class Llama : MonoBehaviour
{

    [Header("Setup")]
    public Transform seatTransform;
    public Transform neckTransform;

    [Header("Head")]
    public GameObject head;
    public float min;
    public float max;
    public float headSpeed;

    [Header("Configure")]
    public float jumpOffForce = 1;

    private bool isSeated = false;
    private Transform playerTransform;
    private PlayerController1 playerController;
    private float originalScale = 1;

    private PlayerController1 animalController;

    private BoxCollider2D box;

    // Use this for initialization
    protected virtual void Awake()
    {
        animalController = GetComponent<PlayerController1>();
        animalController.enabled = false;

        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {

        if (isSeated)
        {
            if (Input.GetButtonDown("ActivateAnimal"))
            {
                Activated();
            }
            if (Input.GetButton("ActivateAnimal"))
            {
                ContinualUse();
            }
            if (Input.GetButtonUp("ActivateAnimal"))
            {
                Deactivated();
            }
            if (playerController.GetIsJumping())
            {
                Unseated();
                return;
            }
        }
        else
        {
            //Reset the neck!
            if (head.transform.localPosition.y > min)
            {
                //move the sprite down
                float currY = head.transform.localPosition.y;
                float newY = currY - headSpeed * Time.deltaTime;
                head.transform.localPosition = new Vector3(head.transform.localPosition.x, newY, head.transform.localPosition.z);

                //move the seat down
                currY = seatTransform.transform.localPosition.y;
                newY = currY - headSpeed * Time.deltaTime;

                seatTransform.transform.localPosition = new Vector3(seatTransform.transform.localPosition.x, newY, seatTransform.transform.localPosition.z);
            }
            //else //make sure no derp llama with short neck
            //{
            //head.transform.position = new Vector3(head.transform.position.x, min, head.transform.position.z);
            //seatTransform.transform.position = new Vector3(seatTransform.transform.position.x, seatTransform.transform.position.y, seatTransform.transform.position.z);
            //}
        }
        if (neckTransform != null)
        {
            Vector3 pos = neckTransform.localPosition;
            Vector3 headPos = head.transform.localPosition;
            Vector3 scale = neckTransform.localScale;
            scale.y = (headPos.y - pos.y) * 2 - 0.4f;
            neckTransform.localScale = scale;
        }
    }

    void LateUpdate()
    {
        if (isSeated)
        {
            playerTransform.position = seatTransform.position;
            Vector3 scale = playerTransform.localScale;
            float sign = Mathf.Sign(transform.localScale.x);
            scale.x = sign;
            playerTransform.localScale = scale;
        }
    }

    //Stuff to override
    protected virtual void OnSeated()
    {

    }

    protected virtual void OnUnSeated()
    {

    }

    //Fires only the first time the animal button is presssed
    protected virtual void Activated()
    {

    }

    //Fires continually while the animal is activated
    protected virtual void ContinualUse()
    {
        //if it is less than two move it!
        if (head.transform.localPosition.y < max)
        {
            //move the sprite
            float currY = head.transform.localPosition.y;
            float newY = currY + headSpeed * Time.deltaTime;
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, newY, head.transform.localPosition.z);

            //move the seat
            currY = seatTransform.transform.localPosition.y;
            newY = currY + headSpeed * Time.deltaTime;

            seatTransform.transform.localPosition = new Vector3(seatTransform.transform.localPosition.x, newY, seatTransform.transform.localPosition.z);
        }
    }

    //Fires when the button stops being pressed
    protected virtual void Deactivated()
    {

    }
    //End stuff to override

    private void Seated()
    {
        isSeated = true;
        originalScale = Mathf.Sign(playerTransform.localScale.x);
        animalController.enabled = true;
        playerController.enabled = false;
        OnSeated();
    }

    private void Unseated()
    {
        isSeated = false;
        Vector3 scale = playerTransform.localScale;
        scale.x = originalScale;
        playerTransform.localScale = scale;

        animalController.enabled = false;
        playerController.enabled = true;
        Rigidbody2D playerBody = playerTransform.GetComponent<Rigidbody2D>();
        if (playerBody)
        {
            playerBody.AddForce(Vector2.up * jumpOffForce, ForceMode2D.Impulse);
        }

        OnUnSeated();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger || isSeated) return;
        Transform root = other.transform.root.transform;
        if (root.CompareTag("Player"))
        {
            playerTransform = root;
            playerController = root.GetComponent<PlayerController1>();
            Seated();
        }
    }
}
