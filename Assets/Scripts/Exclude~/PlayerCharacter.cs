using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour {

    [Title("키 세팅")]
    public KeyCode leftmove;
    public KeyCode rightmove;
    public KeyCode jump;
    public KeyCode down;
    public KeyCode attack;

    [Title("물리속성")]
    public float Hor_Acceleration;
    public float Hor_MaxSpeed;
    public float JumpVelocity;
    public float JumpDuration;
    public float Vrt_MaxSpeed;
    public float LedgeJumpForce;

    [Space]
    public LayerMask TerrainLayer;              // 플레이어가 딛는 땅을 나타내는 레이어를 적용시켜야 합니다.
    public float GroundRCDistance;              // 땅과 얼마나 가까이 있을때 땅 위에 딛는 판정을 할지 결정합니다.
    //public float LedgeRCDistance;               // 모서리 잡기를 판정하기 위한 수평거리입니다.

    [Title("레이케스트 원점")]
    public Transform RCO_Foot;
    //public Transform RCO_LedgeUpper;
    //public Transform RCO_LedgeBelow;


    [Title("애니메이션 파라미터")]
    public string AniPar_MoveKey;
    public string AniPar_OnGround;
    public string AniPar_VrtSpeed;
    public string AniPar_LedgeGrab;
    


    [HideInInspector]
    public bool AllowControl = true;
    //public bool IsGrabbingLedge;

    private Animator anim;
    private Rigidbody2D RBody;

    private bool GroundDetact;
    //private bool LedgeGrabDetact;

    private Vector2 forwarding;

    //private float LedgeGrabTerm = 0;                // 모서리 잡기 직후 조작을 막기 위해 있는 값입니다.
    private float AirTime = 0;                      // 캐릭터가 공중에 있을때 증가하는 값입니다.

    void Start()
    {
        anim = GetComponent<Animator>();
        RBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (transform.rotation.eulerAngles.y == 0)
            forwarding = Vector2.left;
        if (transform.rotation.eulerAngles.y == 180)
            forwarding = Vector2.right;

        //========================= 레이캐스트 관리 ===================================
        GroundDetact = Physics2D.Raycast(RCO_Foot.position, Vector2.down, GroundRCDistance, TerrainLayer);
        //LedgeGrabDetact = !GroundDetact &&
        //                Physics2D.Raycast(RCO_LedgeBelow.position, forwarding, LedgeRCDistance, TerrainLayer) &&
        //               !Physics2D.Raycast(RCO_LedgeUpper.position, forwarding, LedgeRCDistance, TerrainLayer);


        Debug.DrawLine(RCO_Foot.position, RCO_Foot.position + new Vector3(0, -GroundRCDistance, 0), Color.magenta);
        //Debug.DrawRay(RCO_LedgeUpper.position, forwarding, Color.red);
        //Debug.DrawRay(RCO_LedgeBelow.position, forwarding, Color.red);

        if (!GroundDetact)
            AirTime += Time.deltaTime;
        else
            AirTime = 0;

        // ========================= 컨트롤 ===================================

        if (AllowControl)
        {
            //if (!IsGrabbingLedge){
            if (Input.GetKey(rightmove))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);

                if (RBody.velocity.x < 0)
                    RBody.velocity = new Vector2(0, RBody.velocity.y);

                if (RBody.velocity.x < Hor_MaxSpeed)
                    RBody.velocity += new Vector2(Hor_Acceleration, 0);
            }
            else if (Input.GetKey(leftmove))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

                if (RBody.velocity.x > 0)
                    RBody.velocity = new Vector2(0, RBody.velocity.y);

                if (RBody.velocity.x > -Hor_MaxSpeed)
                    RBody.velocity += new Vector2(-Hor_Acceleration, 0);
            }

            if (Input.GetKey(jump))
            {
                Jump();
            }

            if (Input.GetKey(attack) && GroundDetact)
            {

            }

            // ========================= 모서리잡기 ===================================

            //    if (IsGrabbingLedge)
            //    {
            //        LedgeGrabTerm += Time.deltaTime;

            //        if (LedgeGrabTerm > 0.1f)
            //        {
            //            if (Input.GetKey(down))
            //                ReleaseLedge();
            //            else if (Input.GetKey(jump))
            //                Jump();
            //        }
            //    }
            //}

            //if (LedgeGrabDetact)
            //{
            //    GrabLedge();
            //}

            // ========================= 기타 컨트롤 보정 ===================================

            if (GroundDetact)
            {
                if (!AllowControl || !(Input.GetKey(leftmove) || Input.GetKey(rightmove)))
                {
                    RBody.velocity = new Vector2(0, RBody.velocity.y);
                    //RBody.velocity = Vector2.Lerp(RBody.velocity, Vector2.zero, 0.3f);
                }
            }

            LimitVerticalSpeed(-Vrt_MaxSpeed, Vrt_MaxSpeed);

        }
    }

    //========================================================================================

    void Update()
    {
        //애니메이션 관리
        anim.SetBool(AniPar_MoveKey, AllowControl && (Input.GetKey(rightmove) || Input.GetKey(leftmove)));
        anim.SetBool(AniPar_OnGround, GroundDetact);
        anim.SetFloat(AniPar_VrtSpeed, RBody.velocity.y);
        //anim.SetBool(AniPar_LedgeGrab, IsGrabbingLedge);
    }

    //========================================================================================

    public void Jump()
    {
        if (AirTime < JumpDuration && RBody.velocity.y >= -0.1f)
        {
            if (RBody.velocity.y == 0)
                StepSound();
            RBody.velocity = new Vector2(RBody.velocity.x, JumpVelocity);

        }

        //if (IsGrabbingLedge)
        //{
        //    ReleaseLedge();
        //    RBody.velocity = new Vector2(RBody.velocity.x, LedgeJumpForce);
        //}

    }

    public void SwordAttack()
    {

    }

    public void StepSound()
    {

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

    //private void GrabLedge()
    //{

    //    if (!IsGrabbingLedge && RBody.velocity.y < 0)
    //    {
    //        if ((forwarding == Vector2.right && Input.GetKey(rightmove)) || (forwarding == Vector2.left && Input.GetKey(leftmove)))
    //        {
    //            IsGrabbingLedge = true;
    //            RBody.constraints = RigidbodyConstraints2D.FreezePosition;
    //        }
            
    //    }
    //}

    //public void ReleaseLedge()
    //{

    //    if (IsGrabbingLedge)
    //    {
    //        IsGrabbingLedge = false;
    //        RBody.constraints = RigidbodyConstraints2D.None;
    //        RBody.constraints = RigidbodyConstraints2D.FreezeRotation;

    //        if (Input.GetKey(down))
    //        {
    //            transform.Rotate(0, 180, 0);

    //            if (forwarding == Vector2.left)
    //                RBody.velocity = new Vector2(Hor_MaxSpeed / 2, RBody.velocity.y);
    //            if (forwarding == Vector2.right)
    //                RBody.velocity = new Vector2(-Hor_MaxSpeed / 2, RBody.velocity.y);
    //        }
    //        else if (Input.GetKey(rightmove))
    //            RBody.velocity = new Vector2(Hor_MaxSpeed, RBody.velocity.y);
    //        else if (Input.GetKey(leftmove))
    //            RBody.velocity = new Vector2(-Hor_MaxSpeed, RBody.velocity.y);

    //        LedgeGrabTerm = 0f;
    //    }
    //}

    //========================================================================================

}
