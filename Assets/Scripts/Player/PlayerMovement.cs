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
        _rb.MovePosition(_rb.position + dir * Time.fixedDeltaTime);
    }
}
