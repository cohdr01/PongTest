using UnityEngine;

public class SideWalls : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hitInfo) {
		if (hitInfo.name == "Ball")
		{
			string wallName = transform.name;
			GameManager.Score (wallName);
			hitInfo.gameObject.SendMessage ("Reset", 1, SendMessageOptions.RequireReceiver);
		}
	}
}
