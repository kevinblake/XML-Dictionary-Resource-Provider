using System.Web.Compilation;

namespace Localisation.Providers.XmlDictionary
{
    /// <summary>
    /// Provider for simple XML dictionaries.
    /// </summary>
    public class XmlDictionaryProviderFactory : ResourceProviderFactory
    {

        /// <summary>
        /// When overridden in a derived class, creates a global resource provider.
        /// </summary>
        /// <param name="classKey">The name of the resource class.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Compilation.IResourceProvider"/>.
        /// </returns>
        public override IResourceProvider CreateGlobalResourceProvider
            (string classKey)
        {
            return new XmlDictionaryProvider(classKey);
        }

        /// <summary>
        /// When overridden in a derived class, creates a local resource provider.
        /// </summary>
        /// <param name="virtualPath">The path to a resource file.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Compilation.IResourceProvider"/>.
        /// </returns>
        public override IResourceProvider CreateLocalResourceProvider
            (string virtualPath)
        {
            return new XmlDictionaryProvider(virtualPath);
        }
    }
}