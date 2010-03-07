using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.Compilation;
using System.Xml.Linq;


namespace Localisation.Providers.XmlDictionary
{
    /// <summary>
    /// Localisation provider.  Single instance of the class for every resource "class".
    /// </summary>
    public class XmlDictionaryProvider : DisposableBaseType, IResourceProvider
    {
        // resource cache
        private Dictionary<string, Dictionary<string, object>> _mResourceCache;
        private string _classKey;

        private Dictionary<string, Dictionary<string, object>> ResourceCache
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Cache["LocalisationCache"] == null)
                    {
                        _mResourceCache = new Dictionary<string, Dictionary<string, object>>();
                        HttpContext.Current.Cache["LocalisationCache"] = _mResourceCache;
                        return _mResourceCache;
                    }
                    return (Dictionary<string, Dictionary<string, object>>)HttpContext.Current.Cache.Get("LocalisationCache");
                }
                if (_mResourceCache == null)
                {
                    _mResourceCache = new Dictionary<string, Dictionary<string, object>>();
                }
                return _mResourceCache;
            }
        }

        /// <summary>
        /// Gets or sets the XML contents.
        /// </summary>
        /// <value>The root XML element.</value>
        private XElement Xml { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDictionaryProvider"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public XmlDictionaryProvider(string key)
        {
            _classKey = key;
        }

        public bool ResourceDebugMode
        {
            get { return Properties.Settings.Default.LocalisationDebugMode; }
        }

        #region IResourceProvider Members

        /// <summary>
        /// Returns a resource object for the key and culture.
        /// </summary>
        /// <param name="resourceKey">The key identifying a particular resource.</param>
        /// <param name="culture">The culture identifying a localized value for the resource.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that contains the resource value for the <paramref name="resourceKey"/> and <paramref name="culture"/>.
        /// </returns>
        public object GetObject(string resourceKey, CultureInfo culture)
        {
            object resourceValue = null;
            
            Dictionary<string, object> resCacheByCulture = null;

            if (HttpContext.Current == null)
            {
                return "Key: " + resourceKey;
            }

            if (culture == null)
            {
                culture = new CultureInfo(LocalisationSettings.CurrentLocale);
            }

            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }

            if (ResourceCache.ContainsKey(culture.Name))
            {
                resCacheByCulture = ResourceCache[culture.Name];
                if (resCacheByCulture.ContainsKey(_classKey + ":" + resourceKey))
                {
                    resourceValue = resCacheByCulture[_classKey + ":" + resourceKey];
                }
            }

            if ((resourceValue == null) || (ResourceDebugMode))
            {
                string xmlPath = HttpContext.Current.Server.MapPath(string.Concat(Properties.Settings.Default.XmlRoot, LocalisationSettings.CurrentLocale, "/", _classKey.TrimStart('/'), ".xml"));
                FileInfo fi = new FileInfo(xmlPath);

                try
                {
                    Xml = XElement.Load(fi.FullName).Descendants("dictionary").Single();
                }
                catch (System.Xml.XmlException e)
                {
                    throw new LocalisationException(string.Format("Error in XML document {0}", xmlPath), e);
                }

                var textObject = Xml.Descendants("key").Where(ci => (ci.Attribute("id") != null ? ci.Attribute("id").Value : string.Empty) == resourceKey).SingleOrDefault();
                resourceValue = textObject;

                try
                {
                    //Add the key to the cache
                    lock (this)
                    {
                        if (resCacheByCulture == null)
                        {
                            resCacheByCulture = new Dictionary<string, object>();
                            ResourceCache.Add(culture.Name, resCacheByCulture);
                        }
                        if (!ResourceDebugMode)
                        {
                            resCacheByCulture.Add(_classKey + ":" + resourceKey, resourceValue);
                        }
                    }
                }
                catch (ArgumentException)
                {
                    //Log the error, but otherwise fail silently if the key can't be added to the dictionary.  We'll just add it next time.
                    System.Diagnostics.Trace.TraceError("Failed to add " + _classKey + ":" + resourceKey + " to the cache");
                }
            }

            return resourceValue;
        }

        /// <summary>
        /// Gets an object to read resource values from a source.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Resources.IResourceReader"/> associated with the current resource provider.
        /// </returns>
        public IResourceReader ResourceReader
        {
            get
            {
                return new XmlDictionaryResourceReader
                    (ResourceCache);
            }
        }

        #endregion

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        protected override void Cleanup()
        {
            try
            {
                ResourceCache.Clear();
            }
            finally
            {
                base.Cleanup();
            }
        }

    }

    internal sealed class XmlDictionaryResourceReader : IResourceReader
    {
        private readonly IDictionary _resources;

        public XmlDictionaryResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IResourceReader.Close() { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IDisposable.Dispose() { return; }
    }
}