using UnityEngine;

public class UpgradesController : MonoBehaviour
{
	[SerializeField] private GameObject upgradeElement;

	public void OnMoneyUpdatedHandler(float amount) {
		const float UPGRADE_THRESHOLD = 40;
		if (amount >= UPGRADE_THRESHOLD) {
			if (upgradeElement != null) {
				upgradeElement.SetActive(true);
			}
		}
	}
}