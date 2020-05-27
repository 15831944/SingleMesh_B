using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.BL.Style {
    public enum ButtonSelectStyle {
        select,
        deselect,
    }
    public class ButtonStyle : MonoBehaviour
    {
        private Button btnSelf;
        public Sprite normalSprite, selectSprite;
        private void Awake()
        {
            btnSelf = GetComponent<Button>();
            btnSelf.onClick.AddListener(delegate () { OnClickEvent();
            });
        }
        public void SelectOrDeselectByStyle(ButtonSelectStyle buttonSelectStyle) {
            btnSelf.image.sprite = buttonSelectStyle == ButtonSelectStyle.select ? selectSprite : normalSprite;
        }
        public void OnClickEvent() {
            transform.parent.GetComponent<ButtonGroup>().ResetButtonStyle();
            SelectOrDeselectByStyle(ButtonSelectStyle.select);
        }
    }
}

