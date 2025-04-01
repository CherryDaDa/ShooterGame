using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Component.GUI
{
    public class OpenUrlButton : MonoBehaviour, IPointerClickHandler
    {
        public string url;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(url);
        }
    }
}