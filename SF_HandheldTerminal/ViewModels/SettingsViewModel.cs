using SF_HandheldTerminal.Models.Cable;
using SF_HandheldTerminal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SF_HandheldTerminal
{
    /// <summary>
    /// 设置页 ViewModel：当前仅提供静态分组占位与登出占位命令。
    /// </summary>
    public sealed class SettingsViewModel : BaseViewModel
    {
        public ObservableCollection<SettingsGroup> Groups { get; }
        public ICommand LogoutCommand { get; }

        public SettingsViewModel()
        {
            Groups = new ObservableCollection<SettingsGroup>
            {
                new("设备管理", new[]
                {
                    new SettingsItem { IconGlyph = "\ue74a", Title = "设备连接", ValueText = "已连接" },
                    new SettingsItem { IconGlyph = "\ue706", Title = "设备信息" },
                    new SettingsItem { IconGlyph = "\ue730", Title = "固件更新" },
                }),
                new("监测设置", new[]
                {
                    new SettingsItem { IconGlyph = "\ue72b", Title = "监测参数设置" },
                    new SettingsItem { IconGlyph = "\ue707", Title = "告警阈值设置" },
                    new SettingsItem { IconGlyph = "\ue742", Title = "监测频率设置" },
                }),
                new("数据管理", new[]
                {
                    new SettingsItem { IconGlyph = "\ue717", Title = "数据导出" },
                    new SettingsItem { IconGlyph = "\ue716", Title = "数据清除" },
                    new SettingsItem { IconGlyph = "\ue70d", Title = "存储管理" },
                }),
                new("系统设置", new[]
                {
                    new SettingsItem { IconGlyph = "\ue733", Title = "单位设置" },
                    new SettingsItem { IconGlyph = "\ue725", Title = "语言设置", ValueText = "简体中文" },
                    new SettingsItem { IconGlyph = "\ue742", Title = "日期时间" },
                    new SettingsItem { IconGlyph = "\ue70a", Title = "关于我们" },
                }),
            };

            LogoutCommand = new Command(OnLogout);
        }

        private void OnLogout()
        {
            // 占位：等待真实账号体系接入。
        }
    }
}
