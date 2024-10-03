namespace AnimeTrackingWeb;

public static class Extensions
{
    /// <summary>
    /// Замапить контроллер.
    /// </summary>
    /// <param name="endpoints">Строитель эндпоинтов.</param>
    /// <param name="route">Путь к ресурсу.</param>
    /// <typeparam name="T">Тип контроллера.</typeparam>
    /// <returns>Строитель эндпоинтов.</returns>
    public static ControllerActionEndpointConventionBuilder MapBotWebhookRoute<T>(
        this IEndpointRouteBuilder endpoints,
        string route)
    {
        var controllerName = typeof(T).Name.Replace("Controller", "");
        var actionName = typeof(T).GetMethods()[0].Name;

        return endpoints.MapControllerRoute(
            name: "bot_webhook",
            pattern: route,
            defaults: new { controller = controllerName, action = actionName });
    }

    /// <summary>
    /// Парсинг целого числа.
    /// </summary>
    /// <param name="text">Текстовое значение.</param>
    /// <param name="defaultValue">Значение по умолчанию [предустановленно].</param>
    /// <returns>Значение типа Int.</returns>
    public static int ParseInt(this string text, int defaultValue = 0)
    {
        int v;
        if (int.TryParse(text, out v))
        {
            return v;
        }

        return defaultValue;
    }
}