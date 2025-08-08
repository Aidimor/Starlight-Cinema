using UnityEngine;
using UnityEngine.UI;

public class BarTextureOffset : MonoBehaviour
{
    [SerializeField] private RawImage[] _cinemaBars;
    public float _speed;

    void Start()
    {
        // Clone the material for each RawImage to prevent shared changes
        for (int i = 0; i < _cinemaBars.Length; i++)
        {
            if (_cinemaBars[i] != null && _cinemaBars[i].material != null)
            {
                Material clonedMaterial = Instantiate(_cinemaBars[i].material);
                _cinemaBars[i].material = clonedMaterial;
            }
        }
    }

    void Update()
    {
        CinemaBars();
    }

    void CinemaBars()
    {
        if (_cinemaBars.Length >= 2)
        {
            Vector2 offsetLeft = new Vector2(Time.time * -_speed, 0);
            _cinemaBars[0].material.SetTextureOffset("_MainTex", offsetLeft);

            Vector2 offsetRight = new Vector2(Time.time * _speed, 0);
            _cinemaBars[1].material.SetTextureOffset("_MainTex", offsetRight);
        }
    }
}

