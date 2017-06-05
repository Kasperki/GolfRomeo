using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    public Text Title;
    public Button AcceptButton;
    public Button CancelButton;

    void Awake()
    {
        AcceptButton.onClick.AddListener(Cancel);
        CancelButton.onClick.AddListener(Cancel);
    }

	public void Init(string title)
    {
        Title.text = title;
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
