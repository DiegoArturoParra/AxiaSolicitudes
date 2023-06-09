﻿namespace AttentionAxia.Helpers
{
    public static class HashHelper
    {
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static string GenerateHashWithPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static string GenerateEncrypt(string plainText)
        {
            return Encryption.EncryptDecrypt.Encrypt(plainText);
        }

        public static string GenerateDecrypt(string plainText)
        {
            return Encryption.EncryptDecrypt.Decrypt(plainText);
        }
    }
}