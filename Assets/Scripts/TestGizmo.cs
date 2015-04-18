using UnityEngine;
using System.Collections;

public class TestGizmo : MonoBehaviour {
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (new Vector3 (1, 1, 1), 0.5f);
	}
}
