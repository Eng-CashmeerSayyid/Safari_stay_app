using UnityEngine;
using UnityEngine.UI;

public class UpgradeElement : Interactable
{
	private AudioSource spendMoney;

	[SerializeField] private float upgradeCost = 50;
	[SerializeField] private Image progressFill;
	[SerializeField] private Room room;
	[SerializeField] private ParticleSystem particles;

	private void Awake() {
		spendMoney = GetComponent<AudioSource>();
	}

	private void Start() {
		LoadProgress();
	}

	protected override void PlayerInteracted(PlayerController player) {
		if(MoneyController.Instance.Money >= upgradeCost) {
			StartUpgrade();
		}
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
		const float CANCEL_THRESHOLD = 0.9f;
		if(progressFill.fillAmount < CANCEL_THRESHOLD) {
			CancelUpgrade();
		}
	}

	private void LoadProgress() {
		if(SaveSystem.playerProgress.rooms[1].isUnlocked) {
			room.UnlockRoom(false);
			Destroy(gameObject);
		}
		else {
			gameObject.SetActive(false);
		}
	}

	private void StartUpgrade() {
		particles.Play();
		const float UPGRADE_TIME = 5f;
		spendMoney.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, UPGRADE_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => UnlockRoom());
	}
	
	private void CancelUpgrade() {
		particles.Stop();
		spendMoney.Stop();
		LeanTween.cancel(gameObject);
		progressFill.fillAmount = 0;
	}

	private void UnlockRoom() {
		const float SCALE_Z_TIME = 0.4f;
		const float SCALE_Y_TIME = 0.2f;
		LeanTween.scaleZ(room.gameObject, 1f, SCALE_Z_TIME).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() => {
			LeanTween.scaleY(room.gameObject, 1f, SCALE_Y_TIME).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => { 
				room.UnlockRoom();
				MoneyController.Instance.SpendMoney(upgradeCost);
				Destroy(gameObject);
			});
		});
	}
}