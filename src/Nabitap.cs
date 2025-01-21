using System;
using System.IO;
using System.Numerics;
namespace Screval.Nabitap{
    public static class BitapMatch{
        public enum MatchTypes {MatchAny, MatchStart, MatchEnd, MatchComplete, MatchWildcard};
        public static int MatchSingle(string input, string pattern, int n=1, bool ignoreCase=false, MatchTypes MatchType = MatchTypes.MatchAny,bool matchPerLine=false){
            if(pattern.Length==0)return 0;
            if(ignoreCase){
                input=input.ToLower();
                pattern=pattern.ToLower();
            }
            if(input==pattern)return 0;
            if(matchPerLine){
                int index = -1;
                int counter = 0;
                using (var reader = new StringReader(input)){
                    for (string? line = reader.ReadLine(); line!=null; line = reader.ReadLine()){
                        if(MatchSingle(line,pattern,n,false,MatchType,false)>-1){
                            index=counter;
                            break;
                        }
                        counter++;
                    }
                }
                return index;
            }
            ReadOnlySpan<char> inputSpan = input.AsSpan();
            if(MatchType==MatchTypes.MatchWildcard){
                string[] parts = pattern.Split("*");
                int currStart = 0;
                int retVal=-1;
                int initial = pattern[0]=='*'?1:0;
                for(int i = initial;i<parts.Length;i++){
                    var currPart = parts[i];
                    var currResult = MatchSingle(inputSpan.Slice(currStart),currPart,n,MatchTypes.MatchAny);
                    if(currResult==-1)return -1;
                    if(i==initial)retVal=currResult;
                    currStart=currResult;
                }
                return retVal;
            }
            return MatchSingle(inputSpan,pattern,n,MatchType);
        }
        public static int MatchSingle(ReadOnlySpan<char> input, string pattern, int n=1, MatchTypes MatchType = MatchTypes.MatchAny){
            int result = -1;
            int[] row = new int[n+1];
            for(int i  = 0;i<=n;i++)row[i]=1;
            int maxCharCode = -1;
            for(int i = 0;i<input.Length;i++)maxCharCode=Math.Max(maxCharCode,(int)input[i]);
            for(int i = 0;i<pattern.Length;i++)maxCharCode=Math.Max(maxCharCode,(int)pattern[i]);
            int[] patternMask = new int[maxCharCode+1];
            for(int i = 0;i<pattern.Length;i++){
                patternMask[(int)pattern[i]]|=1<<i;
            }
            int length = input.Length;
            int patternLength = pattern.Length;
            long checkMatch = 1<<patternLength;
            for(int i = 0;i<length;i++){
                int oldCol = 0;
                int nextOldCol = 0;
                for(int j = 0;j<=n;j++){
                    int curr = patternMask[(char)input[i]];
                    int currIsFilled = row[j]&curr;
                    int replace = (oldCol|currIsFilled)<<1;
                    int insert = oldCol|(currIsFilled<<1);
                    int delete = (nextOldCol|(currIsFilled))<<1;
                    oldCol=row[j];
                    row[j]=replace|insert|delete|1;
                    nextOldCol=row[j];
                }
                if((row[n]&checkMatch)>0){
                    bool canBreak;
                    switch(MatchType){
                        case MatchTypes.MatchEnd:
                            canBreak=i==length-1;
                        break;
                        case MatchTypes.MatchStart:
                            canBreak=i==pattern.Length;
                        break;
                        case MatchTypes.MatchComplete:
                            canBreak=i==pattern.Length&&i==length-1;
                        break;
                        case MatchTypes.MatchAny:
                        default:
                            canBreak=true;
                        break;
                    }
                    if(!canBreak)continue;
                    result=i;
                    break;
                }
            }
            return result;
        }
    }
}
