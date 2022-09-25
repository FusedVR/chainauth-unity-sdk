/**
 * Copyright 2022 Vasanth Mohan. All rights and licenses reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A class for running a quick test of the SDK's core functions for authentication and reading the blockchain
/// </summary>
namespace FusedVR.Crypto {
    public class TestAPIs : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Unique ID for Client or Email Address")]
        private string email = "";

        [SerializeField]
        [Tooltip("Application Id for FusedVR Chain Auth")]
        private string appId = "";

        // Start is called before the first frame update
        async void Start()
        {
            ChainAuthManager manager = await ChainAuthManager.Register(email, appId); //register client
            try {
                Debug.Log(await manager.GetMagicLink()); //print link for authentication
            } catch (Exception e) {
                Debug.LogError(e);
            }

            if (await manager.AwaitLogin()) {
                RunTests(manager, ChainAuthManager.CHAIN.mumbai); //run tests on mumbai
            }
        }

        //run tests on an authenticated maanger
        async void RunTests(ChainAuthManager mngr, ChainAuthManager.CHAIN chain) {
            string address = mngr.GetAddress(); //gets wallet address
            string balance = await mngr.GetNativeBalance(chain); //gets native balance on the selected chain
            List<Dictionary<string, string>> erc20s = await mngr.GetERC20Tokens(chain); //gets erc-20s
            List<Dictionary<string, string>> nfts = await mngr.GetNFTTokens(chain); //gets nfts

            Debug.Log("Wallet Address : " + address);
            Debug.Log("My Balance: " + balance);

            foreach (Dictionary<string, string> erc20 in erc20s) {
                Debug.Log("-----ERC-20 Token-----");
                foreach (KeyValuePair<string, string> kvp in erc20) {
                    Debug.Log(kvp.Key + " : " + kvp.Value);
                }
            }

            foreach (Dictionary<string, string> nft in nfts) {
                Debug.Log("-----NFT Token-----");
                foreach (KeyValuePair<string, string> kvp in nft) {
                    Debug.Log(kvp.Key + " : " + kvp.Value);
                }
            }
        }
    }
}