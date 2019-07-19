using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCore : MonoBehaviour {

    #region 변수
    [Title("키 세팅")]
    public KeyCode leftmove;
    public KeyCode rightmove;
    public KeyCode jump;
    public KeyCode down;
    public KeyCode attack;
    public KeyCode throwP;
    public KeyCode dash;

    [Title("물리속성")]
    public float Hor_Acceleration = 0.5f;
    public float Hor_MaxSpeed = 5f;
    public float JumpVelocity = 6f;
    public float AirJumpVelocity = 6f;
    public float JumpDuration = 0.25f;
    public float MidairJumpVelocity = 10f;
    public float Vrt_MaxSpeed = 20f;

    [Title("기타 속성")]
    public LayerMask TerrainLayer;              // 플레이어가 딛는 땅을 나타내는 레이어를 적용시켜야 합니다.
    public float GroundRCDistance;              // 땅과 얼마나 가까이 있을때 땅 위에 딛는 판정을 할지 결정합니다.
    public float ForwardRCDistance;              
    public float StepInterval = 1f;             // 캐릭터가 발을 땅에 딛는 주기입니다.

    [Title("원점")]
    public Transform RCO_Foot;
    public Transform RCO_Forward;


    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_MoveKey;
    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_OnGround;
    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_VrtSpeed;


    [Title("프리팹")]
    public GameObject GroundStep;
    public GameObject WalljumpParticle;


    [HideInInspector]
    public bool AllowControl = true;
    [HideInInspector]
    public Vector2 headingTo;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D RBody;

    [HideInInspector]
    public PA_Dash a_Dash;
    [HideInInspector]
    public PA_PickaxeThrow a_PickaxeThrow;
    [HideInInspector]
    public PA_Attack a_Attack;

    [HideInInspector]
    public bool WallHanged = false;             // 캐릭터가 움직임이 고정되어있는지 여부입니다.
    [HideInInspector]
    public bool GroundDetact;
    [HideInInspector]
    public bool ForwardDetact;
    [HideInInspector]
    public bool AirJumped = false;

    private float AirTimer = 0;                     // 캐릭터가 공중에 있을때 증가하는 값입니다.
    private float StepTimer = 0;                    // 캐릭터가 걸을때 시간측정을 위한 값입니다.

    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
        RBody = GetComponent<Rigidbody2D>();
        a_Attack = GetComponent<PA_Attack>();
        a_PickaxeThrow = GetComponent<PA_PickaxeThrow>();
        a_Dash = GetComponent<PA_Dash>();
    }

    void FixedUpdate()
    {
        if (transform.rotation.eulerAngles.y == 0)
            headingTo = Vector2.right;
        if (transform.rotation.eulerAngles.y == 180)
            headingTo = Vector2.left;

        #region 레이캐스트 관리

        GroundDetact = Physics2D.Raycast(RCO_Foot.position, Vector2.down, GroundRCDistance, TerrainLayer);

        Debug.DrawLine(RCO_Foot.position, RCO_Foot.position + new Vector3(0, -GroundRCDistance, 0), Color.magenta);

        ForwardDetact = Physics2D.Raycast(RCO_Forward.position, headingTo, ForwardRCDistance, TerrainLayer);

        Debug.DrawLine(RCO_Forward.position, RCO_Forward.position + new Vector3(ForwardRCDistance,0, 0), Color.cyan);

        if (!GroundDetact)
            AirTimer += Time.deltaTime;
        else
            AirTimer = 0;

        #endregion

        #region 컨트롤

        if (AllowControl)
        {
            if (!WallHanged)
            {

                if (Input.GetKey(rightmove))
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    if (RBody.velocity.x < 0)
                        RBody.velocity = new Vector2(0, RBody.velocity.y);

                    if (RBody.velocity.x < Hor_MaxSpeed)
                        RBody.velocity += new Vector2(Hor_Acceleration, 0);

                    if (GroundDetact && StepTimer > StepInterval)
                        Step();
                    else
                        StepTimer += Time.deltaTime;
                }

                else if (Input.GetKey(leftmove))
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                    if (RBody.velocity.x > 0)
                        RBody.velocity = new Vector2(0, RBody.velocity.y);

                    if (RBody.velocity.x > -Hor_MaxSpeed)
                        RBody.velocity += new Vector2(-Hor_Acceleration, 0);

                    if (GroundDetact && (StepTimer > StepInterval))
                        Step();
                    else
                        StepTimer += Time.deltaTime;

                }
            }

        }

        // ========================= 기타 컨트롤 보정 ===================================


        if ( !AllowControl || !(Input.GetKey(leftmove) || Input.GetKey(rightmove)))
        {
            if (GroundDetact)
            RBody.velocity = Vector2.Lerp(RBody.velocity, new Vector2(0, RBody.velocity.y), 0.25f);
            else
            RBody.velocity = Vector2.Lerp(RBody.velocity, new Vector2(0, RBody.velocity.y), 0.1f);
        }

        LimitVerticalSpeed(-Vrt_MaxSpeed, Vrt_MaxSpeed);

        if (GroundDetact)
            AirJumped = false;

        #endregion

    }

    //========================================================================================

    void Update()
    {
        if (Input.GetKey(jump))
            Jump();

        if (Input.GetKeyDown(jump) && !GroundDetact && !AirJumped)
        {
            AirJump();
        }

        //애니메이션 관리
        anim.SetBool(AniPar_MoveKey, AllowControl && (Input.GetKey(rightmove) || Input.GetKey(leftmove)));
        anim.SetBool(AniPar_OnGround, GroundDetact);
        anim.SetFloat(AniPar_VrtSpeed, RBody.velocity.y);
    }

    //========================================================================================

    public void FixCharacter()
    {
        RBody.velocity = Vector2.zero;
        WallHanged = true;
    }

    public void UnfixCharacter()
    {
        WallHanged = false;
    }

    public void Jump()
    {
        if (AirTimer < JumpDuration && RBody.velocity.y >= -0.1f)
        {
            if (RBody.velocity.y == 0)
                Step();
            RBody.velocity = new Vector2(RBody.velocity.x, JumpVelocity);
        }
    }

    public void AirJump()
    {
        RBody.velocity = new Vector2(RBody.velocity.x, AirJumpVelocity);
        AirJumped = true;
    }

    public void Step()
    {
        StepTimer = 0f;
        if (Mathf.Abs(RBody.velocity.x) > Hor_MaxSpeed / 1.1)
            Instantiate(GroundStep, RCO_Foot.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }

    public void CancelAllActions()
    {
        a_Attack.CancelAttack();
        a_PickaxeThrow.CancelThrow();
    }

    //========================================================================================

    private void LimitVerticalSpeed(float min, float MAX)
    //캐릭터의 수직속도를 최대이상으로 넘어가지 않도록 합니다.
    {
        if (RBody.velocity.y < min)
            RBody.velocity = new Vector2(RBody.velocity.x, min);
        if (RBody.velocity.y > MAX)
            RBody.velocity = new Vector2(RBody.velocity.x, MAX);
    }

    //========================================================================================

    public Vector3 GetMouseWorldpos()
    {
        float d;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z));
        xy.Raycast(ray, out d);
        return ray.GetPoint(d);

    }

}
