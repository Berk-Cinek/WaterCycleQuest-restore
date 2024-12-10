using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    [SerializeField] private bool requireKeyPress = true;                         // Tu�a bas�lmas� gereksinimini inspector'e ekler
    private bool isPlayerNearby = false;
    public Animator transition;
    public float transitionTime = 1f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))                            // E�er tu�a bas�lmas� gerekmiyorsa player'i collider i�ine girdi�inde sonraki scene ���nlar
        {
            isPlayerNearby = true;

            if (!requireKeyPress)                                               
            {
                LoadNextScene();
            }
        }
    }
    private void Update()
    {
        if (requireKeyPress && isPlayerNearby && Input.GetKeyDown(KeyCode.E))    // E�er tu�a bas�lmas� gerekiyorsa ve E tu�una bas�l�rsa player'i sonraki scene ���nlar
        {
            LoadNextScene();
        }
    }
    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Son sahneye ula��ld�");
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
    }

}