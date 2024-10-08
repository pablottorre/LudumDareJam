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
    private bool isCarryingItem = false;
    private Item objectCarrying = null;

    [SerializeField] private Transform _mouthPoint;

    [SerializeField] private float _rotationSpeed;


    [SerializeField] private float cdPressButton;
    private bool canJump = true;
    private bool canMove = true;

    private bool hasStartedDay = false;

    private void Awake()
    {
        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuy);
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, HasStartedNewDay);
        EventManager.SubscribeToEvent(EventNames._OnEndNewDay, HasFinishedNewDay);
    }

    void Start()
    {
        originalSpeed = moveSpeed;
    }


    void Update()
    {
        if (!hasStartedDay) return;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canMove = false;
            canJump = false;

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
            if (!isCarryingItem || objectCarrying.canBeScanned)
            {
                if (moveDirection.magnitude != 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection),
                        _rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                var objectDir =objectCarrying.transform.position - transform.position;
                objectDir.y = 0;
                transform.forward = objectDir;
            }

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
        objectCarrying?.Interact(false);
        objectCarrying = null;
    }

    public void SetterCanMove(bool value)
    {
        canMove = value;
        canJump = value;
    }

    private void HasStartedNewDay(params object[] parameters)
    {
        hasStartedDay = true;
    }
    
    private void HasFinishedNewDay(params object[] parameters)
    {
        hasStartedDay = false;
    }
}