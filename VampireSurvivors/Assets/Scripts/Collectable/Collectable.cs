using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            //Debug.Log("OnTriggerEnter2D");
            if(CanBeCollected(player))
                HandleDestroy();
        }
    }

    protected virtual bool CanBeCollected(Player _player)
    {
        return false;
    }

    protected virtual void HandleDestroy()
    {
        Destroy(this.gameObject);
    }
}
