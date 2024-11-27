using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    [SerializeField] private bool requireKeyPress = true;                         // Tuþa basýlmasý gereksinimini inspector'e ekler
    private bool isPlayerNearby = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (!requireKeyPress)                                                // Eðer tuþa basýlmasý gerekmiyorsa player'i collider içine girdiðinde sonraki scene ýþýnlar
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
}