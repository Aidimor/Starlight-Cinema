using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleLeftRight : MonoBehaviour
{
    public Transform _objectParent;
    public GameObject _player;
    public GameObject _chef;
    public Animator _playerAnimator;
    public GameObject _bullet;
    public float moveSpeed;
    public float _timer;
    public bool _readyToAttack;
    public bool _canServe;
    public bool _movementLocked;

    public float _enemyTimer;
    public int _onEnemy;
    public int _maxEnemies;
    public int _enemiesFail;
    public int _maxEnemiesFail;
    public GameObject _enemyObject;

    public List<GameObject> _allEnemies = new List<GameObject>();

    public bool _finished;
    public bool _win;

    private void Start()
    {
        InstantiateEnemies();
    }

    void Update()
    {
        if (!_movementLocked)
        {
            float moveInput = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1
            Vector3 move = new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
            _player.transform.position += move;

            // Clamp the player's X position between -5 and 5
            Vector3 clampedPosition = _player.transform.localPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -5f, 5f);
            _player.transform.localPosition = clampedPosition;

            if (Input.GetButtonDown("Submit") && _timer <= 0 && _readyToAttack)
            {
                _timer = 0.5f;
                _playerAnimator.GetComponent<Animator>().SetTrigger("Attack");
                _canServe = false;
                StartCoroutine(AttackNumerator());
            }
            ChefController();
            _timer -= Time.deltaTime;
            _playerAnimator.SetBool("Ready", _readyToAttack);
        }
     

        if(_player.transform.localPosition.x > _chef.transform.localPosition.x && _player.transform.localPosition.x < _chef.transform.localPosition.x + 2f && 
            !_readyToAttack && _canServe && Input.GetAxisRaw("Horizontal") != 0)
        {
            _playerAnimator.SetTrigger("Receive");
            _readyToAttack = true;
            _movementLocked = true;
            StartCoroutine(CatchNumerator());
        }

       
        if(_maxEnemies != _onEnemy)
        {
            _enemyTimer -= Time.deltaTime;
        }


        if(_enemyTimer <= 0 && !_finished)
        {
            EnemyAppears();
            _enemyTimer = Random.Range(3f, 5f);
        }

        //if(_onEnemy > _maxEnemies)
        //{
        //    _finished = true;
        //}

 
    }

    public void InstantiateEnemies()
    {
        for (int i = 0; i < _maxEnemies; i++)
        {
            GameObject Enemies = Instantiate(_enemyObject, transform.position, transform.rotation);
            Enemies.GetComponent<EnemyFinalStage>()._scriptParent = this.gameObject.GetComponent<SimpleLeftRight>();
            Enemies.transform.parent = _objectParent.transform;
            Enemies.transform.localScale = new Vector2(1, 1);
            Enemies.transform.localPosition = new Vector2(Random.Range(-5f, 5f), -7f);
            Enemies.GetComponent<EnemyFinalStage>()._speed = Random.Range(200f, 400f);
            _allEnemies.Add(Enemies);
            Enemies.gameObject.SetActive(false);
        }

    }

    public IEnumerator CatchNumerator()
    {
        yield return new WaitForSeconds(0.1f);
        _movementLocked = false;
    }



    public IEnumerator AttackNumerator()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject Bullet = Instantiate(_bullet, _player.transform.position, _player.transform.rotation);
        Bullet.transform.parent = _objectParent.transform;
        Bullet.transform.localScale = new Vector2(1, 1);
        yield return new WaitForSeconds(0.1f);
        _chef.transform.localPosition = new Vector2(Random.Range(-3f, 3f), _chef.transform.localPosition.y);
        _readyToAttack = false;
        yield return new WaitForSeconds(0.5f);
        _canServe = true;
    }

    public void ChefController()
    {
        switch (_readyToAttack)
        {
            case false:
                if (_canServe)
                    _chef.transform.localPosition = Vector2.Lerp(_chef.transform.localPosition, new Vector2(_chef.transform.localPosition.x, 3.5f), 15 * Time.deltaTime);
                break;
            case true:             
                _chef.transform.localPosition = Vector2.Lerp(_chef.transform.localPosition, new Vector2(_chef.transform.localPosition.x, 5.5f), 15 * Time.deltaTime);
                break;
        }
    }

    public void EnemyAppears()
    {
        _allEnemies[_onEnemy].SetActive(true);
        _onEnemy++;
    }

   
    public void WinChecker()
    {
        StartCoroutine(WinLoseNumerator());
    }

    public IEnumerator WinLoseNumerator()
    {
        bool _localFinished = true;
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < _allEnemies.Count; i++)
        {
            if (_allEnemies[i] != null)
            {
                _localFinished = false;
            }
        }

        Debug.Log(_localFinished);
        if (_localFinished)
        {
            _finished = true;
            if (_enemiesFail >= _maxEnemiesFail)
            {
                _win = false;
            }
            else
            {
                _win = true;
            }
            _finished = true;


            _movementLocked = true;
            yield return new WaitForSeconds(0.5f);
            switch (_win)
            {
                case false:
                    _playerAnimator.SetTrigger("Lose");
                    break;
                case true:
                    _playerAnimator.SetTrigger("Win");
                    break;
            }
        }


  
       
    }
    
}
