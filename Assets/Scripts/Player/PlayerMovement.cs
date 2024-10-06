using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody _rb;
    [SerializeField] private PlayerrController _pc;
    [SerializeField] private GameObject modelPlayer;
    [SerializeField] private float timerJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void MovePlayer(Vector3 dir)
    {
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public void JumpPlayer(bool isButtonBelow)
    {
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

        if (goingToPressButton)
        {
            EventManager.TriggerEvent(EventNames._PressButton);
        }
    }
}
