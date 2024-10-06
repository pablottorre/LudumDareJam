using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float _rotationSpeed;


    [SerializeField] private float cdPressButton;
    private bool canJump = true;
    private bool canMove = true;

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
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canMove = false;
            canJump = false;
            StartCoroutine(cdJump());

            if (buttonBelow)
            {
                _pm.JumpPlayer(true);
            }
            else
            {
                _pm.JumpPlayer(false);
            }
        }


        else if (canMove)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            moveDirection = Vector3.right * moveX + Vector3.forward * moveZ;
            moveDirection.Normalize();
            moveDirection.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotationSpeed * Time.deltaTime);
            _pm.MovePlayer(moveDirection * moveSpeed);
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

                objectCarrying = objectsInRange.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                    .First().gameObject.GetComponent<Item>();

                objectCarrying.Interact(true, !objectCarrying.canBeScanned, _mouthPoint);

                isCarryingItem = true;
            }
        }
    }

    private void FixedUpdate()
    {
    }

    IEnumerator cdJump()
    {
        yield return new WaitForSecondsRealtime(cdPressButton);
        canJump = true;
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
                objectCarrying.canBeScanned,
                objectCarrying.Type,
                objectCarrying.GetterActualCode(),
                objectCarrying.hasBeenCashed);
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

    public void SetterCanMove(bool value)
    {
        canMove = value;
    }
}