using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //OnTriggerEnter
    //give the player a coin
    //destroy this object

    [SerializeField] float rotateSpeed = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                Debug.Log("UDPATING COINS");
                player.AddCoins();
            }

            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

}
