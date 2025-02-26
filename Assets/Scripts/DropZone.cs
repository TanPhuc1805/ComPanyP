using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

	public class DropZone : MonoBehaviour, IDropHandler
	{
		public static DropZone Instance { get; private set; }
		public Transform commandPanel;

		private void Awake()
		{
			Instance = this;
		}

		public void OnDrop(PointerEventData eventData)
		{
			GameObject gameObject = eventData.pointerDrag;
			Debug.Log("Drop");
			if (!gameObject.TryGetComponent<DraggableCommand>(out var item)) return;
			//if (gameObject.name != "Function" &&
			//	gameObject.name != "Variable" &&
			//	gameObject.name != "Array" &&
			//	gameObject.name != "Assign" &&
			//	gameObject.name != "Struct" &&
			//	gameObject.name != "StructVariable" &&
			//	gameObject.name != "FileDefine") return;
			item.inPickerBar = false;
			item.parentAfterDrag = commandPanel;
		}

		public void RenderCommandPanelLayout()
		{
			if (commandPanel == null) return;
			RectTransform comandPanelRT = commandPanel.GetComponent<RectTransform>();
			LayoutRebuilder.ForceRebuildLayoutImmediate(comandPanelRT);
		}

		private void Update()
		{
			RenderCommandPanelLayout();
		}
	}