using UnityEngine;
using UnityEngine.UI;

public class Room : Interactable {
	public RoomStates roomState;

	private AudioSource roomUnlocked;
	private NPC npc;

	[SerializeField] private int roomIndex = 0;

	[SerializeField] private BoxCollider doorCollider;
	[SerializeField] private GameObject cleanUI;
	[SerializeField] private Image progressFill;
	[SerializeField] private AudioSource clock;
	[SerializeField] private Transform sleepAnchor;
	[SerializeField] private ParticleSystem sleepyParticles;
	[SerializeField] private NPCsController npcController;

	private void Awake() {
		roomUnlocked = GetComponent<AudioSource>();
	}

	private void Start() {
		LoadRoomState();
	}

	protected override void PlayerInteracted(PlayerController player) {
		if(roomState.isDirty) {
			StartCleaning();
		}
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
		if(roomState.isDirty) {
			CancelCleaning();
		}
	}

	protected override void NpcInteracated(NPC npc) {
		if(npc.CurrentState == NPC.States.BEING_CUSTOMER) {
			this.npc = npc;
			SleepNpc(npc);
		}
	}

	protected override void NpcStoppedInteracting(NPC npc) {
		this.npc = null;
	}

	private void LoadRoomState() {
		roomState = SaveSystem.playerProgress.rooms[roomIndex];
		if(!roomState.isUnlocked) {
			transform.localScale = Vector3.right;
		}
		if(roomState.isOccupied) {
			roomState.isOccupied = false;
			SetDirty(true);
		}
		if(roomState.isDirty) {
			SetDirty(true);
		}
	}

	private void StartCleaning() {
		const float CLEAN_TIME = 5f;
		clock.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, CLEAN_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => SetDirty(false));
	}

	private void CancelCleaning() {
		LeanTween.cancel(gameObject);
		progressFill.fillAmount = 0f;
		clock.Stop();
	}

	private void SleepNpc(NPC npc) {
		sleepyParticles.Play();
		npc.FallAsleep(sleepAnchor);
		const float SLEEP_TIME = 15f;
		Invoke(nameof(WakeNpc), SLEEP_TIME);
	}

	private void WakeNpc() {
		if(npc != null) {
			sleepyParticles.Stop();
			npcController.MakeNpcFreeRoamer(npc);
			npc.WakeUp();
			roomState.isOccupied = false;
			SetDirty(true);
		}
	}

	public void UnlockRoom(bool playSound = true) {
		roomState.isUnlocked = true;
		doorCollider.enabled = true;
		if(playSound)
			roomUnlocked.Play();
	}

	public void SetDirty(bool value) {
		progressFill.fillAmount = 0f;
		roomState.isDirty = value;
		cleanUI.SetActive(value);
	}

	public Vector3 GetSleepPos() {
		return sleepAnchor.position;
	}
}