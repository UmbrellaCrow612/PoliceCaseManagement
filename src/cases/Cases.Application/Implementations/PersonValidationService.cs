using Person.V1;

namespace Cases.Application.Implementations
{
    internal class PersonValidationService(PersonService.PersonServiceClient client)
    {
        private readonly PersonService.PersonServiceClient _client = client;

        public async Task<bool> PersonExistsAsync(string personId)
        {
            var res = await _client.DoesPersonExistAsync(new DoesPersonExistRequest { PersonId = personId });
            return res.Exists;
        }

        public async Task<GetPersonByIdResponse> GetPersonByIdAsync(string personId)
        {
            return await _client.GetPersonByIdAsync(new GetPersonByIdRequest { PersonId = personId });
        }
    }
}
