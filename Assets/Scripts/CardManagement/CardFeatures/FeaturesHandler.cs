using System.Collections.Generic;
using System.Linq;
using UnityTemplateProjects;

namespace CardManagement.CardFeatures
{
    public class FeaturesHandler
    {
        private List<Features> _featuresArray;

        public FeaturesHandler()
        {
            _featuresArray = new List<Features>();
        }
        public FeaturesHandler(Features[] featuresArray)
        {
            if(featuresArray.Length>9) _featuresArray = new List<Features>();
            else _featuresArray = featuresArray.ToList();
        }

        public void AddFeature(Features feature)
        {
            if(_featuresArray.Count>9) return;
            _featuresArray.Add(feature);
        }

        public void AddFeatures(Features[] featuresArray)
        {
            if(_featuresArray.Count+featuresArray.Length>9) return;
            _featuresArray.AddRange(featuresArray);
        }

        public Features[] GetFeatures()
        {
            return _featuresArray.ToArray();
        }
    }
}