using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ChooseSecuenceScript : MonoBehaviour
{
    public List<GameObject> OnGameObjects = new List<GameObject>();
    public List<float> _GameObjectsXpos = new List<float>();
    public Transform _parentTransform;
    public Transform _gameplayTransform;
    public int _totalSecuences;

    public Color[] _allColors;
    public GameObject _colorPrefab;
    public GameObject _gamePrefab;
    public GameObject _pivotPrefab;

    public List<int> _secuenceNumber = new List<int>();
    public List<int> _gameSecuenceNumber = new List<int>();

    public List<float> Xpos = new List<float>();
    public List<GameObject> _pivotList = new List<GameObject>();
    public GameObject _selector;
    public int _onSelectorPos;
    bool _moved;
    public bool _gameStarts;
    public bool _changingPose;
   
    public bool _win;

    void Start()
    {
        //_selector.SetActive(false);
            
        //StartCoroutine(GameStartsNumerator());
    }

    public void GameStarts()
    {
        _selector.SetActive(false);
       
        StartCoroutine(GameStartsNumerator());
    }

    public void ChooseSequence()
    {
      
        _secuenceNumber.Clear();
        _gameSecuenceNumber.Clear();

        // Step 1: Create list of all indices
        List<int> allIndices = new List<int>();
        for (int i = 0; i < _allColors.Length; i++)
        {
            allIndices.Add(i);
        }

        // Step 2: Shuffle allIndices
        for (int i = allIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = allIndices[i];
            allIndices[i] = allIndices[j];
            allIndices[j] = temp;
        }

        // Step 3: Take first _totalSecuences indices
        int count = Mathf.Min(_totalSecuences, allIndices.Count);
        for (int i = 0; i < count; i++)
        {
            _secuenceNumber.Add(allIndices[i]);
        }

        // Step 4: Copy and shuffle for _gameSecuenceNumber
        for (int i = 0; i < _secuenceNumber.Count; i++)
        {
            _gameSecuenceNumber.Add(_secuenceNumber[i]);
        }

        // Step 5: Shuffle _gameSecuenceNumber until it's different from _secuenceNumber
        bool isSameOrder = true;
        int maxAttempts = 10;
        int attempt = 0;

        while (isSameOrder && attempt < maxAttempts)
        {
            attempt++;

            // Fisher-Yates shuffle
            for (int i = _gameSecuenceNumber.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int temp = _gameSecuenceNumber[i];
                _gameSecuenceNumber[i] = _gameSecuenceNumber[j];
                _gameSecuenceNumber[j] = temp;
            }

            // Compare orders
            isSameOrder = true;
            for (int i = 0; i < _secuenceNumber.Count; i++)
            {
                if (_secuenceNumber[i] != _gameSecuenceNumber[i])
                {
                    isSameOrder = false;
                    break;
                }
            }
        }

        // Optional: force a swap if still same (for very small lists)
        if (isSameOrder && _gameSecuenceNumber.Count > 1)
        {
            int temp = _gameSecuenceNumber[0];
            _gameSecuenceNumber[0] = _gameSecuenceNumber[1];
            _gameSecuenceNumber[1] = temp;
        }

        // Step 6: Instantiate color sequence objects
        for (int i = 0; i < count; i++)
        {
            GameObject secuenceColorPrefab = Instantiate(_colorPrefab, transform.position, transform.rotation);
            secuenceColorPrefab.transform.SetParent(_parentTransform, false);
            secuenceColorPrefab.GetComponent<Image>().color = _allColors[_secuenceNumber[i]];
            secuenceColorPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2((150 * i), 0);

         
        }

        _parentTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2((count * -50), _parentTransform.GetComponent<RectTransform>().anchoredPosition.y);

        for (int i = 0; i < _totalSecuences; i++)
        {
            GameObject GameplayP = Instantiate(_gamePrefab, transform.position, transform.rotation);
            GameplayP.transform.SetParent(_gameplayTransform, false);
            GameplayP.GetComponent<Image>().color = _allColors[_gameSecuenceNumber[i]];
            GameplayP.GetComponent<RectTransform>().anchoredPosition = new Vector2((300 * i), 0);
            GameplayP.name = "Object" + i.ToString();
            OnGameObjects.Add(GameplayP);
            _GameObjectsXpos.Add(GameplayP.GetComponent<RectTransform>().anchoredPosition.x);

            if (i > 0)
            {
                Xpos.Add(300 * i);
         
         
            }





        }


        for (int i = 0; i < Xpos.Count; i++)
        {
            GameObject Pp = Instantiate(_pivotPrefab, transform.position, transform.rotation);
            Pp.transform.SetParent(_gameplayTransform, false);
            Pp.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xpos[i] - 150, 0);
            _pivotList.Add(Pp);
        }

        _gameplayTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2((count * -100), _gameplayTransform.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public IEnumerator GameStartsNumerator()
    {
        ChooseSequence();
        yield return new WaitForSeconds(0.5f);     
        _selector.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _gameStarts = true;
    }

    private void Update()
    {
        if (_gameStarts)
        {
            ControllerVoid();
            if (_changingPose)
            {
                ChangingPoseVoid();
            }
        }   
    }

    public void ChangingPoseVoid()
    {
 

        for (int i = 0; i < OnGameObjects.Count; i++)
        {
            OnGameObjects[i].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(OnGameObjects[i].GetComponent<RectTransform>().anchoredPosition, new Vector2(_GameObjectsXpos[i], 0), 10 * Time.deltaTime);
        }
        //OnGameObjects[_onSelectorPos].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(OnGameObjects[_onSelectorPos].GetComponent<RectTransform>().anchoredPosition, new Vector2(_GameObjectsXpos[_onSelectorPos + 1], 0), 10 * Time.deltaTime);
        //OnGameObjects[_onSelectorPos + 1].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(OnGameObjects[_onSelectorPos + 1].GetComponent<RectTransform>().anchoredPosition, new Vector2(_GameObjectsXpos[_onSelectorPos], 0), 10 * Time.deltaTime);
    }

    public void ControllerVoid()
    {
        _selector.transform.position = Vector2.Lerp(_selector.transform.position, _pivotList[_onSelectorPos].transform.position, 10 * Time.deltaTime);

        if(Input.GetAxisRaw("Horizontal") > 0 && !_moved && _onSelectorPos < _pivotList.Count - 1)
        {
            _onSelectorPos++;
            _moved = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && !_moved && _onSelectorPos > 0)
        {
            _onSelectorPos--;
            _moved = true;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            _moved = false;
        }

        if (Input.GetButton("Submit") && !_changingPose)
        {
            _changingPose = true;
            StartCoroutine(ChangingPos());
        }
    }

    public IEnumerator ChangingPos()
    {
        GameObject temp = OnGameObjects[_onSelectorPos];
        OnGameObjects[_onSelectorPos] = OnGameObjects[_onSelectorPos + 1];
        OnGameObjects[_onSelectorPos + 1] = temp;

        int temp2 = _gameSecuenceNumber[_onSelectorPos];
        _gameSecuenceNumber[_onSelectorPos] = _gameSecuenceNumber[_onSelectorPos + 1];
        _gameSecuenceNumber[_onSelectorPos + 1] = temp2;
        yield return new WaitForSeconds(0.2f);
        CompareVoid();
        yield return new WaitForSeconds(0.1f);
        if (_win)
        {          
            _gameStarts = false;
        }
        else
        {
            _changingPose = false;
        }
     
    }

    public void CompareVoid()
    {
        _win = true;
        for (int i = 0; i < _totalSecuences; i++)
        {
            if(_secuenceNumber[i] != _gameSecuenceNumber[i])
            {
                _win = false;
            }
        }
        transform.parent.GetComponent<MainGameplayController>()._wins = _win;
    }


}

