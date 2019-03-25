namespace LogixHealth.EnterpriseLogger.Extensions
{

    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal class Expression
    {
        public List<Regex> Regexes { get; set; }

        public System.Action<Match, object> Action { get; set; }
    }
}
