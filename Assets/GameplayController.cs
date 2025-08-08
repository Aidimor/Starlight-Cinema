using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayController : MonoBehaviour
{
    public GameObject _parent;
    [SerializeField] private Image _mainButton;
    [SerializeField] private Image _timerCircle;

    public float buttonSize;
    public float bpm = 120f;           // Beats Per Minute
    public float minScale;
    public float maxScale;

    private float beatDuration;        // Time of one beat
    private float timer;

    public int _points;
    public bool _available;
    public bool reachZero;
    public TextMeshProUGUI _textMain;
    public TextMeshProUGUI _pointsText;
    public string[] _texts;

    public Image[] _Spheres;
    public int _onSphere;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatDuration = 60f / bpm;      // Duration of a beat in seconds
        timer = 0f;
    
    }

    // Update is called once per frame

    void Update()
    {
        timer += Time.deltaTime;

        // Loop every beat
        if (timer > beatDuration)
            timer -= beatDuration;

        // Normalized time within the beat (0 to 1)
        float t = timer / beatDuration;

        // Smooth pulse using sine wave (0 to 1 to 0)
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.Sin(t * Mathf.PI));

        // Apply scale
        if (_timerCircle != null)
            _timerCircle.transform.localScale = new Vector3(scale, scale, 1f);

        if(scale <= buttonSize)
        {
            _mainButton.color = Color.green;
        }
        else
        {
            _mainButton.color = Color.grey;
            if (reachZero)
            {
                _available = true;               
            }
       
        }

        if (!reachZero && Mathf.Abs(scale - minScale) < 0.05f)
        {
            reachZero = true;
            
        }

        _mainButton.transform.localScale = new Vector2(buttonSize, buttonSize);

        if (Input.GetButtonDown("Submit") && _available)
        {
            _textMain.GetComponent<TextMeshProUGUI>().enabled = true;
            if (scale <= buttonSize)
            {
                _points++;
                _available = false;               
                _textMain.text = _texts[0];
                _textMain.color = Color.green;
                if(_onSphere < 3)
                {
                    _Spheres[_onSphere].color = Color.green;
                    _onSphere++;
                }
          
            }
            else
            {
                _textMain.text = _texts[1];
                _textMain.color = Color.red;
            }

            _textMain.GetComponent<Animator>().SetTrigger("TextOn");
        }
        _pointsText.text = _points.ToString();

        if(_onSphere >= 2)
        {
            //for(int i = 0; i < _Spheres.Length; i++)
            //{
            //    _Spheres[_onSphere].color = Color.white;
            //    _onSphere = 0;
            //}
            transform.parent.GetComponent<MainGameplayController>()._wins = true;           
        }
    }
}
