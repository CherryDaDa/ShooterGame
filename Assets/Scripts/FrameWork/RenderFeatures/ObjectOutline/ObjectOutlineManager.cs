using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

namespace Framework.RenderFeatures.ObjectOutline
{
    public class ObjectOutlineManager :Singleton<ObjectOutlineManager>
    {
        private readonly List<Renderer> _outlineRenderers = new List<Renderer>();

        public void AddOutline(Renderer renderer)
        {
            if (!_outlineRenderers.Contains(renderer))
            {
                _outlineRenderers.Add(renderer);
            }
        }

        public void RemoveOutline(Renderer renderer)
        {
            if (_outlineRenderers.Contains(renderer))
            {
                _outlineRenderers.Remove(renderer);
            }
        }

        public List<Renderer> GetOutlineRenderers()
        {
            return _outlineRenderers;
        }
    }
}