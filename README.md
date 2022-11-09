# SourceExpress.ShorterGuid

This library contains extension methods to generate a 26-character long string from the Guid, which is **URL-safe** and **case-insensitive**! It uses [Base32 encoding](https://en.wikipedia.org/wiki/Base32#RFC_4648_Base32_alphabet) with the following character set: `ABCDEFGHIJKLMNOPQRSTUVWXYZ234567`.

Inspired by [CSharpVitamins.ShortGuid](https://github.com/csharpvitamins/CSharpVitamins.ShortGuid) library which uses Base64 encoding and produces a 22-character long string, however it is case-sensitive!

Available on [NuGet](https://www.nuget.org/packages/sourceexpress.shorterguid/). To install, run the following command in the Package Manager Console:

    PM> Install-Package SourceExpress.ShorterGuid

---

## The Gist

It takes a standard guid like this:

`7ff2d2cb-1e1d-41cc-b478-32adb96082bd`

and shortens it to a smaller string like this:

`ZPJPE7Y5D3GEDNDYGKW3SYECXU` or `zpjpe7y5d3gedndygkw3syecxu`

---


## Using the ShorterGuid

The `ShorterGuid` provides two extension methods for `Guid` class:

```csharp
var guid = Guid.NewGuid();  // 7ff2d2cb-1e1d-41cc-b478-32adb96082bd
var shorterGuid = guid.ToShorterString(); // ZPJPE7Y5D3GEDNDYGKW3SYECXU
```

```csharp
var guid = Guid.NewGuid();  // 7ff2d2cb-1e1d-41cc-b478-32adb96082bd
var shorterGuid = guid.ToLowerShorterString(); // zpjpe7y5d3gedndygkw3syecxu
```

If you need to get `Guid` from the `ShorterGuid string`, you can do that using second extension method:

```csharp
var shorterGuid = "ZPJPE7Y5D3GEDNDYGKW3SYECXU"; // or "zpjpe7y5d3gedndygkw3syecxu"
var guid = shorterGuid.FromShorterString(); // 7ff2d2cb-1e1d-41cc-b478-32adb96082bd
```

Methods contain some internal error handling like wrong character or incorrect length of the `ShorterGuid`. See test project for more details.