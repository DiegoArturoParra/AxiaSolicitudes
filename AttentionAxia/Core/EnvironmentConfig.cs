using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttentionAxia.Core
{
    public static class EnvironmentConfig
    {
        public const string ENVIRONMENT_TESTING = "Testing";
        public const string ENVIRONMENT_PRODUCTION = "Production";
        public const string NAME_CONNECTION_PRODUCTION = "AxiaContextProduction";
        public const string NAME_CONNECTION_TESTING = "AxiaContextTesting";
    }
}