using System;
using JonathansDemo;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [FormerlySerializedAs("promptText")] public GameObject talkableTxtBox;
    private bool playerNearNPC = false;
    private Vector3 promptOffset;

    public float interactionRange = 2f; // for the circles range
    public LayerMask npcLayerMask;

    void Start()
    {
        if (talkableTxtBox != null)
        {
            promptOffset = talkableTxtBox.transform.localPosition;
            talkableTxtBox.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talkableTxtBox != null && playerNearNPC)
        {
            float bob = Mathf.Sin(Time.time * 3f) * 0.008f;
            talkableTxtBox.transform.localPosition = promptOffset + new Vector3(0, bob, 0);
        }

        if (playerNearNPC && Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, npcLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("DialogueNPC"))
                {
                    DialogueNPCScript npc = hitCollider.GetComponent<DialogueNPCScript>();
                    if (npc != null)
                    {
                        npc.OnSpokenTo();
                        break;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DialogueNPC"))
        {
            playerNearNPC = true;
            if (talkableTxtBox != null) talkableTxtBox.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DialogueNPC"))
        {
            playerNearNPC = false;
            if (talkableTxtBox != null) talkableTxtBox.SetActive(false);
        }
    }
}