namespace Style.Core
{
    public abstract class StyleBase
    {
        protected List<StyleViolation> _violations = [];
        protected List<StyleRule> _rules = [];
        public IReadOnlyList<StyleViolation> Violations => _violations.AsReadOnly();
        public IReadOnlyList<StyleRule> StyleRules => _rules.AsReadOnly();

        public void AddRule(StyleRule rule)
        {
            _rules.Add(rule);
        }

        protected void ProcessViolation(StyleViolation? violation)
        {
            if (violation != null)
            {
                _violations.Add(violation);
            }
        }

        public void ClearViolations()
        {
            _violations.Clear();
        }
    }

}
