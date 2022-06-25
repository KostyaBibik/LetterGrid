using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject gridElement;
    
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

    public void CalculateGrid(int width, int height)
    {
        _rectTransform.sizeDelta = new Vector2(width * _rectElement.width + _gridLayout.spacing.x * width, height * _rectElement.height+ _gridLayout.spacing.y * height);
        FillGridWithElements(width * height);
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
