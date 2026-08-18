using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			PlayerInteracted(player);
		}
		
		if(other.TryGetComponent<NPC>(out NPC npc)) {
			NpcInteracated(npc);
		}
	}

	private void OnTriggerStay(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			PlayerInteracting(player);
		}

		if(other.TryGetComponent<NPC>(out NPC npc)) {
			NpcInteracting(npc);
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			PlayerStoppedInteracting(player);
		}

		if(other.TryGetComponent<NPC>(out NPC npc)) {
			NpcStoppedInteracting(npc);
		}
	}

	protected virtual void PlayerInteracted(PlayerController player) { }

	protected virtual void PlayerInteracting(PlayerController player) { }

	protected virtual void PlayerStoppedInteracting(PlayerController player) { }

	protected virtual void NpcInteracated(NPC npc) { }

	protected virtual void NpcInteracting(NPC npc) { }

	protected virtual void NpcStoppedInteracting(NPC npc) { }
}