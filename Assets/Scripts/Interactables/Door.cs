using UnityEngine;

public class Door : Interactable {
	private int characterCountInArea = 0;

	public bool interactableByPlayer = true;
	[SerializeField] private float animationTime = 1f;
	[SerializeField] private float openAngle = -110f;
	[SerializeField] private float closeAngle = 0f;
	[SerializeField] private LeanTweenType easeType = LeanTweenType.easeInOutCubic;
	[Space, SerializeField] private Transform doorObject;
	[SerializeField] private AudioSource doorOpenSfx;
	[SerializeField] private AudioSource doorCloseSfx;

	protected override void PlayerInteracted(PlayerController player) {
		if(!interactableByPlayer)
			return;

		characterCountInArea++;
		OpenDoor();
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
		if(!interactableByPlayer)
			return;

		characterCountInArea--;
		CloseDoor();
	}

	protected override void NpcInteracated(NPC npc) {
		characterCountInArea++;
		OpenDoor();
	}

	protected override void NpcStoppedInteracting(NPC npc) {
		characterCountInArea--;
		CloseDoor();
	}

	private void OpenDoor() {
		LeanTween.cancel(doorObject.gameObject);
		doorOpenSfx.Play();
		LTDescr tween = LeanTween.rotateLocal(doorObject.gameObject, new Vector3(0, openAngle, 0), animationTime);
		tween.setEase(easeType);
	}

	private void CloseDoor() {
		if(characterCountInArea > 0)
			return; // SOMEONE IS STANDING IN INTERACTABLE AREA
		LeanTween.cancel(doorObject.gameObject);
		doorCloseSfx.Play();
		LTDescr tween = LeanTween.rotateLocal(doorObject.gameObject, new Vector3(0, closeAngle, 0), animationTime);
		tween.setEase(easeType);
	}
}