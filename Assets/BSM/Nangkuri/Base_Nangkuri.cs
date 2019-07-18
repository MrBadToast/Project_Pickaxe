using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Base_Nangkuri : MonoBehaviour
{
    public float Speed = 2.0f;
    public float RunSpeed = 3.0f;
    [Space]
    public float RCDistance_Foot;
    public float RCDistance_Forward;

    public Transform RCO_Foot;
    public Transform RCO_Forward;

    Vector2 headingTo;

    bool detactFoot;
    bool detactForward;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawLine(RCO_Foot.position, new Vector2(RCO_Foot.position.x, RCO_Foot.position.y - RCDistance_Foot));
        Debug.DrawLine(RCO_Foot.position, new Vector2(RCO_Forward.position.x + headingTo.x*RCDistance_Forward, RCO_Forward.position.y));

        if (transform.rotation.eulerAngles.y == 180)
            headingTo = Vector2.right;
        else if (transform.rotation.eulerAngles.y == 0)
            headingTo = Vector2.left;
    }

    public void UpdateRays()
    {
        detactFoot = Physics2D.Raycast(RCO_Foot.position, Vector2.down, RCDistance_Foot);
        detactForward = Physics2D.Raycast(RCO_Forward.position, headingTo, RCDistance_Foot);
    }



}
