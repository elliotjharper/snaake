using UnityEngine;

public class ClipboardUtility : MonoBehaviour
{
    public static void CopyToClipboard(string text)
    {
        TextEditor textEditor = new TextEditor
        {
            text = text
        };
        textEditor.SelectAll();
        textEditor.Copy();
    }
}
