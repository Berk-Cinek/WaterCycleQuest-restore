using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    public GameObject QuitMenuCanvas;

    public void YesButton()
    {
        Application.Quit();
    }

    public void NoButton()
    {
        QuitMenuCanvas.SetActive(false);
    }
}
