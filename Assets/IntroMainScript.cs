using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class IntroMainScript : MonoBehaviour
{
    [SerializeField] private PauseController _scriptPause;
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
            public int _onButton;
            public GameObject[] _buttons;
        
        }
        public CandyShopPositions[] _candyShopPositions;
        public bool _buttonChange;
    }
    public CandyShopAssets candyShopAssets;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scriptPause = GetComponent<PauseController>();
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
            if (Input.GetButtonDown("Submit") && !_gameStarted && _controllerOn && !_scriptPause._onPause) {
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

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().enabled = true;

        }

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().SetBool("Active", true);
            yield return new WaitForSeconds(0.1f);
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

                for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
                {
                    candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().SetBool("Active", false);                
                }

            for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
            {
                candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().enabled = true;

            }

            candyShopAssets._onPos++;
            candyShopAssets._changing = true;
            StartCoroutine(CandyShopChangeNumerator());
        }

        if (Input.GetAxisRaw("Horizontal") < 0 && !candyShopAssets._changing)
        {
           
            if (candyShopAssets._onPos > 0)
                for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
                {
                    candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().SetBool("Active", false);
                }

            for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
            {
                candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().enabled = true;

            }

            candyShopAssets._onPos--;
            candyShopAssets._changing = true;
            StartCoroutine(CandyShopChangeNumerator());


        }

        if (candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length > 1)
        {
            if (Input.GetAxisRaw("Vertical") < 0 && !candyShopAssets._buttonChange && candyShopAssets._candyShopPositions[candyShopAssets._onPos]._onButton <
               candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length - 1 && !candyShopAssets._changing)
            {
                candyShopAssets._candyShopPositions[candyShopAssets._onPos]._onButton++;
                ChangeButton();
                candyShopAssets._buttonChange = true;
            }

            if (Input.GetAxisRaw("Vertical") > 0 && !candyShopAssets._buttonChange && candyShopAssets._candyShopPositions[candyShopAssets._onPos]._onButton > 0 && !candyShopAssets._changing)
            {
                candyShopAssets._candyShopPositions[candyShopAssets._onPos]._onButton--;
                ChangeButton();
                candyShopAssets._buttonChange = true;
            }

            if (Input.GetAxisRaw("Vertical") == 0)
            {     
                candyShopAssets._buttonChange = false;
            }
        }



        if (Input.GetButtonDown("Submit") && !_scriptPause._onPause)
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

    public void ChangeButton()
    {
        if(candyShopAssets._onPos == 2)
        {
            int _onRealButton = candyShopAssets._candyShopPositions[candyShopAssets._onPos]._onButton;
            Debug.Log("otadasd");
            for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++){
                candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].transform.localScale = new Vector3(1, 1, 1);          
            }
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[_onRealButton].transform.localScale = new Vector3(1.25f, 1.25f, 1);
        }
    }

    public IEnumerator CandyShopChangeNumerator()
    {
        candyShopAssets._candyShopPositions[0]._onButton = 0;
        candyShopAssets._candyShopPositions[1]._onButton = 0;
        candyShopAssets._candyShopPositions[2]._onButton = 0;

        for (int y = 0; y < candyShopAssets._candyShopPositions.Length; y++)
        {
            for (int i = 0; i < candyShopAssets._candyShopPositions[y]._chars.Length; i++)
            {
                candyShopAssets._candyShopPositions[y]._chars[i].SetBool("Enter", false);               
            }
        }


        //yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._chars[i].SetBool("Enter", true);
            yield return new WaitForSeconds(0.25f);
        }

        for(int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().SetBool("Active", true);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
       

        for (int i = 0; i < candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons.Length; i++)
        {
            candyShopAssets._candyShopPositions[candyShopAssets._onPos]._buttons[i].GetComponent<Animator>().enabled = false;

        }
        ChangeButton();
        candyShopAssets._changing = false;
    }
}
