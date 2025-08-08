using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StepmaniaGameplay : MonoBehaviour
{
    public GameObject _arrowConsumibleObject;
    public GameObject _arrowConsumibleParent;

    [System.Serializable]
    public class ArrowAssets
    {
        public GameObject ArrowPrefab;         // Posición destino (target)
        public RectTransform[] _startingPoses; // Posibles posiciones de aparición
    }

    public ArrowAssets[] _arrowAssets;

    [System.Serializable]
    public class SongBeats
    {
        public int[] beatTime; // Por ejemplo: {0, 2, 4, 6, 8}
    }

    public SongBeats[] _songBeats;
    public int _onBeat = 0;
    public float _multiplier = 1f;
    public int randomPose;

    public Image[] _failBalls;
    public int _failedTimes;
    public List<GameObject> _arrowPrefab = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(BeatNumerator());
    }

    void Update()
    {
        bool izquierda = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool derecha = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        _arrowAssets[0].ArrowPrefab.transform.localScale =
            Vector3.Lerp(_arrowAssets[0].ArrowPrefab.transform.localScale, new Vector3(1, 1, 1), 20 * Time.deltaTime);
        _arrowAssets[1].ArrowPrefab.transform.localScale =
         Vector3.Lerp(_arrowAssets[1].ArrowPrefab.transform.localScale, new Vector3(1, 1, 1), 20 * Time.deltaTime);

        if (izquierda)
        {
            _arrowAssets[0].ArrowPrefab.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            //Debug.Log("🔹 IZQUIERDA presionada");
            foreach (GameObject flecha in _arrowPrefab)
            {
                if (flecha == null) continue;

                RectTransform rect = flecha.GetComponent<RectTransform>();
                if (rect == null) continue;

                float distancia = Vector2.Distance(rect.anchoredPosition, Vector2.zero);

                if (distancia < 2f)
                {
                    Debug.Log($"L = {distancia}");
                }
            }
        }

        if (derecha)
        {
            _arrowAssets[1].ArrowPrefab.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            //Debug.Log("🔹 DERECHA presionada");

            foreach (GameObject flecha in _arrowPrefab)
            {
                if (flecha == null) continue;

                RectTransform rect = flecha.GetComponent<RectTransform>();
                if (rect == null) continue;

                float distancia = Vector2.Distance(rect.anchoredPosition, Vector2.zero);

                if (distancia < 2f)
                {
                    Debug.Log($"R = {distancia}");
                }
            }
        }
    }

    void SpawnArrowAtTime(Vector2 spawnAnchoredPos, Vector2 targetAnchoredPos, double spawnTime)
    {
        GameObject obj = Instantiate(_arrowConsumibleObject, _arrowAssets[randomPose].ArrowPrefab.transform);
        _arrowPrefab.Add(obj);
        RectTransform objRect = obj.GetComponent<RectTransform>();
        objRect.anchoredPosition = spawnAnchoredPos;
        if(randomPose == 0)
        {
            objRect.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            objRect.transform.localScale = new Vector3(1, 1, 1);
        }

        StepArrow arrow = obj.GetComponent<StepArrow>();
        arrow.Initialize(spawnAnchoredPos, targetAnchoredPos, spawnTime);  // 👈 CORRECT
    }

    public IEnumerator BeatNumerator()
    {
        var controller = transform.parent.GetComponent<MainGameplayController>();
        float levelSpeed = _multiplier * controller._onLevel;

        AudioSource audio = GetComponent<AudioSource>();
        double dspStartTime = AudioSettings.dspTime + 1.0; // Espera 1 segundo para sincronizar

        // Convertimos beats (en pasos) a segundos reales con velocidad
        float[] beats = System.Array.ConvertAll(_songBeats[_onBeat].beatTime, b => b / levelSpeed);

        // (Opcional) Si tienes una canción larga sincronizada
        // audio.PlayScheduled(dspStartTime);

        for (int i = 0; i < beats.Length; i++)
        {
            double beatTime = dspStartTime + beats[i];
            double spawnTime = beatTime - 1.0;

            // Wait until it's time to spawn the arrow
            while (AudioSettings.dspTime < spawnTime)
                yield return null;
            randomPose = Random.Range(0, 2);
            if(randomPose == 0)
            {
                Vector2 spawnPosL = _arrowAssets[0]._startingPoses[Random.Range(0, 3)].GetComponent<RectTransform>().anchoredPosition;
                Vector2 targetPosL = Vector2.zero;
                SpawnArrowAtTime(spawnPosL, targetPosL, spawnTime);
            }
            else
            {
                Vector2 spawnPosR = _arrowAssets[1]._startingPoses[Random.Range(0, 3)].GetComponent<RectTransform>().anchoredPosition;
                Vector2 targetPosR = Vector2.zero;
                SpawnArrowAtTime(spawnPosR, targetPosR, spawnTime);

            }
  

            // 👇 Pass the precomputed spawnTime
         

            // Wait until the beat time
            while (AudioSettings.dspTime < beatTime)
                yield return null;

            audio.PlayOneShot(audio.clip);
        }
    }



}

