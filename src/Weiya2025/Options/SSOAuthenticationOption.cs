using System.Collections.Generic;

namespace Weiya2025.Options
{
    internal class SSOAuthenticationOption
    {
        /// <summary>
        /// Authenticate Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Authenticate Issuer Key
        /// </summary>
        public string IssuerKey { get; set; }
        /// <summary>
        /// Audiences
        /// </summary>
        public IEnumerable<string> Audiences { get; set; }
    }
}
