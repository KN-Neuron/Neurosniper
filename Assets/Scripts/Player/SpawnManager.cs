using UnityEngine;
using System.Linq;
using Unity.Multiplayer.Center.Common; // To chyba jakaś pozostałość, ale zostawiam

public class SpawnManager : MonoBehaviour
{
    public GameObject existingPlayer;
    public GameObject existingTargetNPC;

    public Transform defaultSpawnPoint;

    private Transform[] spawnPoints;
    private Transform[] targetSpawnPoints;

    // Zmieniamy Awake na Start - bezpieczniej dla fizyki i pozycjonowania
    void Start()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        GameObject[] targetSpawnPointsObjects = GameObject.FindGameObjectsWithTag("TargetSpawnPoint");

        GameObject defSpawnPoint = GameObject.Find("SpawnPoint_default");

        // Zabezpieczenie na wypadek braku defaulta
        if (defSpawnPoint != null)
            defaultSpawnPoint = defSpawnPoint.transform;

        spawnPoints = spawnPointObjects.Select(go => go.transform).ToArray();
        targetSpawnPoints = targetSpawnPointsObjects.Select(go => go.transform).ToArray();

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No game objects with tag 'SpawnPoint'!");
            return;
        }

        if (targetSpawnPoints.Length == 0)
        {
            Debug.LogError("No game objects with tag 'TargetSpawnPoint'!");
            return;
        }

        if (existingPlayer == null)
        {
            Debug.LogError("No reference to player game object.");
            return;
        }

        SetPlayerPosition();
        SetTargetNPCPostiion();
    }

    private void SetPlayerPosition()
    {
        // --------------- FOR GAME ---------------
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        // === POPRAWKA: Obsługa CharacterController ===
        // Jeśli gracz ma CharacterController, trzeba go wyłączyć przed teleportacją
        CharacterController cc = existingPlayer.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        existingPlayer.transform.position = selectedSpawnPoint.position;
        existingPlayer.transform.rotation = selectedSpawnPoint.rotation; // Warto też ustawić rotację

        // Włączamy z powrotem
        if (cc != null) cc.enabled = true;

        Debug.Log($"Player spawned at: {selectedSpawnPoint.name}");
    }

    private void SetTargetNPCPostiion()
    {
        int randomIndex = Random.Range(0, targetSpawnPoints.Length);
        Transform selectedTargetSpawnPoint = targetSpawnPoints[randomIndex];
        existingTargetNPC.transform.position = selectedTargetSpawnPoint.position;
    }
}