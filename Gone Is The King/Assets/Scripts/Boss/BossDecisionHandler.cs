using UnityEngine;
using UnityEngine.UI;
using JonathansDemo;

public class BossDecisionHandler : MonoBehaviour
{
    [Header("Choice UI")]
    public GameObject choicePanel;
    public Button spareButton;
    public Button fightButton;

    [Header("Transforms")]
    public GameObject npcVersion;     // Version that becomes the talkable NPC
    public GameObject enemyVersion;   // Enemy prefab to fight

    private bool playerInRange = false;
    private bool hasDecided = false;

    private void Start()
    {
        choicePanel.SetActive(false);
        spareButton.onClick.AddListener(Spare);
        fightButton.onClick.AddListener(Fight);
    }

     void Update()
    {
        if (playerInRange && !hasDecided && Input.GetKeyDown(KeyCode.E))
        {
            if (choicePanel != null)
            {
                choicePanel.SetActive(true);
                Debug.Log("📋 Boss choice panel opened.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player entered the boss trigger zone!");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🚪 Player left the boss trigger zone.");
            playerInRange = false;
        }
    }

    private void Spare()
{
    GameManager.Instance.RecordSpare();
    choicePanel.SetActive(false);
    hasDecided = true;

    if (npcVersion != null)
    {
        Instantiate(npcVersion, transform.position, transform.rotation);
    }
    GameManager.Instance.CompletedBoss(true);
    Destroy(gameObject); // remove neutral boss
}

private void Fight()
{
    GameManager.Instance.RecordFight();
    choicePanel.SetActive(false);
    hasDecided = true;

    if (enemyVersion != null)
    {
        Instantiate(enemyVersion, transform.position, transform.rotation);
    }

    Destroy(gameObject); // remove neutral boss
}
}