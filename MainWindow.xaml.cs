using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Tab
{
    abstract class Ciphers
    {
        private static readonly string errorString = "Проверьте правильность введенных данных";
        private static readonly string symbolsErrorInputString = "Проверьте наличие внеалфавитных символов во входной строке";
        private static readonly string parityErrorInputString = "Количество символов во входной строке должно быть четным";

        private static readonly List<string> alphabetsList = new List<string>() { "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ",
                                                                                  "абвгдеёжзийклмнопрстуфхцчшщъыьэюя",
                                                                                  "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                                                                                  "abcdefghijklmnopqrstuvwxyz" };

        private static int getNumber(char letter, List<string> alphabets)
        {
            foreach (var alphabet in alphabets)
                if (alphabet.Contains(letter))
                    return alphabet.IndexOf(letter);
            return -1;
        }

        private static char getLetter(int offset, string alphabet)
        {
            int index = offset % alphabet.Length;
            index = (index < 0) ? index + alphabet.Length : index;
            return alphabet.ElementAt(index);
        }

        private static List<string> GenerateSloganAlphabet(string slogan)
        {
            List<string> sloganAlphabetsList = new List<string>() { "", "", "", "" };

            string formalizedSlogan = String.Empty;

            foreach (var letter in slogan)
            {
                if (alphabetsList.ElementAt(0).Contains(letter) || alphabetsList.ElementAt(1).Contains(letter))
                {
                    sloganAlphabetsList[0] += (!sloganAlphabetsList[0].Contains(Char.ToUpper(letter))) ? Char.ToUpper(letter).ToString() : String.Empty;
                    sloganAlphabetsList[1] += (!sloganAlphabetsList[1].Contains(Char.ToLower(letter))) ? Char.ToLower(letter).ToString() : String.Empty;
                } else if (alphabetsList.ElementAt(2).Contains(letter) || alphabetsList.ElementAt(3).Contains(letter)) {
                    sloganAlphabetsList[2] += (!sloganAlphabetsList[2].Contains(Char.ToUpper(letter))) ? Char.ToUpper(letter).ToString() : String.Empty;
                    sloganAlphabetsList[3] += (!sloganAlphabetsList[3].Contains(Char.ToLower(letter))) ? Char.ToLower(letter).ToString() : String.Empty;
                }
            }

            for (int i = 0; i < alphabetsList.Count; i++)
                foreach (var letter in alphabetsList.ElementAt(i)) sloganAlphabetsList[i] += (!sloganAlphabetsList[i].Contains(letter)) ? letter.ToString() : String.Empty;

            return sloganAlphabetsList;
        }

        public abstract class Caesar
        {
            public static string About()
            {
                return "Каждая буква входного слова смещается на указанное в ключе число позиций" + Environment.NewLine +
                       "Неалфавитные символы в слове не преобразуются" + Environment.NewLine +
                       "Неалфавитные символы в ключе игнорируются";
            }

            public static string Encode(string inputWord, int key)
            {
                string outputWord = String.Empty;
                foreach (var letter in inputWord)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(letter)))
                        outputWord += letter;
                    else foreach (var alphabet in alphabetsList)
                    {
                        if (alphabet.Contains(letter))
                        {
                            outputWord += getLetter(getNumber(letter, alphabetsList) + key, alphabet);
                            continue;
                        }
                    }
                }
                return outputWord;
            }

            public static string Decode(string inputWord, int key)
            {
                string outputWord = String.Empty;
                foreach (var letter in inputWord)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(letter)))
                        outputWord += letter;
                    else foreach (var alphabet in alphabetsList)
                    {
                        if (alphabet.Contains(letter))
                        {
                            outputWord += getLetter(getNumber(letter, alphabetsList) - key, alphabet);
                            continue;
                        }
                    }
                }
                return outputWord;
            }
        }

        public abstract class Atbash
        {
            public static string About()
            {
                return "Каждая буква входного слова заменяется на букву обратного алфавита в той же позиции" + Environment.NewLine +
                       "Неалфавитные символы в слове не преобразуются" + Environment.NewLine +
                       "Ключ не используется";
            }

            public static string Encode(string inputWord)
            {
                string outputWord = String.Empty;
                foreach (var letter in inputWord)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(letter)))
                        outputWord += letter;
                    else foreach (var alphabet in alphabetsList)
                    {
                        if (alphabet.Contains(letter))
                        {
                            outputWord += getLetter(alphabet.Length - getNumber(letter, alphabetsList) - 1, alphabet);
                            continue;
                        }
                    }

                }
                return outputWord;
            }

            public static string Decode(string word)
            {
                return Encode(word);
            }
        }

        public abstract class Motto
        {
            public static string About()
            {
                return "В соответствие каждой букве алфавита ставится буква измененного в соответствии с лозунгом нового алфавита" + Environment.NewLine +
                       "Например, для слова \"информатика\" алфавит будет \"информаткбвгдеёжзл...эюя\"" + Environment.NewLine +
                       "Неалфавитные символы выдают ошибку";
            }

            private static List<string> sloganAlphabets;    
            
            public static string Encode(string inputWord, string slogan)
            {
                string outputWord = String.Empty;
                sloganAlphabets = GenerateSloganAlphabet(slogan);
                foreach (var letter in inputWord)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(letter)))
                        outputWord += letter;
                    else                    
                        foreach (var alphabet in alphabetsList)
                            if (alphabet.Contains(letter))
                                outputWord += sloganAlphabets.ElementAt(alphabetsList.IndexOf(alphabet)).ElementAt(alphabet.IndexOf(letter));                    
                }
                return outputWord;
            }
            
            public static string Decode(string inputWord, string slogan)
            {
                string outputWord = String.Empty;
                sloganAlphabets = GenerateSloganAlphabet(slogan);
                foreach (var letter in inputWord)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(letter)))
                        outputWord += letter;
                    else
                        foreach (var sloganAlphabet in sloganAlphabets)
                            if (sloganAlphabet.Contains(letter))
                                outputWord += alphabetsList.ElementAt(sloganAlphabets.IndexOf(sloganAlphabet)).ElementAt(sloganAlphabet.IndexOf(letter));
                }
                return outputWord;
            }
        }

        public abstract class TrisemusSystem
        {
            public static string About()
            {
                return "Алфавит записывается в квадрат и каждая буква входного слова заменяется на букву того же столбца, но на один ряд ниже" + Environment.NewLine +
                       "Если ниже буквы отсутствуют, берется буква из первого ряда" + Environment.NewLine +
                       "Неалфавитные символы выдают ошибку";
            }

            private static List<string> sloganAlphabets;
            public static string Encode(string inputWord, string slogan)
            {
                const int N = 6;
                string outputWord = String.Empty;
                sloganAlphabets = GenerateSloganAlphabet(slogan);
                foreach (var letter in inputWord)
                {
                    if (!sloganAlphabets.Any(sloganAlphabet => sloganAlphabet.Contains(letter)))
                        outputWord += letter;
                    else {
                        foreach (var sloganAlphabet in sloganAlphabets)
                        {
                            if (sloganAlphabet.Contains(letter))
                            {
                                if (getNumber(letter, sloganAlphabets) < sloganAlphabet.Length - N)
                                    outputWord += getLetter(getNumber(letter, sloganAlphabets) + N, sloganAlphabet);
                                else
                                    outputWord += getLetter(getNumber(letter, sloganAlphabets) % N, sloganAlphabet);
                            }
                        }
                    }
                }
                return outputWord;
            }

            public static string Decode(string inputWord, string slogan)
            {
                const int N = 6;
                string outputWord = String.Empty;
                sloganAlphabets = GenerateSloganAlphabet(slogan);
                foreach (var letter in inputWord)
                {
                    if (!sloganAlphabets.Any(sloganAlphabet => sloganAlphabet.Contains(letter)))
                        outputWord += letter;
                    else
                    {
                        foreach (var sloganAlphabet in sloganAlphabets)
                        {
                            if (sloganAlphabet.Contains(letter))
                            {
                                if (getNumber(letter, sloganAlphabets) < N)
                                {
                                    if (getNumber(letter, sloganAlphabets) < sloganAlphabet.Length % N)
                                        outputWord += getLetter(sloganAlphabet.Length - sloganAlphabet.Length % N + getNumber(letter, sloganAlphabets), sloganAlphabet);
                                    else
                                        outputWord += getLetter(sloganAlphabet.Length - sloganAlphabet.Length % N - N + getNumber(letter, sloganAlphabets), sloganAlphabet);
                                }
                                else
                                    outputWord += getLetter(getNumber(letter, sloganAlphabets) - N, sloganAlphabet);
                            }
                        }
                    }
                }
                return outputWord;
            }
        }

        public abstract class TrisemusTable
        {
            public static string About()
            {
                return "Каждая буква алфавита смещается на количество позиций, равное ее индексу во входном слове" + Environment.NewLine +
                       "Неалфавитные символы в слове не преобразуются";
            }

            public static string Encode(string inputWord)
            {
                string outputWord = String.Empty;
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(inputWord.ElementAt(i))))
                        outputWord += inputWord.ElementAt(i);
                    foreach (var alphabet in alphabetsList)
                    {
                        if (alphabet.Contains(inputWord.ElementAt(i)))
                            outputWord += getLetter(getNumber(inputWord.ElementAt(i), alphabetsList) + i, alphabet);
                    }
                }
                return outputWord;
            }

            public static string Decode(string inputWord)
            {
                string outputWord = String.Empty;
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (!alphabetsList.Any(alphabet => alphabet.Contains(inputWord.ElementAt(i))))
                        outputWord += inputWord.ElementAt(i);
                    foreach (var alphabet in alphabetsList)
                    {
                        if (alphabet.Contains(inputWord.ElementAt(i)))
                            outputWord += getLetter(getNumber(inputWord.ElementAt(i), alphabetsList) - i, alphabet);
                    }
                }
                return outputWord;
            }
        }

        public abstract class PolybianSquare
        {
            public static string About()
            {
                return "Алфавит записывается в квадрат и каждая буква входного слова заменяется на два символа - координаты буквы в квадрате" + Environment.NewLine +
                       "Нецифровые символы выдают ошибку" + Environment.NewLine +
                       "Нечетное количество цифр выдает ошибку";
            }

            public static string Encode(string inputWord, bool language)
            {
                string outputWord = String.Empty;
                foreach (var letter in inputWord)
                {
                    if (language == false)
                    {
                        if (alphabetsList[0].Contains(letter) || alphabetsList[1].Contains(letter))
                            outputWord += (alphabetsList[1].IndexOf(Char.ToLower(letter)) / 6).ToString() + (alphabetsList[1].IndexOf(Char.ToLower(letter)) % 6).ToString();
                        else
                            outputWord += letter;
                    } else {
                        if (alphabetsList[2].Contains(letter) || alphabetsList[3].Contains(letter))
                            outputWord += (alphabetsList[3].IndexOf(Char.ToLower(letter)) / 6).ToString() + (alphabetsList[3].IndexOf(Char.ToLower(letter)) % 6).ToString();
                        else
                            outputWord += letter;
                    }
                }
                return outputWord;
            }
            
            public static string Decode(string inputWord, bool language)
            {
                string outputWord = String.Empty;
                if (inputWord.Count(ch => Char.IsDigit(ch)) % 2 == 1)
                    return errorString;
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (Char.IsDigit(inputWord[i]))
                    {
                        if (Char.IsDigit(inputWord[i + 1]))
                        {                            
                            if (language == false)
                            {
                                if ((int)Char.GetNumericValue(inputWord[i]) * 6 + (int)Char.GetNumericValue(inputWord[i + 1]) >= alphabetsList[1].Length)
                                    return errorString;
                                else 
                                    outputWord += alphabetsList[1].ElementAt((int)Char.GetNumericValue(inputWord[i]) * 6 + (int)Char.GetNumericValue(inputWord[i + 1]));

                            } else {
                                if ((int)Char.GetNumericValue(inputWord[i]) * 6 + (int)Char.GetNumericValue(inputWord[i + 1]) >= alphabetsList[3].Length)
                                    return errorString;
                                else
                                    outputWord += alphabetsList[3].ElementAt((int)Char.GetNumericValue(inputWord[i]) * 6 + (int)Char.GetNumericValue(inputWord[i + 1]));
                            }
                            i++;
                        }
                        else
                            return errorString;
                    } else {
                        outputWord += inputWord[i];
                    }
                }
                return outputWord;
            }
        }

        public abstract class Vigenere
        {
            public static string About()
            {
                return "Каждая буква входного слова смещается на количество позиций, равное индексу ключа" + Environment.NewLine +
                       "Если ниже буквы отсутствуют, берется буква из первого ряда" + Environment.NewLine +
                       "Недостаточная длина ключа восполняется повтором оного" + Environment.NewLine +
                       "Излишняя длина ключа игнорируется" + Environment.NewLine +
                       "Неалфавитные символы выдают ошибку";
            }

            public static string Encode(string inputWord, string key)
            {
                string outputWord = String.Empty;
                string newKey = String.Empty;
                while (newKey.Length < inputWord.Length) newKey += key;
                newKey = newKey.Substring(0, inputWord.Length);
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (!alphabetsList.Any(str => str.Contains(inputWord[i])))
                        outputWord += inputWord[i].ToString();
                    else
                    {
                        foreach (var alphabet in alphabetsList)
                        {
                            if (alphabet.Contains(inputWord[i]))
                            {
                                if (alphabet.Contains(Char.ToLower(newKey[i])))
                                {
                                    outputWord += getLetter(alphabet.IndexOf(inputWord[i]) + 1 + alphabet.IndexOf((Char.ToLower(newKey[i]))), alphabet);
                                }
                                else if (alphabet.Contains(Char.ToUpper(newKey[i])))
                                {
                                    outputWord += getLetter(alphabet.IndexOf(inputWord[i]) + 1 + alphabet.IndexOf((Char.ToUpper(newKey[i]))), alphabet);
                                }
                                else
                                    return errorString;
                            }
                        }
                    }
                }
                return outputWord;
            }

            public static string Decode(string inputWord, string key)
            {
                string outputWord = String.Empty;
                string newKey = String.Empty;
                while (newKey.Length < inputWord.Length) newKey += key;
                newKey = newKey.Substring(0, inputWord.Length);
                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (!alphabetsList.Any(str => str.Contains(inputWord[i])))
                        outputWord += inputWord[i].ToString();
                    else
                    {
                        foreach (var alphabet in alphabetsList)
                        {
                            if (alphabet.Contains(inputWord[i]))
                            {
                                if (alphabet.Contains(Char.ToLower(newKey[i])))
                                {
                                    outputWord += getLetter(alphabet.IndexOf(inputWord[i]) - alphabet.IndexOf((Char.ToLower(newKey[i]))) - 1, alphabet);
                                }
                                else if (alphabet.Contains(Char.ToUpper(newKey[i])))
                                {
                                    outputWord += getLetter(alphabet.IndexOf(inputWord[i]) - alphabet.IndexOf((Char.ToUpper(newKey[i]))) - 1, alphabet);
                                }
                                else
                                    return errorString;
                            }
                        }
                    }
                }
                return outputWord;
            }
        }

        public abstract class Playfair
        {
            public static string About()
            {
                return "Входное слово разделяется на биграммы, которые шифруются в алфавитной матрице по правилам:" + Environment.NewLine +
                       "1. Буквы в одном ряду заменяются на буквы справа от них" + Environment.NewLine +
                       "2. Буквы в одном столбце заменяются на буквы снизу от них" + Environment.NewLine +
                       "3. Буквы в разных рядах и столбцах заменяются на буквы в своем ряду, но в столбце второй буквы" + Environment.NewLine +
                       "Неалфавитные символы и нечетность расшифровываемого сообщения выдают ошибку";
            }

            private static int rows;
            private static int columns;

            private static int getRow(char letter, string matrix) { return matrix.IndexOf(letter) / columns; }
            private static int getCol(char letter, string matrix) { return matrix.IndexOf(letter) % columns; }
            private static char getLetter(int row, int col, string matrix) { return matrix[row * columns + col]; }

            public static string Encode(string inputWord, string key, bool language)
            {
                string outputWord = String.Empty;
                string formattedWord = inputWord.ToUpper();
                List<string> matrix = GenerateSloganAlphabet(key);
                matrix.RemoveAt(3);
                matrix.RemoveAt(1);
                matrix[0] = matrix[0].Remove(matrix[0].IndexOf("Ё"), 1);
                matrix[1] = matrix[1].Remove(matrix[1].IndexOf("I"), 1);

                int lang = (language == false) ? 0 : 1;
                rows = (lang == 0) ? 4 : 5;
                columns = (lang == 0) ? 8 : 5;
                char separator = (lang == 0) ? 'Я' : 'Z';

                if (!inputWord.All(ch => matrix[lang].Contains(Char.ToUpper(ch)))) return symbolsErrorInputString;

                for (int i = 0; i < formattedWord.Length; i += 2)
                    if (i == formattedWord.Length - 1)
                        formattedWord += separator;
                    else
                        if (formattedWord[i] == formattedWord[i + 1])
                        formattedWord = formattedWord.Insert(i + 1, separator.ToString());

                for (int i = 0; i < formattedWord.Length; i += 2) {
                    if (getRow(formattedWord[i], matrix[lang]) == getRow(formattedWord[i + 1], matrix[lang])) {
                        outputWord += (getCol(formattedWord[i], matrix[lang]) + 1 == columns) ?
                            getLetter(getRow(formattedWord[i], matrix[lang]), 0, matrix[lang]) :
                            getLetter(getRow(formattedWord[i], matrix[lang]), getCol(formattedWord[i], matrix[lang]) + 1, matrix[lang]);
                        outputWord += (getCol(formattedWord[i + 1], matrix[lang]) + 1 == columns) ?
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]), 0, matrix[lang]) :
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]), getCol(formattedWord[i + 1], matrix[lang]) + 1, matrix[lang]);
                    } else if (getCol(formattedWord[i], matrix[lang]) == getCol(formattedWord[i + 1], matrix[lang])) {
                        outputWord += (getRow(formattedWord[i], matrix[lang]) + 1 == rows) ?
                            getLetter(0, getCol(formattedWord[i], matrix[lang]) + 1, matrix[lang]) :
                            getLetter(getRow(formattedWord[i], matrix[lang]) + 1, getCol(formattedWord[i], matrix[lang]), matrix[lang]);
                        outputWord += (getRow(formattedWord[i + 1], matrix[lang]) + 1 == rows) ?
                            getLetter(0, getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]) :
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]) + 1, getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]);
                    } else {
                        outputWord += getLetter(getRow(formattedWord[i], matrix[lang]), getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]);
                        outputWord += getLetter(getRow(formattedWord[i + 1], matrix[lang]), getCol(formattedWord[i], matrix[lang]), matrix[lang]);
                    }
                }
                return outputWord;
            }

            public static string Decode(string inputWord, string key, bool language)
            {
                string outputWord = String.Empty;
                string formattedWord = inputWord.ToUpper();
                List<string> matrix = GenerateSloganAlphabet(key);
                matrix.RemoveAt(3);
                matrix.RemoveAt(1);
                matrix[0] = matrix[0].Remove(matrix[0].IndexOf("Ё"), 1);
                matrix[1] = matrix[1].Remove(matrix[1].IndexOf("I"), 1);

                int lang = (language == false) ? 0 : 1;
                rows = (lang == 0) ? 4 : 5;
                columns = (lang == 0) ? 8 : 5;
                char separator = (lang == 0) ? 'Я' : 'Z';

                if (!inputWord.All(ch => matrix[lang].Contains(Char.ToUpper(ch)))) return symbolsErrorInputString;
                if (inputWord.Length % 2 == 1) return parityErrorInputString;

                for (int i = 0; i < formattedWord.Length; i += 2)
                {
                    if (getRow(formattedWord[i], matrix[lang]) == getRow(formattedWord[i + 1], matrix[lang]))
                    {
                        outputWord += (getCol(formattedWord[i], matrix[lang]) - 1 < 0) ?
                            getLetter(getRow(formattedWord[i], matrix[lang]), columns - 1, matrix[lang]) :
                            getLetter(getRow(formattedWord[i], matrix[lang]), getCol(formattedWord[i], matrix[lang]) - 1, matrix[lang]);
                        outputWord += (getCol(formattedWord[i + 1], matrix[lang]) - 1 < 0) ?
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]), columns - 1, matrix[lang]) :
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]), getCol(formattedWord[i + 1], matrix[lang]) - 1, matrix[lang]);
                    }
                    else if (getCol(formattedWord[i], matrix[lang]) == getCol(formattedWord[i + 1], matrix[lang]))
                    {
                        outputWord += (getRow(formattedWord[i], matrix[lang]) - 1 < 0) ?
                            getLetter(rows - 1, getCol(formattedWord[i], matrix[lang]) + 1, matrix[lang]) :
                            getLetter(getRow(formattedWord[i], matrix[lang]) - 1, getCol(formattedWord[i], matrix[lang]), matrix[lang]);
                        outputWord += (getRow(formattedWord[i + 1], matrix[lang]) - 1 < 0) ?
                            getLetter(rows - 1, getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]) :
                            getLetter(getRow(formattedWord[i + 1], matrix[lang]) - 1, getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]);
                    }
                    else
                    {
                        outputWord += getLetter(getRow(formattedWord[i], matrix[lang]), getCol(formattedWord[i + 1], matrix[lang]), matrix[lang]);
                        outputWord += getLetter(getRow(formattedWord[i + 1], matrix[lang]), getCol(formattedWord[i], matrix[lang]), matrix[lang]);
                    }
                }
                return outputWord;
            }
        }

        public abstract class Gamming
        {
            public static string About()
            {
                return "Каждая буква входного слова заменяется на букву с индексом, равным сумме по модулю N индексов буквы слова и ключа" + Environment.NewLine +
                       "Недостаточная длина ключа восполняется повтором оного" + Environment.NewLine +
                       "Излишняя длина ключа игнорируется" + Environment.NewLine +
                       "Неалфавитные символы выдают ошибку";
            }

            private static int ModularAddition(int a, int b, int module) { return (a + b) % module; }

            public static string Encode(string inputWord, string key, bool language, List<string> newAlphabet)
            {
                string outputWord = String.Empty;

                string newKey = String.Empty;
                while (newKey.Length < inputWord.Length) newKey += key;
                newKey = newKey.Substring(0, inputWord.Length);

                int lang = (language == false) ? 0 : 2;

                foreach (var letter in inputWord)
                    if (!newAlphabet[lang].Contains(Char.ToUpper(letter)))
                        return symbolsErrorInputString;

                for (int i = 0; i < inputWord.Length; i++) {
                    if (newAlphabet[lang].Contains(inputWord[i])) {
                        outputWord += getLetter(ModularAddition(newAlphabet[lang].IndexOf(inputWord[i]) + 1, 
                                                                newAlphabet[lang].IndexOf(Char.ToUpper(newKey[i])), 
                                                                newAlphabet[lang].Length), 
                                                newAlphabet[lang]);
                    } else if (newAlphabet[lang + 1].Contains(inputWord[i])) {
                        outputWord += getLetter(ModularAddition(newAlphabet[lang + 1].IndexOf(inputWord[i]) + 1,
                                                                newAlphabet[lang].IndexOf(Char.ToUpper(newKey[i])),
                                                                newAlphabet[lang].Length),
                                                newAlphabet[lang + 1]);
                    } else
                        return symbolsErrorInputString;
                }

                return outputWord;
            }

            public static string Decode(string inputWord, string key, bool language, List<string> newAlphabet)
            {
                string outputWord = String.Empty;

                string newKey = String.Empty;
                while (newKey.Length < inputWord.Length) newKey += key;
                newKey = newKey.Substring(0, inputWord.Length);

                int lang = (language == false) ? 0 : 2;

                foreach (var letter in inputWord)
                    if (!newAlphabet[lang].Contains(Char.ToUpper(letter)))
                        return symbolsErrorInputString;

                for (int i = 0; i < inputWord.Length; i++)
                {
                    if (newAlphabet[lang + 1].Contains(inputWord[i]))
                    {
                        outputWord += getLetter(ModularAddition(newAlphabet[lang + 1].IndexOf(inputWord[i]) - newAlphabet[lang].IndexOf(Char.ToUpper(newKey[i])),
                                                                newAlphabet[lang].Length,
                                                                newAlphabet[lang].Length) - 1,
                                                newAlphabet[lang + 1]);
                    } else if (newAlphabet[lang].Contains(inputWord[i])) {
                        outputWord += getLetter(ModularAddition(newAlphabet[lang].IndexOf(inputWord[i]) - newAlphabet[lang].IndexOf(Char.ToUpper(newKey[i])),
                                                                newAlphabet[lang].Length,
                                                                newAlphabet[lang].Length) - 1,
                                                newAlphabet[lang]);
                    } else
                        return symbolsErrorInputString;
                }

                return outputWord;
            }
        }

        public abstract class BlockTransposing
        {
            public static string About()
            {
                return "Каждый блок символов, равный длине ключа, меняет порядок в соответствии с ним" + Environment.NewLine +
                       "Шифруются любые символы" + Environment.NewLine +
                       "Недостаточная длина ключа восполняется повтором оного" + Environment.NewLine +
                       "Излишняя длина ключа игнорируется" + Environment.NewLine +
                       "Нецифровые символы ключа выдают ошибку";
            }

            public static string Encode(string inputWord, string key)
            {
                List<string> list = key.Split(' ').ToList();
                list.RemoveAll(ch => ch.Contains(' '));
                list = list.Where(ch => !string.IsNullOrWhiteSpace(ch)).ToList();

                string formattedWord = inputWord;
                while (formattedWord.Length % list.Count != 0)
                    formattedWord += "~";

                string output = String.Empty;

                for (int i = 0; i < formattedWord.Length; i += list.Count)
                    for (int j = 0; j < list.Count; j++)
                        output += formattedWord.ElementAt(i + Convert.ToInt32(list[j]) - 1);

                return output;
            }

            public static string Decode(string word, string key)
            {
                return Encode(word, key).Replace("~", "");
            }
        }

        public abstract class SingleTransposing
        {
            public static string About()
            {
                return "Порядок символов в исходном слове формируются в соответствии с порядком в ключе" + Environment.NewLine +
                       "Шифруются любые символы" + Environment.NewLine +
                       "Нецифровые символы ключа выдают ошибку";
            }

            public static string Encode(string word, string key)
            {
                string output = String.Empty;
                List<string> list = key.Split(' ').ToList();
                list.Remove("");
                foreach (var letter in list)
                    output += word.ElementAt(Convert.ToInt32(letter) - 1);
                return output;
            }

            public static string Decode(string word, string key)
            {
                string output = String.Empty;
                List<string> list = key.Split(' ').ToList();
                list.Remove("");

                for (int i = 1; i <= list.Count; i++)
                    output += word.ElementAt(list.IndexOf(i.ToString()));                

                return output;
            }
        }

        public abstract class VerticalTransposing
        {
            public static string About()
            {
                return "Помогите";
            }

            public static string Encode(string word, string key, bool language)
            {
                int lang = (language == false) ? 0 : 2;
                string output = String.Empty;
                string formattedWord = word;
                while (formattedWord.Length % key.Length != 0) formattedWord += " ";
                int[] code = new int[key.Length];
                int numerator = 1;
                foreach (var letter in alphabetsList[lang])
                    for (int i = 0; i < key.Length; i++)
                        if (Char.ToUpper(key[i]) == letter)
                            code[i] = numerator++;
                List<int> codeList = new List<int>();
                for (int i = 0; i < key.Length; i++) codeList.Add(code[i]);
                for (int i = 1; i <= key.Length; i++)
                    for (int j = codeList.IndexOf(i); j < formattedWord.Length; j += key.Length)
                        output += formattedWord[j];
                return output;
            }

            public static string Decode(string word, string key, bool language)
            {
                int lang = (language == false) ? 0 : 2;
                string output = String.Empty;
                string formattedWord = word;
                while (formattedWord.Length % key.Length != 0) formattedWord += " ";
                int[] code = new int[key.Length];
                int numerator = 1;
                foreach (var letter in alphabetsList[lang])
                    for (int i = 0; i < key.Length; i++)
                        if (Char.ToUpper(key[i]) == letter)
                            code[i] = numerator++;
                List<int> codeList = new List<int>();
                for (int i = 0; i < key.Length; i++) codeList.Add(code[i]);

                for (int i = 0; i < formattedWord.Length / key.Length; i++)
                {
                    for (int j = 0; j < codeList.Count; j++)
                    {
                        output += formattedWord[codeList.IndexOf(j + 1) * formattedWord.Length / key.Length + i];
                    }
                }
                return output;
            }
        }

        public abstract class MultiTransposing
        {
            public static string About()
            {
                return "О данном методе";
            }

            public static string Encode(string inputWord, string horizontalKey, string verticalKey)
            {
                string outputWord = String.Empty;
                string formattedWord = inputWord;
                while (formattedWord.Length < horizontalKey.Length * verticalKey.Length)
                    formattedWord += " ";
                for (int i = 1; i <= horizontalKey.Length; i++)
                    for (int j = 1; j <= verticalKey.Length; j++)
                        outputWord += formattedWord[horizontalKey.Length * verticalKey.IndexOf(j.ToString()) + horizontalKey.IndexOf(i.ToString())];
                return outputWord;
            }

            public static string Decode(string word, string horizontalKey, string verticalKey)
            {
                string outputWord = String.Empty;
                string formattedWord = word;
                while (formattedWord.Length < horizontalKey.Length * verticalKey.Length)
                    formattedWord += " ";
                for (int i = 0; i < verticalKey.Length; i++)
                    for (int j = 0; j < horizontalKey.Length; j++)
                        outputWord += formattedWord[((int)Char.GetNumericValue(horizontalKey[j]) - 1) * verticalKey.Length + (int)Char.GetNumericValue(verticalKey[i]) - 1];
                return outputWord;
            }
        }

        public abstract class Gost
        {
            public static string About()
            {
                return "Ключ = 32 символа";
            }

            public struct FileWork
            {
                public byte[] FileToByte(string name)
                {
                    try
                    {
                        return File.ReadAllBytes(name);
                    } catch {
                        return null;
                    }
                }

                public void WriteToFile(string name, byte[] fl) {
                    FileStream file = null;

                    try
                    {
                        file = new FileStream(name, FileMode.Create);
                        file.Write(fl, 0, fl.Length);
                    }
                    catch (IOException exc)
                    {
                        MessageBox.Show("Ошибка ввода-вывода: " + exc.Message);
                    }
                    catch { }
                    finally
                    {
                        if (file != null)
                            file.Close();
                    }
                }
            }

            public abstract class ReplacementTab
            {
                internal static byte[] getTable { get; } = {
                        0x4, 0x2, 0xF, 0x5, 0x9, 0x1, 0x0, 0x8, 0xE, 0x3, 0xB, 0xC, 0xD, 0x7, 0xA, 0x6,
                        0xC, 0x9, 0xF, 0xE, 0x8, 0x1, 0x3, 0xA, 0x2, 0x7, 0x4, 0xD, 0x6, 0x0, 0xB, 0x5,
                        0xD, 0x8, 0xE, 0xC, 0x7, 0x3, 0x9, 0xA, 0x1, 0x5, 0x2, 0x4, 0x6, 0xF, 0x0, 0xB,
                        0xE, 0x9, 0xB, 0x2, 0x5, 0xF, 0x7, 0x1, 0x0, 0xD, 0xC, 0x6, 0xA, 0x4, 0x3, 0x8,
                        0x3, 0xE, 0x5, 0x9, 0x6, 0x8, 0x0, 0xD, 0xA, 0xB, 0x7, 0xC, 0x2, 0x1, 0xF, 0x4,
                        0x8, 0xF, 0x6, 0xB, 0x1, 0x9, 0xC, 0x5, 0xD, 0x3, 0x7, 0xA, 0x0, 0xE, 0x2, 0x4,
                        0x9, 0xB, 0xC, 0x0, 0x3, 0x6, 0x7, 0x5, 0x4, 0x8, 0xE, 0xF, 0x1, 0xA, 0x2, 0xD,
                        0xC, 0x6, 0x5, 0x2, 0xB, 0x0, 0x9, 0xD, 0x3, 0xE, 0x7, 0xA, 0xF, 0x4, 0x1, 0x8 };
            }

            public class BasicStep
            {
                uint N1, N2, X;

                internal BasicStep(ulong dateFragment, uint keyFragment)
                {
                    N1 = (uint)(dateFragment >> 32);
                    N2 = (uint)((dateFragment << 32) >> 32);
                    X = keyFragment;
                }

                internal ulong BasicEncrypt(bool IsLastStep) { return (FourthAndFifthStep(IsLastStep, ThirdStep(SecondStep(FirstStep())))); }

                private uint FirstStep() { return (uint)((X + N1) % (Convert.ToUInt64(Math.Pow(2, 32)))); }

                private uint SecondStep(uint S)
                {
                    uint newS, S0, S1, S2, S3, S4, S5, S6, S7;

                    S0 = S >> 28;
                    S1 = (S << 4) >> 28;
                    S2 = (S << 8) >> 28;
                    S3 = (S << 12) >> 28;
                    S4 = (S << 16) >> 28;
                    S5 = (S << 20) >> 28;
                    S6 = (S << 24) >> 28;
                    S7 = (S << 28) >> 28;

                    S0 = ReplacementTab.getTable[S0];
                    S1 = ReplacementTab.getTable[0x10 + S1];
                    S2 = ReplacementTab.getTable[0x20 + S2];
                    S3 = ReplacementTab.getTable[0x30 + S3];
                    S4 = ReplacementTab.getTable[0x40 + S4];
                    S5 = ReplacementTab.getTable[0x50 + S5];
                    S6 = ReplacementTab.getTable[0x60 + S6];
                    S7 = ReplacementTab.getTable[0x70 + S7];

                    newS = S7 + (S6 << 4) + (S5 << 8) + (S4 << 12) + (S3 << 16) +
                            (S2 << 20) + (S1 << 24) + (S0 << 28);
                    return newS;
                }

                private uint ThirdStep(uint S)
                {
                    return (uint)(S << 11) | (S >> 21);
                }

                private ulong FourthAndFifthStep(bool IsLastStep, uint S)
                {
                    ulong N;
                    S = (S ^ N2);
                    if (!IsLastStep)
                    {
                        N2 = N1;
                        N1 = S;
                    } else
                        N2 = S;
                    N = ((ulong)N2) | (((ulong)N1) << 32);
                    return N;
                }
            }

            public abstract class Converter
            {
                internal static byte[] ConvertToByte(ulong[] fl)
                {
                    byte[] temp = new byte[8];
                    byte[] encrByteFile = new byte[fl.Length * 8];

                    for (int i = 0; i < fl.Length; i++)
                    {
                        temp = BitConverter.GetBytes(fl[i]);
                        for (int j = 0; j < temp.Length; j++)
                            encrByteFile[j + i * 8] = temp[j];
                    }
                    return encrByteFile;
                }

                internal static byte[] ConvertToByte(uint[] fl)
                {
                    byte[] temp = new byte[4];
                    byte[] encrByteFile = new byte[fl.Length * 4];
                    for (int i = 0; i < fl.Length; i++)
                    {
                        temp = BitConverter.GetBytes(fl[i]);
                        for (int j = 0; j < temp.Length; j++)
                            encrByteFile[j + i * 4] = temp[j];
                    }
                    return encrByteFile;
                }

                internal static uint[] GetUIntKeyArray(byte[] byteKey)
                {
                    uint[] key = new uint[byteKey.Length / 4];
                    for (int i = 0; i < key.Length; i++)
                        key[i] = BitConverter.ToUInt32(byteKey, i * 4);
                    return key;
                }

                internal static ulong[] GetULongDataArray(byte[] byteData)
                {
                    ulong[] data = new ulong[byteData.Length / 8];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = BitConverter.ToUInt64(byteData, i * 8);
                    return data;
                }
            }

            public abstract class D32 : Converter
            {
                private static byte[] decrByteFile;
                private static uint[] uintKey;
                private static ulong[] ulongFile;

                public static byte[] Decrypt(string word, string key)
                {
                    byte[] byteKey = Encoding.Default.GetBytes(key);
                    if (word == String.Empty)
                        return Encoding.Default.GetBytes("Введите слово");
                    if ((byteKey == null) || (byteKey.Length != 32))
                        return Encoding.Default.GetBytes("Ключ должен содержать 32 символа");
                    byte[] btFile = Encoding.Default.GetBytes(word.PadRight(64, ' '));
                    uintKey = GetUIntKeyArray(byteKey);
                    ulongFile = GetULongDataArray(btFile);
                    decrByteFile = ConvertToByte(DecryptFile());
                    return decrByteFile;  
                }

                internal byte[] GetDecryptFile { get { return decrByteFile; } }

                private static ulong[] DecryptFile()
                {
                    BasicStep[] K = new BasicStep[8];
                    ulong[] ulongDecrFile = new ulong[ulongFile.Length];

                    for (int k = 0; k < ulongFile.Length; k++)
                    {
                        ulongDecrFile[k] = ulongFile[k];

                        for (int i = 0; i < K.Length; i++)
                        {
                            K[i] = new BasicStep(ulongDecrFile[k], uintKey[i]);
                            ulongDecrFile[k] = K[i].BasicEncrypt(false);
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = K.Length - 1; i >= 0; i--)
                            {
                                K[i] = new BasicStep(ulongDecrFile[k], uintKey[i]);
                                ulongDecrFile[k] = ((j == 2) && (i == 0)) ?
                                    K[i].BasicEncrypt(true) : 
                                    K[i].BasicEncrypt(false);
                            }
                        }
                    }
                    return ulongDecrFile;
                }
            }

            public abstract class E32 : Converter
            {
                private static byte[] encrByteFile;
                private static uint[] uintKey;
                private static ulong[] ulongFile;

                public static byte[] Encrypt(string word, string key)
                {
                    byte[] byteKey = Encoding.Default.GetBytes(key);
                    if (word == String.Empty)
                        return Encoding.Default.GetBytes("Введите слово");
                    if ((byteKey == null) || (byteKey.Length != 32))
                        return Encoding.Default.GetBytes("Ключ должен содержать 32 символа");
                    byte[] btFile = Encoding.Default.GetBytes(word.PadRight(64, ' '));
                    uintKey = GetUIntKeyArray(byteKey);
                    ulongFile = GetULongDataArray(btFile);
                    encrByteFile = ConvertToByte(EncryptFile());
                    return encrByteFile;
                }

                internal byte[] GetEncryptFile { get { return encrByteFile; } }

                private static ulong[] EncryptFile()
                {
                    BasicStep[] K = new BasicStep[8];
                    ulong[] ulongEncrFile = new ulong[ulongFile.Length];

                    for (int k = 0; k < ulongFile.Length; k++)
                    {
                        ulongEncrFile[k] = ulongFile[k];

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < K.Length; i++)
                            {
                                K[i] = new BasicStep(ulongEncrFile[k], uintKey[i]);
                                ulongEncrFile[k] = K[i].BasicEncrypt(false);
                            }
                        }

                        for (int i = K.Length - 1; i >= 0; i--)
                        {
                            K[i] = new BasicStep(ulongEncrFile[k], uintKey[i]);
                            ulongEncrFile[k] = (i != 0) ? 
                                K[i].BasicEncrypt(false) : 
                                K[i].BasicEncrypt(true);
                        }
                    }
                    return ulongEncrFile;
                }
            }
        }
    }

    public partial class MainWindow : Window
    {
        private static readonly List<string> alphabetsList = new List<string>() { "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ",
                                                                                  "абвгдеёжзийклмнопрстуфхцчшщъыьэюя",
                                                                                  "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                                                                                  "abcdefghijklmnopqrstuvwxyz" };
        List<string> newAlphabetsList = alphabetsList;
        InputWindow inputWindow;
        private enum Languages
        {
            Russian = 0,
            English = 1
        };
        private bool flag = false; // 0 - encode, 1 - decode
        private Languages currentLanguage = Languages.Russian; // 0 - RUS, 1 - ENG        
        private static readonly string symbolsErrorKeyString = "Проверьте наличие внеалфавитных символов в ключе";
        private int columnsOfTransposion;
        public MainWindow()
        {
            InitializeComponent();
            SetLocale(Languages.Russian);
            columnsOfTransposion = 0;
            inputWindow = new InputWindow();
            inputWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputWindow.Topmost = true;
            inputBox.TextWrapping = TextWrapping.Wrap;
            outputBox.TextWrapping = TextWrapping.Wrap;
            keyBox.TextWrapping = TextWrapping.Wrap;
            comboBox.Items.Add("Метод Цезаря");                         // работает (1)
            comboBox.Items.Add("Метод Атбаш");                          // работает (2)
            comboBox.Items.Add("Лозунговый метод");                     // работает (3)
            comboBox.Items.Add("Квадрат Полибия");                      // работает (4)
            comboBox.Items.Add("Система Трисемуса");                    // работает (5)
            comboBox.Items.Add("Таблица Трисемуса");                    // работает (6)
            comboBox.Items.Add("Метод Плейфера");                       // работает (7)
            comboBox.Items.Add("Метод Вижинера");                       // работает (8)
            comboBox.Items.Add("Простой перестановки");                 // работает (9)
            comboBox.Items.Add("Блочной перестановки");                 // работает (10)
            comboBox.Items.Add("Вертикальной перестановки");            // работает (11)
            comboBox.Items.Add("Множественной перестановки");           // протестировать (12)
            comboBox.Items.Add("ГОСТ 28147-89");                        // работает (15)
            comboBox.Items.Add("Гаммирование");                         // работает (14)
            comboBox.SelectedIndex = 0;
            modeTextBlock.Text = "🔒" + Environment.NewLine + "ШИФРОВАНИЕ";
            title.Text = comboBox.SelectedItem.ToString().ToUpper();
            for (int i = 0; i < newAlphabetsList.Count; i++) newAlphabetsList[i] += (" .,!?-");
            Start();
        }

        private void SetLocale(Languages language)
        {
            currentLanguage = language;
            languageTextBlock.Text = (language == Languages.English) ? "EN" : "RU";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = (ComboBox)sender;
            title.Text = a.SelectedItem.ToString().ToUpper();
            inputBox.Text = String.Empty;
            outputBox.Text = String.Empty;
            keyBox.Text = String.Empty;

            switch (a.SelectedIndex)
            {
                case 0: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 1: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = false; keyBox.GotFocus -= GetKey; break;
                case 2: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 3: changeLanguageBtn.IsEnabled = true; keyBox.IsEnabled = false; keyBox.GotFocus -= GetKey; break;
                case 4: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 5: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = false; keyBox.GotFocus -= GetKey; break;
                case 6: changeLanguageBtn.IsEnabled = true; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 7: changeLanguageBtn.IsEnabled = true; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 8: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus += GetKey; break;
                case 9: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus += GetKey; break;
                case 10: changeLanguageBtn.IsEnabled = true; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 11: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 12: changeLanguageBtn.IsEnabled = false; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                case 13: changeLanguageBtn.IsEnabled = true; keyBox.IsEnabled = true; keyBox.GotFocus -= GetKey; break;
                default: break;
            }
            Start();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Start();
        }

        private void KeyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Start();
        }

        private void Start()
        {
            switch (comboBox.SelectedIndex)
            {
                case 0: // ШИФР ЦЕЗАРЯ
                    { 
                        toolTipIcon.ToolTip = Ciphers.Caesar.About();
                        outputBox.Text = (keyBox.Text.Length != 0 && keyBox.Text.All(a => Char.IsNumber(a))) ?
                            (flag == false) ? 
                                Ciphers.Caesar.Encode(inputBox.Text, Convert.ToInt32(keyBox.Text)) : 
                                Ciphers.Caesar.Decode(inputBox.Text, Convert.ToInt32(keyBox.Text)) :
                                outputBox.Text;
                        break;
                    }
                    
                case 1: // ШИФР АТБАШ 
                    {
                        toolTipIcon.ToolTip = Ciphers.Atbash.About();
                        outputBox.Text = (flag == false)? 
                            Ciphers.Atbash.Encode(inputBox.Text): 
                            Ciphers.Atbash.Decode(inputBox.Text);
                        break;
                    }

                case 2: // ЛОЗУНГОВЫЙ ШИФР
                    {
                        toolTipIcon.ToolTip = Ciphers.Motto.About();
                        outputBox.Text = (flag == false) ? 
                            Ciphers.Motto.Encode(inputBox.Text, keyBox.Text): 
                            Ciphers.Motto.Decode(inputBox.Text, keyBox.Text);
                        break;
                    }

                case 3: // ПОЛИБИЯ
                    {
                        toolTipIcon.ToolTip = Ciphers.PolybianSquare.About();
                        outputBox.Text = (flag == false) ?
                            Ciphers.PolybianSquare.Encode(inputBox.Text, (currentLanguage == Languages.Russian) ? false : true): 
                            Ciphers.PolybianSquare.Decode(inputBox.Text, (currentLanguage == Languages.Russian) ? false : true);
                        break;
                    }

                case 4: // СИСТЕМА ТРИСЕМУСА
                    {
                        toolTipIcon.ToolTip = Ciphers.TrisemusSystem.About();
                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0 && keyBox.Text.All(a => Char.IsLetter(a))) ? 
                                Ciphers.TrisemusSystem.Encode(inputBox.Text, keyBox.Text) : 
                                outputBox.Text:
                            (keyBox.Text.Length != 0 && keyBox.Text.All(a => Char.IsLetter(a))) ?
                                Ciphers.TrisemusSystem.Decode(inputBox.Text, keyBox.Text) : 
                                outputBox.Text;                            
                        break;
                    }

                case 5: // ТАБЛИЦА ТРИСЕМУСА
                    {
                        toolTipIcon.ToolTip = Ciphers.TrisemusTable.About();
                        outputBox.Text = (flag == false) ? 
                            Ciphers.TrisemusTable.Encode(inputBox.Text):
                            Ciphers.TrisemusTable.Decode(inputBox.Text);
                        break;
                    }

                case 6: // ШИФР ПЛЕЙФЕРА
                    {
                        toolTipIcon.ToolTip = Ciphers.Playfair.About();
                        if (currentLanguage == Languages.Russian)
                            if (!keyBox.Text.All(ch => alphabetsList[0].Contains(ch) || alphabetsList[1].Contains(ch))) {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;
                        else
                            if (!keyBox.Text.All(ch => alphabetsList[2].Contains(ch) || alphabetsList[3].Contains(ch))) {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;

                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0 && keyBox.Text.All(a => Char.IsLetter(a))) ? 
                                Ciphers.Playfair.Encode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian)? false : true): 
                                outputBox.Text:
                            (keyBox.Text.Length != 0 && keyBox.Text.All(a => Char.IsLetter(a))) ? 
                                Ciphers.Playfair.Decode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian) ? false : true): 
                                outputBox.Text;
                        break;
                    }

                case 7: // ШИФР ВИЖИНЕРА
                    {
                        toolTipIcon.ToolTip = Ciphers.Vigenere.About();
                        if (currentLanguage == Languages.Russian)
                            if (!keyBox.Text.All(ch => alphabetsList[0].Contains(ch) || alphabetsList[1].Contains(ch)))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;
                        else
                            if (!keyBox.Text.All(ch => alphabetsList[2].Contains(ch) || alphabetsList[3].Contains(ch)))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;

                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.Vigenere.Encode(inputBox.Text, keyBox.Text): 
                                outputBox.Text:
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.Vigenere.Decode(inputBox.Text, keyBox.Text): 
                                outputBox.Text;
                        break;
                    }

                case 8: // ОДИНАРНОЙ ПЕРЕСТАНОВКИ
                    {
                        toolTipIcon.ToolTip = Ciphers.SingleTransposing.About();
                        columnsOfTransposion = inputBox.Text.Length;
                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.SingleTransposing.Encode(inputBox.Text, keyBox.Text): 
                                outputBox.Text:
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.SingleTransposing.Decode(inputBox.Text, keyBox.Text): 
                                outputBox.Text;
                        break;
                    }

                case 9: // БЛОЧНОЙ ПЕРЕСТАНОВКИ
                    {
                        toolTipIcon.ToolTip = Ciphers.BlockTransposing.About();
                        columnsOfTransposion = 3;
                        inputWindow.RowsChangeEnabled(false);
                        inputWindow.ColsChangeEnabled(true);
                        if (keyBox.Text.Length == 0) {
                            outputBox.Text = "Ключ не должен быть пустой";
                            break;
                        } else outputBox.Text = String.Empty;

                        if (keyBox.Text.Contains('0')) outputBox.Text = "Цифры в ключе должны быть положительными";

                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.BlockTransposing.Encode(inputBox.Text, keyBox.Text): 
                                outputBox.Text:
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.BlockTransposing.Decode(inputBox.Text, keyBox.Text):
                                outputBox.Text;
                        break;
                    }

                case 10: // ВЕРТИКАЛЬНОЙ ПЕРЕСТАНОВКИ
                    {
                        toolTipIcon.ToolTip = Ciphers.VerticalTransposing.About();
                        if (currentLanguage == Languages.Russian)
                            if (!keyBox.Text.All(ch => alphabetsList[0].Contains(ch) || alphabetsList[1].Contains(ch)))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;
                        else
                            if (!keyBox.Text.All(ch => alphabetsList[2].Contains(ch) || alphabetsList[3].Contains(ch)))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;

                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.VerticalTransposing.Encode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian) ? false : true): 
                                outputBox.Text :
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.VerticalTransposing.Decode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian) ? false : true): 
                                outputBox.Text;
                        break;
                    }

                case 11: // МНОЖЕСТВЕННОЙ ПЕРЕСТАНОВКИ
                    {
                        toolTipIcon.ToolTip = Ciphers.MultiTransposing.About();

                        if (keyBox.Text.Length == 0)
                        {
                            outputBox.Text = String.Empty;
                            break;
                        } else {
                            if (!keyBox.Text.Contains(' ') ||
                                 keyBox.Text.Count(ch => ch == ' ') > 1 ||
                                 keyBox.Text.Last() == ' ')
                            {
                                outputBox.Text = "Что-то пошло не так 1";
                                break;
                            }
                            foreach (var letter in keyBox.Text)
                                if (letter != ' ' && Char.IsDigit(letter))
                                {
                                    outputBox.Text = "Что-то пошло не так 2";
                                    break;
                                }
                        }

                        List<string> keys = keyBox.Text.Split(' ').ToList();

                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 1; j <= keys[i].Length; j++)
                            {
                                if (!keys[i].Contains(j.ToString()))
                                {
                                    // MessageBox.Show($"Keys[{i}] = {keys[i]} does not contain {j.ToString()}");
                                    outputBox.Text = "Что-то пошло не так 3";
                                    return;
                                } else {
                                    // MessageBox.Show($"Keys[{i}] = {keys[i]} contains {j.ToString()}");
                                }
                            }
                        }

                        outputBox.Text = (keyBox.Text.Length != 0) ?
                            Ciphers.MultiTransposing.Encode(inputBox.Text, keys[0], keys[1]) :
                            Ciphers.MultiTransposing.Decode(inputBox.Text, keys[0], keys[1]);
                        break;
                    }

                case 12: // ГОСТ 28147-89
                    {
                        toolTipIcon.ToolTip = Ciphers.Gost.About();
                        outputBox.Text = (flag == false) ? 
                            Encoding.Default.GetString(Ciphers.Gost.E32.Encrypt(inputBox.Text, keyBox.Text)):
                            Encoding.Default.GetString(Ciphers.Gost.D32.Decrypt(inputBox.Text, keyBox.Text));
                        break;
                    }

                case 13: // ГАММИРОВАНИЕ
                    {
                        toolTipIcon.ToolTip = Ciphers.Gamming.About();
                        if (currentLanguage == Languages.Russian)
                            if (!keyBox.Text.All(ch => newAlphabetsList[0].Contains(Char.ToUpper(ch))))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;
                        else
                            if (!keyBox.Text.All(ch => newAlphabetsList[2].Contains(Char.ToUpper(ch))))
                            {
                                outputBox.Text = symbolsErrorKeyString;
                                break;
                            } else
                                outputBox.Text = String.Empty;

                        outputBox.Text = (flag == false) ?
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.Gamming.Encode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian) ? false : true, newAlphabetsList): 
                                outputBox.Text:
                            (keyBox.Text.Length != 0) ? 
                                Ciphers.Gamming.Decode(inputBox.Text, keyBox.Text, (currentLanguage == Languages.Russian) ? false : true, newAlphabetsList): 
                                outputBox.Text;
                        break;
                    }
                    
                default:
                    break;
            }
            
        }

        private void GetKey(Object sender, EventArgs e)
        {
            inputWindow.Update(1, columnsOfTransposion);
            inputWindow.ShowDialog();
            keyBox.Text = inputWindow.key;
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            if (!flag)
            {
                flag = !flag;
                modeTextBlock.Text = "🔓" + Environment.NewLine + "РАСШИФРОВКА";
                modeTextBlock.Foreground = (Brush)Resources["FontGreen"];
                inputBox.ToolTip = "Введите зашифровываемое слово";
            } else {
                flag = !flag;
                modeTextBlock.Text = "🔒" + Environment.NewLine + "ШИФРОВАНИЕ";
                modeTextBlock.Foreground = (Brush)Resources["FontRed"];
                inputBox.ToolTip = "Введите расшифровываемое слово";
            }
            Start();
            
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            inputBox.Text = outputBox.Text;
            outputBox.Text = String.Empty;
            Mode_Click(sender, e);
        }

        private void MoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).Y < 35)
                this.DragMove();
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }            

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = (mainWindow.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            inputWindow.Close();
            this.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inputWindow.Close();
        }

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (currentLanguage == Languages.English)
                SetLocale(Languages.Russian);
            else
                SetLocale(Languages.English);
            Start();
        }
    }
}