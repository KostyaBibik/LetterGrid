using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject gridElement;

    [Header("Grid properties")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxWidth;
    [SerializeField] private float minWidth;

    private GridLayoutGroup _gridLayout;
    private RectTransform _rectTransform;
    private float _widthGrid;
    private Rect _rectElement;
    private List<GameObject> _createdLetters = new List<GameObject>();
    
    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _rectElement = gridElement.GetComponent<RectTransform>().rect;
    }

    private void Start()
    {
        _widthGrid = _rectTransform.rect.width;
    }

    public void CalculateGrid(int countOnRow, int countOnColumn)
    {
        var newGridWidth = countOnRow * _rectElement.width + _gridLayout.spacing.x * countOnRow;
        var newGridHeight = countOnColumn * _rectElement.height + _gridLayout.spacing.y * countOnColumn;
        
        var remainsWidth = maxWidth - newGridWidth;
        var remainsHeight = maxHeight - newGridHeight;
        
        _gridLayout.cellSize += new Vector2(remainsWidth/(countOnRow * _gridLayout.spacing.x), 
            remainsHeight/(countOnColumn * _gridLayout.spacing.y));
        _rectTransform.sizeDelta = new Vector2(newGridWidth, newGridHeight);
        
        FillGridWithElements(countOnRow * countOnColumn);
    }

    private void FillGridWithElements(int countElements)
    {
        ClearGrid();
        
        for (int i = 0; i < countElements; i++)
        {
            var element = Instantiate(gridElement, _rectTransform);
            _createdLetters.Add(element);
        }
    }

    private void ClearGrid()
    {
        foreach (var element in _createdLetters)
        {
            Destroy(element);
        }
        _createdLetters.Clear();
    }
}
