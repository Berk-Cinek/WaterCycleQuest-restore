using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SavePanel : MonoBehaviour
{
    public GameObject saveSuccessPanel;

    public void OkayButton()
    {
        saveSuccessPanel.SetActive(false);
    }

}