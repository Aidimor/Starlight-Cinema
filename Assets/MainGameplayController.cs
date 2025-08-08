using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class MainGameplayController : MonoBehaviour
{
    public TimerScript _scriptTimer;
    public int _onLevel = 1;
    public Animator _sceneAnimator;
    public Animator _timerAnimator;
    public Animator _cinemaAnimator;
    public int _OnGame;
    public GameObject[] _scriptsObject;
    public GameObject[] _gamePanel;

    public List<int> _gamePlayList = new List<int>();

    public int _totalGames;
    public TextMeshProUGUI _winLoseText;
    public bool _wins;
    public Image[] _healthImage;
    public int _maxHP;
    public TextMeshProUGUI _onTurnText;
    public bool _gameStarts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
     
    }

    public IEnumerator StartGameNumerator()
    {
        _maxHP = 4;
        _OnGame = 0;
        for(int i = 0; i < 4; i++)
        {
            _healthImage[i].color = Color.green;
        }
        for (int i = 0; i < _totalGames; i++)
        {
            _gamePlayList.Add(Random.Range(0, _scriptsObject.Length));
        }
        ResetValues();
        _onTurnText.text = (_OnGame + 1).ToString("F0");
        yield return new WaitForSeconds(0.1f);
        _sceneAnimator.gameObject.SetActive(true);
        _sceneAnimator.Play("CinemaShopIdle");
        _sceneAnimator.SetBool("GameStarts", true);
        // Wait until the current animation has finished
        AnimatorStateInfo stateInfo = _sceneAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Optional: wait until animator is actually playing the desired animation
        yield return new WaitUntil(() => _sceneAnimator.GetCurrentAnimatorStateInfo(0).IsName("CinemaShopIdle"));

        // Now wait until the animation finishes
        yield return new WaitForSeconds(_sceneAnimator.GetCurrentAnimatorStateInfo(0).length);
        _scriptTimer._secondText.gameObject.SetActive(true);
       
        _sceneAnimator.gameObject.SetActive(false);
        _scriptsObject[_gamePlayList[_OnGame]].SetActive(true);
        _gamePanel[_gamePlayList[_OnGame]].SetActive(true);
        yield return new WaitForSeconds(0.5f);
    
        yield return new WaitForSeconds(0.5f);
        _gameStarts = true;
        yield return new WaitForSeconds(0.25f);
        if (_gamePlayList[_OnGame] != 9)
        {
            _scriptTimer._startTimer = true;
            _timerAnimator.SetBool("TimerIn", true);
            _cinemaAnimator.SetBool("CinemaIn", true);
        }

    }

    public IEnumerator RestartGameNumerator()
    {
        ResetValues();
        _timerAnimator.SetBool("TimerIn", false);
        _cinemaAnimator.SetBool("CinemaIn", false);
        for (int i = 0; i < _scriptsObject.Length; i++)
        {
            _scriptsObject[i].gameObject.SetActive(false);
            _gamePanel[i].gameObject.SetActive(false);
        }
        _sceneAnimator.gameObject.SetActive(true);
        _winLoseText.gameObject.SetActive(true);
        switch (_wins)
        {
            case false:
                _winLoseText.color = Color.red;
                _winLoseText.text = "Lose";
                _sceneAnimator.Play("CinemaShopLoses");
                LoseHealth();
                if (_maxHP == 0)
                {
                    LoseGame();
                    yield return new WaitForSeconds(1);
                    GetComponent<IntroMainScript>()._regresarButton.SetActive(true);
                    while (!Input.GetButtonDown("Submit"))
                    {
                        yield return null;
                    }
                    GetComponent<IntroMainScript>().WinLosePanel.gameObject.SetActive(false);
                    GetComponent<IntroMainScript>()._logoAnimator.SetBool("Starts", true);
                    yield return new WaitForSeconds(2);
                    GetComponent<IntroMainScript>()._gameStarted = false;
                    _sceneAnimator.gameObject.SetActive(false);
                    _winLoseText.text = "";            
                    yield break;
                }
                break;
            case true:
                _winLoseText.color = Color.green;
                _winLoseText.text = "Win";
                _sceneAnimator.Play("CinemaShopWins");            
                break;
        }

        if (_OnGame >= _totalGames - 1)
        {
            WinGame();
            yield return new WaitForSeconds(1);
            GetComponent<IntroMainScript>()._regresarButton.SetActive(true);
            while (!Input.GetButtonDown("Submit"))
            {
                yield return null;
            }
            GetComponent<IntroMainScript>().WinLosePanel.gameObject.SetActive(false);
            GetComponent<IntroMainScript>()._logoAnimator.SetBool("Starts", true);
            yield return new WaitForSeconds(2);
            GetComponent<IntroMainScript>()._gameStarted = false;
            _sceneAnimator.gameObject.SetActive(false);
            _winLoseText.text = "";
            yield break;
        }

        yield return new WaitForSeconds(1);
        _wins = false;
        _OnGame++;
        _onTurnText.text = (_OnGame + 1).ToString("F0");
        _scriptTimer._topTimer = 6 - _onLevel;
        _scriptTimer._timer = 6 - _onLevel;

        _winLoseText.gameObject.SetActive(false);
        // Wait until the current animation has finished
        AnimatorStateInfo stateInfo = _sceneAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Optional: wait until animator is actually playing the desired animation
        yield return new WaitUntil(() => _sceneAnimator.GetCurrentAnimatorStateInfo(0).IsName("CinemaShopIdle"));

        // Now wait until the animation finishes
        yield return new WaitForSeconds(_sceneAnimator.GetCurrentAnimatorStateInfo(0).length);

        Debug.Log("Animation finished");
        _sceneAnimator.gameObject.SetActive(false);
        _scriptsObject[_gamePlayList[_OnGame]].SetActive(true);
        _gamePanel[_gamePlayList[_OnGame]].SetActive(true);
        GameStarts();
        yield return new WaitForSeconds(0.5f);
    

       
        yield return new WaitForSeconds(0.5f);

        if (_gamePlayList[_OnGame] != 9)
        {
            _scriptTimer._startTimer = true;
            _timerAnimator.SetBool("TimerIn", true);
            _cinemaAnimator.SetBool("CinemaIn", true);
        }
     
        _gameStarts = true;

 
        _scriptTimer._carreteFinal.gameObject.SetActive(false);
        _scriptTimer._secondText.gameObject.SetActive(true);
    }

    public void ResetValues()
    {
        int onGameID = _gamePlayList[_OnGame];
     
        switch (onGameID)
        {
            case 0:
                var script1 = _scriptsObject[onGameID].GetComponent<GameplayController>();
                for (int i = 0; i < script1._Spheres.Length; i++)
                {
                    script1._Spheres[i].color = Color.white;
                    script1._onSphere = 0;
                }
                script1._points = 0;
                break;
            case 1:
                var script2 = _scriptsObject[onGameID].GetComponent<MissingLetterScript>();
                script2.letters.Clear();                
                script2._onLetterOption = 0;
                script2._correctLetterPos = 0;
                script2._missingLetter = 0;
                script2._letterOptions[0].text = "";
                script2._letterOptions[1].text = "";
                script2._letterOptions[2].text = "";
                script2._resultPoint.color = Color.white;
                script2._wordText.text = "";             
                script2._gameOn = false;
                script2._Win = false;
                script2._optionChoosed = false;
                script2._girl.GetComponent<RectTransform>().anchoredPosition = new Vector2(1000, 0);
                break;
            case 2:
                var script3 = _scriptsObject[onGameID].GetComponent<CleanGameplay>();
                for(int i = 0; i < script3._allEnemies.Count; i++)
                {
                    Destroy(script3._allEnemies[i].gameObject);
                }
                script3._allEnemies.Clear();
                script3.currentDirection = Vector2.zero;
                break;
            case 3:
                var scriptRuleta = _scriptsObject[onGameID].GetComponent<RuletaScript>();
                scriptRuleta.allSpaces.Clear();
                scriptRuleta.colorsChoosed.Clear();
                scriptRuleta._background.GetComponent<RectTransform>().eulerAngles = Vector3.zero;
                scriptRuleta._buttonImages[0].gameObject.SetActive(true);
                scriptRuleta._buttonImages[1].gameObject.SetActive(false);
                scriptRuleta._cameraImages[0].gameObject.SetActive(true);
                scriptRuleta._cameraImages[1].gameObject.SetActive(false);
                scriptRuleta._winLoseImages[0].gameObject.SetActive(false);
                scriptRuleta._winLoseImages[1].gameObject.SetActive(false);
                scriptRuleta._choosed = false;
                break;
            case 4:
                var script4 = _scriptsObject[onGameID].GetComponent<ChooseSecuenceScript>();
                for(int i = 0; i < script4.OnGameObjects.Count; i++)
                {
                    Destroy(script4.OnGameObjects[i].gameObject);
                }               
                script4.OnGameObjects.Clear();
                script4._GameObjectsXpos.Clear();
                script4._secuenceNumber.Clear();
                script4._gameSecuenceNumber.Clear();
                script4._gameStarts = false;
                script4._changingPose = false;
                script4._win = false;
                break;
            case 5:
                var script5 = _scriptsObject[onGameID].GetComponent<CatchBallScript>();
                script5._glass.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 175);
                script5._ball.GetComponent<RectTransform>().anchoredPosition = new Vector2(1110, -194);
                script5._pushed = false;
                script5._win = false;
                script5._boolBall = false;
                script5._ball.gameObject.SetActive(true);
                script5._catchImages[0].gameObject.SetActive(true);
                script5._catchImages[1].gameObject.SetActive(false);
                script5._catchImages[2].gameObject.SetActive(false);
                script5._melaniImages[0].gameObject.SetActive(true);
                script5._melaniImages[1].gameObject.SetActive(false);
                script5._melaniImages[2].gameObject.SetActive(false);
                script5._melaniImages[3].gameObject.SetActive(false);
                break;
            case 6:
                var script6 = _scriptsObject[onGameID].GetComponent<PopcornLiftScript>();
                script6._gameStarts = false;
                script6._popCornBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250);    
                break;
            case 7:
                var script7 = _scriptsObject[onGameID].GetComponent<CleaningWindowScript>();
                script7.choosenManchas.Clear();
                for(int i = 0; i < script7._allManchas.Length; i++)
                {
                    script7._allManchas[i]._manchaImage.gameObject.SetActive(false);
                    script7._allManchas[i]._manchaPos = Vector2.zero;             
                    script7._allManchas[i]._distance = 0;
                    script7._allManchas[i]._cursorOver = false;
                }
                script7._mopPlayer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;             
                break;
            case 8:
                var script8 = _scriptsObject[onGameID].GetComponent<ComplimentsScript>();
                script8._onOption = 0;
                script8._fillBool = false;     
                break;
        }

    }

    public void GameStarts()
    {
        int onGameID = _gamePlayList[_OnGame];

        switch (onGameID)
        {
            case 0:          
                break;
            case 1:
                var script2 = _scriptsObject[onGameID].GetComponent<MissingLetterScript>();
                script2.ActivationWord();        
                break;
            case 2:
                var script3 = _scriptsObject[onGameID].GetComponent<CleanGameplay>();
                script3.GameStart();
                break;
            case 3:
                var scriptRuleta = _scriptsObject[onGameID].GetComponent<RuletaScript>();
                Debug.Log("falla");
                scriptRuleta.GameStarts();
                break;
            case 4:
                var script4 = _scriptsObject[onGameID].GetComponent<ChooseSecuenceScript>();
                script4.GameStarts();
                break;
            case 5:
                var script5 = _scriptsObject[onGameID].GetComponent<CatchBallScript>();
                StartCoroutine(script5.StartGame());                
                break;
            case 6:
                var script6 = _scriptsObject[onGameID].GetComponent<PopcornLiftScript>();
                script6._gameStarts = true;            
                break;
            case 7:
                var script7 = _scriptsObject[onGameID].GetComponent<CleaningWindowScript>();
                script7._gameStarts = true;
                StartCoroutine(script7.GameStartsNumerator());
                break;
            case 8:
                var script8 = _scriptsObject[onGameID].GetComponent<ComplimentsScript>();
                script8._gameStarts = true;
                StartCoroutine(script8.StartNumerator());
                break;
        }
        Debug.Log(onGameID);
    }

    public void LoseHealth()
    {

        if(_maxHP > 0)
        {
            _maxHP--;

            for (int i = 0; i < 4; i++)
            {
                _healthImage[i].color = Color.black;
            }

            for (int i = 0; i < _maxHP; i++)
            {
                _healthImage[i].color = Color.green;
            }
        }
        else
        {
            _maxHP = 0;
            GetComponent<IntroMainScript>().WinLosePanel.gameObject.SetActive(false);
        }
  
    }

    public void LoseGame()
    {
        var Script = GetComponent<IntroMainScript>();
        Script.WinLosePanel.SetActive(true);
        Script._mainChar.sprite = Script._charSprites[1];
        Script._winLoseText.text = "LOSE";
        Script._winLoseText.color = Color.red;
        _gamePlayList.Clear();
        _OnGame = 0;
    }

    public void WinGame()
    {
        var Script = GetComponent<IntroMainScript>();
        Script.WinLosePanel.SetActive(true);
        Script._mainChar.sprite = Script._charSprites[0];
        Script._winLoseText.text = "WIN";
        Script._winLoseText.color = Color.green;
    }

}
