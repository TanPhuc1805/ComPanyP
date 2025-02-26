using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


	public class DraggableCommand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
	{
		[HideInInspector] public Transform parentAfterDrag;
		[HideInInspector] public int indexAfterDrag;
		[HideInInspector] public GameObject parrentNeedRender;
		private Image image;
		public bool inPickerBar = true;

		private void Awake()
		{
			image = GetComponent<Image>();
			parentAfterDrag = transform.parent;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			//DeleteZone.Instance.gameObject.SetActive(true);
			if (inPickerBar)
			{
				GameObject newGameObject = Instantiate(gameObject, transform);
				newGameObject.name = gameObject.name;
				newGameObject.transform.SetParent(transform.parent);
				newGameObject.transform.SetSiblingIndex(transform.GetSiblingIndex());

				RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
				LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
				LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);

				//if (newGameObject.TryGetComponent<VariableValueCommand>(out var variableValueCommand))
				//{
				//	if (variableValueCommand.relatedParent != null) variableValueCommand.relatedParent.generatedVariables.Add(newGameObject);
				//	if (variableValueCommand.relatedFileParent != null) variableValueCommand.relatedFileParent.generatedVariables.Add(newGameObject);
				//	if (variableValueCommand.relatedStructVariableCommand != null) variableValueCommand.relatedStructVariableCommand.generatedAttributes.Add(newGameObject);
				//}
				//if (newGameObject.TryGetComponent<ArrayElementCommand>(out var arrayElementCommand))
				//{
				//	arrayElementCommand.relatedParent.generatedVariables.Add(newGameObject);
				//}
				//if (newGameObject.TryGetComponent<FunctionValueCommand>(out var functionValueCommand))
				//{
				//	functionValueCommand.relatedParent.newVariables.Add(newGameObject);
				//}
				//if (newGameObject.TryGetComponent<StructVariableCommand>(out var structVariableCommand))
				//{
				//	structVariableCommand.structDefine.generatedVariables.Add(newGameObject);
				//}
			}
			parentAfterDrag = transform.parent;
			indexAfterDrag = transform.GetSiblingIndex();
			transform.SetParent(transform.root);
			transform.SetAsLastSibling();
			//TabGroup.Instance.RenderTabView();

			//Turn off raycast in children and this game object to avoid mouse detect wrong drop place
			if (image != null) image.raycastTarget = false;
			Image[] imageInChild = GetComponentsInChildren<Image>();
			foreach (Image image in imageInChild)
			{
				image.raycastTarget = false;
			}
			TextMeshProUGUI[] textInChild = GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI text in textInChild)
			{
				text.raycastTarget = false;
			}
			TMP_SelectionCaret[] caretInChild = GetComponentsInChildren<TMP_SelectionCaret>();
			foreach (TMP_SelectionCaret carent in caretInChild)
			{
				carent.raycastTarget = false;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			//DeleteZone.Instance.gameObject.SetActive(false);
			if (parentAfterDrag.tag == "PickerBar" || inPickerBar)
			{
				Destroy(gameObject);
				return;
			}
		transform.SetParent(parentAfterDrag);
			if (parentAfterDrag.tag == "CommandPanel" && indexAfterDrag == 0)
			{
				indexAfterDrag = 1;
			}
			transform.SetSiblingIndex(indexAfterDrag);

			//Turn on raycast in children and this game object
			if (image != null) image.raycastTarget = true;
			Image[] imageInChild = GetComponentsInChildren<Image>();
			foreach (Image image in imageInChild)
			{
				image.raycastTarget = true;
			}
			TextMeshProUGUI[] textInChild = GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI text in textInChild)
			{
				text.raycastTarget = true;
			}
			TMP_SelectionCaret[] caretInChild = GetComponentsInChildren<TMP_SelectionCaret>();
			foreach (TMP_SelectionCaret carent in caretInChild)
			{
				carent.raycastTarget = true;
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			GameObject gameObject = eventData.pointerDrag;
			if (transform.parent.tag == "PickerBar") return;
			if (!gameObject.TryGetComponent<DraggableCommand>(out var item)) return;
			if (inPickerBar) return;

			item.parentAfterDrag = parentAfterDrag;
			item.inPickerBar = false;
			item.indexAfterDrag = transform.GetSiblingIndex() + 1;
		}
	}