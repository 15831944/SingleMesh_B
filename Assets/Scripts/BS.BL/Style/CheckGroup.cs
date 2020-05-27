using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.BL.Style
{
    public class CheckGroup : MonoBehaviour
    {
        public List<CheckStyle> listBtn = new List<CheckStyle>();
        public CheckStyle defaultCheck;
        public List<int> listRole = new List<int>();
        // Start is called before the first frame update
        void Start()
        {
            InitView();
            defaultCheck.GetComponent<Button>().onClick.AddListener(delegate () { SelectDefault(); });
        }
        private void OnEnable()
        {
           
        }
        private void OnDisable()
        {
            InitView();
        }

        // Update is called once per frame
        void Update()
        {

        }
        void InitView() {
            listRole.Clear();
            foreach (CheckStyle item in listBtn)
            {
                if (item != defaultCheck)
                {
                    item.ResetState(CheckSelectStyle.deselect);
                }
                else
                {
                    defaultCheck.ResetState(CheckSelectStyle.select);
                }
            }
        }
        void SelectDefault() {
            if (defaultCheck.GetComponent<CheckStyle>().checkSelectStyle == CheckSelectStyle.deselect)
            {
                InitView();
            }
        }
    }
}
