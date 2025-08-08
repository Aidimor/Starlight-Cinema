using UnityEngine;

public class EnemyFinalStage : MonoBehaviour
{
    public SimpleLeftRight _scriptParent;
    public float _speed;
    public bool _destroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (_destroyed)
        {
            case false:
                transform.Translate(Vector2.up * _speed * Time.deltaTime);
                break;
            case true:
                transform.Translate(Vector2.up * -_speed * Time.deltaTime);
                break;
        }

        if(transform.localPosition.y <= -8)
        {

            _scriptParent.WinChecker();
            Destroy(this.gameObject);
        }

        if(transform.localPosition.y >= 6)
        {
    
            _scriptParent._enemiesFail++;
            _scriptParent.WinChecker();
            Destroy(this.gameObject);
        }
    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.layer == 10)
        {
            GetComponent<Animator>().SetTrigger("Leaves");
            Destroy(collision.gameObject);
            GetComponent<Collider2D>().enabled = true;
            _destroyed = true;
            //_scriptParent.WinChecker();
        }
    }
}
