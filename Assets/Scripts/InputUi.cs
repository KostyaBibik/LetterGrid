using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputUi : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button generateBtn;
    [SerializeField] private Button mixBtn;
    [Header("Input Values")]
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;

    private GridController _gridController;

    private void Awake()
    {
        _gridController = FindObjectOfType<GridController>();
    }

    private void Start()
    {
        generateBtn.onClick.AddListener(delegate
        {
            _gridController.CalculateGrid(int.Parse(widthInput.text), int.Parse(heightInput.text));
        });
    }
}
