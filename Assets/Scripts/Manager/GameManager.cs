using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{


    [HideInInspector]
    public bool isDead = true;

    public static PlayerManager playerInstance;

    public static GameManager instance;

    [SerializeField]
    string NextLevel;

    public void PlayerWin()
    {
        if(NextLevel == "")
        SceneFader.instance.fadeToNewScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name);
        else
        SceneFader.instance.fadeToNewScene(NextLevel);
    }

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
            t -= Time.deltaTime * 1f;
            AudioListener.volume = t;
            yield return 0;

        }
        yield return new WaitForSeconds(5f);
        AudioListener.volume = 1;
        SceneFader.instance.fadeToNewScene(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        GameObject Player = FindObjectOfType<PlayerManager>().gameObject;
        Vector3 point = Player.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 10))
            point = hit.point + Vector3.up * Player.transform.localScale.y * 1.5f;
        isDead = false;
        playerInstance = Player.GetComponent<PlayerManager>();
        Player.transform.position = point;
        instance = this;
    }

}
