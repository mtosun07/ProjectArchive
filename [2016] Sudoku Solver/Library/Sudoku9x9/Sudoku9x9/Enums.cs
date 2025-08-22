using System;

namespace Sudoku9x9
{
    public enum SubArrayType : byte
    {
        Row = 1,
        Column = 2,
        Square = 4
    }

    [Flags]
    public enum SubArrayTypes : int
    {
        NONE = 0,
        Row = SubArrayType.Row,
        Column = SubArrayType.Column,
        Square = SubArrayType.Square,
        ALL = Row | Column | Square
    }
}