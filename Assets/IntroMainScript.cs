using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class IntroMainScript : MonoBehaviour
{
    public bool _shopActive;
    public Image _rotationBack;
    public bool _controllerOn;
    public float _rotationSpeed;
    public Animator _logoAnimator;
    public bool _gameStarted;

    public GameObject WinLosePanel;
    public Image _mainChar;
    public Sprite[] _charSprites;
    public TextMeshProUGUI _winLoseText;
    public GameObject _regresarButton;



    [System.Serializable]
    public class CandyShopAssets
    {
        public bool _CandyShopAvailable;
        public bool _changing;
        public int _onPos;
        public GameObject _parent;
        [System.Serializable]
        public class CandyShopPositions
        {
            public Vector3 _position;
            public float _scale;
            public Animator[] _chars; 
        }
        public CandyShopPositions[] _candyShopPositions;
    }
    public CandyShopAssets candyShopAssets;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        candyShopAssets._parent.SetActive(_shopActive);
        switch (_shopActive)
        {
            case true:            
                StartCoroutine(StartNumerator());
                break;
            case false:
                break;
    
        }
     
       
    }

    // Update is called once per frame
    void Update()
    {
        _rotationBack.transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
            if (Input.GetButtonDown("Submit") && !_gameStarted && _controllerOn) {
                StartCoroutine(GetComponent<MainGameplayController>().StartGameNumerator());
             
                _gameStarted = true;
            _logoAnimator.SetBool("Starts", false);
        }

        if (candyShopAssets._CandyShopAvailable)
            CandyShopVoid();

    }

    public IEnumerator StartNumerator()
    {
        yield return new WaitForSeconds(1);
       _logoAnimator.SetBool("Starts", true);

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars[i].SetBool("Enter", true);
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(1);
        _controllerOn = true;

    
    }

    public void CandyShopVoid()
    {
        candyShopAssets._parent.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(candyShopAssets._parent.GetComponent<RectTransform>().anchoredPosition,
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._position, 5 * Time.deltaTime);

        candyShopAssets._parent.transform.localScale = Vector3.Lerp(candyShopAssets._parent.transform.localScale, 
            new Vector3(candyShopAssets._candyShopPositions[candyShopAssets._onPos]._scale, candyShopAssets._candyShopPositions[candyShopAssets._onPos]._scale, 1)
     , 5 * Time.deltaTime);

        if(Input.GetAxisRaw("Horizontal") > 0 && !candyShopAssets._changing)
        {     
            if(candyShopAssets._onPos < 2)
            candyShopAssets._onPos++;
            candyShopAssets._changing = true;
            StartCoroutine(CandyShopChangeNumerator());
        }

        if (Input.GetAxisRaw("Horizontal") < 0 && !candyShopAssets._changing)
        {
           
            if (candyShopAssets._onPos > 0)
                candyShopAssets._onPos--;
            candyShopAssets._changing = true;
            StartCoroutine(CandyShopChangeNumerator());


        }

        if (Input.GetButtonDown("Submit"))
        {
            switch (candyShopAssets._onPos)
            {
                case 1:
                    GetComponent<MainGameplayController>().StartGameNumerator();
                    candyShopAssets._parent.SetActive(false);
                    candyShopAssets._CandyShopAvailable = false;
                    break;
            }
        }

   
    }

    public IEnumerator CandyShopChangeNumerator()
    {
        for(int y = 0; y < candyShopAssets._candyShopPositions.Length; y++)
        {
            for (int i = 0; i < candyShopAssets._candyShopPositions[y]._chars.Length; i++)
            {
                candyShopAssets._candyShopPositions[y]._chars[i].SetBool("Enter", false);               
            }
        }


        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars[i].SetBool("Enter", true);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        candyShopAssets._changing = false;
    }
}
