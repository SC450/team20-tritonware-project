using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LevelLoad (int level)
    {
        SceneManager.LoadScene(level+1);
        GameObject.FindGameObjectWithTag("Music").GetComponent<Ambience>().PlayMusic();
    }

    public void ReturnMainMenu ()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<Ambience>().PlayMusic();
    }
}
