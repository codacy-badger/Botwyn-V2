using Botwyn.Data;
using Botwyn.Objects;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace Botwyn.Handlers
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;
        private static string accountsFile = "./Data/UserAccounts/Accounts.json";

        static UserAccounts()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.GetUserAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.UserID == id
                         select a;
            var account = result.FirstOrDefault();

            if (account == null)
                account = CreateUserAccount(id);

            return account;
        }

        public static List<UserAccount> GetReturningMemebers()
        {
            var result = from a in accounts
                         where a.ReturningForNextRaid == true
                         select a;
            return result.ToList();
        }

        public enum SpecType
        {
            DPS = 0,
            Healer = 10,
            Tank = 20
        }

        public static List<UserAccount> GetTrials()
        {
            var result = from a in accounts
                         where a.IsTrial == true
                         select a;
            return result.ToList();
        }

        public static List<UserAccount> GetSpec(SpecType spec)
        {
            IEnumerable<UserAccount> result = null;
            switch (spec)
            {
                case SpecType.DPS:
                    result = from a in accounts
                                 where a.MainSpec.ToLower() == "dps"
                                 select a;
                    break;
                case SpecType.Healer:
                    result = from a in accounts
                                 where a.MainSpec.ToLower() == "healer"
                                 select a;
                    break;
                case SpecType.Tank:
                    result = from a in accounts
                                 where a.MainSpec.ToLower() == "tank"
                                 select a;
                    break;
                default:
                    break;
            }
            return result.ToList();
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var NewAccount = new UserAccount()
            {
                UserID = id,
                MainChar = "Not Set",
                WowAlt = "not Set",
                MainSpec = "Not Set",
                WowAltSpec = "Not Set",
                GuildRank = "Not Set",
                ReportMade = 0,
                OwnReports = 0,
                AdminReports = 0,
                RaidsMissedWithReason = 0,
                ReturningForNextRaid = false,
                IsTrial = false
            };
            accounts.Add(NewAccount);
            SaveAccounts();
            return NewAccount;
        }

        public enum UpdateType
        {
            WowMain = 0,
            GuildRank = 10,
            WowMainSpec = 20,
            WowAlt = 30,
            WowAltSpec = 40,
            ReportMade = 50,
            UserReport = 60,
            AdminReport = 70,
            MissedRaidWithReason = 80,
            MissedRaidNoReason = 90,
            ReturningForNextRaid = 100,
            IsTrial = 110
        }

        public static void AccountUpdate(SocketUser user, string param, UpdateType type)
        {
            var intresult = 0;
            var result = from a in accounts
                         where a.UserID == user.Id
                         select a;
            var account = result.FirstOrDefault();

            if (account == null)
                account = CreateUserAccount(user.Id);
            else accounts.Remove(account);
            if (int.TryParse(param, out int number))
                intresult = number;

            var returning = false;
            if (param.ToLower() == "true")
                returning = true;

            var trial = false;
            if (param.ToLower() == "true")
                trial = true;

            switch (type)
            {
                case UpdateType.WowMain:
                    account.MainChar = param;
                    break;
                case UpdateType.GuildRank:
                    account.GuildRank = param;
                    break;
                case UpdateType.WowMainSpec:
                    account.MainSpec = param;
                    break;
                case UpdateType.WowAlt:
                    account.WowAlt = param;
                    break;
                case UpdateType.WowAltSpec:
                    account.WowAltSpec = param;
                    break;
                case UpdateType.ReportMade:
                    account.ReportMade = account.ReportMade + intresult;
                    break;
                case UpdateType.UserReport:
                    account.OwnReports = account.OwnReports + intresult;
                    break;
                case UpdateType.AdminReport:
                    account.AdminReports = account.AdminReports + intresult;
                    break;
                case UpdateType.MissedRaidWithReason:
                    account.RaidsMissedWithReason = account.RaidsMissedWithReason + intresult;
                    break;
                case UpdateType.MissedRaidNoReason:
                    account.RaidsMissedNoReason = account.RaidsMissedNoReason + intresult;
                    break;
                case UpdateType.ReturningForNextRaid:
                    account.ReturningForNextRaid = returning;
                    break;
                case UpdateType.IsTrial:
                    account.IsTrial = trial;
                    break;
                default:
                    break;
            }

            accounts.Add(account);
            SaveAccounts();
        }
    }
}
