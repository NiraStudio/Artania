using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Backtory.Core.Public;

public class OfferChecker : AlphaScript
{
    public Sprite FirstBTN, FirstImage;
    public Image BTNImage, PanelImage;
    Animator panelanim;

    bool BTN, Panel, Show;
    // Use this for initialization
    IEnumerator Start()
    {
        panelanim = PanelImage.GetComponent<Animator>();
        /* BacktoryUser.LoginAsGuestInBackground(loginResponse =>
         {
             if (loginResponse.Successful)
             {
                 // Getting new username and password from CURRENT user
                 string guestUsername = BacktoryUser.CurrentUser.Username;
                 string guestPassword = BacktoryUser.CurrentUser.Password;

                 // Logging new username and password
                 Debug.Log("your guest username: " + guestUsername
                     + " & your guest password: " + guestPassword);
                 CheckForOffer();

             }
             else
             {
                 // Operation generally failed, maybe internet connection issue
                 Debug.Log("Login failed for other reasons like network issues.");
             }
         });*/
        
       
            yield return new WaitUntil(() => InternetChecker.Instance.internetConnectBool);
            if (!GameManager.Instance.stateData.hadBoughtFirstOffer)
            {
                BTNImage.gameObject.SetActive(true);
                BTNImage.sprite = FirstBTN;
                PanelImage.sprite = FirstImage;
                BTNImage.GetComponent<Button>().onClick.AddListener(OnCLick);
               // yield return new WaitForSeconds(4f);
                loadingScreen.Instance.Disapear();
            }
            else
            {
                CheckForOffer();
                yield return new WaitUntil(() => Show);
                BTNImage.gameObject.SetActive(true);
                BTNImage.GetComponent<Button>().onClick.AddListener(OnCLick);
                if (loadingScreen.Instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MoveOut"))
                    loadingScreen.Instance.Disapear();
            }
    }
    // Update is called once per frame
    void Update()
    {
        Show =  BTN&&Panel ? true : false;
    }
    void CheckForOffer()
    {
        ProductCheckRequest requestForIphone = new ProductCheckRequest();
        print("Here backtory");

        // Request server whether iphone is available or not
        BacktoryCloudcode.RunInBackground<ProductCheckResponse>(
            // Name of cloud code function
            "test",

            // Request for iphone
            requestForIphone,

            // Callback to handle result
            backtoryResponse =>
            {
                print(backtoryResponse.Code);
                if (backtoryResponse.Successful)
                { // = context.succeed is called in server
                    // Extract ok and price from response
                    ProductCheckResponse result = backtoryResponse.Body;
                    StartCoroutine(loadPanel(result.Img));
                    StartCoroutine(LoadButton(result.URL));
                    
                    // Log the result

                }
            }
        );

    }
    void OnCLick()
    {
        panelanim.SetTrigger("Enter");

    }
    public void Exit()
    {
        panelanim.SetTrigger("Idle");

    }
    IEnumerator loadPanel(string url)
    {
        WWW www = new WWW(url);
        print(url);

        yield return www;
        print("downloaded the Panel Image");
        Sprite image = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        PanelImage.sprite = image;
        Panel = true;
        if (www.error != null)
            Debug.LogError(www.error);
    }
    IEnumerator LoadButton(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        print("downloaded the button Image");

        Sprite image = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        BTNImage.sprite = image;
        BTN = true;
        if (www.error != null)
            Debug.LogError(www.error);
    }
}
public class ProductCheckRequest
{
}


public class ProductCheckResponse
{
    public string URL { get; set; }
    public string Img { get; set; }
}

