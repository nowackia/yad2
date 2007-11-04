using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General {
    public enum MenuState : byte{
        Unlogged = 0,
        Chat,
        GameBrowse,
        _LENGTH,
        Invalid
    }

    public enum MenuAction : byte {
        Login = 0,
        GameBrowseEnter,
        GrameBrowseLeave,
        _LENGTH
    }


    public class PlayerStateMachine {
        static MenuState[,] _transitions;
        static PlayerStateMachine() {
            _transitions = new MenuState[(int)MenuState._LENGTH, (int)MenuAction._LENGTH];
            for (byte i = 0; i < (byte)MenuState._LENGTH; ++i)
                for (byte j = 0; j < (byte)MenuAction._LENGTH; ++j)
                {
                    MenuState state = (MenuState)(i);
                    MenuAction action = (MenuAction)(j);
                    switch(state) {
                        case MenuState.Unlogged:
                            switch(action) {
                                case MenuAction.Login:
                                    _transitions[i,j] = MenuState.Chat;
                                    break;
                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                        case MenuState.Chat:
                            switch(action) {
                                case MenuAction.GameBrowseEnter:
                                    _transitions[i,j] = MenuState.GameBrowse;
                                    break;
                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                        case MenuState.GameBrowse:
                            switch(action) {
                                case MenuAction.GrameBrowseLeave:
                                    _transitions[i,j] = MenuState.Chat;
                                    break;
                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                    }
                }
        }
 
        public static MenuState Transform(MenuState state, MenuAction action) {
            return _transitions[(int)state,(int)action];
        }
    }
}
