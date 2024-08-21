using System;
using System.Threading;

namespace TetrisTest
{
    internal static class Program
    {
        private enum Type
        {
            Pipe,
            Square,
            Ship,
            Lightning,
            L
        }
        private struct Obj
        {
            public byte Ax;
            public byte Ay;
            public byte Bx;
            public byte By;
            public byte Cx;
            public byte Cy;
            public byte Dx;
            public byte Dy;
            public readonly Type Type;
            public byte Position;

            public Obj (byte ax, byte ay, byte bx, byte by, byte cx, byte cy, byte dx, byte dy, Type type, byte position)
            {
                Ax = ax;
                Ay = ay;
                Bx = bx;
                By = by;
                Cx = cx;
                Cy = cy;
                Dx = dx;
                Dy = dy;
                Type = type;
                this.Position = position;
            }
        }
        

        private static readonly bool[,] Grid = new bool[24,10];
        private static Obj _current;
        private static int _score;
        
        private static void PutObj (Obj obj)
        {
            Grid[obj.Ay, obj.Ax] = true;
            Grid[obj.By, obj.Bx] = true;
            Grid[obj.Cy, obj.Cx] = true;
            Grid[obj.Dy, obj.Dx] = true;
        }

        private static void RemoveObj (Obj obj)
        {
            Grid[obj.Ay, obj.Ax] = false;
            Grid[obj.By, obj.Bx] = false;
            Grid[obj.Cy, obj.Cx] = false;
            Grid[obj.Dy, obj.Dx] = false;
        }

        private static Obj CreatePipe ()
        {
            return new Obj(4, 0, 4, 1, 4, 2, 4, 3, Type.Pipe, 1);
        }
        
        private static Obj CreateSquare ()
        {
            return new Obj(3, 2, 3, 3, 4, 2, 4, 3, Type.Square, 1);
        }
        
        private static Obj CreateShip ()
        {
            return new Obj(4, 1, 4, 2, 5, 2, 4, 3, Type.Ship, 1);
        }
        
        private static Obj CreateLightning (byte side)
        {
            return new Obj(4, 1, 4, 2, side, 2, side, 3, Type.Lightning, (byte)(side - 2));
        }
        
        private static Obj CreateL (byte side)
        {
            var pos = side == 3 ? 1 : 5;
            return new Obj(4, 1, 4, 3, 4, 2, side, 3, Type.L, (byte)pos);
        }

