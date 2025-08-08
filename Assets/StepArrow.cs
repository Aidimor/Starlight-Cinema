using UnityEngine;
using UnityEngine.UI;

public class StepArrow : MonoBehaviour
{
    private RectTransform _rect;
    private Vector2 _startPos;
    private Vector2 _targetPos;
    private double _spawnDspTime;

    private const float DURATION = 1f;         // Tiempo para llegar al objetivo
    private const float EXTRA_DURATION = 1f;   // Tiempo extra que sigue avanzando después

    private bool reachedTarget = false;
    private float extraTimer = 0f;



    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        if (_rect == null)
            Debug.LogError("StepArrow necesita un RectTransform.");
    }

    public void Initialize(Vector2 startPos, Vector2 targetPos, double spawnDspTime)
    {
        _startPos = startPos;
        _targetPos = targetPos;
        _spawnDspTime = spawnDspTime;

        if (_rect == null)
            _rect = GetComponent<RectTransform>();

        _rect.anchoredPosition = _startPos;
    }

    void Update()
    {
        if (_rect == null) return;

        double dspNow = AudioSettings.dspTime;
        float t = (float)(dspNow - _spawnDspTime);

        if (t <= DURATION)
        {
            // Primera fase: va hacia el objetivo
            float progress = Mathf.Clamp01(t / DURATION);
            _rect.anchoredPosition = Vector2.Lerp(_startPos, _targetPos, progress);
        }
        else
        {
            // Segunda fase: avanza en la misma dirección por 1 segundo extra
            if (!reachedTarget)
            {
                reachedTarget = true;
                extraTimer = 0f;
            }

            extraTimer += Time.deltaTime;

            // Dirección de avance (misma que la original)
            Vector2 direction = (_targetPos - _startPos).normalized;
            _rect.anchoredPosition += direction * (Vector2.Distance(_startPos, _targetPos) / DURATION) * Time.deltaTime;

            if (extraTimer >= EXTRA_DURATION)
                Destroy(gameObject);
        }
    }
}

