using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridController : MonoBehaviour
{
    [SerializeField] private TMP_Text gridElement;
    [Header("Grid properties")]
    [SerializeField] private float maxWidth;
    [SerializeField] private float maxHeight;
    [SerializeField] private float coefficientScaleElements = 4;
    [SerializeField] private float preferredSizeElement = 64;
    [Space, SerializeField] private float timeMixLetters = 2f;

    private GridLayoutGroup _gridLayout;
    private RectTransform _rectTransform;
    private List<GameObject> _createdLetters = new List<GameObject>();

    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    
    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void CalculateGrid(int countOnRow, int countOnColumn)
    {
        _gridLayout.enabled = true;
        
        var startSizeGrid = new Vector2(countOnRow * (preferredSizeElement + _gridLayout.spacing.x),
            countOnColumn * (preferredSizeElement + _gridLayout.spacing.y));
        
        var ratio = Mathf.Min(maxWidth / startSizeGrid.x, maxHeight / startSizeGrid.y);
        var newCellSize = preferredSizeElement + ratio * coefficientScaleElements;
        
        _gridLayout.cellSize = new Vector2(newCellSize, newCellSize);
        _rectTransform.sizeDelta = new Vector2(countOnRow * (newCellSize + _gridLayout.spacing.x),
            countOnColumn * (newCellSize + _gridLayout.spacing.y));
        
        FillGridWithElements(countOnRow * countOnColumn);
    }

    private void FillGridWithElements(int countElements)
    {
        ClearGrid();

        for (int i = 0; i < countElements; i++)
        {
            var element = Instantiate(gridElement, _rectTransform);
            element.text = Alphabet[Random.Range(0, Alphabet.Length)].ToString();
            _createdLetters.Add(element.gameObject);
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

    public IEnumerator MixLetters()
    {
        if (_createdLetters.Count <= 1)
        {
            yield break;
        }
        
        _gridLayout.enabled = false;

        List<Vector3> posesToMix = new List<Vector3>();
        List<Vector3> newPoses = new List<Vector3>();
        foreach (var letter in _createdLetters)
        {
            posesToMix.Add(letter.transform.position);
        }

        foreach (var letter in _createdLetters)
        {
            var randomCounter = Random.Range(0, posesToMix.Count);
            if (posesToMix[randomCounter] == letter.transform.position)
            {
                Debug.Log("Return");
                yield return StartCoroutine(nameof(MixLetters));
                yield break;
            }
            
            newPoses.Add(posesToMix[randomCounter]);
            posesToMix.RemoveAt(randomCounter);
        }

        Debug.Log("start Smooth");
        yield return StartCoroutine(nameof(SmoothReplaceElement), newPoses.ToArray());
        Debug.Log("end Smooth");
    }

    private IEnumerator SmoothReplaceElement(Vector3[] targetPoses)
    {
        var startPoses = new List<Vector3>();
        foreach (var letter in _createdLetters)
        {
            startPoses.Add(letter.transform.position);
        }
        
        var timeMoving = 0f;
        do
        {
            timeMoving += Time.deltaTime;
            timeMoving = Mathf.Clamp(timeMoving, 0, timeMixLetters);
            for (int i = 0; i < _createdLetters.Count; i++)
            {
                _createdLetters[i].transform.position = 
                    Vector3.Lerp(startPoses[i], targetPoses[i], timeMoving / timeMixLetters);
            }
            yield return null;
        } while (timeMoving / timeMixLetters < 1f);
    }
}


