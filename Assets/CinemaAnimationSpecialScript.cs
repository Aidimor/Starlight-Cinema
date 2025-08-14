using UnityEngine;

public class CinemaAnimationSpecialScript : MonoBehaviour
{
    public GameObject IntermissionBack;

    public void ActivateBackIntermission()
    {
        IntermissionBack.SetActive(true);
    }

    public void DectivateBackIntermission()
    {
        IntermissionBack.SetActive(false);
    }



}
