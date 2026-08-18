using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitingQueueController : MonoBehaviour
{
	private List<NPC> customers;
	private List<Vector3> waitingQueuePositions;
	private Vector3 enterancePosition;

	private void Awake() {
		waitingQueuePositions = new List<Vector3>();
		customers = new List<NPC>();
		foreach (Transform child in transform) {
			waitingQueuePositions.Add(child.position);
		}
		enterancePosition = waitingQueuePositions[waitingQueuePositions.Count - 1];
	}

	public bool CanAddCustomerToQueue() {
		return customers.Count < waitingQueuePositions.Count;
	}

	public void AddCustomerToQueue(NPC customer) {
		customers.Add(customer);
		customer.MoveTo(enterancePosition);
		customer.OnTargetReached += () => {
			int index = customers.IndexOf(customer);
			customer.MoveTo(waitingQueuePositions[index]);
		};
	}

	public void RemoveCustomerFromQueue(NPC customer) {
		if(customers.Contains(customer)) {
			customers.Remove(customer);
		}
		customer.OnTargetReached = null;
		customer.SetStateFreeRoam();
	}

	public NPC GetFirstCustomer() {
		if(customers.Count == 0) return null;

		NPC customer = customers[0];
		customers.Remove(customer);
		customer.OnTargetReached = null;
		return customer;
	}

	public bool IsCustomer(NPC npc) {
		return customers.Contains(npc);
	}
}