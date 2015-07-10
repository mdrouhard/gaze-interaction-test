using UnityEngine;
using UnityEditor;
using System.Collections;

//[ExecuteInEditMode]
public class Data : MonoBehaviour {
	public Mesh[] meshes;
	public Material[] materials;

	public int numNodes;
	public float dataRange;


	// initialization
	void Start () {
		gameObject.AddComponent<MeshFilter> ().mesh = 
			meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = 
			materials [Random.Range(0, materials.Length)];

		for (int i = 0; i < (numNodes - 1); i++) {
			CreateChild();
		}
	}

	private void CreateChild() {
		GameObject atom = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		atom.transform.parent = this.transform;

		Vector3 direction = new Vector3(Random.Range (-1f, 1f), Random.Range (-1f, 1f), Random.Range (-1f, 1f));
		atom.transform.localPosition = direction * (Random.Range (1f, dataRange) + 0.5f);
	}

//	private void Initialize(Data parent /*, int childIndex */) {
//		meshes = parent.meshes;
//		materials = parent.materials;
//		maxDepth = parent.maxDepth;
//		maxRotationSpeed = parent.maxRotationSpeed;
//		depth = parent.depth + 1;
//		spawnProbability = parent.spawnProbability;
//		childScale = parent.childScale;
//		transform.parent = parent.transform;
//		transform.localScale = Vector3.one * childScale;
//		transform.localPosition = childDirections[Random.Range (0,childDirections.Length)] * 
//			(Random.Range (1f, dataRange) + 0.5f /* * childScale */);
//		transform.localRotation = childOrientations[childIndex];
//	}
	
//	private void CreateChildren() {
//		for (int i = 0; i < childDirections.Length; i++) {
//			if(Random.value < spawnProbability) {
//				new GameObject ("Child").AddComponent<Data> ().
//					Initialize (this, i);
//			}
//		}
//	}
	
	// Update is called once per frame
	void Update () {
//		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
