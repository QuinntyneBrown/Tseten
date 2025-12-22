// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Security.Cryptography;
using Tseten.Core;

namespace Tseten.Infrastructure;

public class PasswordHasher : IPasswordHasher
{
    private const int Iterations = 10000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public string HashPassword(string password, byte[] salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA1, HashSize);
        return Convert.ToBase64String(hash);
    }

    public byte[] GenerateSalt()
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public bool VerifyPassword(string password, string hashedPassword, byte[] salt)
    {
        var newHash = HashPassword(password, salt);
        return string.Equals(newHash, hashedPassword, StringComparison.Ordinal);
    }
}
