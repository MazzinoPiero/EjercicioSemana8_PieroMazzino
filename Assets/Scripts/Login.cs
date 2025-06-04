using UnityEngine;
using GameJolt.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    void Start()
    {
        GameJoltUI.Instance.ShowSignIn((success) =>
        {
            if (success)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Debug.LogError("An error has ocurred while trying to log in");
            }
        });
    }
}