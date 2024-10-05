using UnityEngine;

public class PlayerrController : MonoBehaviour
{

    [SerializeField] private PlayerMovement _pm;

    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ;
    }

    private void FixedUpdate()
    {
        _pm.MovePlayer(moveDirection * moveSpeed);
    }


}
