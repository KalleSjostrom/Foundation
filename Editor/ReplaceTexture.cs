using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceTexture : ScriptableWizard
{
	public float tileSize = 1;
	public bool randomRotation = true;
	public Texture texture;
	public GameObject[] Objects;
	
	[MenuItem("Custom/Replace Texture")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Replace Texture", typeof(ReplaceTexture), "Replace Texture");
	}

    void OnWizardCreate()
    {
		foreach (GameObject go in Objects)
		{
			Vector2 textureScale = new Vector2(tileSize, tileSize);
			int indexX = Mathf.RoundToInt(1 / textureScale.x);
			int indexY = Mathf.RoundToInt(1 / textureScale.y);

			int randomX = Random.Range(0, indexX);
			int randomY = Random.Range(0, indexY);

			float x = randomRotation ? Mathf.Sign(Random.value - 0.5f) : 1;
			float y = randomRotation ? Mathf.Sign(Random.value - 0.5f) : 1;
			
			MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();

			Material tempMaterial = new Material(renderer.sharedMaterial);
			tempMaterial.mainTexture = texture;
			
			Vector2 scale  = new Vector2(textureScale.x * x, textureScale.y * y);
			tempMaterial.mainTextureScale = scale;
			Vector2 offset = new Vector2(randomX * textureScale.x, randomY * textureScale.y);
			tempMaterial.mainTextureOffset = offset;
			
			renderer.sharedMaterial = tempMaterial;
		}
    }
}