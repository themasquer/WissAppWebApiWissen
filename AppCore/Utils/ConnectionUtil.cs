using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Config;

namespace AppCore.Utils
{
    // Projelerde şifrelenmiş user id ve password alanlarını deşifreleyerek connection string dönen utility class
    public static class ConnectionUtil
    {
        public static string GetDecryptedConnectionString(string connectionString)
        {
            connectionString = StringUtil.ConvertTRtoEN(connectionString);
            string userId = StringUtil.GetStringBetween(connectionString, "user id=", ";");
            string password = StringUtil.GetStringBetween(connectionString, "password=", ";");
            connectionString = connectionString.Replace(userId, CryptoUtil.Decrypt(userId, CryptoConfig.CryptoKey))
                .Replace(password, CryptoUtil.Decrypt(password, CryptoConfig.CryptoKey));
            return connectionString;
        }
    }
}
