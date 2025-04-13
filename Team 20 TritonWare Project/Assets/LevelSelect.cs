using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LevelLoad (int level)
    {
        // var tmp = transform;
        // Debug.Log(tmp.position);
        // print(tmp.position);
        // print("hihi");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
