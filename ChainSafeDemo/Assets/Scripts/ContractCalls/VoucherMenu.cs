using System;
using Models;
using UnityEngine;
using System.Collections;
using Newtonsoft.Json; // used for json serialization with args
using UnityEngine.Networking; // needed for web requests

#if UNITY_WEBGL
public class VoucherMenu : MonoBehaviour
{
    // This script has been moved from the GetVoucherWebGL721.cs example in the Minter scripts folder to show you how to make additional changes
    public GameObject SuccessPopup;
    public GameObject IPFSImageHolder;
    public GameObject IPFSImage;
    string gateway = "https://ipfs.chainsafe.io/ipfs/";
    string metadataUri;
    string downloadURI;

    public async void Get721VoucherButton()
    {
        var voucherResponse721 = await EVM.Get721Voucher();
        Debug.Log("Voucher Response 721 Signature : " + voucherResponse721.signature);
        Debug.Log("Voucher Response 721 Uri : " + voucherResponse721.uri);
        Debug.Log("Voucher Response 721 Signer : " + voucherResponse721.signer);
        Debug.Log("Voucher Response 721 Min Price : " + voucherResponse721.minPrice);
        // saves the voucher to player prefs, you can change this if you like to fit your system
        PlayerPrefs.SetString("WebGLVoucher721Sig", voucherResponse721.signature);
        PlayerPrefs.SetString("WebGLVoucher721URI", voucherResponse721.uri);
        PlayerPrefs.SetString("WebGLVoucher721Signer", voucherResponse721.signer);
        PlayerPrefs.SetString("WebGLVoucher721MinPrice", voucherResponse721.minPrice.ToString());
        // initialize gateway and uri
        metadataUri = gateway + voucherResponse721.uri;
        // start json web request
        StartCoroutine(GetURIData());
    }

    IEnumerator GetURIData() {
        // json web request
        UnityWebRequest www = UnityWebRequest.Get(metadataUri);
        yield return www.SendWebRequest();
        // error or display result
        if (www.result != UnityWebRequest.Result.Success) {
        Debug.Log(www.error);
        }
        else 
        {
        // Show results as text
        downloadURI = www.downloadHandler.text;
        // gets ipfs image from voucher uri
        GetIPFSImage(downloadURI);
        }
    }

    public async void GetIPFSImage(string downloadURI)
    {
        // map key values to model and deserialize
        URIData deserializedProduct = JsonConvert.DeserializeObject<URIData>(downloadURI);
        Debug.Log("Updating Image...Please Wait!");
        // get ipfs image texture via uri substring (substring removes x amount of chars from the start of the string, we remove 7 here to get rid of the ipfs://)
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(gateway + deserializedProduct.image.Substring(7));
        await textureRequest.SendWebRequest();
        IPFSImage.GetComponent<Renderer>().material.mainTexture = (DownloadHandlerTexture.GetContent(textureRequest));
        IPFSImageHolder.SetActive(true);
        Debug.Log("Image Updated!");
        SuccessPopup.SetActive(true);
    }
}
#endif