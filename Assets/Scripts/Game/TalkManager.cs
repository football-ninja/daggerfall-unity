﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2017 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Michael Rauter (Nystul)
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.UserInterface;

namespace DaggerfallWorkshop.Game
{
    /// <summary>
    /// this class manages talk topics and resulting actions/answers
    /// </summary>
    public class TalkManager : MonoBehaviour
    {
        #region Singleton

        static TalkManager instance = null;
        public static TalkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    if (!FindTalkManager(out instance))
                    {
                        GameObject go = new GameObject();
                        go.name = "TalkManager";
                        instance = go.AddComponent<TalkManager>();
                    }
                }
                return instance;
            }        
        }

        public static bool HasInstance
        {
            get
            {
                return (instance != null);
            }
        }

        public static bool FindTalkManager(out TalkManager talkManagerOut)
        {
            talkManagerOut = GameObject.FindObjectOfType<TalkManager>();
            if (talkManagerOut == null)
            {
                DaggerfallUnity.LogMessage("Could not locate TalkManager GameObject instance in scene!", true);
                return false;
            }

            return true;
        }

        private void SetupSingleton()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                if (Application.isPlaying)
                {
                    DaggerfallUnity.LogMessage("Multiple TalkManager instances detected in scene!", true);
                    Destroy(gameObject);
                }
            }
        }

        #endregion

        #region Fields        
     
        public enum ListItemType
        {
            Item,
            ItemGroup,
            Navigation
        }

        public enum QuestionType
        {
            NoQuestion, // used for list entries that are not of ListItemType item
            News,
            Work,
            LocalBuilding,
            Regional,
            Person,
            Thing
        }

        public class ListItem
        {
            public ListItemType type = ListItemType.Item; // list item can be either a normal item, a navigation item (to get to parent list) or an item group (contains list of child items)
            public string caption = "undefined";
            public QuestionType questionType = QuestionType.NoQuestion;
            public List<ListItem> listChildItems = null; // null if type == ListItemType.Navigation or ListItemType.Item, only contains a list if type == ListItemType.ItemGroup
            public List<ListItem> listParentItems = null; // null if type == ListItemType.ItemGroup or ListItemType.Item, only contains a list if type == ListItemType.Navigation
        }

        List<ListItem> listTopicLocation;
        List<ListItem> listTopicPerson;
        List<ListItem> listTopicThings;

        #endregion

        #region Properties

        public List<ListItem> ListTopicLocation
        {
            get { return listTopicLocation; }
        }

        public List<ListItem> ListTopicPerson
        {
            get { return listTopicPerson; }
        }

        public List<ListItem> ListTopicThings
        {
            get { return listTopicThings; }
        }

        #endregion

        #region Unity

        void Awake()
        {
            SetupSingleton();

            PrepareTestTopicLists();
        }

        void OnDestroy()
        {

        }

        void OnEnable()
        {
            
        }

        void OnDisable()
        {
            
        }

        void Start()
        {

        }

        void Update()
        {

        }

        #endregion

        #region Public Methods

        public string GetQuestionText(TalkManager.ListItem listItem, DaggerfallTalkWindow.TalkTone talkTone)
        {
            int toneIndex = DaggerfallTalkWindow.TalkToneToIndex(talkTone);
            string question = "question about " + listItem.caption + ": " + DaggerfallUnity.Instance.TextProvider.GetRandomText(7225 + toneIndex);
            return question;
        }

        public string GetAnswerText(TalkManager.ListItem listItem)
        {
            string answer = "answer about " + listItem.caption + ": " + DaggerfallUnity.Instance.TextProvider.GetRandomText(7285) + DaggerfallUnity.Instance.TextProvider.GetRandomText(7332);
            return answer;
        }

        #endregion

        #region Private Methods

        void PrepareTestTopicLists()
        {
            listTopicLocation = new List<ListItem>();
            ListItem itemGroup;
            for (int i = 0; i < 20; i++)
            {
                itemGroup = new ListItem();
                itemGroup.type = ListItemType.ItemGroup;                
                itemGroup.caption = "shop type " + i + " group";
                listTopicLocation.Add(itemGroup);
            }
            itemGroup = new ListItem();
            itemGroup.type = ListItemType.ItemGroup;
            itemGroup.caption = "General";
            listTopicLocation.Add(itemGroup);
            itemGroup = new ListItem();
            itemGroup.type = ListItemType.ItemGroup;
            itemGroup.caption = "Regional";
            itemGroup.listChildItems = new List<ListItem>();            
            for (int i = 0; i < 7; i++)
            {
                ListItem item;
                if (i == 0)                   
                {
                    item = new ListItem();
                    item.type = ListItemType.Navigation;
                    item.caption = "Previous List";
                    item.listParentItems = listTopicLocation;
                    itemGroup.listChildItems.Add(item);
                }
                item = new ListItem();
                item.type = ListItemType.Item;
                item.questionType = QuestionType.Regional;
                item.caption = "inner item " + i + " in group";
                itemGroup.listChildItems.Add(item);
            }
            listTopicLocation.Add(itemGroup);

            listTopicPerson = new List<ListItem>();
            for (int i = 0; i < 12; i++)
            {
                ListItem item = new ListItem();
                item.type = ListItemType.Item;
                item.questionType = QuestionType.Person;
                item.caption = "dummy person " + i + " (here will be the name of the person later on)";
                listTopicPerson.Add(item);
            }

            listTopicThings = new List<ListItem>();
            for (int i = 0; i < 30; i++)
            {
                ListItem item = new ListItem();
                item.type = ListItemType.Item;
                item.questionType = QuestionType.Thing;
                item.caption = "thing " + i;
                listTopicThings.Add(item);
            }
        }

        #endregion

        #region event handlers

        #endregion
    }
}