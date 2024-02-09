using SupersonicWisdomSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    private void Awake()
    {
        SupersonicWisdom.Api.AddOnReadyListener(load);
        SupersonicWisdom.Api.Initialize();
    }
    void load()
    {
        SceneManager.LoadScene("MergeScene");
    }
}
