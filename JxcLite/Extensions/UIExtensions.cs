namespace JxcLite.Extensions;

public static class UIExtensions
{
    public static void Table(this RenderTreeBuilder builder, string className, Action content)
    {
        builder.OpenElement(0, "table");
        if (!string.IsNullOrEmpty(className))
            builder.AddAttribute(1, "class", className);
        content();
        builder.CloseElement();
    }

    public static void Tr(this RenderTreeBuilder builder, string className, Action content)
    {
        builder.OpenElement(0, "tr");
        if (!string.IsNullOrEmpty(className))
            builder.AddAttribute(1, "class", className);
        content();
        builder.CloseElement();
    }

    public static void Th(this RenderTreeBuilder builder, string className, string content)
    {
        builder.OpenElement(0, "th");
        if (!string.IsNullOrEmpty(className))
            builder.AddAttribute(1, "class", className);
        builder.AddContent(2, content);
        builder.CloseElement();
    }

    public static void Td(this RenderTreeBuilder builder, string className, string content)
    {
        builder.OpenElement(0, "td");
        if (!string.IsNullOrEmpty(className))
            builder.AddAttribute(1, "class", className);
        builder.AddContent(2, content);
        builder.CloseElement();
    }
}