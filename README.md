# Nabitap: Bitap Algorithm in C#

Nabitap is a C# implementation of the Bitap algorithm, a fast and efficient approximate string matching algorithm. It supports deletions, insertions, substitutions, and case insensitive matching.
The Bitap algorithm efficiently finds substrings that match with fewer than n differences, based on the Levenshtein distance, which measures the number of insertions, deletions, or substitutions required to transform one string into another. For more information on editing distance, you can refer to [here](https://en.wikipedia.org/wiki/Levenshtein_distance).

## Table of Contents
- [Installation](#installation)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)

## Installation

To install the Nabitap module, run the following command in your .NET project:

```bash
dotnet add package Nabitap
```
## Usage
```csharp
using Screval.Nabitap;

string input = "this is a test string";
string pattern = "twst";

// Matching using the default settings
int matchIndex = BitapMatch.MatchSingle(input, pattern);
Console.WriteLine(matchIndex); // Output: Index of the end of match or -1
// Default maximum difference amount is 1 so it should match and return 13, end of the matched string

int exactMatch = BitapMatch.MatchSingle(input, pattern, 0);
Console.WriteLine(exactMatch); // Output: -1 as the string `twst` cannot be matched with 0 differences to the input

```

## API Reference
`BitapMatch.MatchSingle`
```csharp
public static int MatchSingle(
    string input, 
    string pattern, 
    int n = 1, 
    bool ignoreCase = false, 
    MatchTypes MatchType = MatchTypes.MatchAny, 
    bool matchPerLine = false
)
```
```csharp
public static int MatchSingle(
    ReadOnlySpan<char> input, 
    string pattern, 
    int n = 1, 
    MatchTypes MatchType = MatchTypes.MatchAny
)

````


The BitapMatch.MatchTypes enum specifies the type of matching behavior. Available options:
- MatchAny: Matches anywhere in the string.
- MatchStart: Matches only if the pattern is at the start of the string.
- MatchEnd: Matches only if the pattern is at the end of the string.
- MatchComplete: Matches only if the pattern exactly matches the input.
- MatchWildcard: Matches using wildcard patterns.

## Contributing
While this project was initially created in part of my Screval project, if you'd like to improve Nabitap, feel free to open a pull request or submit an issue on the project's GitHub repository.

## License  

This project is licensed under the MIT License.