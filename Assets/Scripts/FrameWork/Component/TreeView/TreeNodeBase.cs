using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class TreeNodeBase : MonoBehaviour
{
    public Sprite down, right, dot;
    
    public bool IsOpen { get; set; }//子物体开启状态
    public int ChildrenCount => _childList.Count;

    private Button _downArrow;//下箭头按钮
    private List<TreeNodeBase> _childList;//子物体集合
    private Vector2 _startSize;//起始大小

    public T GetChild<T>(int index) where T : TreeNodeBase
    {
        return (T)(_childList.Count > 0 ? _childList[Mathf.Clamp(index, 0,  _childList.Count)] : (T)default);
    }

    private void Awake()
    {
        _childList = new List<TreeNodeBase>();
        _downArrow = this.transform.Find("ContentPanel/ArrowButton").GetComponent<Button>();
        _downArrow.onClick.AddListener(() =>
        {
            if (IsOpen)
            {
                CloseChild();
                // IsOpen = false;
            }
            else
            {
                OpenChild();
                // IsOpen = true;
            }
        });
        _startSize = this.GetComponent<RectTransform>().sizeDelta;
        IsOpen = false;
    }

    //添加子物体到集合
    private void AddChild(TreeNodeBase parentTreeNodeBase)
    {
        _childList.Add(parentTreeNodeBase);
        if (_childList.Count >= 1)
        {
            _downArrow.GetComponent<Image>().sprite = right;
        }
    }

    /// <summary>
    /// 设置父物体，父物体不为一级菜单
    /// </summary>
    /// <param name="parentTreeNodeBase"></param>
    public void SetItemParent(TreeNodeBase parentTreeNodeBase)
    {
        this.transform.parent = parentTreeNodeBase.transform;
        parentTreeNodeBase.AddChild(this);
        this.GetComponent<VerticalLayoutGroup>().padding = new RectOffset((int)parentTreeNodeBase._downArrow.GetComponent<RectTransform>().sizeDelta.x, 0, 0, 0);
        if (parentTreeNodeBase.IsOpen)
        {
            this.GetComponent<TreeNodeBase>().AddParentSize((int)this.gameObject.GetComponent<RectTransform>().sizeDelta.y);
        }
        else
        {
            this.transform.gameObject.SetActive(false);        
        }
    }

    /// <summary>
    /// 设置父物体，父物体为一级菜单
    /// </summary>
    /// <param name="tran"></param>
    public void SetBaseParent(Transform tran)
    {
        this.transform.parent = tran;
    }

    /// <summary>
    /// 增加一个子物体后更新Panel大小
    /// </summary>
    /// <param name="change"></param>
    public void UpdateRectTranSize(int change)
    {
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_startSize.x, this.gameObject.GetComponent<RectTransform>().sizeDelta.y + change);
    }
    /// <summary>
    /// 增加父物体高度
    /// </summary>
    /// <param name="parentItem"></param>
    /// <param name="change"></param>
    public void AddParentSize(int change)
    {
        if (this.transform.parent.GetComponent<TreeNodeBase>() != null)
        {
            var item = this.transform.parent.GetComponent<TreeNodeBase>();
            item.UpdateRectTranSize(change);
            item.AddParentSize(change);
        }
    }

    /// <summary>
    /// 关闭子物体列表
    /// </summary>
    public void CloseChild()
    {
        if (!IsOpen || _childList.Count == 0) return;
        foreach (TreeNodeBase child in _childList)
        {
            child.gameObject.SetActive(false);
            child.GetComponent<TreeNodeBase>().AddParentSize(-(int)child.gameObject.GetComponent<RectTransform>().sizeDelta.y);
        }
        _downArrow.GetComponent<Image>().sprite = right;
        IsOpen = false;
    }

    /// <summary>
    /// 打开子物体列表
    /// </summary>
    public void OpenChild()
    {
        if (IsOpen || _childList.Count == 0) return;
        foreach (TreeNodeBase child in _childList)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<TreeNodeBase>().AddParentSize((int)child.gameObject.GetComponent<RectTransform>().sizeDelta.y);
        }
        _downArrow.GetComponent<Image>().sprite = down;
        IsOpen = true;
    }

    //填充Item数据
    public virtual void InitPanelContent(object treeNodeData)
    {
        
    }

}
