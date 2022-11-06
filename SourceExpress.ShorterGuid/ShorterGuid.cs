using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SourceExpress.ShorterGuid;
public static class ShorterGuid
{
    private static char[] dictionary = "012345abcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static Dictionary<char, int> reversedDictionary = new Dictionary<char, int>(dictionary.Select((c, index) => new KeyValuePair<char, int>(c, index)));
    private const int _bitMask = 31;
    private const int _bitShift = 5; // 5 bits max value is 32, i.e. we need 32 chars to encode 5 bits
    private const int _bitsInByte = 8;
    private const int _bytesinGuid = 16; // 128 bits in Guid
    private const int _outputLength = 26; // (128 bits in Guid / 5 bits) = ~26 chars


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
                    buffer |= (data[offset++] & 0xff);
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

    public static Guid FromShorterString(this string encoded)
    {
        if (encoded == null)
            throw new ArgumentNullException(nameof(encoded));

        if (encoded.Length != _outputLength)
            throw new ArgumentOutOfRangeException($"Encoded string should be exactly {_outputLength} characters long");

        var result = new byte[_bytesinGuid];
        var buffer = 0;
        var next = 0;
        var bitsLeft = 0;
        foreach (var c in encoded.ToLower())
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