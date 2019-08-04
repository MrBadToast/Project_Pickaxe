using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObject : MonoBehaviour
{
    bool Enabled = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Enabled)
        {
            StartCoroutine(HurtBySpike(collision.GetComponent<PlayerCore>()));
        }
    }

    private IEnumerator HurtBySpike(PlayerCore player)
    {
        player.Hurt(1, player.transform.position + new Vector3(Random.Range(-1.0f,1.0f), -2f));
        Enabled = false;
        player.ControlAllowed = false;
        yield return new WaitForSeconds(0.2f);


        if (player.CurrentHealth > 0)
        {
            ChapterManager.Instance.PlayerToLastPoint();
            player.RBody.velocity = Vector2.zero;

            yield return new WaitForSeconds(1.0f);
            player.ControlAllowed = true;
            Enabled = true;
        }
    }
}
