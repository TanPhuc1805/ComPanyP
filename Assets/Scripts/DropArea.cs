using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    public RectTransform contentRect; // Gán Content của ScrollView vào Inspector
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform); // Chuyển object vào DropArea
            eventData.pointerDrag.transform.position = transform.position; // Đưa object vào vị trí trung tâm
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }
    }

    
}
