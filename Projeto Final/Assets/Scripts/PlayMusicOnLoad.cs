using UnityEngine;

public class PlayMusicOnLoad : MonoBehaviour
{
    public string musicName;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(musicName);
        }
    }
}
