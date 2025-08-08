using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private MainGameplayController _scriptMain;
    public float _topTimer;
    public float _timer;
    public Image _timerImage;
    public Image _timerCarrete;
  
    public Image _carreteFinal;
    float _fillAmmount;
    public TextMeshProUGUI _secondText;
    public bool _startTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_scriptMain)
        {
            TimerVoid();
        }
  
    }

    public void TimerVoid()
    {
        if (_topTimer <= 0) return;

        _timer = Mathf.Clamp(_timer, 0, _topTimer);
        _fillAmmount = Mathf.Clamp01(_timer / _topTimer);

        _timerImage.fillAmount = _fillAmmount;
        _secondText.text = (_timer + 0.1f).ToString("F0");

        // Move from +850 (full) to -850 (empty)
        float posX = Mathf.Lerp(850f, -850f, _fillAmmount);
        RectTransform rt = _timerImage.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-posX, rt.anchoredPosition.y);

        if (_startTimer)
        {
            _timer -= Time.deltaTime * _scriptMain._onLevel;
        }
        _timerCarrete.GetComponent<RectTransform>().anchoredPosition = new Vector2(rt.anchoredPosition.x, _timerCarrete.GetComponent<RectTransform>().anchoredPosition.y);

        if(_timer <= 0)
        {
            
            _carreteFinal.gameObject.SetActive(true);
            _secondText.gameObject.SetActive(false);
            _scriptMain._gameStarts = false;
            _startTimer = false;
            _scriptMain._sceneAnimator.gameObject.SetActive(true);
            StartCoroutine(_scriptMain.RestartGameNumerator());
            _timer = _topTimer;
    
        }
    }



}
