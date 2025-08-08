using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CleanGameplay _scriptClean;
    public Transform _exitTarget;
    public int _exitPoint;
    public float _distanceToPlayer;
    public float _distanceToGarbage;
    public bool _outOfMap;
    //public bool _wiped;
    public GameObject[] _garbageImages;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_outOfMap)
        Movement();
    }

    public void Movement()
    {
        // Calculate distance to player
        _distanceToPlayer = Vector3.Distance(transform.position, _scriptClean._player.transform.position);
        _distanceToGarbage = Vector3.Distance(transform.position, _exitTarget.transform.position);

        // Define max speed and min speed


        // Map distance to speed: closer means faster
        //// Clamp distance so it never goes to zero to avoid division by zero
        //float clampedDistance = Mathf.Clamp(_distanceToPlayer, 0.1f, 10f);

        // Inverse proportional speed
        float speed = Mathf.Lerp(_scriptClean.maxSpeed * 2, _scriptClean.minSpeed, /*clampedDistance / */10f);

        // If close enough, move towards exit with calculated speed
 
            if (_distanceToPlayer <= _scriptClean._player.transform.localScale.y * _scriptClean._cleanDiameter/* && _scriptClean._player.transform.position.y < this.transform.localPosition.y*/)
            {
         
                this.transform.position = Vector3.MoveTowards(this.transform.position, (Vector3)_exitTarget.transform.position, speed * Time.deltaTime);
            //_wiped = true;
        }
        else
        {
            //_wiped = false;
        }

        //_scriptClean._swiping = _wiped;

        if (_distanceToGarbage <= _scriptClean._reachDiameter)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, (Vector3)_exitTarget.transform.position, speed * Time.deltaTime);
         
        }

        if (transform.position == _exitTarget.transform.position)
        {
            _outOfMap = true;
            _scriptClean.WinChecker();
        }
    }
}
