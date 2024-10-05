using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void MovePlayer(Vector3 dir)
    {
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }
}
