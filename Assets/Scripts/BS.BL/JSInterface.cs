using BS.BL.Two;
using BS.BL.Two.Element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Interface
{
    public class JSInterface : MonoBehaviour
    {
        private ElementContainer elementContainer;
        private void Awake()
        {
            elementContainer = GameObject.Find("ResLabel").GetComponent<ElementContainer>();
            Invoke("WebglInput", 2f);
        }
        private void WebglInput() { 
        
            WebGLInput.captureAllKeyboardInput = false;
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                setTrackPlay(2);
            }
        }
        #region 北向接口
        /// <summary>
        /// 初始化基础数据接口
        /// </summary>
        /// <param name="data"></param>
        public void initBasicData(string data)
        {
            Debug.Log("初始化基础数据接口" + data);
            //elementContainer.InitBasicData(data);
            (Two.Manager.GetInstance().GetManager(ManagerType.initData) as InitData).InitBasicData(elementContainer, data);
        }
        /// <summary>
        /// 添加资源实体接口
        /// </summary>
        /// <param name="res"></param>
        public void refreshRes(string res)
        {
            Debug.Log("添加资源实体接口" + res);
            elementContainer.RefreshRes(res);
        }
        /// <summary>
        /// 刷新资源接口
        /// </summary>
        /// <param name="entitys"></param>
        public void refreshResEntity(string entitys)
        {
            Debug.Log("刷新资源接口" + entitys);
            //elementContainer.RefreshResEntity(entitys);
        }
        /// <summary>
        /// 通过资源id设置当前资源
        /// </summary>
        /// <param name="resId"></param>
        public void setDominantRes(string resId)
        {
            Debug.Log("通过资源id设置当前资源" + resId);
            elementContainer.SetDominantRes(resId);
        }
        /// <summary>
        /// 通过资源类型设置当前资源，支持多选
        /// </summary>
        /// <param name="resType"></param>
        public void setDominantResType(string resType)
        {
            Debug.Log("通过资源类型设置当前资源" + resType);
            if (!string.IsNullOrEmpty(resType))
            {
                elementContainer.SetDminantResByTypes(resType);
            }
        }
        /// <summary>
        /// 通过巷道id设置当前巷道
        /// </summary>
        /// <param name="lineId"></param>
        public void setDominantTunnel(string lineId)
        {
            Debug.Log("通过巷道ID设置当前巷道" + lineId);
            elementContainer.SetDominatTunnel(lineId);
        }
        /// <summary>
        /// 通过区域ID设置当前区域
        /// </summary>
        /// <param name="areaId"></param>
        public void setDominantArea(string areaResult)
        {
            Debug.Log("通过区域id设置当前区域" + areaResult);
            elementContainer.SetDominatArea(areaResult);
        }
        /// <summary>
        /// 设置掘进巷道
        /// </summary>
        /// <param name="tunnelResult"></param>
        public void setTunnelLine(string tunnelResult)
        {
            Debug.Log("设置掘进巷道进度" + tunnelResult);
            elementContainer.SetTunnel(tunnelResult);
        }
        /// <summary>
        /// 设置路径接口
        /// </summary>
        /// <param name="ways"></param>
        public void setWay(string ways)
        {
            Debug.Log("设置路径" + ways);
            elementContainer.SetWays(ways);
        }
        /// <summary>
        /// 直接进入路径编辑模式
        /// </summary>
        public void setWayEdit()
        {
            elementContainer.SetWays(null);
        }
        /// <summary>
        /// 通过当前模式ID设置当前模式
        /// </summary>
        /// <param name="modeId"></param>
        public void setDominantMode(int modeId)
        {
            Debug.Log("通过当前模式ID设置当前模式");
            elementContainer.SetDominantMode(modeId);
        }
        /// <summary>
        /// 显示地上地下接口
        /// </summary>
        /// <param name="ground"></param>
        public void showGround(string ground)
        {
            Debug.Log("通过当前地上或地下显示地上地下");
            elementContainer.ShowGround(ground);
        }
        /// <summary>
        /// 通过巡检点位设置巡检
        /// </summary>
        /// <param name="inspection"></param>
        public void setInspection(string inspection)
        {
            Debug.Log("通过巡检点位设置巡检" + inspection);
            elementContainer.SetInspection(inspection);
        }
        /// <summary>
        /// 框选接口
        /// </summary>
        /// <param name="frame">0-可以框选，1-结束框选</param>
        public void setFrameSelection(int frame)
        {
            Debug.Log("开始框选接口");
            elementContainer.CurrentRes(frame);
        }
        public void deletePoint(string pointMsg) {
            Debug.Log("要删除的点位");
            elementContainer.DeletePoint(pointMsg);
        }
        public void adjustRange(int rate) {
            Debug.Log(rate == 1 ? "增加范围" : "缩小范围");
            elementContainer.AdjustRange(rate);
        }
        /// <summary>
        /// 关闭详情页
        /// </summary>
        public void closeDetail() {
            elementContainer.ResetLabel();
        }
        /// <summary>
        /// 轨迹回放
        /// </summary>
        /// <param name="trackResult"></param>
        public void trackPlayBack(string trackResult) {
            Debug.Log("轨迹回放");
            elementContainer.AddTrack(trackResult);
            //setTrackPlay(0);
        }
        /// <summary>
        /// 设置轨迹回放速度
        /// </summary>
        /// <param name="speed"></param>
        public void setTrackSpeed(float speed) {
            Debug.Log("设置轨迹回放速度" + speed);
            elementContainer.SetTrackSpeed(speed);
        }
        /// <summary>
        /// 拖拽轨迹回放
        /// </summary>
        /// <param name="degree"></param>
        public void setTrackDegree(float degree) {
            Debug.Log("拖拽轨迹回放进度条" + degree);
            elementContainer.SetTrackDegree(degree);
        }
        /// <summary>
        /// 设置动画开始和暂停
        /// </summary>
        /// <param name="isPlay"></param>
        public void setTrackPlay(int isPlay) {
            switch (isPlay)
            {
                case 0:
                    Debug.Log("开始播放");
                    elementContainer.SetTrackPlay(isPlay);
                    break;
                case 1:
                    Debug.Log("暂停播放");
                    elementContainer.SetTrackPlay(isPlay);
                    break;
                case 2:
                    Debug.Log("停止播放");
                    DynamicElement.GetInstance().ResetStart();
                    break;
                default:
                    break;
            }
            
        }
        #endregion
        #region 南向接口
        /// <summary>
        /// 三维场景加载完毕接口
        /// </summary>
        public void initFinished()
        {
            Application.ExternalCall("initFinished");
        }
        /// <summary>
        /// 选择当前区域接口
        /// </summary>
        /// <param name="areaId"></param>
        public void selectCurrentArea(string areaId)
        {
            Application.ExternalCall("selectCurrentArea", areaId);
        }
        /// <summary>
        /// 选择当前资源接口
        /// </summary>
        /// <param name="resId"></param>
        public void selectCurrentRes(string resId)
        {
            Application.ExternalCall("selectCurrentRes", resId);
            //ResTest.GetInstance().SetImageAndText(true, resId);
        }
        /// <summary>
        /// 选择当前巷道接口
        /// </summary>
        /// <param name="runnelId"></param>
        public void selectCurrentTunnel(string runnelId)
        {
            Application.ExternalCall("selectCurrentTunnel", runnelId);
        }
        /// <summary>
        /// 框选发送资源id数组
        /// </summary>
        /// <param name="resList"></param>
        public void selectCurrentResList(string resList)
        {
            Debug.Log("框选的数组" + resList);
            if (string.IsNullOrEmpty(resList))
            {
                resList = "[]";
            }
            Application.ExternalCall("selectCurrentResList", resList);
        }
        /// <summary>
        /// 发送巡检点位接口
        /// </summary>
        /// <param name="pointId"></param>
        public void sendInspectionPointPosition(string pointId)
        {
            Application.ExternalCall("sendInspectionPointPosition", pointId);
        }
        /// <summary>
        /// 设置路径点位
        /// </summary>
        /// <param name="way"></param>
        public void sendWayPointPosition(string way)
        {
            Application.ExternalCall("sendWayPointPosition", way);
        }
        public void sendAllLine(string linemsg)
        {
            Application.ExternalCall("sendAllLine", linemsg);

        }
        public void sendAlarmRes(string alarmRes) {
            Application.ExternalCall("sendAlarmRes", alarmRes);
        }
        public void unSelectRes(string resName) {
            Application.ExternalCall("unSelectRes", resName);
            //ResTest.GetInstance().SetImageAndText(false, resName);
        }
        public void sendTrackDegree(string degree) {
            Application.ExternalCall("sendTrackDegree", degree);
        }
        #endregion
    }
}

