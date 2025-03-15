using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Import DoTween

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Transform _selectionCorners; // Hiệu ứng chọn (4 góc)

    private static Tile _selectedTile; // Lưu trữ Tile đang được chọn

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _selectionCorners.localScale = Vector3.zero; // Ẩn hiệu ứng ban đầu
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        // Nếu có Tile khác đang được chọn, tắt highlight của nó
        if (_selectedTile != null && _selectedTile != this)
        {
            _selectedTile.SetSelected(false);
        }

        // Chọn Tile hiện tại
        bool isSelecting = (_selectedTile != this);
        SetSelected(isSelecting);

        // Cập nhật Tile được chọn
        _selectedTile = isSelecting ? this : null;
    }

    // Hàm bật/tắt hiệu ứng chọn (4 góc) với DoTween Scale Loop
    public void SetSelected(bool isSelected)
    {
        _selectionCorners.DOKill(); // Dừng animation trước đó (nếu có)

        if (isSelected)
        {
            _selectionCorners.gameObject.SetActive(true);
            _selectionCorners.localScale = Vector3.one * 0.8f; // Bắt đầu từ 90%

            _selectionCorners.DOScale(Vector3.one, 0.3f) // Scale từ 0.9 -> 1.0
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo); // Hiệu ứng lặp vô hạn
        }
        else
        {
            _selectionCorners.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => _selectionCorners.gameObject.SetActive(false));
        }
    }
}
