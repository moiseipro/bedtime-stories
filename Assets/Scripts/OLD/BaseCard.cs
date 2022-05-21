using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Card {
    public int id = 0;
    public int horrorValue = 0;
    public int reasonValue = 0;
    public string name = "";
    public string tag = "";
    public int[] dangerValue = new int[2] {0,0};
    public string[] typeAction = new string[2] {"", ""};
    public string[] description = new string[2] {"Test","Test"};
}
[Serializable]
public class GetCardsList{
    public List<Card> events = new List<Card>();
    public List<Card> story = new List<Card>();
    public List<Card> actions = new List<Card>();
    public List<Card> hero = new List<Card>();
}

[Serializable]
    public class Status {
        public int id = 0;
        public int stack = 0;
        public int turn = 0;
        public string typeAction = "";
        public string name = "";
        public string tag = "";
        public string description = "Test";
    }
    [Serializable]
    public class GetStatusesList{
        public List<Status> statuses = new List<Status>();
    }
