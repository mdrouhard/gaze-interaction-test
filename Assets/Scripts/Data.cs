using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {
	public Mesh[] meshes;
	public Material material;
	
	public int maxDepth;
	private int depth;
	
	public float childScale;
	public float spawnProbability;
	public float maxRotationSpeed;
	
	private float rotationSpeed;
	
	private Material[,] materials = null;
	
	private void InitializeMaterials() {
		materials = new Material[maxDepth + 1, 2];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i/ (maxDepth - 1f);
			t *= t;
			materials[i, 0] = new Material(material);
			materials[i, 0].color =
				Color.Lerp (Color.blue, Color.yellow, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color =
				Color.Lerp (Color.cyan, Color.white, t);
		}
		materials [maxDepth, 1].color = Color.magenta;
	}
	
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
		if (materials == null) {
			InitializeMaterials ();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = 
			meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = 
			materials [depth, Random.Range(0,2)];
		if (depth < maxDepth) {
			StartCoroutine(CreateChildren());
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
	
	private IEnumerator CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			if(Random.value < spawnProbability) {
				yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
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
