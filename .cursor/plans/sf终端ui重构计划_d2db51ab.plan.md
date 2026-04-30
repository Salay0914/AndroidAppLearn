---
name: SF终端UI重构计划
overview: 基于设计图三屏（概览/监测/设置）对 SF_HandheldTerminal 主界面 UI 做整体重构，沿用 Syncfusion 控件风格，并新建轻量 ViewModel 提供静态占位数据。
todos:
  - id: design_tokens
    content: 新增 Styles/CableTheme.xaml（色板 + Chip/Card/SectionHeader/PrimaryButton 样式）并合并进 LightTheme
    status: completed
  - id: models
    content: 新建 Models/Cable/ 下的占位数据模型（CableDevice/AlarmRecord/MonitoringRecord/SensorReading/DeviceStatusSummary/HealthBreakdown/SettingsGroup/SettingsItem）
    status: completed
  - id: vm_overview
    content: 重写 OverviewViewModel 内容为占位字段（StatusSummary、HealthBreakdown、LatestAlarms、RecentRecords、HealthScore、UpdatedAtText）
    status: completed
  - id: vm_monitoring
    content: 重写 MonitoringViewModel 内容为占位字段（CurrentCable、SensorReadings、SelectedTabIndex、SearchText）
    status: completed
  - id: vm_settings
    content: 新增 SettingsViewModel（Groups + Logout 占位命令）并在 Settings.xaml.cs 里赋值 BindingContext
    status: completed
  - id: page_overview
    content: 重写 Views/Overview.xaml：Hero/状态概览/环形图/最新告警/最近记录 五区块
    status: completed
  - id: page_monitoring
    content: 重写 Views/Monitoring.xaml：AppBar/搜索/当前电缆卡/4段 Tab/传感器列表/开始监测按钮
    status: completed
  - id: page_settings
    content: 重写 Views/Settings.xaml：分组设置列表 + 退出登录按钮
    status: completed
  - id: stub_pages
    content: 新建占位子页：AlarmListPage/MonitorHistoryPage/CableDetailPage/ScannerPage/SettingsDetailPage 并接路由
    status: completed
  - id: csproj_register
    content: 在 SF_HandheldTerminal.csproj 中注册新增的 XAML（CableTheme、各子页）为 MauiXaml/Compile
    status: completed
  - id: verify
    content: Android 与 Windows 各跑一轮，检查滚动/颜色/环形图/占位子页跳转是否正常
    status: completed
isProject: false
---

## 设计基线

### 视觉令牌（拟新增 [SF_HandheldTerminal/Styles/CableTheme.xaml](SF_HandheldTerminal/Styles/CableTheme.xaml)）

- 主色：`PrimaryBlue=#1F3A6E`、`PrimaryBlueDeep=#142A55`（Hero/AppBar/主按钮）
- 渐变：`HeroGradientStart=#1B3A6F` → `HeroGradientEnd=#1E4D8A`
- 强调：`AccentBlue=#2F6FE0`（链接、辅助按钮）
- 风险/状态：`StatusHigh=#E74C3C`、`StatusMedium=#F5A623`、`StatusNormal=#2BBF6E`、`StatusOffline=#9AA4B0`
- 背景：`SurfacePage=#F4F6FA`、`SurfaceCard=#FFFFFF`
- 文字：`TextPrimary=#1A2333`、`TextSecondary=#6B7280`、`TextTertiary=#9AA4B0`
- 卡片圆角 12、浅阴影；Chip 圆角 6、浅底深字（红/橙/绿底各对应浅色变体）

### 控件分配

