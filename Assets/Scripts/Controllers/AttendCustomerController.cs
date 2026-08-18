using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendCustomerController : Interactable {
	private bool isPlayerAttending = false;
	private NPC customer;

	[SerializeField] private List<Room> rooms;
	[SerializeField] private WaitingQueueController waitingQueueController;
	[SerializeField] private CanvasGroup attendUI;
	[SerializeField] private Image progressFill;
	[SerializeField] private AudioSource clock;
	[SerializeField] private MoneyBundle moneyBundle;
	[SerializeField] private RandomPositionGenerator moneySpawnArea;

	private bool IsNpcInArea { get { return customer != null && waitingQueueController.IsCustomer(customer); } }

	private void Update() {
		attendUI.alpha = CanAttendCustomer() ? 1f : 0.2f;
	}

	protected override void PlayerInteracting(PlayerController player) {
		if(CanAttendCustomer()) {
			StartAttending();
		}
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
		if(CanAttendCustomer()) {
			CancelAttending();
		}
	}

	protected override void NpcInteracated(NPC npc) {
		if(npc.CurrentState == NPC.States.FREE_ROAMING)
			return;
		customer = npc;
	}

	protected override void NpcStoppedInteracting(NPC npc) {
		if(customer == npc) {
			customer = null;
		}
	}

	private bool CanAttendCustomer() {
		return IsNpcInArea && GetAvailableRoom() != null;
	}

	private Room GetAvailableRoom() {
		return rooms.Find(room => room.roomState.isUnlocked && !room.roomState.isOccupied && !room.roomState.isDirty);
	}

	private void StartAttending() {
		if(isPlayerAttending)
			return;

		isPlayerAttending = true;
		const float ATTEND_TIME = 3f;
		clock.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, ATTEND_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => {
			SpawnMoney();
			progressFill.fillAmount = 0f;
			clock.Stop();
			NPC customer = waitingQueueController.GetFirstCustomer();
			if(customer != null) {
				GotoRoom(customer);
			}
			isPlayerAttending = false;
		});
	}

	private void CancelAttending() {
		LeanTween.cancel(gameObject);
		progressFill.fillAmount = 0f;
		clock.Stop();
		isPlayerAttending = false;
	}

	private void GotoRoom(NPC customer) {
		Room availableRoom = GetAvailableRoom();
		if(availableRoom != null) {
			availableRoom.roomState.isOccupied = true;
			customer.MoveTo(availableRoom.GetSleepPos());
		}
	}

	private void SpawnMoney() {
		Instantiate(moneyBundle, moneySpawnArea.GetPosition(), Quaternion.identity);
	}

}