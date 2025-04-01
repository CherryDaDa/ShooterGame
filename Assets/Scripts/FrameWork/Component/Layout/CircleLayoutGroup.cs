using UnityEngine;
using UnityEngine.UI;

namespace Framework.Component.Layout
{
    [AddComponentMenu("Layout/Circle Layout Group", 152)]
    public class CircleLayoutGroup : LayoutGroup
    {
        [SerializeField]
        private float radius = 250.0f;

        [SerializeField]
        private bool clockwise = true;

        [Range(0f, 360f)]
        [SerializeField]
        private float startOffsetAngle = 250.0f;  // 起始偏移角度

        [Range(0f, 360f)]
        [SerializeField]
        private float spacingAngle = 36.0f;      // 间隔角度

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                MarkLayoutForRebuild();
            }
        }

        public bool Clockwise
        {
            get { return clockwise; }
            set
            {
                clockwise = value;
                MarkLayoutForRebuild();
            }
        }

        public float StartOffsetAngle
        {
            get { return startOffsetAngle; }
            set
            {
                startOffsetAngle = value;
                MarkLayoutForRebuild();
            }
        }

        public float SpacingAngle
        {
            get { return spacingAngle; }
            set
            {
                spacingAngle = value;
                MarkLayoutForRebuild();
            }
        }

        protected CircleLayoutGroup() { }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            MarkLayoutForRebuild();
        }

        public override void CalculateLayoutInputVertical() { }

        public override void SetLayoutHorizontal() { }

        public override void SetLayoutVertical()
        {
            ArrangeItems();
        }

        private void ArrangeItems()
        {
            int itemCount = rectChildren.Count;
            for (int i = 0; i < itemCount; i++)
            {
                float angle = (clockwise ? -1 : 1) * (startOffsetAngle * Mathf.Deg2Rad + i * spacingAngle * Mathf.Deg2Rad);
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                rectChildren[i].anchoredPosition = new Vector2(x, y);
            }
        }

        private void MarkLayoutForRebuild()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            MarkLayoutForRebuild();
        }
#endif
    }
}
