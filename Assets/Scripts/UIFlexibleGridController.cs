using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(GridLayoutGroup))]
public class UIFlexibleGridController : MonoBehaviour
{
    public bool scaleOnlyOnStartup = true;
    public bool keepAspectRatio;
    public bool keepSpacingZero = true;
    private float _cellAspectRatio;
    private GridLayoutGroup _glg;
    private float _lastHeight;
    private float _lastWidth;

    private RectTransform _rt;

    private void Start()
    {
        _rt = GetComponent<RectTransform>();
        _glg = GetComponent<GridLayoutGroup>();
        if (_rt == null || _glg == null)
        {
            Debug.LogError("UIFlexibleGridController couldn't find a RectTransform or a GridLayoutGroup");
            return;
        }

        _lastWidth = _rt.rect.width;
        _lastHeight = _rt.rect.height;
        _cellAspectRatio = _rt.rect.width / _rt.rect.height;
        if (_glg.constraint == GridLayoutGroup.Constraint.FixedColumnCount ||
            _glg.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            UpdateCellSizes();
        }
        else
        {
            Debug.LogWarning(
                "GridLayoutGroup contraints do not make this UIFlexibleGridController necessary. Consider removing it.");
        }
    }

    private void Update()
    {
        if (scaleOnlyOnStartup) return;
        if (!HasSizedChanged()) return;
        _lastWidth = _rt.rect.width;
        _lastHeight = _rt.rect.height;
        UpdateCellSizes();
    }

    private bool HasSizedChanged()
    {
        return _lastHeight != _rt.rect.height || _lastWidth != _rt.rect.width;
    }

    private string GameObjectPathName(Transform t)
    {
        if (t.parent == null)
            return t.name;
        return GameObjectPathName(t.parent) + "/" + t.name;
    }

    private void UpdateCellSizes()
    {
        var w = 0f;
        var h = 0f;
        var sx = 0f;
        var sy = 0f;
        w = _rt.rect.width / _glg.constraintCount;
        h = _rt.rect.height / _glg.constraintCount;
        if (w == 0 || h == 0)
        {
            Debug.LogError(string.Format("Invalid width ({0}) or height ({1}) at {2}", w, h,
                GameObjectPathName(transform)));
            return;
        }

        if (!keepSpacingZero)
        {
            w = (int) w;
            h = (int) h;
        }

        if (keepAspectRatio) h = w * _cellAspectRatio;

        var newSize = _glg.cellSize;
        newSize.x = w;
        newSize.y = h;
        _glg.cellSize = newSize;
        if (!keepSpacingZero && _glg.constraintCount != 1)
        {
            sx = (_rt.rect.width - w * _glg.constraintCount) / (_glg.constraintCount - 1);
            sy = (_rt.rect.height - w * _glg.constraintCount) / (_glg.constraintCount - 1);
        }

        var newSpacing = _glg.spacing;
        newSpacing.x = sx;
        newSpacing.y = sy;
        _glg.spacing = newSpacing;
    }
}