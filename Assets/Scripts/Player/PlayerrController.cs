using System;
using System.Linq;
using UnityEngine;

public class PlayerrController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _pm;

    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;
    private float originalSpeed;
    [SerializeField] private float slowedSpeed;
    [SerializeField] private float fastSpeed;

    private bool buttonBelow = false;
    private bool isInRangeOfItem = false;
    private Item objectInRange = null;
    private bool isCarryingItem = false;
    private Item objectCarrying = null;

    [SerializeField] private Transform _mouthPoint;

    private void Awake()
    {
        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuy);
    }

    void Start()
    {
        originalSpeed = moveSpeed;
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = Vector3.right * moveX + Vector3.forward * moveZ;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (buttonBelow)
            {
                EventManager.TriggerEvent(EventNames._PressButton);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isCarryingItem)
            {
                isCarryingItem = false;
                objectCarrying.Interact(false);
                objectCarrying = null;
            }
            else
            {
                var objectsInRange = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Item"));

                if (!objectsInRange.Any()) return;

                isCarryingItem = true;
                objectCarrying = objectsInRange.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                    .First().gameObject.GetComponent<Item>();
                objectCarrying.Interact(true, _mouthPoint);
                objectInRange = null;
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

        else if (other.gameObject.layer == 8 && isCarryingItem)
        {
            EventManager.TriggerEvent(EventNames._CheckForLaser,
                objectCarrying.Cost,
                objectCarrying.Name,
                objectCarrying.canBeScaned,
                objectCarrying.Type,
                objectCarrying.GetterActualCode());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            buttonBelow = false;
        }
    }

    private void OnFinishBuy(params object[] parameters)
    {
        isCarryingItem = false;
        objectCarrying.Interact(false);
        objectCarrying = null;
    }
}