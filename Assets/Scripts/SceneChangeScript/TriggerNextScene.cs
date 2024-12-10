using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    [SerializeField] private bool requireKeyPress = true;                         // Tuþa basýlmasý gereksinimini inspector'e ekler
    private bool isPlayerNearby = false;
    public Animator transition;
    public float transitionTime = 1f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))                            // Eðer tuþa basýlmasý gerekmiyorsa player'i collider içine girdiðinde sonraki scene ýþýnlar
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
        if (requireKeyPress && isPlayerNearby && Input.GetKeyDown(KeyCode.E))    // Eðer tuþa basýlmasý gerekiyorsa ve E tuþuna basýlýrsa player'i sonraki scene ýþýnlar
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
            Debug.LogWarning("Son sahneye ulaþýldý");
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
    }

}