using UnityEngine;

public class GroundController : MonoBehaviour
{

    private void OnMouseDown()
    {
        BuilderController.DeSelectLastBuilder();
    }

}