- 主 Tab：沿用 [SF_HandheldTerminal/Views/Main/MainTabPage.xaml](SF_HandheldTerminal/Views/Main/MainTabPage.xaml) 中 `SfTabView`，仅替换内容
- 健康环形图：`Syncfusion.Maui.Charts` 的 `SfCircularChart` + `DoughnutSeries`，中心 `Annotation` 显示百分比
- 监测页 4 段切换（实时数据/趋势图/告警记录/设备信息）：`Syncfusion.Maui.Toolkit.SegmentedControl` 或 `SfTabView` 内嵌
- 卡片/列表：原生 `Border` + `CollectionView`/`SfListView`
- 状态徽章：`Border` + `Label`（或 `Syncfusion.Maui.Core.Chips.SfChip`）
- 搜索栏：`Border` + `Entry`（搜索图标用 FontIcons.xaml 的 `Search`）
- 主按钮："开始新一轮监测" 用 `SfButton` + 自定义样式

## 文件改动清单

### 新增

- [SF_HandheldTerminal/Styles/CableTheme.xaml](SF_HandheldTerminal/Styles/CableTheme.xaml) + `.xaml.cs`：色板、Chip 样式、CardBorder 样式、SectionHeader 样式、PrimaryButton 样式；并在 [SF_HandheldTerminal/Themes/LightTheme.xaml](SF_HandheldTerminal/Themes/LightTheme.xaml) 的 `MergedDictionaries` 中合并
- 占位数据模型放在 `SF_HandheldTerminal/Models/Cable/`：
  - `CableDevice.cs`（Id、Name、Location、Status、UpdatedAt、HealthLevel）
  - `AlarmRecord.cs`（CableName、Description、RiskLevel）
  - `MonitoringRecord.cs`（CableName、Status、Time）
  - `SensorReading.cs`（Name、Value、Unit、Threshold、Status、IconGlyph、IconBackground）
  - `DeviceStatusSummary.cs`（Total、Normal、Alarm、Offline、UpdatedAt）
  - `HealthBreakdown.cs`（Healthy、LightRisk、HighRisk）
  - `SettingsGroup.cs` / `SettingsItem.cs`（Group 含 Title 与 Items；Item 含 IconGlyph、Title、ValueText、HasNavigation）
- 占位子页（仅外壳，进入有空白卡片，避免点击崩溃）：
  - `SF_HandheldTerminal/Views/Cable/AlarmListPage.xaml`
  - `SF_HandheldTerminal/Views/Cable/MonitorHistoryPage.xaml`
  - `SF_HandheldTerminal/Views/Cable/CableDetailPage.xaml`
  - `SF_HandheldTerminal/Views/Cable/ScannerPage.xaml`
  - `SF_HandheldTerminal/Views/Settings/SettingsDetailPage.xaml`（按 SettingsItem.Title 复用）
- `SF_HandheldTerminal/ViewModels/SettingsViewModel.cs`：仅暴露 `Groups: ObservableCollection<SettingsGroup>` 和登出命令占位

### 重构（保留路径与类名，内容全替换）

- [SF_HandheldTerminal/Views/Overview.xaml](SF_HandheldTerminal/Views/Overview.xaml) 与 [SF_HandheldTerminal/Views/Overview.xaml.cs](SF_HandheldTerminal/Views/Overview.xaml.cs)
  - 顶层 `ScrollView` + `VerticalStackLayout`，间距 12，背景 `SurfacePage`
  - **Hero 卡**：渐变 `Border`，左侧标题"铁路电缆监测手持终端" + "您好，运维人员 ▾"；右下角 `Image`（占位用 Resources/Images 现有图，资源缺失先用图标 Label）
  - **设备状态概览卡**：标题行（"设备状态概览" + 刷新 IconButton）+ 更新时间 + 4 列 Grid，每格圆形图标背景 + 数字 + 名称（数据绑 `OverviewViewModel.StatusSummary`）
  - **电缆健康状态卡**：左 `SfCircularChart` 环形图（中心 `ChartAnnotation` 显示 `83%` + "健康度"）；右图例三行（健康/轻度风险/高风险，各带圆点）
  - **最新告警卡**：标题 + "查看全部 ›"（点击导航 `AlarmListPage`），下方 `BindableLayout` 渲染两条
  - **最近监测记录卡**：标题 + "查看全部 ›"（导航 `MonitorHistoryPage`），下方 3 行（左电缆名、中状态 chip、右时间）
