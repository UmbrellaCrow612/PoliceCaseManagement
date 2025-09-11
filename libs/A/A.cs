namespace A
{
    /// <summary>
    /// Asseration class
    /// </summary>
    public static class AS
    {
        public static void AssertNotNull(params (object Value, string Name)[] args)
        {
            foreach (var (value, name) in args)
            {
                ArgumentNullException.ThrowIfNull(value, name);
            }
        }
    }
}
