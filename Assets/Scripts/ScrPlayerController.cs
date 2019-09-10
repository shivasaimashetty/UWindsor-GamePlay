using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScrPlayerController : MonoBehaviour {

    #region Define Variables

    // Animator Parameter
    private const string SpeedAnimFloat = "Speed";

    // Animator Jump Triggers
    private const string JumpAnimTrigger = "Jump";
    private const string JumpFallAnimTrigger = "Fall";
    private const string JumpEndAnimTrigger = "JumpEnd";
    private const string DamageAnimTrigger = "Damage";

    // Axis
    private const string HorAxis = "Horizontal";
    private const string VerAxis = "Vertical";
    private const string MouseXAxis = "Mouse X";

    // Tags 
    private const string EnemyTag = "Enemy";
    private const string DeathTag = "Death";

    private enum JumpStateEnum {None, Start, Fall};

    protected CharacterController cc;
    protected Transform tr;
    protected Animator animator;

    public float fwdSpeed = 3;
    public float bwdSpeed = 1;
    private float rightSpeed = 3;
    public float rotateSpeed = 30;

    public float gravitySpeed = 3;
    private float gravityValue = 0;

    public float jumpValue = 5;
    private bool jumping;
    private JumpStateEnum jumpState;

    private float crossFadeAnimation = .3f;
    private bool damaging;
    #endregion

    #region MonoFunc

    protected void Awake()
    {
        cc = GetComponent<CharacterController>();
        tr = transform;
        animator = GetComponent<Animator>();

        //Physics.gravity = Vector3.zero;
    }

    protected void Update()
    {
        MovementProccess();
    }

    protected void OnTriggerEnter(Collider col)
    {
        switch(col.tag)
        {
            case EnemyTag:
                ApplyDamage();
                break;

            case DeathTag:
                ApplyDead();
                break;
        }
    }

    #endregion

    #region MyFunc

    private void MovementProccess()
    {
        
        GravityProccess();


        float verSpeed = Input.GetAxis(VerAxis);
        verSpeed = verSpeed > 0 ? verSpeed * fwdSpeed : verSpeed * bwdSpeed;

        cc.Move((gravityValue * -tr.up + verSpeed * tr.forward + 0 * tr.right * rightSpeed) * Time.deltaTime);

        tr.Rotate(new Vector3(0, Input.GetAxis(HorAxis) * rotateSpeed * Time.deltaTime));

        animator.SetFloat(SpeedAnimFloat, Input.GetAxis(VerAxis));
    }

    private void GravityProccess()
    {
        gravityValue = cc.isGrounded && gravityValue > 0 ? 0 : gravityValue + gravitySpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            ApplyJump();

        if (jumping)
        {
            switch(jumpState)
            {
                case JumpStateEnum.Start:
                    if (gravityValue >= 0)
                    {
                        animator.SetTrigger(JumpFallAnimTrigger);
                        jumpState = JumpStateEnum.Fall;
                    }
                    break;

                case JumpStateEnum.Fall:
                    if (gravityValue == 0)
                    {
                        animator.SetTrigger(JumpEndAnimTrigger);
                        jumping = false;
                        jumpState = JumpStateEnum.None;
                    }
                    break;
            }
        }
    }

    private void ApplyJump()
    {
        gravityValue = -jumpValue;
        jumping = true;
        animator.SetTrigger(JumpAnimTrigger);
        jumpState = JumpStateEnum.Start;
    }

    private void AnimationPlay(string nameState)
    {
        animator.CrossFade(nameState, crossFadeAnimation);
    }

    private void ApplyDamage()
    {
        animator.SetTrigger(DamageAnimTrigger);
    }

    public void ApplyDead()
    {
        ScrGameManager.Instance.Respawn(tr);
    }

    #endregion
}
