using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RuletaScript : MonoBehaviour
{
    //public int _WinColorID;
    public int _realWinColorID;
    public Image _background;
    public GameObject _semiCirclePrefabs;
    public int _totalSpaces;
    public List<GameObject> allSpaces = new List<GameObject>();
    public List<int> colorsChoosed = new List<int>();

    public float _rotationSpeed;
    public bool _choosed;
    public bool _starts;
    public Color[] _allColors;

    public Image _flecha;

    public GameObject OnFragment;

    public Vector2[] _betweenIntervals;

    public Image[] _winLoseImages;
    public Image[] _buttonImages;
    public Image[] _cameraImages;

    void Start()
    {
        SemiCircleSet();
        StartCoroutine(StartNumerator());
    }

    public void GameStarts()
    {
        SemiCircleSet();
        StartCoroutine(StartNumerator());
    }



    IEnumerator StartNumerator()
    {
        yield return new WaitForSeconds(1 / transform.parent.GetComponent<MainGameplayController>()._onLevel);
        _starts = true;
    }

    void Update()
    {
        ControllerScript();
        //if (!_choosed)
        //IntConotroller();
        //UpdateSelectedFragmentByAngle();
    }

    void ControllerScript()
    {
        if (!_choosed)
            _background.transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Submit") && !_choosed && _starts)
            StartCoroutine(ChooseNumerator());
    }


    void SemiCircleSet()
    { 
        GenerateRandomOrder();

        int random = Random.Range(0, _totalSpaces);
    /*    _WinColorID = colorsChoosed[random]*/;
        

        float fillAmountPerFragment = 1f / _totalSpaces;
        float anglePerFragment = 360f / _totalSpaces;
        float startAngle = anglePerFragment / 2f;  // Centrar primer fragmento

        for (int i = 0; i < _totalSpaces; i++)
        {
            GameObject Spaces = Instantiate(_semiCirclePrefabs, _background.transform.localPosition, Quaternion.identity);

            Spaces.name = colorsChoosed[i].ToString();

            allSpaces.Add(Spaces);
            Spaces.transform.SetParent(_background.transform, false);

            Image img = Spaces.GetComponent<Image>();
            img.fillAmount = fillAmountPerFragment;
            img.color = _allColors[colorsChoosed[i]];

            //Spaces.transform.localScale = Vector3.one;
            Spaces.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            // Rotar para que el fragmento quede centrado en su sector
            float angle = startAngle + i * anglePerFragment;
            Spaces.transform.rotation = Quaternion.Euler(0f, 0f, angle);

           
            //if (colorsChoosed[i] == _WinColorID)
            //{
            //    _realWinColorID = i;
            //}
        }

        _flecha.color = _allColors[colorsChoosed[0]];

    }

    void GenerateRandomOrder()
    {
        colorsChoosed.Clear();

        for (int i = 0; i < _totalSpaces; i++)
            colorsChoosed.Add(i);

        for (int i = 0; i < colorsChoosed.Count - 1; i++)
        {
            int rand = Random.Range(i, colorsChoosed.Count);
            int temp = colorsChoosed[i];
            colorsChoosed[i] = colorsChoosed[rand];
            colorsChoosed[rand] = temp;
        }
    }

    public IEnumerator ChooseNumerator()
    {
        _choosed = true;
        _buttonImages[0].gameObject.SetActive(false);
        _buttonImages[1].gameObject.SetActive(true);
        _cameraImages[0].gameObject.SetActive(false);
        _cameraImages[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        if (_background.GetComponent<RectTransform>().eulerAngles.z >= _betweenIntervals[_totalSpaces].x && _background.GetComponent<RectTransform>().eulerAngles.z <= _betweenIntervals[_totalSpaces].y)
        {
            _winLoseImages[0].gameObject.SetActive(true);
            transform.parent.GetComponent<MainGameplayController>()._wins = true;        
        }
        else
        {
            _winLoseImages[1].gameObject.SetActive(true);   
            transform.parent.GetComponent<MainGameplayController>()._wins = false;
        }
   
        transform.parent.GetComponent<MainGameplayController>()._scriptTimer._timer = 1;



    }



}
