namespace Challenge.Core.Constants
{
    /// <summary>
    /// These are a list challenges
    /// A challenge is when you need extra temproy tokens to valid a user action
    /// for example the deletion of a case by a user - even if it is secured and they have the role 
    /// we wouold like them to put there password in again and then would would recieve a chllagnge token like a jwt token
    /// this woule be valided on the endpoint and subsequent requests and expire as well
    /// </summary>
    public static class ChallengeConstants
    {
        public const string DELETE_CASE = "deleteCase";

        public static IReadOnlyCollection<string> Challenges =>
        [
            DELETE_CASE
        ];
    }
}
