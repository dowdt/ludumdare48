using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Transform SpawnPoint;

    public static PlayerManager playerInstance;

    public static GameManager instance;


    private void Awake()
    {
        Vector3 point = SpawnPoint.position;
        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 10))
            point = hit.point + Vector3.up * Player.transform.localScale.y * 1.5f;
        playerInstance = Instantiate(Player,point , SpawnPoint.rotation).GetComponent<PlayerManager>();
        instance = this;
    }

    private void OnDrawGizmos()
    {
        Vector3 point = SpawnPoint.position;
        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 10))
        {
            point = hit.point + Vector3.up * Player.transform.localScale.y * 1.5f;
            Gizmos.DrawLine(SpawnPoint.position,point);
        }
        Gizmos.DrawIcon(SpawnPoint.position,"PlayerSpawn");
        if (SpawnPoint)
            Gizmos.DrawWireCube(point,new Vector3(Player.transform.localScale.x, Player.transform.localScale.y*3, Player.transform.localScale.z));
    }
}
