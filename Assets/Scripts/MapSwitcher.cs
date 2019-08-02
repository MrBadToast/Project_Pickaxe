using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcher : MonoBehaviour
{
    public GameObject Map1;
    public Collider2D Map1_CamColider;
    public Transform Map1_EnterPos;
    [Space]
    public GameObject Map2;
    public Collider2D Map2_CamColider;
    public Transform Map2_EnterPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SwitchMap(collision);
        }
    }

    private void SwitchMap(Collider2D collision)
    {
        if (Map1.activeInHierarchy)
        {
            Map2.SetActive(true);
            MainVirtualCamera.Instance.SetConfinerColider(Map2_CamColider);
            collision.transform.position = Map2_EnterPos.position;
            Map1.SetActive(false);
        }
        else if (Map2.activeInHierarchy)
        {
            Map1.SetActive(true);
            MainVirtualCamera.Instance.SetConfinerColider(Map1_CamColider);
            collision.transform.position = Map1_EnterPos.position;
            Map2.SetActive(false);
        }

        
    }
}
