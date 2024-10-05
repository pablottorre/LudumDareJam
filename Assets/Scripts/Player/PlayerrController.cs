using UnityEngine;

public class PlayerrController : MonoBehaviour
{

    [SerializeField] private PlayerMovement _pm;

    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    private bool buttonBelow = false;

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
    }

    private void FixedUpdate()
    {
        _pm.MovePlayer(moveDirection * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            buttonBelow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
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
