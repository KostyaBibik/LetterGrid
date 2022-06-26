using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputUi : MonoBehaviour
{
    [SerializeField] private TMP_Text warningTxt;
    [Header("Buttons")]
    [SerializeField] private Button generateBtn;
    [SerializeField] private Button mixBtn;
    [Header("Input Values")]
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [Header("Available values")]
    [SerializeField] private int maxCountOnRow;
    [SerializeField] private int minCountOnRow;
    [SerializeField] private int maxCountOnColumn;
    [SerializeField] private int minCountOnColumn;
    [SerializeField] private float timeShowWarning = 2f;
    
    private GridController _gridController;
    private string _previousInputWidth;
    private string _previousInputHeight;

    private const string WarningExceedingMsg = "Too much value";
    private const string WarningDeficientMsg = "The value is too small";
    private const string WarningIncorrectInput = "Incorrect input";
    private const string WarningNotEnoughLettersToMix = "Not enough letters to mix them";
    
    private void Awake()
    {
        _gridController = FindObjectOfType<GridController>();
    }

    private void Start()
    {
        _previousInputWidth = widthInput.text;
        _previousInputHeight = heightInput.text;

        generateBtn.onClick.AddListener(delegate
        {
            if(CheckCorrectInput(widthInput.text) && CheckCorrectInput(heightInput.text))
            {
                _gridController.CalculateGrid(int.Parse(widthInput.text), int.Parse(heightInput.text));
            }
        });
        
        mixBtn.onClick.AddListener(delegate
        {
            StartCoroutine(nameof(MixLetters));
        });
        
        widthInput.onEndEdit.AddListener(delegate(string value)
        {
            if (!CheckCorrectInput(value))
            {
                return;
            }
                
            var newCount = int.Parse(value);
            if (newCount > maxCountOnRow)
            {
                StartCoroutine(nameof(ShowWarning), WarningExceedingMsg);
                widthInput.text = _previousInputWidth;
            }
            else if (newCount < minCountOnRow)
            {
                StartCoroutine(nameof(ShowWarning), WarningDeficientMsg);
                widthInput.text = _previousInputWidth;
            }
            else
            {
                _previousInputWidth = widthInput.text;
            }
        });
        
        heightInput.onEndEdit.AddListener(delegate(string value)
        {
            if (!CheckCorrectInput(value))
            {
                return;
            }
            
            var newCount = int.Parse(value);
            if (newCount > maxCountOnColumn)
            {
                StartCoroutine(nameof(ShowWarning), WarningExceedingMsg);
                heightInput.text = _previousInputHeight;
            }
            else if (newCount < minCountOnColumn)
            {
                StartCoroutine(nameof(ShowWarning), WarningDeficientMsg);
                heightInput.text = _previousInputHeight;
            }
            else
            {
                _previousInputHeight = heightInput.text;
            }
        });
    }

    private bool CheckCorrectInput(string checkString)
    {
        if (int.TryParse(checkString, out var widthValue))
        {
            return true;
        }
        else
        {
            StartCoroutine(nameof(ShowWarning), WarningIncorrectInput);
            return false;
        }
    }
    
    private IEnumerator MixLetters()
    {
        if (!_gridController.CanToMixLetters())
        {
            StartCoroutine(nameof(ShowWarning), WarningNotEnoughLettersToMix);
            yield break;
        }
        
        mixBtn.interactable = false;
        generateBtn.interactable = false;
        
        yield return _gridController.StartCoroutine(_gridController.MixLetters());
        
        generateBtn.interactable = true;
        mixBtn.interactable = true;
    }

    private IEnumerator ShowWarning(string msg)
    {
        warningTxt.text = msg;
        warningTxt.gameObject.SetActive(true);
        warningTxt.alpha = 1f;
        warningTxt.CrossFadeAlpha(0f,timeShowWarning,false);
        
        yield return new WaitForSeconds(timeShowWarning);
        
        warningTxt.gameObject.SetActive(false);
    }
}
