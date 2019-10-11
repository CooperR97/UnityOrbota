using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PIDI_2DSimpleReflection : MonoBehaviour {

    [Range(-10,10)]
	public float SurfaceLevel;

	void OnWillRenderObject () {
        if ( GetComponent<SpriteRenderer>().sharedMaterial.HasProperty("_SurfaceLevel") ){
            GetComponent<SpriteRenderer>().sharedMaterial.SetFloat("_SurfaceLevel", SurfaceLevel);
        }
	}

}
