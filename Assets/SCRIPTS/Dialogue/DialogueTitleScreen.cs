using UnityEngine;

public class DialogueTitleScreen : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.Instance.PlaySFXWithSource(Random.Range(0, 5), 0.5f);
    }
}
