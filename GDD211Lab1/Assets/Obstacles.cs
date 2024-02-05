using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private Collider despawn;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private int score;
    [SerializeField] private PaceControls player;
    private void Update()
    {
        if (player.State == StateCheck.PLAY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.25f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (player.State == StateCheck.PLAY)
        {
            if (other.gameObject.tag == "Despawn")
            {
                transform.position = spawnLocation.position;
                score += 1;
            }
        }
    }
}
