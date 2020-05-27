using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace SuperTreeView
{
    public class ItemScript : MonoBehaviour
    {
        public Button mExpandBtn;
        public Image mIcon;
        public Image mSelectImg;
        public Button mClickBtn;
        public Text mLabelText;
        public Sprite collapseSprite;
        public Sprite expandSprite;
        public Image point;

        public Color selectColor, normalColor;

        string mData = "";
        private TreeView mTreeView;

        public delegate void OnClickItem(string itemName);

        public OnClickItem onClickItem;

        public string Data
        {
            get
            {
                return mData;
            }
            set
            {
                mData = value;
            }
        }

        void Start()
        {
            mExpandBtn.onClick.AddListener(OnExpandBtnClicked);
            mClickBtn.onClick.AddListener(OnItemClicked);
            mTreeView = GameObject.Find("TreeView").GetComponent<TreeView>();

        }

        public void Init()
        {
            SetExpandBtnVisible(false);
            SetExpandStatus(true);
            IsSelected = false;
        }

        void OnExpandBtnClicked()
        {
            TreeViewItem item = GetComponent<TreeViewItem>();
            item.DoExpandOrCollapse();
        }


        public void SetItemInfo(string iconSpriteName,string labelTxt,string data = "")
        {
            Init();
            mIcon.sprite = ResManager.Instance.GetSpriteByName(iconSpriteName);
            mLabelText.text = labelTxt;
            mData = data;

        }

        void OnItemClicked()
        {
            for (int i = 0; i < mTreeView.AllTreeViewItemList.Count; i++)
            {
                mTreeView.AllTreeViewItemList[i].GetComponent<ItemScript>().UnSelected();
            }
            Selected();
            TreeViewItem item = GetComponent<TreeViewItem>();
            item.RaiseCustomEvent(CustomEvent.ItemClicked, null);
            Debug.Log("TreeViewItem Clicked " + Data);
            if (onClickItem != null) {
                onClickItem(Data);
            }
            
        }

        public void Selected()
        {
            mLabelText.color = selectColor;
        }
        public void UnSelected()
        {
            mLabelText.color = normalColor;
        }

        public void SetExpandBtnVisible(bool visible)
        {
            if (visible)
            {
                mExpandBtn.gameObject.SetActive(true);
                point.gameObject.SetActive(false);
            }
            else
            {
                mExpandBtn.gameObject.SetActive(false);
                point.gameObject.SetActive(true);
            }
        }

        public bool IsSelected
        {
            get
            {
                return mSelectImg.gameObject.activeSelf;
            }
            set
            {
                mSelectImg.gameObject.SetActive(value);
            }
        }
        public void SetExpandStatus(bool expand)
        {
            if (expand)
            {
                //mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, -90);
                mExpandBtn.image.sprite = collapseSprite;
            }
            else
            {
                //mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, 0);
                mExpandBtn.image.sprite = expandSprite;

            }
        }


    }

}