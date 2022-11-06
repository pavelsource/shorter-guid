# SourceExpress.ShorterGuid

This library contains extension methods `ToShorterString()` for `Guid` and `FromShorterString()` for `String`. It generates a 26 character string from the Guid, which is **URL-safe** and **case-insensitive**!

Inspired by [CSharpVitamins.ShortGuid](https://github.com/csharpvitamins/CSharpVitamins.ShortGuid) which uses Base64 encoding and produces 22-character string, however this library uses Base32-ish encoding, and only uses the following character set: `012345abcdefghijklmnopqrstuvwxyz`.


Available on [NuGet](https://www.nuget.org/packages/sourceexpress.shorterguid/). To install, run the following command in the Package Manager Console:

    PM> Install-Package SourceExpress.ShorterGuid

---

## The Gist

It takes a standard guid like this:

`b392ebf5-88d3-49b9-9ad8-1d4a73431787`

and shortens it to a smaller string like this:

`yrpt5gynl2wonaqs3p5baksrkw`

---


## Using the ShorterGuid

The `ShorterGuid` provides an extension method for `Guid` class:

```csharp
var guid = Guid.NewGuid();  // b392ebf5-88d3-49b9-9ad8-1d4a73431787
var shorterGuid = guid.ToShorterString(); // yrpt5gynl2wonaqs3p5baksrkw
```

If you need to get `Guid` from the `ShorterGuid string`, you can do that using second extension method:

```csharp
var shorterGuid = "yrpt5gynl2wonaqs3p5baksrkw";
var guid = shorterGuid.FromShorterString(); // b392ebf5-88d3-49b9-9ad8-1d4a73431787
```

Methods contain some internal error handling like wrong character or incorrect length of the `ShorterGuid`. See test project for more details.