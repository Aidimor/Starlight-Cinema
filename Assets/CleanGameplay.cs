using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CleanGameplay : MonoBehaviour
{
    public int _totalEnemies;
    public float moveSpeed;
    public float smoothTime = 0.1f;
    public GameObject _player;
    public Animator _playerAnimator;
    public GameObject _enemy;
    public GameObject _map;
    public List<GameObject> _allEnemies = new List<GameObject>();
    public Vector3 currentDirection;
    public GameObject _gameParent;
    public List<Transform> _allExits = new List<Transform>();

    public float maxSpeed;
    public float minSpeed;
    public bool winCheked;
    public float _cleanDiameter;
    public float _reachDiameter;
    public bool _swiping;

    public float proximityDistance; // Change as needed

    [System.Serializable]
    public class FinishPanel
    {
        public GameObject Parent;
        public Image[] _guys;
        public Image _girl;
        public Image[] _words;
    }
    public FinishPanel _finishPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStart();  

    }

    public void GameStart()
    {
        for (int i = 0; i < _totalEnemies; i++)
        {
            // Instantiate enemy at map's position and rotation
            GameObject Enemy = Instantiate(_enemy, _map.transform.position, _map.transform.rotation);
            Enemy.GetComponent<EnemyScript>()._scriptClean = GetComponent<CleanGameplay>();
            Enemy.GetComponent<EnemyScript>()._garbageImages[Random.Range(0, 4)].SetActive(true);
           

            // Add to enemy list
            _allEnemies.Add(Enemy);

            // Set parent in hierarchy
            Enemy.transform.parent = _map.transform;
            Enemy.transform.localScale = new Vector3(1, 1, 1);
            // -------- Generate position between -3 and 3 in X and Y --------
            Vector2 randomPoint;
            do
            {
                randomPoint = new Vector2(Random.Range(-2f, 2f), Random.Range(-0.75f, 0.25f));
            } while (randomPoint.magnitude < 0.1f); // Optional: Avoid center

            // Set enemy local position relative to parent
            Enemy.transform.localPosition = randomPoint;

            // -------- Find closest target from the list --------
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform target in _allExits)
            {
                float dist = Vector2.Distance(Enemy.transform.position, target.position); // world positions
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestTarget = target;
                }
            }

            // Assign closest target to enemy
            EnemyScript enemyAI = Enemy.GetComponent<EnemyScript>();
            if (enemyAI != null && closestTarget != null)
            {
                enemyAI._exitTarget = closestTarget;
            }
            Enemy.transform.localPosition = new Vector3(Enemy.transform.localPosition.x, Enemy.transform.localPosition.y, -0.3f);
        }

        winCheked = false;
        _player.transform.localPosition = new Vector3(0, -3f, _player.transform.localPosition.z);
    }



    // Update is called once per frame
    void Update()
    {
        if (!winCheked)
        {
            PlayerController();
            //if (transform.parent.GetComponent<MainGameplayController>()._scriptTimer._timer <= 0.1f)
            //{
            //    WinChecker();
            //}
          
        }

        CheckPlayerEnemyDistance();
    }

    void PlayerController()
    {
        // Raw input (snappy)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 targetDirection = new Vector3(inputX, inputY, 0f).normalized;

        // Smooth interpolation factor
        float lerpFactor = smoothTime * Time.deltaTime;

        // Smoothly transition to target direction
        currentDirection = Vector3.Lerp(currentDirection, targetDirection, lerpFactor);

        // Move the player
        _player.transform.Translate(currentDirection * moveSpeed * Time.deltaTime);

        // --- Animation: Check if player is moving ---
        if (currentDirection.magnitude > 0.01f)
        {
  

            // --- Rotation ---
            float angle = Mathf.Atan2(-currentDirection.x, currentDirection.y) * Mathf.Rad2Deg;
            _playerAnimator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
   

        if(inputX == 0 && inputY == 0)
        {
            _playerAnimator.SetBool("Walking", false);
        }
        else
        {
            _playerAnimator.SetBool("Walking", true);
        }
    }



    public void WinChecker()
    {
        transform.parent.GetComponent<MainGameplayController>()._wins = true;
        for (int i = 0; i < _allEnemies.Count; i++)
        {
            if (!_allEnemies[i].GetComponent<EnemyScript>()._outOfMap)
            {
                transform.parent.GetComponent<MainGameplayController>()._wins = false;
            }
            else
            {              
            
            }
        }

        if (transform.parent.GetComponent<MainGameplayController>()._wins)
        {
            transform.parent.GetComponent<MainGameplayController>()._scriptTimer._timer = 1.5f;
            _gameParent.GetComponent<Animator>().Play("WinAnimation");
            winCheked = true;
        }
       
    }
    void CheckPlayerEnemyDistance()
    {
        bool shouldSwipe = false;

        foreach (GameObject enemy in _allEnemies)
        {
            if (enemy == null || enemy.GetComponent<EnemyScript>()._outOfMap)
                continue;

            float distance = Vector3.Distance(_player.transform.position, enemy.transform.position);
            if (distance <= proximityDistance)
            {
                shouldSwipe = true;
                break; // No need to check further, one valid enemy is close enough
            }
        }

        _playerAnimator.SetBool("Swipe", shouldSwipe);
    }


}
