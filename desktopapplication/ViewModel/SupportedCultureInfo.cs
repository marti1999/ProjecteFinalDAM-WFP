using System;
using System.Globalization;
using System.Resources;
using MultiLanguage.Infrastructure;

namespace MultiLanguage
{
    /// <summary>
    /// Encapsulates all the information needed to support a given culture.
    /// </summary>
    public class SupportedCultureInfo : ISupportedCultureInfo
    {
        private readonly CultureInfo _culture;
        private ResourceManager _resourceManager;
        public SupportedCultureInfo(CultureInfo culture, ResourceManager resourceManager)
        {
            _culture = culture;
            _resourceManager = resourceManager;
        }
        public CultureInfo Culture { get { return _culture; } }
        public string GetString(string name)
        {
            if (_resourceManager != null)
            {
                return _resourceManager.GetString(name);
            }
            return name;
        }
        public void Release()
        {
            if (_resourceManager != null)
            {
                _resourceManager.ReleaseAllResources();
                _resourceManager = null;
            }
        }
        public IFormatProvider FormatProvider { get { return _culture; } }
        public string DisplayName { get { return _culture.DisplayName; } }
        public override string ToString()
        {
            return _culture.DisplayName;
        }
    }
}