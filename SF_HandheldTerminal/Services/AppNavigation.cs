namespace SF_HandheldTerminal.Services
{
    /// <summary>
    /// 简化跨 Tab/嵌套页内容获取根 Navigation 的辅助。
    /// 主 Tab 容器可能把页面内容剥离成 ContentView 挂到 SfTabView，
    /// 因此 Page.Navigation 不可靠；此处统一通过 Window.Page 获取。
    /// </summary>
    public static class AppNavigation
    {
        public static INavigation? GetRootNavigation()
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            return window?.Page?.Navigation;
        }

        public static Task PushAsync(Page page)
        {
            var nav = GetRootNavigation();
            return nav is null ? Task.CompletedTask : nav.PushAsync(page);
        }
    }
}