        private static void PrintGrid ()
        {
            Console.WriteLine("Score : " + _score);
            Console.WriteLine("=====Grid=====");
            for (byte i = 4; i < 24; i++)
            {
                Console.Write("||");
                for (byte j = 0; j < 10; j++)
                {
                    if (Grid[i, j])
                    {
                        if (IsInObj(j, i))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("X");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("x");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                        
                    else
                        Console.Write("-");
                }
                Console.WriteLine("||");
            }
            Console.WriteLine("===End Grid===");
            Console.WriteLine();
        }

        private static void CreateObj ()
        {
            Random obj = new Random();
            switch (obj.Next(5))
            {
                case 0:
                    _current = CreatePipe();
                    break;
                case 1:
                    _current = CreateSquare();
                    break;
                case 2:
                    _current = CreateShip();
                    break;
                case 3:
                    _current = CreateLightning(obj.Next(2) == 1 ? (byte)3 : (byte)5);
                    break;
                case 4:
                    _current = CreateL(obj.Next(2) == 1 ? (byte)3 : (byte)5);
                    break;
            }
            
            PutObj(_current);
        }

        private static bool IsInObj (Byte x, Byte y)
        {
            if (_current.Ax == x && _current.Ay == y)
                return true;
            if (_current.Bx == x && _current.By == y)
                return true;
            if (_current.Cx == x && _current.Cy == y)
                return true;
            return _current.Dx == x && _current.Dy == y;
        }

        private static bool NextStepPossible()
        {
            if (_current.Ay == 23 || _current.By == 23 || _current.Cy == 23 || _current.Dy == 23)
                return false;
            Byte x = _current.Ax;
            Byte y = _current.Ay;
            y++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Bx;
            y = _current.By;
            y++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Cx;
            y = _current.Cy;
            y++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Dx;
            y = _current.Dy;
            y++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;

            return _current.Ay < 23 && _current.By < 23 && _current.Cy < 23 && _current.Dy < 23;
        }

        private static void GoDown ()
        {
            if (!NextStepPossible())
                return;
            RemoveObj(_current);
            _current.Ay++;
            _current.By++;
            _current.Cy++;
            _current.Dy++;
            PutObj(_current);
            PrintGrid();
        }

        private static bool GoRightPossible ()
        {
            if (_current.Ax == 9 || _current.Bx == 9 || _current.Cx == 9 || _current.Dx == 9)
                return false;
            Byte x = _current.Ax;
            Byte y = _current.Ay;
            x++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Bx;
            y = _current.By;
            x++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Cx;
            y = _current.Cy;
            x++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Dx;
            y = _current.Dy;
            x++;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;

            return _current.Ax < 9 && _current.Bx < 9 && _current.Cx < 9 && _current.Dx < 9;
        }

        private static void GoRight()
        {
            if (!NextStepPossible())
                return;
            if (!GoRightPossible())
                return;
            RemoveObj(_current);
            _current.Ax++;
            _current.Bx++;
            _current.Cx++;
            _current.Dx++;
            PutObj(_current);
            PrintGrid();
        }

        private static bool GoLeftPossible()
        {
            if (_current.Ax == 0 || _current.Bx == 0 || _current.Cx == 0 || _current.Dx == 0)
                return false;
            Byte x = _current.Ax;
            Byte y = _current.Ay;
            x--;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Bx;
            y = _current.By;
            x--;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Cx;
            y = _current.Cy;
            x--;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;
            x = _current.Dx;
            y = _current.Dy;
            x--;
            if (Grid[y, x] && !IsInObj(x, y))
                return false;

            return _current.Ax > 0 && _current.Bx > 0 && _current.Cx > 0 && _current.Dx > 0;
        }
        
        private static void GoLeft()
        {
            if (!NextStepPossible())
                return;
            if (!GoLeftPossible())
                return;
            RemoveObj(_current);
            _current.Ax--;
            _current.Bx--;
            _current.Cx--;
            _current.Dx--;
            PutObj(_current);
            PrintGrid();
        }

        private static void TurnIfPossible()
        {
            if (!NextStepPossible())
                return;
            RemoveObj(_current);
            switch (_current.Type)
            {
                case Type.Pipe:
                    if (_current.Position == 1)
                    {
                        if (_current.Bx - 1 >= 0 && _current.Bx + 2 <= 9 && !Grid[_current.By, _current.Bx - 1]
                            && !Grid[_current.By, _current.Bx + 1]
                            && !Grid[_current.By, _current.Bx + 2])
                        {
                            _current.Ay = _current.Cy = _current.Dy = _current.By;
                            _current.Ax--;
                            _current.Cx++;
                            _current.Dx += 2;
                            _current.Position = 2;
                        }
                    }

                    else if (_current.By + 2 <= 23 &&
                             !Grid[_current.By - 1, _current.Bx]
                             && !Grid[_current.By + 1, _current.Bx]
                             && !Grid[_current.By + 2, _current.Bx])
                    {
                        _current.Ax = _current.Cx = _current.Dx = _current.Bx;
                        _current.Ay--;
                        _current.Cy++;
                        _current.Dy += 2;
                        _current.Position = 1;
                    }
                    break;
                
                case Type.Square:
                    break;
                
                case Type.Ship:
                    switch (_current.Position)
                    {
                        case 1:
                            if (_current.Bx - 1 >= 0 && !Grid[_current.By, _current.Bx - 1])
                            {
                                _current.Ay++;
                                _current.Ax--;
                                _current.Position++;
                            }
                            break;
                        case 2:
                            if (!Grid[_current.By - 1, _current.Bx])
                            {
                                _current.Cy--;
                                _current.Cx--;
                                _current.Position++;
                            }
                            break;
                        case 3:
                            if (_current.Bx + 1 <= 9 && !Grid[_current.By, _current.Bx + 1])
                            {
                                _current.Dy--;
                                _current.Dx++;
                                _current.Position++;
                            }
                            break;
                        case 4:
                            if (_current.By + 1 <= 23 && !Grid[_current.By + 1, _current.Bx])
                            {
                                _current.Ay--;
                                _current.Ax++;
                                _current.Cy++;
                                _current.Cx++;
                                _current.Dy++;
                                _current.Dx--;
                                _current.Position++;
                            }
                            break;
                    }

                    break;
                
                case Type.Lightning:
                    switch (_current.Position)
                    {
                        case 1:
                            if (_current.Bx + 1 <= 9 &&
                                !Grid[_current.By + 1, _current.Bx] && !Grid[_current.By + 1, _current.Bx + 1])
                            {
                                _current.Ay += 2;
                                _current.Dx += 2;
                                _current.Position = 2;
                            }
                            break;
                        case 2:
                            if (!Grid[_current.By - 1, _current.Bx] && !Grid[_current.By + 1, _current.Bx - 1])
                            {
                                _current.Ay -= 2;
                                _current.Dx -= 2;
                                _current.Position = 1;
                            }
                            break;
                        case 3:
                            if (_current.Bx - 1 >= 0 &&
                                !Grid[_current.By + 1, _current.Bx] && !Grid[_current.By + 1, _current.Bx - 1])
                            {
                                _current.Ay += 2;
                                _current.Dx -= 2;
                                _current.Position = 4;
                            }
                            break;
                        case 4:
                            if (!Grid[_current.By + 1, _current.Bx + 1] && !Grid[_current.By - 1, _current.Bx])
                            {
                                _current.Ay -= 2;
                                _current.Dx += 2;
                                _current.Position = 3;
                            }
                            break;
                    }

                    break;
                
                case Type.L:
                    switch (_current.Position)
                    {
                        case 1:
                            if (_current.Bx + 2 <= 9 &&
                                !Grid[_current.By, _current.Bx + 1] && !Grid[_current.By, _current.Bx + 2])
                            {
                                _current.Ay += 2;
                                _current.Ax++;
                                _current.Dx += 3;
                                _current.Position = 2;
                            }
                            break;
                        case 2:
                            if (_current.By + 2 <= 23 &&
                                !Grid[_current.By + 1, _current.Bx] && !Grid[_current.By + 2, _current.Bx])
                            {
                                _current.Dx -= 2;
                                _current.Dy++;
                                _current.Cy += 3;
                                _current.Position = 3;
                            }
                            break;
                        case 3:
                            if (_current.Bx - 2 >= 0 &&
                                !Grid[_current.By, _current.Bx - 1] && !Grid[_current.By, _current.Bx - 2])
                            {
                                _current.Cy -= 2;
                                _current.Cx--;
                                _current.Ax -= 3;
                                _current.Position = 4;
                            }
                            break;
                        case 4:
                            if (!Grid[_current.By - 1, _current.Bx] && !Grid[_current.By - 2, _current.Bx])
                            {
                                _current.Ax += 2;
                                _current.Ay -= 2;
                                _current.Cx++;
                                _current.Cy--;
                                _current.Dx--;
                                _current.Dy--;
                                _current.Position = 1;
                            }
                            break;
                        case 5:
                            if (_current.Bx + 2 <= 9 && _current.By + 1 <= 23 &&
                                !Grid[_current.By, _current.Bx + 2] && !Grid[_current.By + 1, _current.Bx])
                            {
                                _current.Ax += 2;
                                _current.Ay += 2;
                                _current.Cy += 2;
                                _current.Position = 6;
                            }
                            break;
                        case 6:
                            if (_current.Bx - 1 >= 0 && _current.By + 2 <= 23 &&
                                !Grid[_current.By, _current.Bx - 1] && !Grid[_current.By + 2, _current.Bx])
                            {
                                _current.Ax -= 2;
                                _current.Ay += 2;
                                _current.Dx -= 2;
                                _current.Position = 7;
                            }
                            break;
                        case 7:
                            if (_current.Bx - 2 >= 0 &&
                                !Grid[_current.By, _current.Bx - 2] && !Grid[_current.By - 1, _current.Bx])
                            {
                                _current.Ax -= 2;
                                _current.Ay -= 2;
                                _current.Cy -= 2;
                                _current.Position = 8;
                            }
                            break;
                        case 8:
                            if (_current.Bx + 1 <= 23 &&
                                !Grid[_current.By, _current.Bx + 1] && !Grid[_current.By - 2, _current.Bx])
                            {
                                _current.Ax += 2;
                                _current.Ay -= 2;
                                _current.Dx += 2;
                                _current.Position = 5;
                            }
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            PutObj(_current);
            PrintGrid();
        }

        private static void DestroyLine(int index)
        {
            for (int i = index; i > 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    Grid[i, j] = Grid[i - 1, j];
                }
            }
        }
        
        private static void CheckRemovableLines()
        {
            byte[] arr = {_current.Ay, _current.By, _current.Cy, _current.Dy};
            Array.Sort(arr);
            bool destroyable = true;
            for (int ind = 0; ind < 4; ind++)
            {
                for (int i = 0; i < 10; ++i)
                {
                    if (Grid[arr[ind], i]) continue;
                    destroyable = false;
                    break;
                }

                if (destroyable)
                {
                    DestroyLine(arr[ind]);
                    _score += 100;
                }
                destroyable = true;
            }
            PrintGrid();
        }

        private static void GoDownSiz()
        {
            RemoveObj(_current);
            while (NextStepPossible())
            {
                _current.Ay++;
                _current.By++;
                _current.Cy++;
                _current.Dy++;
            }
            PutObj(_current);
            PrintGrid();
        }
        
        private static void Render ()
        {
            CreateObj();
            while (NextStepPossible())
            {
                GoDown();
                Thread.Sleep(200);
                while (NextStepPossible())
                {
                    GoDown();
                    Thread.Sleep(200);

                }

                _score += 10;
                CheckRemovableLines(); 
                Thread.Sleep(200);
                CreateObj();
            }
        }

        private static void Inputs()
        {
            while (true)
            {
                var c = Console.ReadKey(true);
                switch (c.Key)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        GoRight();
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Q:
                        GoLeft();
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (NextStepPossible())
                            GoDown();
                        break;
                    case ConsoleKey.Spacebar:
                        TurnIfPossible();
                        break;
                    case ConsoleKey.Enter:
                        GoDownSiz();
                        break;
                }
            }
        }
        
        public static void Main (string[] args)
        {
            //Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            ThreadStart child = Inputs;
            Thread childThread = new Thread(child);
            childThread.Start();
            Render();
            childThread.Abort();
            Console.WriteLine("Your Score is " + _score + "!");
        }
    }
}