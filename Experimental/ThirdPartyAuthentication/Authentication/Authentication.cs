using ThirdPartyAuthentication.Caches;

namespace ThirdPartyAuthentication.Authentication
{
    public class Authentication
    {
        public CacheProvider Cache;

        public Authentication(CacheProvider cache) =>
            Cache = cache;

        public IEnumerable<int> GenerateRandomNumbers() =>
            Enumerable.Range(0, 3).Select(_ => new Random().Next(0, 9) );

        public int SetPendingRequest(int identifier, string description)
        {
            var randomNumbers = GenerateRandomNumbers();

            Cache.Set(identifier.ToString(), randomNumbers.ElementAt(0));

            return randomNumbers.ElementAt(0);
        }

        public bool CheckRequestApproval(int identifier, int number)
        {
            if (Cache.Contains(identifier.ToString()))
            {
                return Cache.Get<int>(identifier.ToString()) == number;
            }

            return false;
        }

    }
}
