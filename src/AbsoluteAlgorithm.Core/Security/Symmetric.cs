using System.Security.Cryptography;
using System.Text;

namespace AbsoluteAlgorithm.Core.Security;

/// <summary>
/// Provides high-performance symmetric encryption and decryption using AES-256.
/// </summary>
public static class Symmetric
{
    private const int KeySizeBits = 256;
    private const int BlockSizeBits = 128;

    /// <summary>
    /// Generates a random symmetric key.
    /// </summary>
    public static string GenerateKey()
    {
        using var aes = Aes.Create();
        aes.KeySize = KeySizeBits;
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }

    /// <summary>
    /// Encrypts text using a base64 key and returns a base64 string containing the IV + Ciphertext.
    /// </summary>
    public static string Encrypt(string plainText, string base64Key)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        var key = Convert.FromBase64String(base64Key);
        var data = Encoding.UTF8.GetBytes(plainText);

        var encryptedBytes = Encrypt(data, key);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Decrypts a base64 string (IV + Ciphertext) using a base64 key.
    /// </summary>
    public static string Decrypt(string cipherText, string base64Key)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        var key = Convert.FromBase64String(base64Key);
        var data = Convert.FromBase64String(cipherText);

        var decryptedBytes = Decrypt(data, key);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    /// <summary>
    /// Raw byte encryption using AES-CBC with a prepended IV.
    /// </summary>
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV(); // Always generate a new IV for every encryption

        using var encryptor = aes.CreateEncryptor();
        var cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);

        // Prepend IV to the ciphertext so it's available for decryption
        var result = new byte[aes.IV.Length + cipherText.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(cipherText, 0, result, aes.IV.Length, cipherText.Length);

        return result;
    }

    /// <summary>
    /// Raw byte decryption where the IV is expected to be the first 16 bytes.
    /// </summary>
    public static byte[] Decrypt(byte[] dataWithIv, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        var iv = new byte[aes.BlockSize / 8]; // 16 bytes
        var cipherText = new byte[dataWithIv.Length - iv.Length];

        Buffer.BlockCopy(dataWithIv, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(dataWithIv, iv.Length, cipherText, 0, cipherText.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
    }
}