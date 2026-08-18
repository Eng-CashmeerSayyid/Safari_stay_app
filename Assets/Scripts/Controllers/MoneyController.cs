using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MoneyController : MonoBehaviour
{
	private float money = 0f;

	[SerializeField] private TMP_Text moneyText;

	public UnityEvent<float> OnMoneyUpdated;

	public float Money {
		get => money;
		set {
			float newMoney = Mathf.Max(0, value);
			AnimateCounter(money, newMoney);
			money = newMoney;
			OnMoneyUpdated?.Invoke(money);
		}
	}

	public static MoneyController Instance { get; private set; }

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		Money = SaveSystem.playerProgress.money;
	}

	private void OnDestroy() {
		Instance = null;
	}

	private void AnimateCounter(float oldMoney, float newMoney) {
		const float ANIMATION_TIME = 0.4f;
		LTDescr textTween = LeanTween.value(gameObject, oldMoney, newMoney, ANIMATION_TIME);
		textTween.setOnUpdate((float newMoney) => {
			moneyText.text = $"{newMoney:00}";
		});
		textTween.setEase(LeanTweenType.easeOutQuart);

		LTDescr scaleTween = LeanTween.value(gameObject, 1.2f, 1f, ANIMATION_TIME);
		scaleTween.setOnUpdate((float scale) => { 
			moneyText.transform.localScale = Vector3.one * scale;
		});
		textTween.setEase(LeanTweenType.easeInOutBounce);
	}

	public void AddMoney(float amount) {
		Money += amount;
	}

	public void SpendMoney(float amount) {
		if(Money < amount) {
			return; // Insufficient funds
		}
		Money -= amount;
	}

}