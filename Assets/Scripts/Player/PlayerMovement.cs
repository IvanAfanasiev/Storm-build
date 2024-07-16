using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Joystick joystick;
    public Rigidbody rb;

    bool isPlayingAnimation = false;

    Animator anim;

    [SerializeField]
    Inventory inventory;

    public ITrigger lastSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isPlayingAnimation)
            return;

        Movement();
    }

    public void Movement()
    {
        Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
        rb.velocity = new Vector3(dir.x, 0, dir.y) * speed;
    }



    public void StartAction()
    {
        isPlayingAnimation = true;
    }

    public void FinishAction()
    {
        isPlayingAnimation = false;

        lastSource.ActionChanges();
        lastSource = null;
    }
}