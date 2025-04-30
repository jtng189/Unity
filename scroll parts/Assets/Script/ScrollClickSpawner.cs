using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollClickSpawner : MonoBehaviour
{
    public GameObject detailScrollPrefab; // SET THIS IN INSPECTOR TO THE "Detail_Scroll" PREFAB
    public float spawnDistance = 0.35f; // Distance in front of player to spawn (may need to be changed depending on the 'Detail_Scroll.fbx' scaling value)
    public GameObject playerCapsule; // SET THIS IN INSPECTOR TO THE "PlayerCapsule" PREFAB

    private Camera mainCamera;
    private GameObject spawnedScroll; 
    void Start()
    {
        if (playerCapsule == null)
        {
            playerCapsule = GameObject.Find("PlayerCapsule");
            if (playerCapsule == null)
            {
                Debug.LogError("PlayerCapsule not found in scene.");
                return;
            }
        }

        // Find the camera under PlayerCapsule
        mainCamera = playerCapsule.transform.Find("PlayerCameraRoot/Main Camera")?.GetComponent<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found under PlayerCapsule > PlayerCameraRoot.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click interaction (edit this later for VR inputs)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Scroll")
                {
                    SpawnDetailScroll();
                }
            }
        }
    }

    public void SpawnDetailScroll()
    {
        if (detailScrollPrefab != null && playerCapsule != null)
        {
            Vector3 spawnPosition = playerCapsule.transform.position + playerCapsule.transform.forward * spawnDistance;
            spawnPosition.y += 2.8f;

            Quaternion baseRotation = playerCapsule.transform.rotation;
            Quaternion offsetRotation = Quaternion.Euler(0, 90, -90);
            Quaternion finalRotation = baseRotation * offsetRotation;

            // Destroy existing scroll if present before creating a new one
            if (spawnedScroll != null)
            {
                Destroy(spawnedScroll);
            }

            spawnedScroll = Instantiate(detailScrollPrefab, spawnPosition, finalRotation);

            // Hook up the button handler with the correct spawner reference
            ScrollButtonHandler buttonHandler = spawnedScroll.GetComponentInChildren<ScrollButtonHandler>();
            if (buttonHandler != null)
            {
                buttonHandler.SetSpawner(this);
            }
            else
            {
                Debug.LogWarning("ScrollButtonHandler not found in spawned scroll.");
            }
        }
        else
        {
            Debug.LogWarning("Detail Scroll Prefab or PlayerCapsule is not assigned.");
        }
    }

    public void DeleteSpawnedScroll()
    {
        if (spawnedScroll != null)
        {
            Destroy(spawnedScroll);
            spawnedScroll = null;
        }
    }
}