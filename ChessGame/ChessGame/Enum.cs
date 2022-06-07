using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public enum chess_type
    {
        blank,
        big,
        small
    };
    public enum player_type
    {
        blank,
        great,
        little
    };
    public enum player_action
    {
        DoMove,
        DoRegret,
        DoSurrender,
        DoMsg
    };
}
