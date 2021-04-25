using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Transform SpawnPoint;

    [HideInInspector]
    public bool isDead = true;

    public static PlayerManager playerInstance;

    public static GameManager instance;

    public void PlayerDie() {
        isDead = true;
        InGameUserInterface.instance.Die();
        StartCoroutine("death");
    }
    IEnumerator death()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * 3f;
            AudioListener.volume = t;
            yield return 0;

        }
        yield return new WaitForSeconds(5f);
        AudioListener.volume = 1;
        SceneFader.instance.fadeToNewScene(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        Vector3 point = SpawnPoint.position;
        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 10))
            point = hit.point + Vector3.up * Player.transform.localScale.y * 1.5f;
        isDead = false;
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
