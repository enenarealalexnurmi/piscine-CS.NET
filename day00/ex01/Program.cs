using System;
using System.IO;
using System.Text.RegularExpressions;

static int LevenshteinDistance(string name, string dictName)
{
    int lenName = name.Length;
    int lenDictName = dictName.Length;
    int[,] dist = new int[lenName + 1, lenDictName + 1];

    if (lenName == 0)
        return lenDictName;
    if (lenDictName == 0)
        return lenName;
    for (int i = 0; i <= lenName; i++)
        dist[i, 0] = i;
    for (int j = 0; j <= lenDictName; j++)
        dist[0, j] = j;
    for (int i = 1; i <= lenName; i++)
    {
        for (int j = 1; j <= lenDictName; j++)
        {
            int cost = (dictName[j - 1] == name[i - 1]) ? 0 : 1;
            dist[i, j] = Math.Min(Math.Min(dist[i - 1, j] + 1, dist[i, j - 1] + 1), dist[i - 1, j - 1] + cost);
        }
    }
    return dist[lenName, lenDictName];
}

static bool IsCorrectString(string inputString)
{
    Regex r = new Regex("^[a-zA-Z -]+$");
    if (r.IsMatch(inputString))
        return true;
    else
        return false;
}

try
{
    Console.WriteLine("Enter name:");
    string name = Console.ReadLine();
    if (name.Length == 0 || !IsCorrectString(name))
        throw new Exception();
    string answer;
    int levDist;
    bool nameFound = false;
    foreach (string dictName in File.ReadLines("./us.txt"))
    {
        levDist = LevenshteinDistance(name, dictName);
        if (levDist == 0)
        {
            nameFound = true;
            break;
        }
        if (levDist < 3)
        {
            Console.WriteLine("Did you mean \"{0}\"? Y/N", dictName);
            answer = Console.ReadLine();
            while (answer != "Y" && answer != "N")
            {
                Console.WriteLine("Type please Y or N...");
                answer = Console.ReadLine();
            }
            if (answer == "Y")
            {
                nameFound = true;
                name = dictName;
                break;
            }
            else continue;
        }
    }
    if (nameFound)
        Console.WriteLine("Hello, {0}", name);
    else
        throw new Exception();
}
catch
{
    Console.WriteLine("Your name was not found.");
}