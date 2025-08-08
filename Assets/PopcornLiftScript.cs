using UnityEngine;
using System.Collections;

public class PopcornLiftScript : MonoBehaviour
{
    public float _restPose;
    public float _goalPose;
    public GameObject _popCornBox;
    public float _weight;
    public float _strenght;
    public bool _gameStarts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartNumerator());
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStarts)   
            PopCornGameplay();
    }

    public void PopCornGameplay()
    {
       
            _popCornBox.GetComponent<RectTransform>().anchoredPosition =
                Vector2.MoveTowards(_popCornBox.GetComponent<RectTransform>().anchoredPosition, 
                new Vector2(_popCornBox.GetComponent<RectTransform>().anchoredPosition.x, _restPose), _weight * Time.deltaTime);

            if (Input.GetButtonDown("Submit"))
            {
            Debug.Log("Pressed");
                _popCornBox.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(_popCornBox.GetComponent<RectTransform>().anchoredPosition.x, + _popCornBox.GetComponent<RectTransform>().anchoredPosition.y + _strenght);
            }

        if (_popCornBox.GetComponent<RectTransform>().anchoredPosition.y >= _goalPose)
        {
            _gameStarts = false;
            transform.parent.gameObject.GetComponent<MainGameplayController>()._wins = true;
        }


    }

    public IEnumerator StartNumerator()
    {
        yield return new WaitForSeconds(1);
        _gameStarts = true;
    }
}
