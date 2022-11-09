using System.Text;

namespace SourceExpress.ShorterGuid;
public static class ShorterGuid
{
    private static char[] dictionary = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray(); // RFC 4648 Base32
    private static Dictionary<char, int> reversedDictionary = new Dictionary<char, int>(dictionary.Select((c, index) => new KeyValuePair<char, int>(c, index)));
    private const int _bitMask = 31;
    private const int _bitShift = 5; // 5 bits max value is 32, i.e. we need 32 chars to encode 5 bits
    private const int _bitsInByte = 8;
    private const int _bytesinGuid = 16; // 128 bits in Guid
    private const int _outputLength = 26; // (128 bits in Guid / 5 bits) = ~26 chars


    /// <summary>
    /// Converts Guid to a 26-character long Base32-encoded string in lower case. It uses RFC 4648 Base32 alphabet
    /// </summary>
    /// <param name="guid">Target Guid</param>
    /// <returns>Base32-encoded Guid in lower case</returns>
    public static string ToLowerShorterString(this Guid guid)
    {
        return guid.ToShorterString().ToLower();
    }

    /// <summary>
    /// Converts Guid to a 26-character long Base32-encoded string in upper case. It uses RFC 4648 Base32 alphabet
    /// </summary>
    /// <param name="guid">Target Guid</param>
    /// <returns>Base32-encoded Guid in upper case</returns>
    public static string ToShorterString(this Guid guid)
    {
        var data = guid.ToByteArray();
        var result = new StringBuilder(_outputLength);

        var last = data.Length;
        var offset = 0;
        int buffer = data[offset++];
        var bitsLeft = _bitsInByte;
        while (bitsLeft > 0 || offset < last)
        {
            if (bitsLeft < _bitShift)
            {
                if (offset < last)
                {
                    buffer <<= _bitsInByte;
                    buffer |= (data[offset++] & byte.MaxValue);
                    bitsLeft += _bitsInByte;
                }
                else
                {
                    int pad = _bitShift - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }
            int index = _bitMask & (buffer >> (bitsLeft - _bitShift));
            bitsLeft -= _bitShift;
            result.Append(dictionary[index]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Converts 26-character long Base32-encoded string to Guid. It is case-insensitive and uses RFC 4648 Base32 alphabet
    /// </summary>
    /// <param name="shorterString">Base32-encoded Guid</param>
    /// <returns>Original Guid</returns>
    public static Guid FromShorterString(this string shorterString)
    {
        if (shorterString == null)
            throw new ArgumentNullException(nameof(shorterString));

        if (shorterString.Length != _outputLength)
            throw new ArgumentOutOfRangeException($"Encoded string should be exactly {_outputLength} characters long");

        var result = new byte[_bytesinGuid];
        var buffer = 0;
        var next = 0;
        var bitsLeft = 0;
        foreach (var c in shorterString.ToUpperInvariant())
        {
            if (!reversedDictionary.ContainsKey(c))
                throw new FormatException($"Invalid character: '{c}'");

            buffer <<= _bitShift;
            buffer |= reversedDictionary[c] & _bitMask;
            bitsLeft += _bitShift;
            if (bitsLeft >= _bitsInByte)
            {
                result[next++] = (byte)(buffer >> (bitsLeft - _bitsInByte));
                bitsLeft -= _bitsInByte;
            }
        }

        return new Guid(result);
    }
}