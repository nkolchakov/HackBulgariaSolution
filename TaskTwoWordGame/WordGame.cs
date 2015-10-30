using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTwoWordGame
{
    /* 
     * input:
i	v	a	n
e	v	n	h
i	n	a	v
m	v	v	n
q	r	i	t
     
output: 3
     * 
custom tests:

i	v	a	n
e	v	i	h
i	n	v	v
m	v	a	n
q	r	n	t 

output: 2
     * 
i	v	a	i
n	v	v	n
i	a	a	v
n	v	v	n
i	v	a	i

output: 4
     * 
i	v	n	n
v	v	a	i
a	n	v	v
n	a	i	i
n	r	n	t
     
output: 3
     * 
     * 
i	v	i	n
v	v	a	i
c	n	v	v
n	a	i	i
a	r	n	t
     
output: 0
     * */
    class WordGame
    {
        static void Main()
        {
            string[,] matrix = new string[5, 4];
            string[] pattern = { "i", "v", "a", "n" };
            int count = 0;
            // read input 
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var line = Console.ReadLine();
                var lettersAsString = line.Split('\t');

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = lettersAsString[j];
                }
            }

            // check all directions if possible for every element in the matrix
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    count += checkVerticals(i, j, matrix, pattern);
                    count += checkHorizontals(i, j, matrix, pattern);
                    count += checkAcross(i, j, matrix, pattern);
                }
            }
            Console.WriteLine(count);
        }
        static int checkHorizontals(int row, int col, string[,] matrix, string[] pattern)
        {
            if (col + 3 <= matrix.GetLength(1) - 1) // from left to right 
            {
                foreach (var symbol in pattern)
                {
                    if (matrix[row, col] != symbol)
                    {
                        return 0;
                    }
                    col++;
                }
            }
            else if (col - 3 >= 0) // from right to left
            {
                foreach (var symbol in pattern)
                {
                    if (matrix[row, col] != symbol)
                    {
                        return 0;
                    }
                    col--;
                }
            }
            else // neither left or right
            {
                return 0;
            }
            return 1; //add to count if "ivan" is found
        }
        static int checkVerticals(int row, int col, string[,] matrix, string[] pattern)
        {
            int totalRows = matrix.GetLength(0);

            if (row + 3 <= matrix.GetLength(0) - 1) // from top to bottom
            {
                foreach (var symbol in pattern)
                {
                    if (matrix[row, col] != symbol)
                    {
                        return 0;
                    }
                    row++;
                }
            }
            else if (row - 3 >= 0) // from bot to top
            {
                foreach (var symbol in pattern)
                {
                    if (matrix[row, col] != symbol)
                    {
                        return 0;
                    }
                    row--;
                }
            }
            else // neither top or bot
            {
                return 0;
            }
            return 1; //add to count if "ivan" is found
        }
        static int checkAcross(int row, int col, string[,] matrix, string[] pattern)
        {
            if (col == 3 || col == 0 ) // does it have space across
            {
                if (row + pattern.Length - 1 <= matrix.GetLength(0) - 1) // have space to go from top to bot across
                {

                    if (col == 0)
                    {

                        foreach (var symbol in pattern)
                        {
                            if (matrix[row, col] != symbol)
                            {
                                return 0;
                            }
                            row++;
                            col++;
                        }
                    }
                    else if (col == 3)
                    {

                        foreach (var symbol in pattern)
                        {
                            if (matrix[row, col] != symbol)
                            {
                                return 0;
                            }
                            row++;
                            col--;
                        }
                    }
                }
                else if (row - pattern.Length  >= 0) // have space to go from  bot to top accross
                {
                    if (col == 0) // if at the beginning -> go left to right accross
                    {
                        foreach (var symbol in pattern)
                        {
                            if (matrix[row, col] != symbol)
                            {
                                return 0;
                            }
                            row--;
                            col++;
                        }
                    }
                    else if (col == 3) // if at the end -> go right to left accross
                    {
                        foreach (var symbol in pattern)
                        {
                            if (matrix[row, col] != symbol)
                            {
                                return 0;
                            }
                            row--;
                            col--;
                        }
                    }
                }
                else // no space 
                {
                    return 0;
                }

                return 1; // if "ivan" is found add to count

            }
            else // if not enough space for the pattern word
            {
                return 0;
            }
        }

    }
}
