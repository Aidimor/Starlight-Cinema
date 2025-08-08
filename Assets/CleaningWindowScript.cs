using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CleaningWindowScript : MonoBehaviour
{
    public GameObject _mopPlayer;
    public float moveSpeed;
    public bool _gameStarts;

    [System.Serializable]
    public class AllManchas
    {
        public Image _manchaImage;
        public Vector2 _manchaPos;        
        public float _distance;
        public bool _cursorOver;
        public bool _activa;
    }
    public AllManchas[] _allManchas;

    public List<int> choosenManchas = new List<int>();
    public int _totalManchas;
    public float _minDistance;
    public float _multiplier;

    void Start()
    {
 
        StartCoroutine(GameStartsNumerator());

    }

    public IEnumerator GameStartsNumerator()
    {
        PickRandomManchas();
        yield return new WaitForSeconds(1);
        _gameStarts = true;
    }

    // Rename Shuffle to PickRandomManchas and remove unused parameter
    void PickRandomManchas()
    {
        for (int i = 0; i < _allManchas.Length; i++)
        {
            _allManchas[i]._manchaPos = _allManchas[i]._manchaImage.GetComponent<RectTransform>().anchoredPosition;
        }
        choosenManchas = PickUniqueRandomNumbers(_allManchas.Length, _totalManchas);

        for (int i = 0; i < choosenManchas.Count; i++)
        {
            _allManchas[choosenManchas[i]]._manchaImage.gameObject.SetActive(true);
            _allManchas[choosenManchas[i]]._manchaImage.gameObject.transform.localScale = new Vector3(1, 1, 1);
            _allManchas[choosenManchas[i]]._activa = true;
        }
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

    // Update is called once per frame
    void Update()
    {
        if (_gameStarts)
            MopController();
    }

    public void MopController()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _mopPlayer.GetComponent<RectTransform>().anchoredPosition += input * moveSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Submit"))
        {
            LocateMob();
        }
    }


    public void LocateMob()
    {
        for(int i = 0; i < choosenManchas.Count; i++)
        {
            _allManchas[choosenManchas[i]]._distance = Vector2.Distance(
                _mopPlayer.GetComponent<RectTransform>().anchoredPosition, 
                _allManchas[choosenManchas[i]]._manchaImage.GetComponent<RectTransform>().anchoredPosition);

            if(_allManchas[choosenManchas[i]]._distance < _minDistance && _allManchas[choosenManchas[i]]._manchaImage.gameObject.active)
            {
                _allManchas[choosenManchas[i]]._activa = false;
                _allManchas[choosenManchas[i]]._manchaImage.gameObject.active = false;


            }
        }

        WinsChecker();
    }

    public void WinsChecker()
    {
        bool win = true;
        for(int i = 0; i < _allManchas.Length; i++)
        {
            if (_allManchas[i]._activa)
            {
                win = false;
            }            
        }
        transform.parent.gameObject.GetComponent<MainGameplayController>()._wins = win;
    }
}
