using UnityEngine;

public class RandomPositionGenerator : MonoBehaviour
{
	[SerializeField] private float radius = 7f;

	public Vector3 GetPosition() {
		Vector3 pos = Random.insideUnitCircle * radius;
		pos.z = pos.y;
		pos.y = 0;
		pos += transform.position;
		return pos;
	}
}