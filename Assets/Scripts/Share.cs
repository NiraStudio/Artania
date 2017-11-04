using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Share : MonoBehaviour {

    public GameObject CanvasShareObj,buttonsCanvas,secondButtonsCanvas;
    public Text score;
    private bool isProcessing = false;
    private bool isFocus = false;

    public void ShareBtnPress()
    {
        if (!isProcessing)
        {
            CanvasShareObj.SetActive(true);
            buttonsCanvas.SetActive(false);
            secondButtonsCanvas.SetActive(false);
            score.text = GameManager.Language(GameManager.Instance.stateData.HighScore + " رکورد", "High Score " + GameManager.Instance.stateData.HighScore, score);
            StartCoroutine(ShareScreenshot());
        }
    }

    IEnumerator ShareScreenshot()
    {
        isProcessing = true;

        yield return new WaitForEndOfFrame();

        Application.CaptureScreenshot("screenshot.png", 2);
        string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.3f);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                GameManager.Language("کی میتونه رکورد منو بزنه ؟؟","Who can beat me ??"));
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share your new score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }

        yield return new WaitUntil(() => isFocus);
        CanvasShareObj.SetActive(false);
        buttonsCanvas.SetActive(true);
        secondButtonsCanvas.SetActive(true);
        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
}