- [SF_HandheldTerminal/Views/Monitoring.xaml](SF_HandheldTerminal/Views/Monitoring.xaml) 与 [SF_HandheldTerminal/Views/Monitoring.xaml.cs](SF_HandheldTerminal/Views/Monitoring.xaml.cs)
  - 顶部蓝色 AppBar 区（标题"监测" + 右侧扫码 IconButton，点击导航 `ScannerPage`）
  - 搜索行：圆角 `Border` 包 `Entry` + 右侧 漏斗 IconButton
  - 当前电缆卡：电缆名 + 高风险 chip + 右箭头（点击导航 `CableDetailPage`），位置/监测时间/在线点
  - Tab 选择条：4 段（实时数据/趋势图/告警记录/设备信息）；本轮只填"实时数据" Tab，其它 Tab 留空白卡片占位
  - 实时数据：`CollectionView` 渲染 `MonitoringViewModel.SensorReadings`，每项 = 圆形图标 + 名称/阈值 + 值/状态 chip
  - 底部固定主按钮"开始新一轮监测"（`SfButton`，主蓝填充，圆角 8，全宽），暂时只 `DisplayAlert` 占位
- [SF_HandheldTerminal/Views/Settings.xaml](SF_HandheldTerminal/Views/Settings.xaml) 与 [SF_HandheldTerminal/Views/Settings.xaml.cs](SF_HandheldTerminal/Views/Settings.xaml.cs)
  - 顶部 AppBar 区（"设置"标题）
  - `CollectionView`（IsGrouped=True）渲染 `SettingsViewModel.Groups`：分组头 SectionHeader 样式、分组卡片白底圆角，每项左图标 + 文字、右值/箭头
  - 底部"退出登录"按钮（红色描边、白底）

### 重写 ViewModel 内容（保留类名）

- [SF_HandheldTerminal/ViewModels/OverviewViewModel.cs](SF_HandheldTerminal/ViewModels/OverviewViewModel.cs)：清空 HealthCare/Travel 字段；只暴露 `StatusSummary`、`HealthBreakdown`、`LatestAlarms`、`RecentRecords`、`HealthScore`、`UpdatedAtText`，构造函数中赋静态占位
- [SF_HandheldTerminal/ViewModels/MonitoringViewModel.cs](SF_HandheldTerminal/ViewModels/MonitoringViewModel.cs)：清空 Travel 字段；暴露 `CurrentCable`、`SensorReadings`、`SelectedTabIndex`、`SearchText`
- 新增 `SettingsViewModel`，绑到 `Views/Settings.xaml`（在 [SF_HandheldTerminal/Views/Settings.xaml.cs](SF_HandheldTerminal/Views/Settings.xaml.cs) `BindingContext = new SettingsViewModel()`）

### 暂不动

- `Views/Login.xaml`、`Views/TaskNotification.xaml`、`Views/SimpleAboutUs.xaml`、`Views/NoInternetConnection.xaml`、`Views/SomethingWentWrong.xaml`、`MapsFeatures.xaml`、`Models/HealthCareModel.cs`/`ChartModel.cs`/`Models/Travel.cs`、旧 `OverviewViewModel` 中的 `CaloriesChartData` 等周边类（孤儿但暂保留，等清理轮次再处理）
- 主 Tab 外壳 [SF_HandheldTerminal/Views/Main/MainTabPage.xaml](SF_HandheldTerminal/Views/Main/MainTabPage.xaml)：底部 Tab 行外观（图标/字号/高度）已 OK，本轮不改
- 所有真实数据/服务接入

## 验证方式

- Android（主目标）+ Windows 各跑一次，校验：
  - 三个主 Tab 滚动正常、卡片不溢出
  - 环形图百分比正确居中
  - 状态 chip 颜色区分明显
  - 占位子页能从相应入口点击进入并安全返回
  - 浅色主题下颜色与设计图视觉接近