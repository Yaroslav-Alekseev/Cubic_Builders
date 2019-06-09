using UnityEngine;
using UnityEngine.EventSystems;

public class GroundController : MonoBehaviour
{
    /// <summary>
    /// Обрабатывает клики по земле
    /// </summary>

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) //запрет клика по земле сквозь кнопки GUI
        {
            BuilderController.DeSelectLastBuilder(); //снять выделение со строителя, если клик по земле
        }
    }

}
