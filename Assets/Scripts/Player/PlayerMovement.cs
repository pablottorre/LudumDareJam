using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody _rb;
    [SerializeField] private PlayerrController _pc;
    [SerializeField] private GameObject modelPlayer;
    [SerializeField] private float timerJump;

    private Vector3 _modelPos;

    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _keysLayer;
    
    private void Start()
    {
        _modelPos = modelPlayer.transform.localPosition;
        _rb = GetComponent<Rigidbody>();
    }


    public void MovePlayer(Vector3 dir)
    {
        modelPlayer.transform.localPosition = _modelPos;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public void JumpPlayer(bool isButtonBelow)
    {
        _rb.linearVelocity = Vector3.zero;
        LeanTween.move(modelPlayer, modelPlayer.transform.position + new Vector3(0,0.5f,0), timerJump).setEaseOutCubic()
            .setOnComplete(() => PlayerFinishJump(isButtonBelow));
    }

    private void PlayerFinishJump(bool value)
    {
        LeanTween.move(modelPlayer, modelPlayer.transform.position - new Vector3(0, 0.5f, 0), timerJump).setEaseInCubic().
            setOnComplete(()=>PlayerCompletedJump(value));
    }

    private void PlayerCompletedJump(bool goingToPressButton)
    {
        _pc.SetterCanMove(true);

        if (Physics.Raycast(transform.position, Vector3.down, out var raycastHit, _checkDistance, _keysLayer)
            || Physics.Raycast(transform.position + Vector3.right, Vector3.down, out raycastHit, _checkDistance, _keysLayer)
            || Physics.Raycast(transform.position + Vector3.left, Vector3.down, out raycastHit, _checkDistance, _keysLayer)
            || Physics.Raycast(transform.position + Vector3.forward, Vector3.down, out raycastHit, _checkDistance, _keysLayer)
            || Physics.Raycast(transform.position + Vector3.back, Vector3.down, out raycastHit, _checkDistance, _keysLayer))
        {
            raycastHit.collider.GetComponent<Keyboard>().PressButonRegistery();
        }
    }
}
