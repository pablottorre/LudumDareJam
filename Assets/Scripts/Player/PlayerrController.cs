using UnityEngine;

public class PlayerrController : MonoBehaviour
{

    [SerializeField] private PlayerMovement _pm;

    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    private bool buttonBelow = false;
    private bool isInRangeOfItem = false;
    private GameObject objectInRange = null;
    private bool isCarryingItem = false;
    private GameObject objectCarrying = null;

    void Start()
    {

    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.Space))
        {
            if (buttonBelow)
            {
                EventManager.TriggerEvent(EventNames._PressButton);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (isInRangeOfItem && !isCarryingItem)
            {
                EventManager.TriggerEvent(EventNames._GrabObject, objectInRange);
                isCarryingItem = true;
                objectCarrying = objectInRange;
                objectInRange = null;
            }
            else if (isCarryingItem)
            {
                EventManager.TriggerEvent(EventNames._ReleaseObject, objectInRange);
                isCarryingItem = false;
                objectCarrying = null;
            }
        }
    }

    private void FixedUpdate()
    {
        _pm.MovePlayer(moveDirection * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && !isCarryingItem)
        {
            isInRangeOfItem = true;
            objectInRange = other.gameObject;
        }


        else if (other.gameObject.layer == 7)
        {
            buttonBelow = true;
        }

        else if (other.gameObject.layer == 8 && isCarryingItem)
        {
            EventManager.TriggerEvent(EventNames._CheckForLaser);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isInRangeOfItem = false;
            objectInRange = null;
        }

        else if (other.gameObject.layer == 7)
        {
            buttonBelow = false;
        }
    }


    public void GrabObject()
    {
        EventManager.TriggerEvent(EventNames._GrabObject);
    }

    public void ReleaseObject()
    {
        EventManager.TriggerEvent(EventNames._ReleaseObject);
    }


}
