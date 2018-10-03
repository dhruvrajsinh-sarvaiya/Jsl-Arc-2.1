﻿using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class GetWalletResponse
    {
        public class User
        {
            public string user { get; set; }
            public List<string> permissions { get; set; }
        }

        public class Freeze
        {
        }

        public class CoinSpecific
        {
        }

        public class CoinSpecific2
        {
            public string redeemScript { get; set; }
        }

        public class ReceiveAddress
        {
            public string address { get; set; }
            public int chain { get; set; }
            public int index { get; set; }
            public string coin { get; set; }
            public string wallet { get; set; }
            public CoinSpecific2 coinSpecific { get; set; }
        }

        public class Action
        {
            public string type { get; set; }
        }

        public class Condition
        {
            public string amountString { get; set; }
            public int timeWindow { get; set; }
            public List<string> groupTags { get; set; }
            public List<object> excludeTags { get; set; }
        }

        public class Rule
        {
            public string id { get; set; }
            public string coin { get; set; }
            public string type { get; set; }
            public Action action { get; set; }
            public Condition condition { get; set; }
        }

        public class Policy
        {
            public string id { get; set; }
            public string label { get; set; }
            public int version { get; set; }
            public DateTime date { get; set; }
            public List<Rule> rules { get; set; }
        }

        public class Admin
        {
            public Policy policy { get; set; }
        }

        public class GetWalletRootObject : BizResponseClass
        {
            public string id { get; set; }
            public List<User> users { get; set; }
            public string coin { get; set; }
            public string label { get; set; }
            public int m { get; set; }
            public int n { get; set; }
            public List<string> keys { get; set; }
            public List<string> tags { get; set; }
            public bool disableTransactionNotifications { get; set; }
            public Freeze freeze { get; set; }
            public bool deleted { get; set; }
            public int approvalsRequired { get; set; }
            public CoinSpecific coinSpecific { get; set; }
            public int balance { get; set; }
            public int confirmedBalance { get; set; }
            public int spendableBalance { get; set; }
            public int balanceString { get; set; }
            public int confirmedBalanceString { get; set; }
            public int spendableBalanceString { get; set; }
            public ReceiveAddress receiveAddress { get; set; }
            public Admin admin { get; set; }
        }
    }
}