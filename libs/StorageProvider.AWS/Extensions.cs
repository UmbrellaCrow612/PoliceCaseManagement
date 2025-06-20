namespace StorageProvider.AWS
{
    public static class Extensions
    {
        /// <summary>
        /// Adds AWS implamentation of the underlying storage provider
        /// </summary>
        public static IServiceCollection AddAWSStorageProvider(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO
            // add proj ref
            // ref aws packages
            // ref the abstractions package
            // implment a internal class for that generic interface 
            // add it to Di here services.AddScoped<IStor, AWSInternal>();
            // then call it App proj or infra proj
            // redo file atta upload to use this and the wy we stor it 
            // to use aws sns lamda functions to add it to out db after client uploads it
            // Add AWS settings object to use IOptionspattern and validate the settings on start and ad it to the Di as well
            
            return services;
        }
    }
}