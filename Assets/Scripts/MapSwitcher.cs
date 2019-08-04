using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcher : MonoBehaviour
{
    public MapInfo Map1;
    public Transform Map1_EnterPos;
    [Space]
    public MapInfo Map2;
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
        if (Map1.MapObject.activeInHierarchy)
        {
            Map2.MapObject.SetActive(true);
            MainVirtualCamera.Instance.SetConfinerColider(Map2.camColider);
            collision.transform.position = Map2_EnterPos.position;
            Map1.MapObject.SetActive(false);
        }
        else if (Map2.MapObject.activeInHierarchy)
        {
            Map1.MapObject.SetActive(true);
            MainVirtualCamera.Instance.SetConfinerColider(Map1.camColider);
            collision.transform.position = Map1_EnterPos.position;
            Map2.MapObject.SetActive(false);
        }

        
    }
}
