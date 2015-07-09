using UnityEngine;
using UnityEditor;
using System.Collections;

//[ExecuteInEditMode]
public class Data : MonoBehaviour {
	public Mesh[] meshes;
	public Material[] materials;

	public int maxDepth;
	private int depth;
	
	public float childScale;
	public float spawnProbability;
	public float maxRotationSpeed;
	
	private float rotationSpeed;
	
	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};
	
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};
	
	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MeshFilter> ().mesh = 
			meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = 
			materials [Random.Range(0, materials.Length)];
		if (depth < maxDepth) {
			CreateChildren();
		}
		
		rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
	}
	
	private void Initialize(Data parent, int childIndex) {
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		maxRotationSpeed = parent.maxRotationSpeed;
		depth = parent.depth + 1;
		spawnProbability = parent.spawnProbability;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * 
			(Random.Range (1f, 5f) + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}
	
	private void CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			if(Random.value < spawnProbability) {
				new GameObject ("Child").AddComponent<Data> ().
					Initialize (this, i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
