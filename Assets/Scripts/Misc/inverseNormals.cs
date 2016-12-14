using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshFilter))]
public class inverseNormals : BaseObject, ISnakeItemPickup, IPauseExit, IPauseEnter
{
	#region IPauseEvents implementation
	public void OnPauseEnter()
	{
		paused = true;
		if(material!=null){
			material.SetColor ("_EmissionColor", Color.clear);
		}
	}
	public void OnPauseExit()
	{
		paused = false;
	}
	#endregion
	#region ISnakeItemEvents implementation
	public void OnSnakeItemPickup(global::StickyItem obj, global::SnakeInventory snakeInv)
	{
    	if(sticky!=null && material != null){
    		material.SetColor ("_EmissionColor", Color.clear);
    	}
	}
	public void OnSnakeItemDrop(global::StickyItem obj, global::SnakeInventory snakeInv)
	{

	}
	#endregion

	public Material material;
	Color color = Color.white;
	Ease easing = Ease.Linear;
	float duration = 1.8f;
	float emission = 0.3f;
	public StickyItem sticky;
	bool paused = true;

	void Start ()
    {
    	sticky = transform.parent.GetComponent<StickyItem>();
    	var renderer = GetComponent<Renderer>();
    	material = renderer.material;
    	if(sticky==null || material == null){
    		this.enabled = false;
    		return;
    	}

    	material.SetColor ("_EmissionColor", Color.clear);
    	material.EnableKeyword ("_EMISSION");


		//MeshFilter filter = GetComponent(typeof (MeshFilter)) as MeshFilter;
		//if (filter != null)
		//{
        //    Mesh mesh = filter.mesh;
//
		//	Vector3[] normals = mesh.normals;
		//	for (int i = 0; i < normals.Length; i++)
		//		normals[i] = -normals[i];
		//	mesh.normals = normals;
//
		//	for (int m = 0; m < mesh.subMeshCount; m++)
		//	{
		//		int[] triangles = mesh.GetTriangles(m);
		//		for (int i = 0; i < triangles.Length; i += 3)
		//		{
		//			int temp = triangles[i + 0];
		//			triangles[i + 0] = triangles[i + 1];
		//			triangles[i + 1] = temp;
		//		}
		//		mesh.SetTriangles(triangles, m);
		//	}
		//}
    			StartCoroutine(TweenStart());
	}

	IEnumerator TweenStart(){

    	float rand = Random.Range(0f, 0.1f);
    	//float randTwo = Random.Range(-0.1f, 0.1f);
    	yield return new WaitForSeconds(rand);
		if(material	!= null)
				DOTween.To(()=>emission, x=> emission = x, 0f, duration).SetEase(easing).SetLoops(-1, LoopType.Yoyo);
	}

	void Update(){
		if(!sticky.Stuck && !paused){
			float nice = Mathf.Clamp((emission-0.18f)*2f,0, 0.5f);
			material.SetColor ("_EmissionColor", color * nice);
		}
	}
}
