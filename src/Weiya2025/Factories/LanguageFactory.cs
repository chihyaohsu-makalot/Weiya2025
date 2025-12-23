using IniParser;
using IniParser.Model;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Weiya2025.Factories
{
    internal class LanguageFactory
    {
        protected readonly ILogger _logger;
        protected readonly FileIniDataParser _ini;

        public LanguageFactory(ILogger<LanguageFactory> logger)
        {
            _logger = logger;
            _ini = new FileIniDataParser();
        }

        private const string ConfigFile = "i18n.ini";

        /// <summary>
        /// 取得語言設定
        /// </summary>
        /// <returns></returns>
        public string GetCurrentLanguage()
        {
            EnsureLanguageConfig();

            //get ini
            var data = _ini.ReadFile(ConfigFile);

            if (data is null)
            {
                return "zh-Hant";
            }

            return data["language"]["default"];
        }

        /// <summary>
        /// 設定語言
        /// </summary>
        /// <param name="lang"></param>
        public void SetLanguage(string lang)
        {
            var data = new IniData();
            data["language"]["default"] = lang;

            _ini.WriteFile(ConfigFile, data);
            _logger.LogInformation($"設定語系 當前語系為 : {lang}");
        }

        /// <summary>
        /// 確保 INI 檔案存在
        /// </summary>
        private void EnsureLanguageConfig()
        {
            var config = new FileInfo(ConfigFile);
            if (!config.Exists)
            {
                SetLanguage("zh-Hant");
            }
        }
    }
}
