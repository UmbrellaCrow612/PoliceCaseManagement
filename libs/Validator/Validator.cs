namespace Validator
{
    /// <summary>
    /// Allows you to define validation logic similar to fluent validation
    /// </summary>
    /// <typeparam name="T">The class you wan to validate against</typeparam>
    public abstract class Validator<T>
    {
        private readonly List<Rule<T>> _rules = [];

        /// <summary>
        /// Add a rule as part of rules to run against the <see cref="T"/>, write the case that would return true for the case you dot want.
        /// For example:
        /// AddRule(x => string.IsNullOrWhiteSpace(x.Name), "Name should not be empty or null")
        /// </summary>
        /// <param name="validator">A function of expression against <see cref="T"/> that returns a boolean result expression.</param>
        /// <param name="message">A Optional error message to go with this rule to be shown</param>
        public void AddRule(Func<T, bool> validator, string? message = null)
        {
            _rules.Add(new Rule<T> { Func = validator, Message = message });
        }

        /// <summary>
        /// Run rules defined for this validator
        /// </summary>
        public ValidatorResult<T> Execute(T item)
        {
            var result = new ValidatorResult<T>();

            foreach (var rule in _rules)
            {
                var passed = rule.Func(item);
                if (passed)
                {
                    result.Errors.Add(rule.Message ?? "No message provided");
                }
            }

            if (result.Errors.Count != 0) return result;
            result.IsSuccessful = true;
            return result;
        }
    }
}
