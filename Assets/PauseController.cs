using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    [SerializeField] private MainGameplayController _scriptMain;
    public bool _onPause;
    [SerializeField] private float _carreteSpeed = 50f;
    public Image[] _carretes;
    public Animator PausePanelAnimator;

    [Header("Pause Menu Buttons")]
    public Image buttonUp;     // Asignar en inspector
    public Image buttonDown;   // Asignar en inspector
    private Image[] pauseMenuButtons;
    private int selectedIndex = 0;

    [Header("Button Scale")]
    public float normalScale = 1f;
    public float highlightedScale = 1.2f;

    void Start()
    {
        // Hacer que el animator ignore Time.timeScale
        if (PausePanelAnimator != null)
        {
            PausePanelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        //// Inicializar botones en array
        pauseMenuButtons = new Image[] { buttonUp, buttonDown };

        // EventSystem check
        if (EventSystem.current == null)
        {
            GameObject es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        HighlightButton();
    }

    void Update()
    {
        // Rotar carretes con unscaled time
        if (_carretes != null && _carretes.Length >= 2)
        {
            _carretes[0].transform.Rotate(Vector3.forward * _carreteSpeed * Time.unscaledDeltaTime);
            _carretes[1].transform.Rotate(Vector3.forward * _carreteSpeed * Time.unscaledDeltaTime);
        }



        // Navegación de pausa
        if (_onPause)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Vertical") > 0)
            {
                selectedIndex = 0; // solo dos botones, arriba siempre índice 0
                HighlightButton();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") < 0)
            {
                selectedIndex = 1; // abajo siempre índice 1
                HighlightButton();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //pauseMenuButtons[selectedIndex].onClick.Invoke();
            }

            if ( Input.GetButtonDown("Submit"))
            {
                switch (selectedIndex)
                {
                    case 0:
                        TogglePause();
                        break;
                    case 1:
                        Debug.Log("Sales");
                        break;
                }
                //pauseMenuButtons[selectedIndex].onClick.Invoke();
            }

            // Toggle pausa con Input
            if (Input.GetButtonDown("Pause"))
            {
                TogglePause();
            }
        }
        else
        {
            // Toggle pausa con Input
            if (Input.GetButtonDown("Pause"))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        _onPause = !_onPause;

        // Activar animación del panel de pausa
        if (PausePanelAnimator != null)
        {
            PausePanelAnimator.SetBool("PauseOn", _onPause);
        }

        if (_onPause)
        {
            Time.timeScale = 0f;
            HighlightButton();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void HighlightButton()
    {
        for (int i = 0; i < pauseMenuButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                pauseMenuButtons[i].transform.localScale = Vector3.one * highlightedScale;
            }
            else
            {
                pauseMenuButtons[i].transform.localScale = Vector3.one * normalScale;
            }
        }

        // Mantener seleccionado en EventSystem
        EventSystem.current.SetSelectedGameObject(pauseMenuButtons[selectedIndex].gameObject);
    }
}

