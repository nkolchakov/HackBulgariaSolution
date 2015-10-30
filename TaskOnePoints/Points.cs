using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOnePoints
{
    class Points
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(new char[] { ':', ' ', '(', ',', ')' },
                StringSplitOptions.RemoveEmptyEntries);

            string commands = Console.ReadLine();

            int x = int.Parse(input[input.Length - 2]);
            int y = int.Parse(input[input.Length - 1]);

            bool inverted = true;

            foreach (var command in commands)
            {
                if (inverted)
                {
                    switch (command)
                    {
                        case '~': inverted = !inverted;
                            break;
                        case '>': x++;
                            break;
                        case '<': x--;
                            break;
                        case '^': y--;
                            break;
                        case 'v': y++;
                            break;
                        default:
                            break;
                    }
                }
                else // inverted, when ~ is pressed
                {
                    switch (command)
                    {
                        case '~': inverted = !inverted;
                            break;
                        case '>': x--;
                            break;
                        case '<': x++;
                            break;
                        case '^': y++;
                            break;
                        case 'v': y--;
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine(String.Format("({0}, {1})", x, y));
        }
    }
}
