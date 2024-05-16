using UnityEngine;

public class HintButton : MonoBehaviour
{
    public BallSpawner ballSpawner;

    public void OnHintButtonClicked()
    {
        ballSpawner.ShowHintWord();
    }
}
