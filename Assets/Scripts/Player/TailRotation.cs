using UnityEngine;

public class TailRotation : MonoBehaviour
{
    [SerializeField] private GameObject tail;
    [SerializeField] private float timer;


    void Start()
    {
        RotateLeft();
    }

    private void RotateLeft()
    {
        LeanTween.rotateX(tail, 326,timer).setEaseInOutCubic().setOnComplete(()=> RotateRight());
    }

    private void RotateRight()
    {
        LeanTween.rotateX(tail, 386, timer).setEaseInOutCubic().setOnComplete(() => RotateLeft());
    }

}
