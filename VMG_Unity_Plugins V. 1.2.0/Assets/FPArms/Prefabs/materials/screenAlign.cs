using UnityEngine;

//Script requires the GameObject to have a Renderer component
[RequireComponent(typeof(Renderer))]

public class screenAlign : MonoBehaviour{
	public Vector2 screenPosition = new Vector2(0, 0);
	public Vector3 screenRotation = new Vector3(0,0,0);//Uncommented
	public Camera cameraUI;

    //negative z direction points into the page away from you
    public float tempZ = -8f;

	void Start(){
		cameraUI =  Camera.main;
	}

	void Update (){
		/*Vector3 tempScreenPosition = screenPosition;
		Vector3 tempScreenRotation = screenRotation;
		tempScreenPosition.z = -cameraUI.transform.position.z;
		this.transform.rotation = cameraUI.transform.rotation;
		this.transform.LookAt(cameraUI.transform);
		Vector3 worldPosition = cameraUI.ScreenToWorldPoint(tempScreenPosition);
		Vector3 worldPosition = cameraUI.ViewportToWorldPoint(new Vector3(1 + screenPosition.x, 1 + screenPosition.y, tempZ));

                if (downLeft){
					worldPosition.x -= renderer.bounds.size.x * tempScreenPosition.x / Screen.width;
					worldPosition.y += renderer.bounds.size.y * (1 - tempScreenPosition.y / Screen.height);
					transform.position = worldPosition;
				}

				if (downRight){
					worldPosition.x -= renderer.bounds.size.x * -1 * tempScreenPosition.x / Screen.width;
					worldPosition.y += renderer.bounds.size.y * (1 - tempScreenPosition.y / Screen.height);
					transform.position = worldPosition;
				}

		transform.position = worldPosition;*/
	}
}