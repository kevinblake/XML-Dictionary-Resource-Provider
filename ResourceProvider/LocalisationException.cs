using System;

namespace Localisation
{
    /// <summary>
    /// Localisation Exception, to be raised where keys, classes, or cultures cannot be found
    /// </summary>
    public class LocalisationException : Exception
    {
        public LocalisationException(string message, Exception innerException) : base(message, innerException) { }
    }



}