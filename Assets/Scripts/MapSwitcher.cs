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
    [Space]
    public bool keepY = false;
    public bool keepX = false;


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
            if (keepY == true)
                collision.transform.position = new Vector2(Map2_EnterPos.position.x, collision.transform.position.y);
            else if( keepX == true)
                collision.transform.position = new Vector2(collision.transform.position.x, Map2_EnterPos.position.y);
            else
                collision.transform.position = Map2_EnterPos.position;
            Map1.MapObject.SetActive(false);
        }
        else if (Map2.MapObject.activeInHierarchy)
        {
            Map1.MapObject.SetActive(true);
            MainVirtualCamera.Instance.SetConfinerColider(Map1.camColider);
            if (keepY == true)
                collision.transform.position = new Vector2(Map1_EnterPos.position.x, collision.transform.position.y);
            else if (keepX == true)
                collision.transform.position = new Vector2(collision.transform.position.x, Map1_EnterPos.position.y);
            else
                collision.transform.position = Map1_EnterPos.position;
            Map2.MapObject.SetActive(false);
        }
    }
}
