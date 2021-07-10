using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using System;

namespace MoreNote.Logic.Service.PasswordSecurity
{
    public class PasswordStoreFactory
    {
        public static IPasswordStore Instance(string hash_algorithm, int PasswordStoreDegreeOfParallelism=8, int PasswordStoreMemorySize=1024)
        {
            switch (hash_algorithm.ToLower())
            {
                case "sha256":
                    return new Sha256PasswordStore();

                case "argon2":
                    return new Argon2PasswordStore()
                    {
                        DegreeOfParallelism = PasswordStoreDegreeOfParallelism,
                        MemorySize = PasswordStoreMemorySize
                    };
                case "bcrypt":
                    return new BCryptPasswordStore();

                case "pbkdf2":
                    return new PDKDF2PasswordStore();

                default:
                    throw new ArgumentException("hash_algorithm is error", "hash_algorithm");
            }
        }
        public static IPasswordStore Instance(SecurityConfig securityConfig)
        {
            return Instance(securityConfig.PasswordHashAlgorithm,securityConfig.PasswordStoreDegreeOfParallelism,securityConfig.PasswordStoreMemorySize);
        }
        public static IPasswordStore Instance(User user)
        {
            return Instance(user.PasswordHashAlgorithm , user.PasswordDegreeOfParallelism, user.PasswordMemorySize);
        }
    }
}