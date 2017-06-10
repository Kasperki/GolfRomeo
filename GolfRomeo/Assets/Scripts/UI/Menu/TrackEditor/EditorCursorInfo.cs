using UnityEngine;
using UnityEngine.UI;

public class EditorCursorInfo : MonoBehaviour
{
    public Text SubmitButton;
    public Text CancelMenuButton;
    public Text CopyButton;
    public Text DeleteButton;
    public Text ZoomButtonText;

    private CursorEditor editorCursor;

    public void Init(CursorEditor editorCursor)
    {
        this.editorCursor = editorCursor;

        SubmitButton.text = editorCursor.ControlScheme.Submit.ToString();
        CopyButton.text = editorCursor.ControlScheme.Duplicate.ToString();
        DeleteButton.text = editorCursor.ControlScheme.Delete.ToString();
    }

	void Update ()
    {
        DeleteButton.transform.parent.parent.gameObject.SetActive(editorCursor.GetSelectedObject != null ? true : false);

        if (editorCursor.CursorUI.IsActive())
        {
            CancelMenuButton.text = "CANCEL:" + " CTRL";
            ZoomButtonText.text = "BRUSH: Scroll";
        }
        else
        {
            CancelMenuButton.text = "MENU:" + " CTRL";
            ZoomButtonText.text = "ZOOM: Scroll";
        }

        if (editorCursor.GetSelectedObject != null)
        {
            ZoomButtonText.text = "Rotate: Scroll";
        }
    }
}
