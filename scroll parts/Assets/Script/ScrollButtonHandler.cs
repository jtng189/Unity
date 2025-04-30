using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonHandler : MonoBehaviour
{
    private ScrollClickSpawner spawner;

    public void SetSpawner(ScrollClickSpawner sourceSpawner)
    {
        spawner = sourceSpawner;
    }

    public void OnButtonClick()
    {
        if (spawner != null)
        {
            spawner.DeleteSpawnedScroll();
        }
        else
        {
            Debug.LogWarning("Spawner not assigned in ScrollButtonHandler.");
        }
    }
}
