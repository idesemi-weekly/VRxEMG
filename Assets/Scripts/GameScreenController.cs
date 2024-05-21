using UnityEngine;
using UnityEngine.UI;

public class GameScreenController : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        // Add your logic here
    }
}
