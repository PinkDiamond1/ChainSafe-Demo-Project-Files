using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEditor;
using UnityEngine.UI; // needed when accessing text elements
using UnityEngine;

#if UNITY_WEBGL
public class MintMenu : MonoBehaviour
{
    // This script has been moved from the MintWebGL721.cs example in the Minter scripts folder to show you how to make additional changes
    public GameObject SuccessPopup;
    public Text responseText;

    public async void MintNFT()
    {
        string account = PlayerPrefs.GetString("Account");
        string chain = "ethereum";
        string network = "goerli"; // mainnet ropsten kovan rinkeby goerli
        string to = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
        string cid = "QmXjWjjMU8r39UCEZ8483aNedwNRFRLvvV9kwq1GpCgthj";
        string type721 = "721";
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid, type721);
        // connects to user's browser wallet (metamask) to send a transaction
        try
        {   
            string response = await Web3GL.SendTransactionData(nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.gasPrice,nftResponse.tx.gasLimit, nftResponse.tx.data);
            print("Response: " + response);
            SuccessPopup.SetActive(true);
            responseText.text = "Success! TXHash: " + response;
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
#endif