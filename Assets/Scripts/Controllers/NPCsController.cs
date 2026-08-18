using System.Collections.Generic;
using UnityEngine;

public class NPCsController : MonoBehaviour
{
	private List<NPC> freeRoamingNpc;
	private List<NPC> customerNpc;

	[SerializeField] private WaitingQueueController waitingQueueController;

	private void Awake() {
		freeRoamingNpc = new List<NPC>(GetComponentsInChildren<NPC>());
		customerNpc = new List<NPC>();
	}

	private void Start() {
		const float REPETITION_TIME = 5f;
		InvokeRepeating(nameof(MakeNpcCustomer), REPETITION_TIME, REPETITION_TIME);
	}

	public void MakeNpcCustomer() {
		if(freeRoamingNpc.Count < 0)
			return;

		if(!waitingQueueController.CanAddCustomerToQueue())
			return;

		NPC npc = freeRoamingNpc[Random.Range(0, freeRoamingNpc.Count)];
		freeRoamingNpc.Remove(npc);
		customerNpc.Add(npc);

		waitingQueueController.AddCustomerToQueue(npc);
	}

	public void MakeNpcFreeRoamer(NPC npc) {
		if(!customerNpc.Contains(npc))
			return;

		customerNpc.Remove(npc);
		freeRoamingNpc.Add(npc);

		waitingQueueController.RemoveCustomerFromQueue(npc);
	}
}