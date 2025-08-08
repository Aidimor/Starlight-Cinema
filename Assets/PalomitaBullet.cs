using UnityEngine;

public class PalomitaBullet : MonoBehaviour
{
    public float _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
    
        if(transform.localPosition.y <= -7f)
        {
            Destroy(this.gameObject);
        }
    }
}
