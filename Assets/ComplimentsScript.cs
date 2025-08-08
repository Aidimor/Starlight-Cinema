using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class ComplimentsScript : MonoBehaviour
{
    [System.Serializable]
    public class OptionsClass
    {
        public string _Option1;
        public string _Option2;
        public int _correctChoice;
    }
    public OptionsClass[] _optionsClass;   
    public List<int> _optionsChoosed = new List<int>();
    [SerializeField] private TextMeshProUGUI[] _textOptions;
    [SerializeField] private GameObject _selector;   
    [SerializeField] private Image[] _characterImages;
    [SerializeField] private Sprite[] _actionSprites;
    public int _onSelector;
    public int _totalOptions;
    public int _onOption;
    public float _fillSpeed;
    public bool _fillBool;
    public bool _gameStarts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
        StartCoroutine(StartNumerator());
    }

    void PickRandomOptions()
    { 
        _optionsChoosed = PickUniqueRandomNumbers(_optionsClass.Length, _totalOptions);  
    }

    List<int> PickUniqueRandomNumbers(int maxExclusive, int count)
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < maxExclusive; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = Random.Range(i, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }      
        return numbers.GetRange(0, Mathf.Min(count, maxExclusive));
    }

    public IEnumerator StartNumerator()
    {
        PickRandomOptions();
        if (_onOption == 0)
        {
            yield return new WaitForSeconds(0.5f / transform.parent.gameObject.GetComponent<MainGameplayController>()._onLevel);
        }

        _selector.gameObject.SetActive(true);
        _textOptions[0].text = _optionsClass[_optionsChoosed[_onOption]]._Option1;
        _textOptions[1].text = _optionsClass[_optionsChoosed[_onOption]]._Option2;
        _selector.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f / transform.parent.gameObject.GetComponent<MainGameplayController>()._onLevel);
        _fillBool = true;
        _gameStarts = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStarts)
            InputController();
        if(_fillBool)
        _characterImages[1].fillAmount += 
            (_fillSpeed * Time.deltaTime) * transform.parent.gameObject.GetComponent<MainGameplayController>()._onLevel;

        if(_characterImages[1].fillAmount >= 1)
        {
            _gameStarts = false;
            transform.parent.gameObject.GetComponent<MainGameplayController>()._wins = false;
        }
        else
        {
            transform.parent.gameObject.GetComponent<MainGameplayController>()._wins = true;
        }
    }

    public void InputController()
    {
        if(Input.GetAxisRaw("Horizontal") > 0)       
            _onSelector = 1;
        

        if (Input.GetAxisRaw("Horizontal") < 0)        
            _onSelector = 0;

        _selector.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(_selector.GetComponent<RectTransform>().anchoredPosition,
            _textOptions[_onSelector].GetComponent<RectTransform>().anchoredPosition, 25 * Time.deltaTime);

        if (Input.GetButtonDown("Submit"))
            StartCoroutine(ChooseOption());
    }

    public IEnumerator ChooseOption()
    {
        _gameStarts = false;       
        _textOptions[_optionsClass[_optionsChoosed[_onOption]]._correctChoice].color = Color.green;
        switch (_onSelector == _optionsClass[_optionsChoosed[_onOption]]._correctChoice)
        {
            case true:
                _characterImages[1].fillAmount = 0f;
                yield return new WaitForSeconds(0.25f / transform.parent.gameObject.GetComponent<MainGameplayController>()._onLevel);
                break;
            case false:
                _characterImages[0].sprite = _actionSprites[1];
                _characterImages[1].sprite = _actionSprites[1];           
                yield return new WaitForSeconds(0.25f / transform.parent.gameObject.GetComponent<MainGameplayController>()._onLevel);
                _characterImages[0].sprite = _actionSprites[0];
                _characterImages[1].sprite = _actionSprites[0];
                break;
        }
        _textOptions[0].text = "";
        _textOptions[1].text = "";
        _textOptions[0].color = Color.white;
        _textOptions[1].color = Color.white;
        _selector.gameObject.SetActive(false); 
        _onOption++;
        StartCoroutine(StartNumerator());       
    }
}
