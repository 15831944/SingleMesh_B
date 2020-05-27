using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Style {
    public class ButtonGroup : MonoBehaviour
    {
        public List<ButtonStyle> listBtn = new List<ButtonStyle>();
        public ButtonStyle defaultBtn;

        // Start is called before the first frame update
        void Start()
        {
            InitButtonView();
        }
        /// <summary>
        /// 初始化button样式
        /// </summary>
        void InitButtonView() {
            foreach (ButtonStyle item in listBtn)
            {
                item.SelectOrDeselectByStyle(ButtonSelectStyle.deselect);
            }
            if (defaultBtn != null)
            {
                if (listBtn.Contains(defaultBtn))
                {
                    defaultBtn.SelectOrDeselectByStyle(ButtonSelectStyle.select);
                }

            }
        }
        public void ResetButtonStyle() {
            foreach (ButtonStyle item in listBtn)
            {
                item.SelectOrDeselectByStyle(ButtonSelectStyle.deselect);
            }
        }
        private void OnDisable()
        {
            InitButtonView();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

