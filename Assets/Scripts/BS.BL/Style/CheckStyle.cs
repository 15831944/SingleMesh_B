using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.BL.Style
{
    public enum CheckSelectStyle
    {
        select,
        deselect,
    }
    public class CheckStyle : MonoBehaviour
    {
        private Button checkSelf;
        public Sprite normalSprite, selectSprite;
        public CheckSelectStyle checkSelectStyle;
        public int role;
        // Start is called before the first frame update
        private void Awake()
        {
            checkSelf = GetComponent<Button>();
            checkSelf.onClick.AddListener(delegate () {
                SelectOrDeselect();
            });
        }
        public void SelectOrDeselect()
        {
            if (checkSelectStyle == CheckSelectStyle.select)
            {
                transform.parent.GetComponent<CheckGroup>().listRole.Add(role);
            }
            else
            {
                if (transform.parent.GetComponent<CheckGroup>().listRole.Contains(role))
                {
                    transform.parent.GetComponent<CheckGroup>().listRole.Remove(role);
                }
            }
            checkSelectStyle = checkSelf.image.sprite == normalSprite ? CheckSelectStyle.deselect : CheckSelectStyle.select;
            checkSelf.image.sprite = checkSelf.image.sprite == normalSprite ? selectSprite : normalSprite;
            if (transform.localPosition.y == 4)
            {
                transform.DOLocalMoveY(-6, 0.5f);
            }
            if (transform.localPosition.y == -6)
            {
                transform.DOLocalMoveY(4, 0.5f);
            }
            
        }
        public void ResetState(CheckSelectStyle cstyle)
        {
            checkSelectStyle = cstyle;
            checkSelf.image.sprite = cstyle == CheckSelectStyle.select ? selectSprite : normalSprite;
            if (cstyle == CheckSelectStyle.deselect)
            {
                if (transform.parent.GetComponent<CheckGroup>().listRole.Contains(role))
                {
                    transform.parent.GetComponent<CheckGroup>().listRole.Remove(role);
                }
            }
        }
    }
}
