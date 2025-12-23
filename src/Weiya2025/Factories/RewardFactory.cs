using IniParser;
using IniParser.Model;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using Weiya2025.Options;

namespace Weiya2025.Factories
{
    internal class RewardFactory
    {
        protected readonly ILogger _logger;
        protected readonly FileIniDataParser _ini;
        private IniData _data;

        public RewardFactory(ILogger<RewardFactory> logger)
        {
            _logger = logger;
            _ini = new FileIniDataParser();
            EnsureIniConfig();
        }

        private const string ConfigFile = "reward.ini";

        /// <summary>
        /// 重新載入得獎者
        /// </summary>
        /// <param name="round"></param>
        /// <param name="users"></param>
        public void ReloadRewardedUser(string round, ObservableCollection<UserOption> users)
        {
            if (users is null)
                users = new ObservableCollection<UserOption>();

            foreach (var unit in _data[round])
            {
                var tmp = unit.KeyName.Split("@");
                var division = tmp[0];
                var department = tmp[1];

                users.Add(new UserOption
                {
                    Division = division,
                    Department = department,
                    Name = unit.Value
                });
            }
        }

        /// <summary>
        /// 紀錄得獎者
        /// </summary>
        /// <param name="round"></param>
        /// <param name="order"></param>
        /// <param name="name"></param>
        public void SetRewardUser(string round, string unit, string name)
        {
            _data[round][unit] = name;

            _ini.WriteFile(ConfigFile, _data);
            _logger.LogInformation($"抽出 {round} 得獎者 {unit} : {name}");
        }


        /// <summary>
        /// 確保 INI 檔案存在
        /// </summary>
        private void EnsureIniConfig()
        {
            var config = new FileInfo(ConfigFile);

            _data = config.Exists ? _ini.ReadFile(ConfigFile) : new IniData();
            if (!config.Exists)
            {
                _data["尾牙"]["年度"] = "2025";
                _ini.WriteFile(ConfigFile, _data);
                _logger.LogInformation("尾牙獎金紀錄表 初始化");
            }
        }
    }
}
