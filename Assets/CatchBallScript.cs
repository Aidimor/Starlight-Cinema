using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class CatchBallScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject _ball;
    public GameObject _glass;
    public GameObject _stage;
    public bool _startGame;
    public float _speed;
    public float _glassSpeed;
    public bool _pushed;
    public bool _win;
    public bool _boolBall;

    public Image[] _catchImages;
    public GameObject[] _melaniImages;
    public Animator _explosionParticle;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (_startGame)
        {
            if (_boolBall)
            {
                BallMoving();
            }
         
            if (!_pushed)
            {
                GlassController();
            }
      
        }
        _stage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(_stage.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 100 * Time.deltaTime);

        _catchImages[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(_glass.GetComponent<RectTransform>().anchoredPosition.x,
            _catchImages[0].GetComponent<RectTransform>().anchoredPosition.y);
    }

    public IEnumerator StartGame()
    {
        _ball.transform.localScale = new Vector2(1, _ball.transform.localScale.y);
        int _randomIntRat = Random.Range(0, 10);
        if(_randomIntRat < 2)
        {
            _ball.GetComponent<Animator>().Play("Rat2");
        }
        else
        {
            _ball.GetComponent<Animator>().Play("Rat1");
        }
        yield return new WaitForSeconds(1 / transform.parent.GetComponent<MainGameplayController>()._onLevel);
        _speed = Random.Range(500, 1000);
        _startGame = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1f) / transform.parent.GetComponent<MainGameplayController>()._onLevel);
        _boolBall = true;
    }

    public void BallMoving()
    {
        _ball.transform.position += Vector3.right * -_speed * Time.deltaTime;
    }

    public void GlassController()
    {
     
        //_glass.transform.position += new Vector3(Input.GetAxis("Horizontal") * _glassSpeed * Time.deltaTime, 0f, 0f);

        Vector3 pos = _glass.transform.localPosition;

        // Update position using input
        pos.x += Input.GetAxis("Horizontal") * _glassSpeed * Time.deltaTime;

        // Clamp the X position
        pos.x = Mathf.Clamp(pos.x, -750f, 750f);

        // Apply it back
        _glass.transform.localPosition = pos;

        if (Input.GetButtonDown("Submit") && !_pushed)
        {
            StartCoroutine(CatchNumerator());
        }

    }

    public IEnumerator CatchNumerator()
    {
       
        _pushed = true;
        RectTransform rectBall = _ball.GetComponent<RectTransform>();
        RectTransform rect = _glass.GetComponent<RectTransform>();
        if (rect == null)
        {
           
            yield break;
        }

        Vector2 targetUp = new Vector2(rect.anchoredPosition.x, 300);

        while (Vector2.Distance(rect.anchoredPosition, targetUp) > 0.1f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, targetUp, 2000f * Time.deltaTime);
            yield return null;
        }


        _catchImages[0].gameObject.SetActive(false);
        _catchImages[1].gameObject.SetActive(true);
        _melaniImages[0].gameObject.SetActive(false);
        _melaniImages[1].gameObject.SetActive(true);


        Vector2 targetDown = new Vector2(rect.anchoredPosition.x, -115);

        while (Vector2.Distance(rect.anchoredPosition, targetDown) > 0.1f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, targetDown, 6000f * Time.deltaTime);
            _explosionParticle.Play("ExplosionParticle");           
            yield return null;
            _stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5f);
        }
     

        float ballXpos = _ball.GetComponent<RectTransform>().anchoredPosition.x;
        float glassXpos = _glass.GetComponent<RectTransform>().anchoredPosition.x;
        float halfGlassWidth = (_glass.transform.localScale.x * 50f) - 50f;

        yield return new WaitForSeconds(0.05f / transform.parent.GetComponent<MainGameplayController>()._onLevel);
        _melaniImages[1].gameObject.SetActive(false);

        if (ballXpos > glassXpos - halfGlassWidth && ballXpos < glassXpos + halfGlassWidth)
        {
            transform.parent.GetComponent<MainGameplayController>()._wins = true;
            _ball.gameObject.SetActive(false);
            _melaniImages[2].gameObject.SetActive(true);
            //_winText.gameObject.SetActive(true);
            _startGame = false;
        }
        else
        {
            _startGame = false;

            //RectTransform rectG = _glass.GetComponent<RectTransform>();
            //RectTransform ballRectB = _ball.GetComponent<RectTransform>();

            //float ballXpos = ballRectB.anchoredPosition.x;
            //float glassXpos = rectG.anchoredPosition.x;

            _catchImages[1].gameObject.SetActive(false);
            _catchImages[2].gameObject.SetActive(true);
            _melaniImages[3].gameObject.SetActive(true);

            Vector2 target;

            if (ballXpos > glassXpos)
            {
                // Ball is to the right → move to the right (off-screen)
                target = new Vector2(1500f, rectBall.anchoredPosition.y);
                _ball.transform.localScale = new Vector2(-_ball.transform.localScale.x, _ball.transform.localScale.y);
                _catchImages[2].gameObject.transform.localScale = new Vector2(-_catchImages[2].gameObject.transform.localScale.x, _catchImages[2].gameObject.transform.localScale.y);
                _melaniImages[3].gameObject.transform.localScale = new Vector2(-_melaniImages[3].gameObject.transform.localScale.x, _melaniImages[3].gameObject.transform.localScale.y);
            }
            else
            {
                // Ball is to the left → move to the left (off-screen)
                target = new Vector2(-1500f, rectBall.anchoredPosition.y);
                _ball.transform.localScale = new Vector2(_ball.transform.localScale.x, _ball.transform.localScale.y);
                _catchImages[2].gameObject.transform.localScale = new Vector2(_catchImages[2].gameObject.transform.localScale.x, _catchImages[2].gameObject.transform.localScale.y);
                _melaniImages[3].gameObject.transform.localScale = new Vector2(_melaniImages[3].gameObject.transform.localScale.x, _melaniImages[3].gameObject.transform.localScale.y);
            }

            // Smooth movement
            while (Vector2.Distance(rectBall.anchoredPosition, target) > 0.1f)
            {
                rectBall.anchoredPosition = Vector2.MoveTowards(rectBall.anchoredPosition, target, 3000f * Time.deltaTime);
                yield return null;
            }


        }
    
        transform.parent.GetComponent<MainGameplayController>()._scriptTimer._timer = 1;
        //yield return new WaitForSeconds(2);
        //Application.LoadLevel(0);



    }


}
